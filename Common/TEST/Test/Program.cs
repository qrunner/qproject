using System;
using System.Collections.Generic;
using System.Threading;
using Common.DPL;
using Common.DPL.InMemory;
using Common.DPL.ThreadParallel;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IRunner runner = new ActionRunner(() => Console.WriteLine(DateTime.Now), Timeout.Infinite)
            {
                ThreadsCount = 3
            };
            runner.Start();            
            Console.ReadLine();
            runner.Stop();
            
            /*
            IRunner runner = new ThreadQueueSystem<string>(new Pipeline<string>(100), Readline, b => Console.WriteLine("Entered: {0}", b.Object), 10, 1)
            {
                ListenerParallelizm = 3,
                ProcessorParallelizm = 3
            };
            runner.Start();
            */
        }

        private static IEnumerable<string> Readline()
        {
            yield return Console.ReadLine();
        }
    }
}