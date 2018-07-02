using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace DiningPhilosophers
{
    [DebuggerDisplay("Name: {Name}, Forks: {_leftFork.ForkNumber}, {_rightFork.ForkNumber}")]
    class Philosopher : IDisposable
    {
        private readonly int _index;
        private readonly TimeSpan _thinkingTime;
        private readonly TimeSpan _eatingTime;
        private readonly Fork _leftFork;
        private readonly Fork _rightFork;
        private Random _random;
        private bool _dinnerInProgress = true;
        private Thread _thread;
        public string Name { get; }

        public static ConcurrentQueue<long> TimeQueue { get; } = new ConcurrentQueue<long>();

        public Philosopher(int index, string name, TimeSpan thinkingTime, TimeSpan eatingTime, Fork leftFork, Fork rightFork)
        {
            _index = index;
            _thinkingTime = thinkingTime;
            _eatingTime = eatingTime;
            _leftFork = leftFork;
            _rightFork = rightFork;
            Name = name;
            _random = new Random();
            _thread = new Thread(DinnerLoop);
            _thread.Start();
        }

        private void DinnerLoop()
        {
            Console.WriteLine($"{Name} arrived at the diningroom.");
            while (_dinnerInProgress)
            {
                Ponder();

                if (_index % 2 == 0)
                {
                    GrabRightFork();
                    GrabLeftFork();
                }
                else
                {
                    GrabLeftFork();
                    GrabRightFork();
                }
                Eat();
                ReleaseForks();
            }
        }

        void Ponder()
        {
            Console.WriteLine($"{Name} ponders for a while.");
            Thread.Sleep(_random.Next(0, (int) _thinkingTime.TotalMilliseconds));
            Console.WriteLine($"{Name} stopped thinking.");
        }

        void GrabLeftFork()
        {
            var watch = Stopwatch.StartNew();
            Monitor.Enter(_leftFork);
            watch.Stop();
            TimeQueue.Enqueue(watch.ElapsedMilliseconds);
            
            Console.WriteLine($"{Name} grabbed the left fork {_leftFork.ForkNumber}.");
        }

        void GrabRightFork()
        {
            var watch = Stopwatch.StartNew();
            Monitor.Enter(_rightFork);
            watch.Stop();
            TimeQueue.Enqueue(watch.ElapsedMilliseconds);
            Console.WriteLine($"{Name} grabbed the right fork {_rightFork.ForkNumber}.");
        }

        void Eat()
        {
            Console.WriteLine($"{Name} is eating.");
            Thread.Sleep(_random.Next(0, (int) _eatingTime.TotalMilliseconds));
            Console.WriteLine($"{Name} finished eating.");
        }

        void ReleaseForks()
        {
            if (_random.Next(0, 10) % 2 == 0)
            {
                PutDownLeftFork();
                PutDownRightFork();
            }
            else
            {
                PutDownRightFork();
                PutDownLeftFork();
            };
        }

        void PutDownLeftFork()
        {
            Monitor.Exit(_leftFork);
            Console.WriteLine($"{Name} put down the left fork {_leftFork.ForkNumber}.");
        }

        void PutDownRightFork()
        {
            Monitor.Exit(_rightFork);
            Console.WriteLine($"{Name} put down the right fork {_rightFork.ForkNumber}.");
        }


        private void ReleaseUnmanagedResources()
        {
            _dinnerInProgress = false;
            _thread.Join();
            Console.WriteLine($"{Name} left the diningroom.");
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Philosopher()
        {
            ReleaseUnmanagedResources();
        }
    }
}
