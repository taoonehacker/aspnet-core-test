using System;

namespace Tao.Project.IOC
{
    public class Base : IDisposable
    {
        public Base()
        {
            Console.WriteLine($"Instance of {GetType().Name} is created.");
        }

        public void Dispose()
        {
            Console.WriteLine($"Instance of {GetType().Name} is disposed.");
        }
    }


    public interface IFoo
    {
    }

    public class Foo : Base, IFoo
    {
    }
}