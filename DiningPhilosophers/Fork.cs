using System.Diagnostics;

namespace DiningPhilosophers
{
    [DebuggerDisplay("Fork: {ForkNumber}")]
    class Fork
    {
        public int ForkNumber { get; }

        public Fork(int forkNumber)
        {
            ForkNumber = forkNumber;
        }
    }
}
