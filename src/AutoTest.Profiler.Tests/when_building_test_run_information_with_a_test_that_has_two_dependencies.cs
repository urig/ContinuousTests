﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AutoTest.Profiler.Tests
{
    [TestFixture]
    public class when_building_test_run_information_with_a_test_that_has_two_dependencies
    {
        private List<TestRunInformation> items;

        [SetUp]
        public void Setup()
        {
            var assembler = new TestRunInfoAssembler();
            var entries = new[]
                              {
                                  new ProfilerEntry {Type = ProfileType.Enter, Functionid = 1, Method = "Test", Runtime = "Test", Sequence = 1, IsTest = true},
                                  new ProfilerEntry {Type = ProfileType.Enter, Functionid = 2, Method = "Method1", Runtime = "RMethod1", Sequence = 2},
                                  new ProfilerEntry {Type = ProfileType.Leave, Functionid = 2, Method = "Method1", Runtime = "RMethod1", Sequence = 3},
                                  new ProfilerEntry {Type = ProfileType.Enter, Functionid = 3, Method = "Method2", Runtime = "RMethod2", Sequence = 4},
                                  new ProfilerEntry {Type = ProfileType.Leave, Functionid = 3, Method = "Method2", Runtime = "RMethod2", Sequence = 5},
                                  new ProfilerEntry {Type = ProfileType.Leave, Functionid = 1, Method = "Test", Runtime = "Test", Sequence = 6}
                              };
            items = assembler.Assemble(entries).ToList();
        }

        [Test]
        public void a_single_test_information_is_created()
        {
            Assert.AreEqual(1, items.Count);
        }

        [Test]
        public void the_test_chain_has_first_dependency_in_order()
        {
            Assert.AreEqual("RMethod1", items[0].TestChain.Children.ToList()[0].Name);
        }

        [Test]
        public void the_test_chain_has_second_dependency_in_order()
        {
            Assert.AreEqual("RMethod2", items[0].TestChain.Children.ToList()[1].Name);
        }

        [Test]
        public void the_test_chain_has_no_children_after_first_dependency()
        {
            Assert.AreEqual(0, items[0].TestChain.Children.ToList()[0].Children.Count());
        }

        [Test]
        public void the_test_chain_has_no_children_after_second_dependency()
        {
            Assert.AreEqual(0, items[0].TestChain.Children.ToList()[1].Children.Count());
        }
    }
}