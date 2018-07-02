using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DiningPhilosophers
{
    internal class Program
    {
        private static readonly List<string> _philosopherNames = new List<string>
        {
            "Aristotle",
            "Kant",
            "Hegel",
            "Marx",
            "Marcus Aurelius",
            "Wittgenstein",
            "Plato",
            "Socrates",
            "Cicero",
            "Galileo",
            "Voltaire",
            "Rousseau",
            "Schopenhauer",
            "Heidegger",
            "Nietzsche"
        };

        private static void Main(string[] args)
        {
            var numberOfPhilosophers = GetNumberOfPhilosophers();
            var thinkingTime = GetThinkingTime();
            var eatingTime = GetEatingTime();

            AddAdditionalNames(numberOfPhilosophers);

            var forks = Enumerable.Range(1, numberOfPhilosophers).Select(_ => new Fork(_)).ToList();

            var watch = Stopwatch.StartNew();
            var philosophers = Enumerable.Range(0, numberOfPhilosophers - 1).Select(index =>
                    new Philosopher(index + 1, _philosopherNames[index], thinkingTime, eatingTime, forks[index],
                        forks[index + 1]))
                .ToList();

            philosophers.Add(new Philosopher(numberOfPhilosophers, _philosopherNames[numberOfPhilosophers - 1],
                thinkingTime, eatingTime, forks[numberOfPhilosophers - 1], forks[0]));


            Console.ReadLine();

            philosophers.ForEach(_ => _.Dispose());

            watch.Stop();
            Console.WriteLine($"Total Runtime: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Average waited time: {Philosopher.TimeQueue.Sum() / numberOfPhilosophers} ms");

            Console.ReadLine();
        }

        private static TimeSpan GetThinkingTime()
        {
            while (true)
            {
                Console.Write("Enter the maximum thinking time in milliseconds: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out var thinkingTime))
                {
                    return TimeSpan.FromMilliseconds(thinkingTime);
                }
            }
        }

        private static TimeSpan GetEatingTime()
        {
            while (true)
            {
                Console.Write("Enter the maximum eating time in milliseconds: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out var eatingTime))
                {
                    return TimeSpan.FromMilliseconds(eatingTime);
                }
            }
        }

        private static int GetNumberOfPhilosophers()
        {
            while (true)
            {
                Console.Write("Enter the number of philosophers: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out var numberOfPhilosophers))
                {
                    return numberOfPhilosophers;
                }
            }

        }

        private static void AddAdditionalNames(int numberOfPhilosophers)
        {
            if (numberOfPhilosophers > _philosopherNames.Count)
                _philosopherNames.AddRange(Enumerable.Range(1, numberOfPhilosophers - _philosopherNames.Count)
                    .Select(i => $"Philosopher {i}"));
        }
    }
}