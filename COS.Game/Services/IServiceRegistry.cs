using System;
using System.Collections.Generic;

namespace COS.Game.Services
{
    /// <summary>
    /// An instance of IServiceRegistry will allow storing and retrieving instances by type.
    /// </summary>
    public interface IServiceRegistry
    {
        void Add<T>(T service) where T : class;
        T Get<T>() where T : class;
        void Remove<T>() where T : class;
    }
    
    public sealed class ServiceRegistry : IServiceRegistry
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        public void Add<T>(T service) where T : class
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var type = typeof(T);
            lock (_services)
            {
                if (_services.ContainsKey(type))
                    throw new ArgumentException("Service is already registered with this type", nameof(type));
                _services.Add(type, service);
            }
        }

        public T Get<T>() where T : class
        {
            var type = typeof(T);
            lock (_services)
            {
                if (_services.ContainsKey(type))
                    return (T) _services[type];
            }

            return null;
        }

        public void Remove<T>() where T : class
        {
            var type = typeof(T);
            lock (_services)
            {
                _services.Remove(type);
            }
        }
    }

    public static class ServiceRegistryExtensions
    {
        public static T GetOrThrow<T>(this IServiceRegistry registry) where T : class
        {
            var service = registry.Get<T>();
            if (service == null) throw new ServiceNotFoundException(typeof(T));
            return service;
        }
    }

    public class ServiceNotFoundException : Exception
    {
        public Type ServiceType { get; }

        public ServiceNotFoundException(Type serviceType) : base($"Service [{serviceType.Name}] not found")
        {
            ServiceType = serviceType;
        }
    }
}