using System;
using System.Collections.Generic;
using System.Linq;

namespace _01Knapsack
{
    public enum PenaltyMethod
    {
        Static,
        Adaptive
    }
    public class GeneticAlgorithm
    {
        public PenaltyMethod PenaltyMethod { get; set; }
        public int RunNumber { get; set; }
        public int PopulationSize { get; set; } = 10;
        public int GenerationsPerRun { get; set; } = 100;
        public Knapsack BestOfRun { get; set; }
        public double BestOfRunFitness { get; set; }

        public List<Knapsack> CurrentGeneration { get; set; }
        public List<List<Knapsack>> Generations { get; set; } = new List<List<Knapsack>>();

        public List<Package> Packages { get; set; } = new List<Package>();

        public double MutationRate;
        public double CrossoverRate;

        private readonly int _maxKnapsackWeight;
        private readonly int _knapsackCapacity;

        public GeneticAlgorithm(int maxKnapsackWeight, double crossoverRate, double mutationRate, PenaltyMethod penaltyMethod)
        {
            RunNumber = 0;
            PenaltyMethod = penaltyMethod;
            _maxKnapsackWeight = maxKnapsackWeight;
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            _knapsackCapacity = 20;
            CurrentGeneration = new List<Knapsack>();
            GetPreDefinedPackages();
            //GetRandomPackages();
            InitializePopulation();
        }

        public Knapsack Select()
        {
            var parent1 = BinaryTournament(CurrentGeneration);
            var parent2 = BinaryTournament(CurrentGeneration);

            return GetKnapsackFitness(parent1) > GetKnapsackFitness(parent2) ? parent1 : parent2;
        }

        public Knapsack Crossover(Knapsack parent1, Knapsack parent2)
        {
            var random = RandomGenerator.Random.NextDouble();
            var child = new Knapsack(_knapsackCapacity, _maxKnapsackWeight);

            if (random <= CrossoverRate)
            {
                var crossoverPoint = RandomGenerator.Random.Next(1, 20);
                var endPoint = _knapsackCapacity - crossoverPoint;
                child.BitString = $"{parent1.BitString.Substring(0, crossoverPoint)}{parent2.BitString.Substring(crossoverPoint, endPoint)}";

                return child;
            }

            return GetKnapsackFitness(parent1) > GetKnapsackFitness(parent2) ? parent1 : parent2;
        }

        public Knapsack Mutate(Knapsack candidate)
        {
            var bitString = "";
            foreach (var bit in candidate.BitString)
            {
                var random = RandomGenerator.Random.NextDouble();
                if (random <= MutationRate)
                    bitString += bit == '1' ? '0' : '1';
                else
                {
                    bitString += bit;
                }
            }

            candidate.BitString = bitString;
            return candidate;
        }

        private Knapsack BinaryTournament(List<Knapsack> candidates)
        {
            var rand = RandomGenerator.Random.Next(0, PopulationSize);
            var rand2 = RandomGenerator.Random.Next(0, PopulationSize);
            var parent = GetKnapsackFitness(candidates[rand]) > GetKnapsackFitness(candidates[rand2]) ?
                candidates[rand] : candidates[rand2];

            return parent;
        }

        private void InitializePopulation()
        {
            for (var i = 0; i < PopulationSize; i++)
            {
                CurrentGeneration.Add(new Knapsack(_knapsackCapacity, _maxKnapsackWeight));
            }

            BestOfRun = CurrentGeneration[0];

            foreach (var knapsack in CurrentGeneration)
            {
                var knapsackFitness = GetKnapsackFitness(knapsack);
                var bestKnapsackFitness = GetKnapsackFitness(BestOfRun);
                if (knapsackFitness > bestKnapsackFitness)
                {
                    BestOfRun = knapsack;
                    BestOfRunFitness = knapsackFitness;
                }
            }

            Generations.Add(CurrentGeneration);
        }

        public void GetPreDefinedPackages()
        {
            for (var i = 1; i < _knapsackCapacity + 1; i++)
            {
                Packages.Add(new Package
                {
                    Value = i,
                    Weight = i * _maxKnapsackWeight + 5
                });
            }
        }

        public void GetRandomPackages()
        {
            const double num = 1.6180339887d;
            for (var i = 0; i < _knapsackCapacity; i++)
            {
                Packages.Add(new Package(_maxKnapsackWeight, num));
            }
        }

        public double GetKnapsackFitness(Knapsack knapsack)
        {
            var totalWeight = (double)knapsack.GetTotalWeight(Packages);
            var totalValue = (double)knapsack.GetTotalValue(Packages);

            var fitness = 0d;

            if (totalWeight <= _maxKnapsackWeight)
                return totalValue;

            switch (PenaltyMethod)
            {
                case PenaltyMethod.Static:
                    fitness = 0;
                    break;
                case PenaltyMethod.Adaptive:
                    double penalty = 1 / (totalWeight - _maxKnapsackWeight);
                    fitness = totalValue * penalty - 1;
                    break;
            }

            return fitness;
        }

        public void AddNewGeneration()
        {
            var newGeneration = new List<Knapsack>();

            for (int i = 0; i < PopulationSize; i++)
            {
                var candidate = Mutate(Crossover(Select(), Select()));

                newGeneration.Add(candidate);

                var knapsackFitness = GetKnapsackFitness(candidate);
                var bestKnapsackFitness = GetKnapsackFitness(BestOfRun);
                if (knapsackFitness > bestKnapsackFitness)
                {
                    BestOfRun = candidate;
                    BestOfRunFitness = knapsackFitness;
                }
            }

            //Console.WriteLine(RunNumber);
            RunNumber++;
            //Console.WriteLine(BestOfRunFitness);
            CurrentGeneration = newGeneration;
            Generations.Add(newGeneration);
        }

    }
}