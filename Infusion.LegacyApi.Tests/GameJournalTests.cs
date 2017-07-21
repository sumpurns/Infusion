﻿using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infusion.LegacyApi.Tests
{
    [TestClass]
    public class GameJournalTests
    {
        [TestMethod]
        public void Contains_is_case_insensitive_by_default()
        {
            var source = new JournalSource();
            var journal = new GameJournal(source, null);

            source.AddMessage("name", "this is an AfK check", 0, 0);

            journal.Contains("afk").Should().BeTrue();
        }

        [TestMethod]
        public void Contains_returns_false_if_journal_doesnt_contains_string()
        {
            var source = new JournalSource();
            var journal = new GameJournal(source, null);

            source.AddMessage("name", "this is an AfK check", 0, 0);

            journal.Contains("and now for completely something else").Should().BeFalse();
        }

        [TestMethod]
        public void Can_await_entries_received_between_lastaction_and_WaitAny()
        {
            bool executed = false;
            var source = new JournalSource();
            var journal = new GameJournal(source, null);

            source.AddMessage("name", "before last action", 0, 0);
            source.NotifyLastAction();
            source.AddMessage("name", "after last action", 0, 0);

            journal.When("after last action", () => executed = true).WaitAny(TimeSpan.FromMilliseconds(100));

            executed.Should().BeTrue();
        }

        [TestMethod]
        public void Doesnt_await_entries_received_before_lastaction()
        {
            bool executed = false;
            bool timeoutExecuted = false;
            var source = new JournalSource();
            var journal = new GameJournal(source, null);

            source.AddMessage("name", "before last action", 0, 0);
            source.NotifyLastAction();
            source.AddMessage("name", "after last action", 0, 0);

            journal.When("before last action", () => executed = true).WhenTimeout(() => timeoutExecuted = true).WaitAny(TimeSpan.FromMilliseconds(10));

            timeoutExecuted.Should().BeTrue();
            executed.Should().BeFalse();
        }

        [TestMethod]
        public void WaitAny_chekcs_only_entries_created_afer_last_check()
        {
            var source = new JournalSource();
            var journal = new GameJournal(source, null);
            bool firstCheckExecuted = false;
            bool secondCheckExecuted = false;

            source.AddMessage("name", "message1", 0, 0);
            journal.When("message1", () => { firstCheckExecuted = true; }).WaitAny(TimeSpan.FromMilliseconds(1));
            journal.When("message1", () => { secondCheckExecuted = true; }).WaitAny(TimeSpan.FromMilliseconds(1));

            firstCheckExecuted.Should().BeTrue();
            secondCheckExecuted.Should().BeFalse();
        }

        [TestMethod]
        public void Can_enumerate_and_add_to_journal_concurrently()
        {
            var source = new JournalSource();
            var journal = new GameJournal(source, null);

            source.AddMessage("name", "first message", 0, 0);

            using (var enumerator = journal.GetEnumerator())
            {
                enumerator.MoveNext().Should().BeTrue();
                enumerator.Current.Message.Should().Be("first message");
                source.AddMessage("name", "second, concurrently added message", 0, 0);
                enumerator.MoveNext().Should().BeFalse();
            }
        }

        [TestMethod]
        public void Can_see_entry_received_after_journal_deletion()
        {
            var source = new JournalSource();
            var journal = new GameJournal(source, null);

            source.AddMessage("name", "first message", 0, 0);
            journal.Delete();
            source.AddMessage("name", "message after delete", 0, 0);

            journal.Contains("message after delete").Should().BeTrue();
        }

        [TestMethod]
        public void Cannot_see_entry_received_before_journal_deletion()
        {
            var source = new JournalSource();
            var journal = new GameJournal(source, null);

            source.AddMessage("name", "message before delete", 0, 0);
            journal.Delete();

            journal.Contains("message before delete").Should().BeFalse();
        }

        [TestMethod]
        public void Cannot_see_entries_received_before_journal_instantiation()
        {
            var source = new JournalSource();
            source.AddMessage("name", "message before instantiation", 0, 0);

            var journal = new GameJournal(source, null);

            journal.Contains("message before instantiation").Should().BeFalse();
        }

        [TestMethod]
        public void Can_see_entries_received_after_journal_instantiation()
        {
            var source = new JournalSource();
            var journal = new GameJournal(source, null);

            source.AddMessage("name", "message after instantiation", 0, 0);

            journal.Contains("message after instantiation").Should().BeTrue();
        }

        [TestMethod]
        public void ContainsAnyWord_is_not_affected_by_previous_call_to_WaitAny()
        {
            var source = new JournalSource();
            var journal = new GameJournal(source, null);

            source.AddMessage("name", "message1", 0, 0);

            journal.When("message1", () => { }).WaitAny(TimeSpan.FromMilliseconds(1));

            journal.ContainsAnyWord("message1").Any().Should().BeTrue();
        }
    }
}