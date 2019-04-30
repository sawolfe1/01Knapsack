using System;
using System.Collections.Generic;
using System.Linq;

namespace _01Knapsack
{
    public class GeneticAlgorithm
    {
        public int PopulationSize { get; set; } = 20;
        public int GenerationsPerRun { get; set; } = 20;
        public Knapsack BestOfRun { get; set; }

        public List<Knapsack> CurrentGeneration { get; set; }
        public List<List<Knapsack>> Generations { get; set; } = new List<List<Knapsack>>();

        public List<Package> Packages { get; set; } = new List<Package>();

        public double MutationRate;
        public double CrossoverRate;

        private readonly int _maxKnapsackWeight;
        private readonly int _knapsackCapacity;

        public GeneticAlgorithm(int maxKnapsackWeight, double crossoverRate, double mutationRate)
        {
            _maxKnapsackWeight = maxKnapsackWeight;
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            _knapsackCapacity = 20;
            CurrentGeneration = new List<Knapsack>();
            GetRandomPackages();
            InitializePopulation();
        }

        public Knapsack Select()
        {
            var parent1 = BinaryTournament(CurrentGeneration);
            var parent2 = BinaryTournament(CurrentGeneration);

            return GetKnapsackFitness(parent1) < GetKnapsackFitness(parent2) ? parent1 : parent2;
        }

        public Knapsack Crossover(Knapsack parent1, Knapsack parent2)
        {
            var random = RandomGenerator.Random.NextDouble();
            var child = new Knapsack(_knapsackCapacity, _maxKnapsackWeight);

            if (random >= CrossoverRate)
            {
                var middle = _knapsackCapacity / 2;
                child.BitString = $"{parent1.BitString.Substring(0, middle)}{parent2.BitString.Substring(middle, middle)}";

                return child;
            }

            var split = 3;
            var par1 = parent1.BitString.Substring(0, split);
            var par2 = parent2.BitString.Substring(split, _knapsackCapacity - split);

            child.BitString = par1 + par2; //"10100011101010101001";

            return child;

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
            var parent = GetKnapsackFitness(candidates[rand]) < GetKnapsackFitness(candidates[rand2]) ?
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
                if (GetKnapsackFitness(knapsack) > GetKnapsackFitness(BestOfRun))
                    BestOfRun = knapsack;
            }

            Generations.Add(CurrentGeneration);
        }

        public void GetPreDefinedPackages()
        {
            for (var i = 0; i < _knapsackCapacity; i++)
            {
                Packages.Add(new Package
                {
                    Value = i + 1,
                    Weight = 2 * i
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

            if (totalWeight <= _maxKnapsackWeight)
                return totalValue;

            return totalValue / totalWeight;
        }

        public void AddNewGeneration()
        {
            var newGeneration = new List<Knapsack>();

            for (int i = 0; i < PopulationSize; i++)
            {
                var candidate = Mutate(Crossover(Select(), Select()));

                newGeneration.Add(candidate);

                if (GetKnapsackFitness(candidate) > GetKnapsackFitness(BestOfRun))
                {
                    BestOfRun = candidate;
                }
            }

            CurrentGeneration = newGeneration;
            Generations.Add(newGeneration);
        }

    }
}