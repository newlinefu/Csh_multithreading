using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Multithreading
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<int> dividing = new Task<int>(() => Divide(10, 0));
            dividing.Start();
            
            try
            {
                Console.WriteLine(dividing.Result);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"Aggregate error catch {ex.Message}");
            }
        }

        private static int Divide(int a, int b)
        {
            return a / b;
        }
    }
}