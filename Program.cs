using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    class Program
    {
        static void Main(string[] args)
        {
            UserManager um = new UserManager();
            var users = um.InitializeAll(1500);

            int counter = 0;
            Stopwatch sw = new Stopwatch();
            
            List<User> firstPart = users.Take(500).ToList();
            List<User> secondPart = users.Skip(500).Take(500).ToList();
            List<User> thirdPart = users.Skip(1000).Take(500).ToList();
            
            Thread firstTr = new Thread(() => um.DoWork(firstPart, CancellationToken.None));
            Thread secondTr = new Thread(() => um.DoWork(secondPart, CancellationToken.None));
            Thread thirdTr = new Thread(() => um.DoWork(thirdPart, CancellationToken.None));

            sw.Start();
            
            #region Sync Test
            
            um.DoWork(users, CancellationToken.None); // 00:00:23.4344743
            Console.WriteLine($"\nSYNC TIME: {sw.Elapsed}\n");
            
            #endregion
            
            sw.Restart();

            #region Thread test

            firstTr.Start();
            secondTr.Start();
            thirdTr.Start();
            
            firstTr.Join();
            secondTr.Join();
            thirdTr.Join();
            Console.WriteLine($"\nTHREAD TIME: {sw.Elapsed}\n"); // 00:00:07.8620426

            #endregion
            
            sw.Restart();

            #region Parallel method test

            um.DoWorkParallel(users, CancellationToken.None); // 00:00:03.2248416
            Console.WriteLine($"\nPARALLEL METHOD TIME: {sw.Elapsed}\n");

            #endregion
           
            sw.Restart();

            #region Parallel invoke test
            
            Parallel.Invoke(() =>
            {
                um.DoWork(firstPart, CancellationToken.None);
                Interlocked.Increment(ref counter);
            }, () =>
            {
                um.DoWork(secondPart, CancellationToken.None);
                Interlocked.Increment(ref counter);
            }, () =>
            {
                um.DoWork(thirdPart, CancellationToken.None);
                Interlocked.Increment(ref counter);
            });  // 00:00:07.8507226
            
            while (true)
            {
                if (counter == 3)
                {
                    Console.WriteLine($"\nPARALLEL FOREACH TIME: {sw.Elapsed}\n");
                    sw.Stop();
                    break;
                }
            }
            
            #endregion
        }
    }
}