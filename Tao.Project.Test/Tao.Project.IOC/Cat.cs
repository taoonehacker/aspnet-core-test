using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Tao.Project.IOC
{
    /// <summary>
    /// 依赖注入容器 哆啦A梦
    /// </summary>
    public class Cat : IServiceProvider, IDisposable
    {
        // 根容器Cat
        internal readonly Cat _root;

        // 所有添加的服务注册
        internal readonly ConcurrentDictionary<Type, ServiceRegistry> _registries;

        // 所有非Fransient服务实例
        private readonly ConcurrentDictionary<Key, object> _services;

        // 带释放的集合
        private readonly ConcurrentBag<IDisposable> _disposables;

        /// <summary>
        /// 确保本条指令不会因编译器的优化而省略
        /// </summary>
        private volatile bool _disposed;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Cat()
        {
            _registries = new ConcurrentDictionary<Type, ServiceRegistry>();
            _services = new ConcurrentDictionary<Key, object>();
            _root = this;
            _disposables = new ConcurrentBag<IDisposable>();
        }
        
        internal Cat(Cat parent)
        {
            _root = parent._root;
            _registries = _root._registries;
            _services = new ConcurrentDictionary<Key, object>();
            _disposables = new ConcurrentBag<IDisposable>();
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="serviceRegistry"></param>
        /// <returns></returns>
        public Cat Register(ServiceRegistry serviceRegistry)
        {
            EnsureNotDisposed();

            if (_registries.TryGetValue(serviceRegistry.ServiceType, out var existing))
            {
                _registries[serviceRegistry.ServiceType] = serviceRegistry;
                serviceRegistry.Next = serviceRegistry;
            }
            else
            {
                _registries[serviceRegistry.ServiceType] = serviceRegistry;
            }

            return this;
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            EnsureNotDisposed();

            if (serviceType == typeof(Cat) || serviceType == typeof(IServiceProvider))
            {
                return this;
            }

            ServiceRegistry registry;

            // IEnumerable<T>
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var elementType = serviceType.GetGenericArguments()[0];
                if (!_registries.TryGetValue(elementType, out registry))
                {
                    return Array.CreateInstance(elementType, 0);
                }

                var registries = registry.AsEnumerable();
                var services = registries.Select(it => GetServiceCore(it, Type.EmptyTypes)).ToArray();

                Array array = Array.CreateInstance(elementType, services.Length);

                services.CopyTo(array, 0);
                return array;
            }

            //Generic
            if (serviceType.IsGenericType && !_registries.ContainsKey(serviceType))
            {
                var definition = serviceType.GetGenericTypeDefinition();
                return _registries.TryGetValue(definition, out registry) ? GetServiceCore(registry, serviceType.GenericTypeArguments) : null;
            }

            // Normal
            return _registries.TryGetValue(serviceType, out registry) ? GetServiceCore(registry, new Type[0]) : null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
            _services.Clear();
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Cat));
            }
        }

        private object GetServiceCore(ServiceRegistry serviceRegistry, Type[] genericArguments)
        {
            var key = new Key(serviceRegistry, genericArguments);
            var serviceType = serviceRegistry.ServiceType;

            switch (serviceRegistry.Lifetime)
            {
                case Lifetime.Root:
                    return GetOrCreate(_root._services, _root._disposables);
                    break;
                case Lifetime.Self:
                    return GetOrCreate(_services, _disposables);
                    break;
                default:
                    var service = serviceRegistry.Factory(this, genericArguments);
                    if (service is IDisposable disposable && disposable != this)
                    {
                        _disposables.Add(disposable);
                    }

                    return service;
            }

            object GetOrCreate(ConcurrentDictionary<Key, object> services, ConcurrentBag<IDisposable> disposables)
            {
                if (services.TryGetValue(key, out var service))
                {
                    return service;
                }

                service = serviceRegistry.Factory(this, genericArguments);
                services[key] = service;
                if (service is IDisposable disposable)
                {
                    _disposables.Add(disposable);
                }

                return service;
            }
        }
    }
}