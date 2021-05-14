using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
            
            Thread firstTr = new Thread(() => um.DoWork(firstPart));
            Thread secondTr = new Thread(() => um.DoWork(secondPart));
            Thread thirdTr = new Thread(() => um.DoWork(thirdPart));
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            firstTr.Start();
            secondTr.Start();
            thirdTr.Start();

            firstTr.Join();
            secondTr.Join();
            thirdTr.Join();
            
            Console.WriteLine(sw.Elapsed);
            sw.Reset();
        }
    }
}