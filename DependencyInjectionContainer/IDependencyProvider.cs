using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainer
{
    internal interface IDependencyProvider
    {
        public T Resolve<T>();
    }
}
