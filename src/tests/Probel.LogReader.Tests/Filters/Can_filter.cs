using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Plugins.Debug;
using System;
using System.Linq;
using Xunit;

namespace Probel.LogReader.TestCases.Filters
{
    public class Can_filter
    {
        #region Methods

        [Fact]
        public void On_category_exclusive()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new CategoryFilter() { Operator = "not in", Operand = Guid.NewGuid().ToString() };

            ((DebugPlugin)plugin).SetCategory("category");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() > 0);
            }
        }
        [Fact]
        public void On_category_exclusive_no_result_case_insensitive()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new CategoryFilter() { Operator = "not in", Operand = "category" };

            ((DebugPlugin)plugin).SetCategory("cAtEgOrY");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }


        [Fact]
        public void On_category_exclusive_no_result_multiple()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new CategoryFilter() { Operator = "not in", Operand = "blahblah, CATEGORY " };

            ((DebugPlugin)plugin).SetCategory("cAtEgOrY");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }

        [Fact]
        public void On_category_exclusive_no_result()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new CategoryFilter() { Operator = "not in", Operand = "category" };

            ((DebugPlugin)plugin).SetCategory("category");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }

        [Fact]
        public void On_category_inclusive()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new CategoryFilter() { Operator = "in", Operand = "category" };

            ((DebugPlugin)plugin).SetCategory("category");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() > 0);
            }
        }

        [Fact]
        public void On_category_inclusive_no_result()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new CategoryFilter() { Operator = "in", Operand = Guid.NewGuid().ToString() };

            ((DebugPlugin)plugin).SetCategory("category");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }

        [Fact]
        public void On_level_exclusive()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new LevelFilter() { Operator = "not in", Operand = Guid.NewGuid().ToString() };

            ((DebugPlugin)plugin).SetLevel("level");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() > 0);
            }
        }

        [Fact]
        public void On_level_exclusive_no_result_case_insensitive()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new LevelFilter() { Operator = "not in", Operand = "level" };

            ((DebugPlugin)plugin).SetLevel("lEvEl");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }

        [Fact]
        public void On_level_exclusive_no_result()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new LevelFilter() { Operator = "not in", Operand = "category" };

            ((DebugPlugin)plugin).SetLevel("category");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }

        [Fact]
        public void On_level_inclusive()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new LevelFilter() { Operator = "in", Operand = "category" };

            ((DebugPlugin)plugin).SetLevel("category");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() > 0);
            }
        }

        [Fact]
        public void On_level_inclusive_no_result()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new LevelFilter() { Operator = "in", Operand = Guid.NewGuid().ToString() };

            ((DebugPlugin)plugin).SetLevel("category");
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }

        [Fact]
        public void On_time_greater_or_equal()
        {
            PluginBase plugin = new DebugPlugin();
            var now = DateTime.Now;
            ((DebugPlugin)plugin)
                .Clear()
                .Add(new LogRow()
                {
                    Level = Guid.NewGuid().ToString(),
                    Logger = Guid.NewGuid().ToString(),
                    Time = now.AddMinutes(10)
                }, new LogRow()
                {
                    Level = Guid.NewGuid().ToString(),
                    Logger = "group",
                    Time = now.AddMinutes(1)
                }, new LogRow()
                {
                    Level = "debug",
                    Logger = Guid.NewGuid().ToString(),
                    Time = now.AddMinutes(1)
                }).Commit();

            IFilterExpression filter1 = new TimeFilter() { Operator = ">=", Operand = "10" };

            var days = plugin.GetDays();
            Assert.Single(days);

            var logs = plugin.GetLogs(days.ElementAt(0));
            
            logs = filter1.Filter(logs);
            Assert.Single(logs);
        }

        [Fact]
        public void On_time_greater_than()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new TimeFilter() { Operator = ">", Operand = "15" };

            ((DebugPlugin)plugin).AddMinutes(100);
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() > 0);
            }
        }

        [Fact]
        public void On_time_greater_than_no_result()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new TimeFilter() { Operator = ">", Operand = "15" };

            ((DebugPlugin)plugin).AddMinutes(1);
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }

        [Fact]
        public void On_time_greater_than_or_equal_no_result()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new TimeFilter() { Operator = ">=", Operand = "15" };

            ((DebugPlugin)plugin).AddMinutes(1);
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }

        [Fact]
        public void On_time_less_or_equal()
        {
            PluginBase plugin = new DebugPlugin();
            var now = DateTime.Now;
            ((DebugPlugin)plugin)
                .Clear()
                .Add(new LogRow()
                {
                    Level = Guid.NewGuid().ToString(),
                    Logger = Guid.NewGuid().ToString(),
                    Time = now.AddMinutes(10)
                }, new LogRow()
                {
                    Level = Guid.NewGuid().ToString(),
                    Logger = "group",
                    Time = now.AddMinutes(60)
                }, new LogRow()
                {
                    Level = "debug",
                    Logger = Guid.NewGuid().ToString(),
                    Time = now.AddMinutes(60)
                }).Commit();

            IFilterExpression filter1 = new TimeFilter() { Operator = "<=", Operand = "10" };

            var days = plugin.GetDays();
            Assert.Single(days);

            var logs = plugin.GetLogs(days.ElementAt(0));
            logs = filter1.Filter(logs).ToList();
            Assert.Single(logs);
        }

        [Fact]
        public void On_time_less_or_equal_on_multiple_days()
        {
            PluginBase plugin = new DebugPlugin();
            var now = DateTime.Now;
            ((DebugPlugin)plugin)
                .Clear()
                .Add(new LogRow()
                {
                    Level = Guid.NewGuid().ToString(),
                    Logger = Guid.NewGuid().ToString(),
                    Time = now.AddMinutes(10)
                }, new LogRow()
                {
                    Level = Guid.NewGuid().ToString(),
                    Logger = "group",
                    Time = now.AddDays(-2)
                }, new LogRow()
                {
                    Level = "debug",
                    Logger = Guid.NewGuid().ToString(),
                    Time = now.AddDays(-10)
                }).Commit();

            IFilterExpression filter1 = new TimeFilter() { Operator = "<=", Operand = "10" };

            var days = plugin.GetDays();
            var logs = plugin.GetLogs(days.ElementAt(0));
            logs = filter1.Filter(logs).ToList();
            Assert.Single(logs);
        }

        [Fact]
        public void On_time_less_than()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new TimeFilter() { Operator = "<", Operand = "15" };

            ((DebugPlugin)plugin).AddMinutes(1);
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() > 0);
            }
        }

        [Fact]
        public void On_time_less_than_or_equal()
        {
            PluginBase plugin = new DebugPlugin();
            IFilterExpression filter = new TimeFilter() { Operator = "<=", Operand = "15" };

            ((DebugPlugin)plugin).AddMinutes(1);
            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() > 0);
            }
        }

        [Fact]
        public void With_composite_AND_Can_filter()
        {
            PluginBase plugin = new DebugPlugin();

            ((DebugPlugin)plugin).AddMinutes(1);
            ((DebugPlugin)plugin).SetCategory("group");
            ((DebugPlugin)plugin).SetLevel("debug");

            IFilterExpression filter1 = new TimeFilter() { Operator = "<=", Operand = "15" };
            IFilterExpression filter2 = new CategoryFilter() { Operator = "in", Operand = "group" };
            IFilterExpression filter3 = new LevelFilter() { Operator = "in", Operand = "debug" };

            IFilterComposite filter = new AndFilterComposite();
            filter.Add(filter1, filter2, filter3);

            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() > 0);
            }
        }

        [Fact]
        public void With_composite_AND_no_result_with_filter_that_has_results()
        {
            PluginBase plugin = new DebugPlugin();

            ((DebugPlugin)plugin).AddMinutes(100);
            ((DebugPlugin)plugin).SetCategory("group");
            ((DebugPlugin)plugin).SetLevel("debug");

            IFilterExpression filter1 = new TimeFilter() { Operator = "<=", Operand = "15" };
            IFilterExpression filter2 = new CategoryFilter() { Operator = "in", Operand = "group" };
            IFilterExpression filter3 = new LevelFilter() { Operator = "in", Operand = "debug" };

            IFilterComposite filter = new AndFilterComposite();
            filter.Add(filter1, filter2, filter3);

            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0, $"Found '{logs.Count()}' logs");
            }
        }

        [Fact]
        public void With_composite_AND_filter_no_result()
        {
            PluginBase plugin = new DebugPlugin();

            ((DebugPlugin)plugin).AddMinutes(100);
            ((DebugPlugin)plugin).SetCategory(Guid.NewGuid().ToString());
            ((DebugPlugin)plugin).SetLevel(Guid.NewGuid().ToString());

            IFilterExpression filter1 = new TimeFilter() { Operator = "<=", Operand = "15" };
            IFilterExpression filter2 = new CategoryFilter() { Operator = "in", Operand = "group" };
            IFilterExpression filter3 = new LevelFilter() { Operator = "in", Operand = "debug" };

            IFilterComposite filter = new AndFilterComposite();
            filter.Add(filter1, filter2, filter3);

            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                logs = filter.Filter(logs);
                Assert.True(logs.Count() == 0);
            }
        }

        [Fact]
        public void With_composite_OR_Can_filter()
        {
            PluginBase plugin = new DebugPlugin();
            var now = DateTime.Now;
            ((DebugPlugin)plugin)
                .Clear()
                .Add(new LogRow()
                {
                    Level = Guid.NewGuid().ToString(),
                    Logger = Guid.NewGuid().ToString(),
                    Time = now.AddMinutes(10)
                }, new LogRow()
                {
                    Level = Guid.NewGuid().ToString(),
                    Logger = "group",
                    Time = now.AddMinutes(100)
                }, new LogRow()
                {
                    Level = "debug",
                    Logger = Guid.NewGuid().ToString(),
                    Time = now.AddMinutes(100)
                }).Commit();

            IFilterExpression filter1 = new TimeFilter() { Operator = ">=", Operand = "9" };
            IFilterExpression filter2 = new CategoryFilter() { Operator = "in", Operand = "group" };
            IFilterExpression filter3 = new LevelFilter() { Operator = "in", Operand = "debug" };

            IFilterComposite filter = new OrFilterComposite();
            filter.Add(filter1, filter2, filter3);

            var days = plugin.GetDays();
            Assert.Single(days);

            var logs = plugin.GetLogs(days.ElementAt(0));
            logs = filter.Filter(logs);
            Assert.Equal(3, logs.Count());
        }

        #endregion Methods
    }
}