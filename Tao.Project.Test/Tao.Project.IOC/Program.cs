using System;

namespace Tao.Project.IOC
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var root = new Cat()
                .Register<IFoo, Foo>(Lifetime.Transient))
            {
                using (var cat = root.CreateChild())
                {
                    cat.GetService<IFoo>();
                    Console.WriteLine("Child cat is disposed.");
                }

                Console.WriteLine("Root cat is disposed.");
            }
        }
    }
}