using System;
using System.Collections.Generic;

namespace Statistics
{
    public class Stats
    {
        public float average;
        public float max;
        public float min;
    }

    public class StatsComputer
    {
        public Stats CalculateStatistics(List<float> numbers)
        {
            var stats = new Stats
            {
                average = float.NaN,
                max = float.NaN,
                min = float.NaN
            };

            if (numbers == null || numbers.Count == 0)
            {
                return stats;
            }

            float sum = 0;
            float max = float.NegativeInfinity;
            float min = float.PositiveInfinity;

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
