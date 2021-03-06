#load "colors.csx"
#load "targeting.csx"
#load "graphicseffects.csx"

using System;
using System.Collections.Generic;
using Infusion;
using Infusion.LegacyApi;

public static class HitPointNotifier
{
    public static ScriptTrace Trace = UO.Trace.Create();
    public static IPrintHitPointNotification Mode = HitPointNotificationModes.Standard;

    public static void Run()
    {
        var journal = UO.CreateEventJournal();

        journal.When<CurrentHealthUpdatedEvent>(OnHealthUpdated)
            .Incomming();
    }
    
    public static void Enable()
    {
        UO.CommandHandler.Invoke("hpnotify");
    }
    
    public static void Disable()
    {
        UO.CommandHandler.Terminate("hpnotify");
    }

    public static void Toggle()
    {
        if (UO.CommandHandler.IsCommandRunning("hpnotify-toggle"))
            Disable();
        else
            Enable();
    }

    private static void OnHealthUpdated(CurrentHealthUpdatedEvent args)
    {
        if (IsFirstStatusBarUpdate(args))
           return;
           
        // It seems like Sphere adds HP disregardig maximum HP. Then Sphere sends another
        // update packet which adjust HP to maximum value.
        if (IsHealingOverMaximumUpdate(args))
            return;
    
        var delta = args.UpdatedMobile.CurrentHealth - args.OldHealth;

        Mode.Print(delta, args.UpdatedMobile);
    }

    private static bool IsHealingOverMaximumUpdate(CurrentHealthUpdatedEvent args) =>
        args.UpdatedMobile.Id == UO.Me.PlayerId && args.UpdatedMobile.CurrentHealth > args.UpdatedMobile.MaxHealth;    

    private static bool IsFirstStatusBarUpdate(CurrentHealthUpdatedEvent args) =>
        args.OldHealth == 0 && args.UpdatedMobile.CurrentHealth == args.UpdatedMobile.MaxHealth;
}

public interface IPrintHitPointNotification
{
    void Print(int delta, Mobile updatedMobile);
}

public class AboveAllMobilesNotificationPrinter : IPrintHitPointNotification
{
    public Color EnemyColor { get; set; } = Colors.Green;
    public Color FriendColor { get; set; } = Colors.Blue;
    public Color MyColor { get; set; } = Colors.LightBlue;
    public Color PetsColor { get; set; } = Colors.Blue;

    public void Print(int delta, Mobile mobile)
    {
        var deltaText = (delta > 0) ? "+" + delta.ToString() : delta.ToString();

        Color textColor;
        
        if (mobile.Id == UO.Me.PlayerId)
        {
            textColor = MyColor;
        }
        else if (mobile.CanRename && Pets.MyPets.Contains(mobile.Id))
        {
            textColor = PetsColor;
        }
        else if (mobile.Notoriety == Notoriety.Friend)
        {
            textColor = FriendColor;
        }
        else
        {
            textColor = EnemyColor;
        }
        
    
        var currentHealthText = mobile.Id == UO.Me.PlayerId ?
            mobile.CurrentHealth.ToString() :
            $"{mobile.CurrentHealth} %";
    
        UO.ClientPrint($"{deltaText}/{currentHealthText}", "hpnotify",
            mobile.Id, mobile.Type, SpeechType.Speech, textColor, log: false);        
    }
}

public class StandardPrinter : IPrintHitPointNotification
{
    private Dictionary<ObjectId, string> prefixes = new Dictionary<ObjectId, string>();

    public void AddPrefix(ObjectId id, string prefix)
    {
        prefixes[id] = prefix;
    }
    
    public bool PlayerHealEnabled { get; set; } = false;
    public bool OthersHealEnabled { get; set; } = false;

    public Color PlayerHealColor { get; set; } = Colors.None;
    public Color PlayerHarmColor { get; set; } = Colors.Red;
    public GraphicsEffect PlayerHarmEffect { get; set; } = GraphicsEffect.EnergyTwister;
    public Color OthersHealColor { get; set; } = Colors.None;
    public Color OthersHarmColor { get; set; } = Colors.None;
   
    public bool MeAndTargetOnly { get; set; } = true;

    public void Print(int delta, Mobile mobile)
    {
        if (UO.Me.PlayerId == mobile.Id && delta < 0 && mobile.CurrentHealth >= mobile.MaxHealth)
        {
            HitPointNotifier.Trace.Log($"Correction detected: delta {delta}; current {mobile.CurrentHealth}; max {mobile.MaxHealth}");
            return;
        }            
    
        if (delta > 0 && UO.Me.PlayerId == mobile.Id && !PlayerHealEnabled)
            return;
        if (delta > 0 && UO.Me.PlayerId != mobile.Id && !OthersHealEnabled)
            return;
            
        if (UO.Me.PlayerId == mobile.Id && PlayerHarmEffect != null)
            PlayerHarmEffect.PlayOverPlayer();
    
        var deltaText = (delta > 0) ? "+" + delta.ToString() : delta.ToString();
        var textColor = mobile.Id == UO.Me.PlayerId ? 
            ((delta > 0) ? PlayerHealColor : PlayerHarmColor) :
            ((delta > 0) ? OthersHealColor : OthersHarmColor);
       
        if (MeAndTargetOnly && mobile.Id != UO.Me.PlayerId
            && Targeting.SelectedTarget.HasValue && mobile.Id != Targeting.SelectedTarget.Value)
        {
            return;
        }
        
        var currentHealthText = mobile.Id == UO.Me.PlayerId ?
           mobile.CurrentHealth.ToString() :
           $"{mobile.CurrentHealth} %";
        
        string text;
        
        if (prefixes.TryGetValue(mobile.Id, out string prefix))
            text = $"{prefix}: {deltaText}/{currentHealthText}";
        else
            text = $"{deltaText}/{currentHealthText}";
        
        UO.ClientPrint(text, "hpnotify", mobile.Id, mobile.Type, SpeechType.Speech, textColor, log: false);        
   }
}

public static class HitPointNotificationModes
{
    public static AboveAllMobilesNotificationPrinter AboveAllMobiles { get; } =
        new AboveAllMobilesNotificationPrinter();
    public static StandardPrinter Standard { get; } =
        new StandardPrinter();        
}

UO.RegisterBackgroundCommand("hpnotify", HitPointNotifier.Run);
UO.RegisterCommand("hpnotify-toggle", HitPointNotifier.Toggle);
UO.RegisterCommand("hpnotify-enable", HitPointNotifier.Enable);
UO.RegisterCommand("hpnotify-disable", HitPointNotifier.Disable)