﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Infusion.TextFilters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infusion.Tests.TextFilters
{
    [TestClass]
    public class NameFilterTests
    {
        [TestMethod]
        public void Can_filter_out_name()
        {
            new NameFilter().IsPassing("Pipka: Pipka").Should().BeFalse();
        }

        [TestMethod]
        public void Can_filter_out_name_with_amount()
        {
            new NameFilter().IsPassing("copper wire: 41 copper wire").Should().BeFalse();
        }

        [TestMethod]
        public void Can_pass_normal_speech()
        {
            new NameFilter().IsPassing("Pipka: blablalba").Should().BeTrue();
        }

        [TestMethod]
        public void Can_pass_speech_starting_with_speaker_name()
        {
            new NameFilter().IsPassing("Pipka: Pipka blablalba").Should().BeTrue();
        }

        [TestMethod]
        public void Can_pass_speech_starting_with_speaker_name_and_is_longer_than_name_by_one_character()
        {
            new NameFilter().IsPassing("Pipka: Pipkaa").Should().BeTrue();
        }

        [TestMethod]
        public void Can_pass_speech_starting_with_number_without_name()
        {
            new NameFilter().IsPassing("copper wire: 41 blabla").Should().BeTrue();
        }

        [TestMethod]
        public void Can_pass_speech_with_number_only()
        {
            new NameFilter().IsPassing("copper wire: 41").Should().BeTrue();
        }

        [TestMethod]
        public void Can_pass_unstructured_text()
        {
            new NameFilter().IsPassing("blablalba").Should().BeTrue();
        }
    }
}