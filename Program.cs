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
            var users = um.InitializeAll(150);
            
            List<User> firstPart = users.Take(50).ToList();
            List<User> secondPart = users.Skip(50).Take(50).ToList();
            List<User> thirdPart = users.Skip(100).Take(50).ToList();

            int counter = 0;
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            
            Parallel.Invoke(() =>
            {
                um.DoWork(firstPart, CancellationToken.None);
                Interlocked.Increment(ref counter);
            },() =>
            {
                um.DoWork(secondPart, CancellationToken.None);
                Interlocked.Increment(ref counter);
            },() =>
            {
                um.DoWork(thirdPart, CancellationToken.None);
                Interlocked.Increment(ref counter);
            });

            while (true)
            {
                if (counter == 3)
                {
                    Console.WriteLine(sw.Elapsed);
                    sw.Reset();
                    break;
                }
            }
        }
    }
}