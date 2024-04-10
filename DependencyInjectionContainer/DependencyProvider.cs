using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace DependencyInjectionContainer
{
    public class DependencyProvider : IDependencyProvider
    {
        private DependencyConfig config;
        private ConcurrentDictionary<Type, object> singletonVals = new ConcurrentDictionary<Type, object>();

        public DependencyProvider(DependencyConfig config) 
        {
            this.config = config;
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private object Resolve(Type forType)
        {
            ImplementationWrapper wrapper = null;
            if (typeof(IEnumerable).IsAssignableFrom(forType))
            {
                Type type = forType.GetGenericArguments()[0];
                var implementations = Array.CreateInstance(type, config.Dependencies[type].Count);
                for (int i = 0; i < implementations.Length; i++)
                {
                    var obj = CreateInstance(config.Dependencies[type][i]);
                    implementations.SetValue(obj, i);
                }
                return implementations;
            }
            if (forType.GenericTypeArguments.Length > 0) {
                if (!config.Dependencies.ContainsKey(forType) &&
                    config.Dependencies.ContainsKey(forType.GetGenericTypeDefinition()))
                {
                    Type genericType = config.Dependencies[forType.GetGenericTypeDefinition()][0].ImplementedType.MakeGenericType(forType.GetGenericArguments()[0]);
                    wrapper = new ImplementationWrapper(config.Dependencies[forType.GetGenericTypeDefinition()][0].LifeTimeType ,genericType);
                }
            }
            if (wrapper == null)
            {
                wrapper = config.Dependencies[forType][0];
            }
            return CreateInstance(wrapper);
        }

        private object CreateInstance(ImplementationWrapper implementation) 
        {
            if (implementation.LifeTimeType == LifeTimeType.SINGLETON &&
                singletonVals.ContainsKey(implementation.ImplementedType))
            {
                return singletonVals[implementation.ImplementedType];
            }
            ConstructorInfo mainConstructor = implementation.ImplementedType.GetConstructors()[0];
            ParameterInfo[] args = mainConstructor.GetParameters();
            object[] paramsToFill = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                paramsToFill.SetValue(Resolve(args[i].ParameterType) ,i);
            }
            var obj = Activator.CreateInstance(implementation.ImplementedType, paramsToFill);
            if (implementation.LifeTimeType == LifeTimeType.SINGLETON)
            {
                singletonVals.TryAdd(implementation.ImplementedType, obj);
            }
            return obj;
        }
    }
}
