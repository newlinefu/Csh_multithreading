using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    Divide(10, 0);
                }
                catch (DivideByZeroException e)
                {
                    Console.WriteLine("Thread error catch | " + e.Message);
                }
            });
            t.Start();
            t.Join();

            Task<int> task = new Task<int>(() => Divide(10, 0));
            task.ContinueWith(
                tsk => Console.WriteLine("Task error catch | " + tsk?.Exception?.Message), 
                TaskContinuationOptions.OnlyOnFaulted
            );
            task.Start();
            
            Thread.Sleep(1000);
        }

        private static int Divide(int a, int b)
        {
            return a / b;
        }
    }
}