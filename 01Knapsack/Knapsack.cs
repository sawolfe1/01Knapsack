using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Threading;

namespace _01Knapsack
{
    public class Knapsack
    {
        public int MaxCapacity { get; set; }
        public int MaxWeight { get; set; }
        public string BitString { get; set; }
        public List<Package> Packages { get; set; }


        public Knapsack(int maxCapacity, int maxWeight)
        {
            MaxCapacity = maxCapacity;
            MaxWeight = maxWeight;
            //Packages = packages;

            var sb = new StringBuilder(MaxCapacity);
            for (int i = 0; i < MaxCapacity; i++)
            {
                var random = RandomGenerator.Random.NextDouble();
                if (random <= 0.5)
                    sb.Append('0');
                else
                {
                    sb.Append('1');
                }
                //sb.Append(random < 0.5);
            }

            BitString = sb.ToString();
        }

        public int GetTotalValue(List<Package> packages)
        {
            var totalValue = 0;
            for (var i = 0; i < MaxCapacity; i++)
            {
                var package = packages[i];
                if (BitString[i] == '1')
                {
                    totalValue += package.Value;
                }

            }

            return totalValue;
        }

        public int GetTotalWeight(List<Package> packages)
        {
            var totalWeight = 0;
            for (var i = 0; i < MaxCapacity; i++)
            {
                var package = packages[i];
                if (BitString[i] == '1')
                {
                    totalWeight += package.Weight;
                }

            }
            return totalWeight;
        }
    }
}