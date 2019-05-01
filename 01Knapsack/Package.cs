using System;

namespace _01Knapsack
{
    public class Package
    {
        public int Weight { get; set; }
        public int Value { get; set; }
        public double Fitness => Value / (double)Weight;

        public Package()
        {

        }
        public Package(int maxPackageWeight, double multiplier)
        {
            var weight = RandomGenerator.Random.Next(1, (int)Math.Round(multiplier * maxPackageWeight));
            Weight = weight;
            Value = (int)Math.Round(multiplier * weight);
        }
    }
}