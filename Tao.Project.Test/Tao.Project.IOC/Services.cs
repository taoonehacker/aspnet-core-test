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

    public interface IGux
    {
    }

    [MapTo(typeof(IGux), Lifetime.Root)]
    public class Gux : Base, IGux
    {
    }
    
    public interface IBar { }
    public interface IBaz { }
    
    public class Bar : Base, IBar { }
    public class Baz : Base, IBaz { }
    
    public interface IFoobar<T1, T2> { }
    
    public class Foobar<T1, T2> : IFoobar<T1, T2>
    {
        public IFoo Foo { get; }
        public IBar Bar { get; }
        public Foobar(IFoo foo, IBar bar)
        {
            Foo = foo;
            Bar = bar;
        }
    }
}