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

            // ThreadPool.QueueUserWorkItem((part) => um.DoWork((List<User>) part), firstPart);
            // ThreadPool.QueueUserWorkItem((part) => um.DoWork((List<User>) part), secondPart);
            // ThreadPool.QueueUserWorkItem((part) => um.DoWork((List<User>) part), thirdPart);

            Task.Run(() => um.DoWork(firstPart));
            Task.Run(() => um.DoWork(secondPart));
            Task.Run(() => um.DoWork(thirdPart));
            
            Thread.Sleep(10000);
        }
    }
}