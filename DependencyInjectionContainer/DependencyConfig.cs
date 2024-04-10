using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainer
{

    public enum LifeTimeType
    {
        SINGLETON,
        INSTANCE_PER_DEPENDENCY
    }

    public class ImplementationWrapper
    {
        public LifeTimeType LifeTimeType { get; }
        public Type ImplementedType { get; }

        public ImplementationWrapper(LifeTimeType type, Type implementedType)
        {
            LifeTimeType = type;
            ImplementedType = implementedType;
        }
    }

    public class DependencyConfig
    {
        public Dictionary<Type, List<ImplementationWrapper>> Dependencies { get; }

        public DependencyConfig() 
        {
            Dependencies = new Dictionary<Type, List<ImplementationWrapper>>();
        }

        public void Register<T, M>(LifeTimeType type = LifeTimeType.SINGLETON) where M : T where T : class
        {
            Register(typeof(T), typeof(M), type);
        }

        private void Register(Type Interface, Type Implementation, LifeTimeType type = LifeTimeType.SINGLETON)
        {
            ImplementationWrapper wrapper = new ImplementationWrapper(type, Implementation);
            if (Dependencies.ContainsKey(Interface))
            {
                Dependencies[Interface].Add(wrapper);
            } else
            {
                List<ImplementationWrapper> queue = [wrapper];
                Dependencies.Add(Interface, queue);
            }
        }
    }
}
