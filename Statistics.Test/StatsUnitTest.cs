using System;
using System.Collections.Generic;
using Xunit;
using Statistics;

namespace Statistics.Test
{
    public class StatsUnitTest
    {
        // Human body temperatures are typically reported to one decimal place
        // (e.g. 98.6). A tolerance of 0.001 is well within sensor precision
        // and accounts for floating-point rounding.
        const float epsilon = 0.001F;

        [Fact]
        public void ReportsAverageMinMax()
        {
            var statsComputer = new StatsComputer();
            var computedStats = statsComputer.CalculateStatistics(
                new List<float>{98.6F, 98.2F, 97.8F, 102.2F});
            Assert.True(Math.Abs(computedStats.average - 99.2F) <= epsilon);
            Assert.True(Math.Abs(computedStats.max - 102.2F) <= epsilon);
            Assert.True(Math.Abs(computedStats.min - 97.8F) <= epsilon);
        }

        [Fact]
        public void ReportsNaNForEmptyInput()
        {
            var statsComputer = new StatsComputer();
            var computedStats = statsComputer.CalculateStatistics(
                new List<float>{});
            // All fields of computedStats (average, max, min) must be
            // float.NaN (not-a-number), as described in
            // https://docs.microsoft.com/en-us/dotnet/api/system.double.nan?view=netcore-3.1
            Assert.True(float.IsNaN(computedStats.average));
            Assert.True(float.IsNaN(computedStats.max));
            Assert.True(float.IsNaN(computedStats.min));
        }

        [Fact]
        public void ReportsNaNForNullInput()
        {
            // A disconnected IoT device may send no payload at all.
            var statsComputer = new StatsComputer();
            var computedStats = statsComputer.CalculateStatistics(null);
            Assert.True(float.IsNaN(computedStats.average));
            Assert.True(float.IsNaN(computedStats.max));
            Assert.True(float.IsNaN(computedStats.min));
        }

        [Fact]
        public void ReportsSameValueForSingleMeasurement()
        {
            // A device that has reported exactly one reading so far.
            var statsComputer = new StatsComputer();
            var computedStats = statsComputer.CalculateStatistics(
                new List<float>{98.6F});
            Assert.True(Math.Abs(computedStats.average - 98.6F) <= epsilon);
            Assert.True(Math.Abs(computedStats.max - 98.6F) <= epsilon);
            Assert.True(Math.Abs(computedStats.min - 98.6F) <= epsilon);
        }

        [Fact]
        public void HandlesIdenticalMeasurements()
        {
            // A sensor stuck at a constant reading.
            var statsComputer = new StatsComputer();
            var computedStats = statsComputer.CalculateStatistics(
                new List<float>{99.0F, 99.0F, 99.0F});
            Assert.True(Math.Abs(computedStats.average - 99.0F) <= epsilon);
            Assert.True(Math.Abs(computedStats.max - 99.0F) <= epsilon);
            Assert.True(Math.Abs(computedStats.min - 99.0F) <= epsilon);
        }

        [Fact]
        public void HandlesSubZeroMeasurements()
        {
            // An ambient/skin sensor exposed to freezing conditions can
            // legitimately report negative temperatures.
            var statsComputer = new StatsComputer();
            var computedStats = statsComputer.CalculateStatistics(
                new List<float>{-5.0F, 0.0F, 5.0F});
            Assert.True(Math.Abs(computedStats.average - 0.0F) <= epsilon);
            Assert.True(Math.Abs(computedStats.max - 5.0F) <= epsilon);
            Assert.True(Math.Abs(computedStats.min - (-5.0F)) <= epsilon);
        }

        [Fact]
        public void HandlesLargeStreamOfMeasurements()
        {
            // IoT devices stream continuously; ensure a large batch is
            // aggregated correctly. Constant 100.0 keeps the expectation exact.
            var measurements = new List<float>();
            for (int i = 0; i < 100000; i++)
            {
                measurements.Add(100.0F);
            }
            var statsComputer = new StatsComputer();
            var computedStats = statsComputer.CalculateStatistics(measurements);
            Assert.True(Math.Abs(computedStats.average - 100.0F) <= epsilon);
            Assert.True(Math.Abs(computedStats.max - 100.0F) <= epsilon);
            Assert.True(Math.Abs(computedStats.min - 100.0F) <= epsilon);
        }
    }
}
