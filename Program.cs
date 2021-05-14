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
            
            
            sw.Start();
            um.DoWork(users, CancellationToken.None); // 00:00:23.3498619
            Console.WriteLine($"\nSYNC TIME: {sw.Elapsed}\n");
            sw.Restart();
            
            sw.Start();
            Thread t = new Thread(() => um.DoWork(users, CancellationToken.None)); // 00:00:23.4280668
            t.Start();
            t.Join();
            Console.WriteLine($"\nTHREAD TIME: {sw.Elapsed}\n");
            sw.Restart();
            
            sw.Start();
            um.DoWorkParallel(users, CancellationToken.None); // 00:00:02.7757682
            Console.WriteLine($"\nPARALLEL FOREACH TIME: {sw.Elapsed}\n");
            sw.Restart();
            
            sw.Start();
            Parallel.Invoke(() =>
            {
                um.DoWork(users, CancellationToken.None);
                Interlocked.Increment(ref counter);
            }); // 00:00:23.3873979
            while (true)
            {
                if (counter == 1)
                {
                    Console.WriteLine($"\nPARALLEL FOREACH TIME: {sw.Elapsed}\n");
                    sw.Stop();
                    break;
                }
            }
        }
    }
}