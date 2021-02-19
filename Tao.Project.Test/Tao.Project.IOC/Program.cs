using System;
using System.Reflection;

namespace Tao.Project.IOC
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var root = new Cat()
                .Register<IFoo, Foo>(Lifetime.Transient)
                .Register(Assembly.GetEntryAssembly()))
            {
                using (var cat = root.CreateChild())
                {
                    cat.GetService<IFoo>();
                    cat.GetService<IGux>();
                    Console.WriteLine("Child cat is disposed.");
                }

                Console.WriteLine("Root cat is disposed.");
            }
        }
    }
}