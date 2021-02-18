using System;

namespace Tao.Project.IOC
{
    internal class Key : IEquatable<Key>
    {
        public ServiceRegistry Registry { get; }
        public Type[] GeneriaArguments { get; }

        public Key(ServiceRegistry registry, Type[] generiaArguments)
        {
            this.Registry = registry;
            this.GeneriaArguments = generiaArguments;
        }


        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Key other)
        {
            if (this.Registry != other.Registry)
            {
                return false;
            }

            if (this.GeneriaArguments.Length != other.GeneriaArguments.Length)
            {
                return false;
            }

            for (int index = 0; index < this.GeneriaArguments.Length; index++)
            {
                if (GeneriaArguments[index] != other.GeneriaArguments[index])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = Registry.GetHashCode();
            for (int index = 0; index < GeneriaArguments.Length; index++)
            {
                hashCode ^= GeneriaArguments[index].GetHashCode();
            }

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is Key key && Equals(key);
        }
    }
}