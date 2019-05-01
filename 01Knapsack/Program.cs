using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01Knapsack
{
    class Program
    {
        public class Result
        {
            public double Fitness { get; set; }
            public int Weight { get; set; }
            public string BitString { get; set; }

            public Result(double fitness, int weight, string bitString)
            {
                Fitness = fitness;
                Weight = weight;
                BitString = bitString;
            }
        }
        static void Main(string[] args)
        {
            var Pc = 0.8;
            var Pm = 0.1;
            var maxWeight = 50;
            var penaltyMethod = PenaltyMethod.Static;

            while (true)
            {

                Console.WriteLine("Press ENTER to Run:");
                Console.ReadLine();

                var bestOfRunFitness = new List<double>();
                var bestOfRuns = new List<Knapsack>();

                for (int x = 0; x < 10; x++)
                {

                    var run = new GeneticAlgorithm(maxWeight, Pc, Pm, penaltyMethod);

                    GenerateGenerations(run);
                    bestOfRunFitness.Add(run.BestOfRunFitness);
                    bestOfRuns.Add(run.BestOfRun);
                    Console.WriteLine(run.BestOfRun.BitString);
                    //foreach (var package in run.Packages)
                    //{
                    //    Console.WriteLine(package.Fitness);
                    //}
                }

                var max = bestOfRunFitness.Max();
                var avg = bestOfRunFitness.Average();
                Console.WriteLine($"Penalty Method: {penaltyMethod}");
                Console.WriteLine($"Max Fitness: {max}");
                Console.WriteLine($"Avg Fitness: {avg}");
                Console.WriteLine($"StDev of Fitness: {StandDev(bestOfRunFitness)}");

            }

        }

        private static void GenerateGenerations(GeneticAlgorithm run)
        {
            for (int i = 0; i < run.GenerationsPerRun - 1; i++)
            {
                run.AddNewGeneration();
            }
        }

        public static double StandDev(List<double> values)
        {
            double ret = 0;
            var count = values.Count;
            if (count > 1)
            {
                double avg = values.Average();
                double sum = values.Sum(d => (d - avg) * (d - avg));
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }
    }
}
