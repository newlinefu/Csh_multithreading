using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    public class UserManager
    {
        private readonly int _threadsCount;
        public UserManager()
        {
            _threadsCount = Environment.ProcessorCount;
        }

        public List<User> InitializeAll(int size = 1500)
        {
            List<User> users = new List<User>();
            
            for (int i = 0; i < size; i++)
            {
                users.Add(new User($"User {i}"));
            }

            return users;
        }
        
        public void DoWork(List<User> users, CancellationToken ct)
        {
            foreach (var user in users)
            {
                if(ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }
                
                Console.WriteLine(user.Name);
                Thread.Sleep(10);
            }
        }

        public void DoWorkParallel(List<User> users, CancellationToken ct, int threadsCount = -1)
        {
            if (threadsCount < 1)
            {
                threadsCount = _threadsCount;
            }

            ParallelOptions po = new ParallelOptions();
            po.MaxDegreeOfParallelism = threadsCount;
            po.CancellationToken = ct;
            
            Parallel.ForEach(
                users,
                (u, loopState, index) =>
                {
                    Console.WriteLine(u.Name);
                    Thread.Sleep(10);
                    po.CancellationToken.ThrowIfCancellationRequested();
                }
            );
        }
    }
}