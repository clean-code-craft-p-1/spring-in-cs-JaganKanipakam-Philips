using System;
using System.Collections.Generic;

namespace Statistics
{
    public class Stats
    {
        public double average;
        public double max;
        public double min;
    }

    public class StatsComputer
    {
        public Stats CalculateStatistics(List<double> numbers)
        {
            var stats = new Stats
            {
                average = double.NaN,
                max = double.NaN,
                min = double.NaN
            };

            if (numbers == null || numbers.Count == 0)
            {
                return stats;
            }

            double sum = 0;
            double max = double.NegativeInfinity;
            double min = double.PositiveInfinity;

            foreach (var number in numbers)
            {
                sum += number;
                if (number > max) max = number;
                if (number < min) min = number;
            }

            stats.average = sum / numbers.Count;
            stats.max = max;
            stats.min = min;

            return stats;
        }
    }
}
