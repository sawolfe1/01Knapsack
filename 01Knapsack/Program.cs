using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01Knapsack
{
    class Program
    {
        static void Main(string[] args)
        {
            var N = 10;
            var Pc = 0.8;
            var Pm = 0.1;
            var maxWeight = 20;

            while (true)
            {

                Console.WriteLine("Press ENTER to Run Again:");
                Console.ReadLine();

                var bestOfRunFitness = new List<double>();

                for (int x = 0; x < 31; x++)
                {

                    var run = new GeneticAlgorithm(maxWeight, Pc, Pm);

                    GenerateGenerations(run);
                    bestOfRunFitness.Add(run.GetKnapsackFitness(run.BestOfRun));
                    //Console.WriteLine(run.BestOfRun.BitString);
                }

                Console.WriteLine(bestOfRunFitness.Max());
            }

        }

        private static void GenerateGenerations(GeneticAlgorithm run)
        {
            for (int i = 0; i < run.GenerationsPerRun; i++)
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
