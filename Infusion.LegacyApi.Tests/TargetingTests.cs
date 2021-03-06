﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Infusion.LegacyApi.Tests.Packets;
using Infusion.Packets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infusion.LegacyApi.Tests
{
    [TestClass]
    public class TargetingTests
    {
        [TestMethod]
        public void Can_wait_for_target_When_no_last_action()
        {
            ConcurrencyTester.Run(() =>
            {
                var testProxy = new InfusionTestProxy();
                var task = Task.Run(() => testProxy.Api.WaitForTarget(TimeSpan.MaxValue));
                testProxy.Api.WaitForTargetStartedEvent.AssertWaitOneSuccess();
                testProxy.ServerPacketHandler.HandlePacket(TargetCursorPackets.TargetCursor);

                task.AssertWaitFastSuccess();
            });
        }

        [TestMethod]
        public void Can_wait_for_target_after_last_action_When_TargetCursor_arrives_before_waiting_starts()
        {
            ConcurrencyTester.Run(() =>
            {
                var testProxy = new InfusionTestProxy();

                testProxy.Api.NotifyAction(DateTime.UtcNow.AddMilliseconds(-1));
                testProxy.ServerPacketHandler.HandlePacket(TargetCursorPackets.TargetCursor);

                bool waitResult = false;
                var task = Task.Run(() => waitResult = testProxy.Api.WaitForTarget(TimeSpan.MaxValue));

                task.AssertWaitFastSuccess();
                waitResult.Should().BeTrue();
            });
        }

        [TestMethod]
        public void Can_wait_for_target_after_last_action_When_TargetCursor_arrives_before_waiting_starts_and_failure_messages_are_specified()
        {
            ConcurrencyTester.Run(() =>
            {
                var testProxy = new InfusionTestProxy();

                testProxy.Api.NotifyAction(DateTime.UtcNow.AddMilliseconds(-1));
                testProxy.ServerPacketHandler.HandlePacket(TargetCursorPackets.TargetCursor);

                bool waitResult = false;
                var task = Task.Run(() =>
                    waitResult = testProxy.Api.WaitForTarget(TimeSpan.MaxValue, "some", "failure", "messages"));

                task.AssertWaitFastSuccess();
                waitResult.Should().BeTrue();
            });
        }

        [TestMethod]
        public void Timeouts_when_waiting_for_target_and_TargetCursor_arrives_before_last_action()
        {
            var testProxy = new InfusionTestProxy();

            testProxy.ServerPacketHandler.HandlePacket(TargetCursorPackets.TargetCursor);
            testProxy.Api.NotifyAction(DateTime.UtcNow.AddMilliseconds(1));

            var task = Task.Run(() => testProxy.Api.WaitForTarget(TimeSpan.MaxValue));

            task.Wait(100).Should().BeFalse();
        }

        [TestMethod]
        public void Can_terminate_before_target_because_fail_message_received()
        {
            ConcurrencyTester.Run(() =>
            {
                var testProxy = new InfusionTestProxy();

                testProxy.Api.NotifyAction(DateTime.UtcNow.AddMilliseconds(-1));

                testProxy.ServerPacketHandler.HandlePacket(SpeechPackets.FailureMessageFromServer);

                bool waitResult = true;
                var task = Task.Run(
                    () => waitResult = testProxy.Api.WaitForTarget(TimeSpan.MaxValue, "failure message"));
                task.AssertWaitFastSuccess();

                waitResult.Should().BeFalse();
            });
        }

        [TestMethod]
        public void WaitForTarget_resets_WaitTargetObject()
        {
            var testProxy = new InfusionTestProxy();

            testProxy.Api.WaitTargetObject(0x40001234);

            var task = Task.Run(() => testProxy.Api.WaitForTarget(TimeSpan.MaxValue));
            testProxy.Api.WaitForTargetStartedEvent.AssertWaitOneSuccess();
            testProxy.ServerPacketHandler.HandlePacket(TargetCursorPackets.TargetCursor);

            task.AssertWaitFastSuccess();

        }

        [TestMethod]
        public void AskForLocation_returns_location_selected_on_client()
        {
            var testProxy = new InfusionTestProxy();

            var task = Task.Run(() =>
            {
                var result = testProxy.Api.AskForLocation();
                result.HasValue.Should().BeTrue();
                result.Value.Location.Should().Be(new Location3D(0x09EC, 0x0CF9, 0));
            });

            testProxy.Api.AskForTargetStartedEvent.AssertWaitOneSuccess();
            testProxy.PacketReceivedFromClient(new Packet(0x6C, new byte[]
            {
                0x6C, 0x01, 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0xEC, 0x0C, 0xF9, 0x00,
                0x00, 0x00, 0x00,
            }));

            task.AssertWaitFastSuccess();
        }

        [TestMethod]
        public void Cancels_AskForLocation_When_server_requests_target()
        {
            ConcurrencyTester.Run(() =>
            {
                var testProxy = new InfusionTestProxy();

                var task = Task.Run(() => { testProxy.Api.AskForLocation().Should().BeNull(); });

                testProxy.Api.AskForTargetStartedEvent.AssertWaitOneSuccess();
                testProxy.PacketReceivedFromServer(new Packet(0x6C, new byte[]
                {
                    0x6C, 0x01, 0x00, 0x00, 0x00, 0x25, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00
                }));

                task.AssertWaitFastSuccess();
            });
        }

        [TestMethod]
        public void AskForLocation_waits_for_target_from_client_When_server_have_asked_for_target_before()
        {
            var testProxy = new InfusionTestProxy();

            testProxy.PacketReceivedFromClient(new Packet(0x6C, new byte[]
            {
                0x6C, 0x00, 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x00, 0x06, 0x39, 0x0E, 0x0A, 0x75, 0x0C, 0x92, 0x00,
                0x00, 0x01, 0x90,
            }));

            var task = Task.Run(() => { testProxy.Api.AskForLocation().Value.Location.Should().Be(new Location3D(0x09EC, 0x0CF9, 0)); });

            testProxy.Api.AskForTargetStartedEvent.AssertWaitOneSuccess();

            testProxy.PacketReceivedFromClient(new Packet(0x6C, new byte[]
            {
                0x6C, 0x01, 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0xEC, 0x0C, 0xF9, 0x00,
                0x00, 0x00, 0x00,
            }));

            task.AssertWaitFastSuccess();
        }

        [TestMethod]
        public void Cancels_AfkForItem_When_server_requests_target()
        {
            ConcurrencyTester.Run(() =>
            {
                var testProxy = new InfusionTestProxy();

                var task = Task.Run(() => { testProxy.Api.AskForItem().Should().BeNull(); });

                testProxy.Api.AskForTargetStartedEvent.AssertWaitOneSuccess();
                testProxy.PacketReceivedFromServer(new Packet(0x6C, new byte[]
                {
                    0x6C, 0x01, 0x00, 0x00, 0x00, 0x25, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00
                }));

                task.AssertWaitFastSuccess();
            });
        }

        [TestMethod]
        public void Cancels_AskForMobile_When_server_requests_target()
        {
            ConcurrencyTester.Run(() =>
            {
                var testProxy = new InfusionTestProxy();

                var task = Task.Run(() => { testProxy.Api.AskForMobile().Should().BeNull(); });

                testProxy.Api.AskForTargetStartedEvent.AssertWaitOneSuccess();
                testProxy.PacketReceivedFromServer(new Packet(0x6C, new byte[]
                {
                    0x6C, 0x01, 0x00, 0x00, 0x00, 0x25, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00
                }));

                task.AssertWaitFastSuccess();
            });
        }

        [TestMethod]
        public void AskForMobile_returns_null_When_client_cancels_target_and_some_target_was_already_selected()
        {
            var testProxy = new InfusionTestProxy();

            testProxy.PacketReceivedFromClient(new Packet(0x6C, new byte[]
            {
                0x6C, 0x00, 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x11, 0x12, 0x13, 0x14, 0x09, 0xEC, 0x0C, 0xF9, 0x00,
                0x00, 0x00, 0x00,
            }));

            var task = Task.Run(() => { testProxy.Api.AskForMobile().Should().BeNull(); });

            testProxy.Api.AskForTargetStartedEvent.AssertWaitOneSuccess();
            testProxy.PacketReceivedFromClient(new Packet(0x6C, new byte[]
            {
                0x6C, 0x01, 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
            }));

            task.AssertWaitFastSuccess();
        }

        [TestMethod]
        public void Can_set_future_autotargets()
        {
            ConcurrencyTester.Run(() =>
            {
                var testProxy = new InfusionTestProxy();
                var targetSet = new AutoResetEvent(false);

                var task = Task.Run(() =>
                {
                    testProxy.Api.WaitTargetObject(0x12345678, 0x87654321);
                    targetSet.Set();
                });

                targetSet.WaitOne();

                testProxy.PacketReceivedFromServer(new Packet(0x6C, new byte[]
                {
                    0x6C, 0x01, 0x00, 0x00, 0x00, 0x25, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00
                }));

                testProxy.PacketReceivedFromServer(new Packet(0x6C, new byte[]
                {
                    0x6C, 0x01, 0x00, 0x00, 0x00, 0x25, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00
                }));

                task.AssertWaitFastSuccess();

                testProxy.PacketsSentToServer.Count(x => x.Id == 0x6C).Should().Be(2);
            });
        }
    }
}
