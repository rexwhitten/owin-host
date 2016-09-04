using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin
{
    using System.Reflection;
    using System.Runtime.Serialization;

    public class ObjectScanner
    {
        #region Ctor
        internal ConstructorInfo DefaultConstructor(Type implementation) {

            foreach(var ctor in implementation.GetConstructors())
            {
                var pars = ctor.GetParameters();
                if(pars.Length == 0)
                {
                    return ctor;
                }
            }

            return null;
        }

        internal ConstructorInfo ParameterizedConstructor(Type implementation) {
            foreach (var ctor in implementation.GetConstructors())
            {
                var pars = ctor.GetParameters();
                if (pars.Length != 0)
                {
                    return ctor;
                }
            }

            return null;
        }
        #endregion

        
        internal static Type[] Scan<TInterface, TAttributeType>() where TAttributeType : Attribute
        {
            var types = Assembly.GetExecutingAssembly()
                                .GetTypes()
                                .Where(t => t.GetInterfaces().Contains(typeof(TInterface)))
                                .Where(t => t.GetCustomAttributes(typeof(TAttributeType)).Any());
            return types.ToArray();
        }

        internal static Type[] ScanAssembly<TInterface>(Assembly assembly)
        {
            var types = assembly.GetTypes()
                                .Where(t => t.GetInterfaces().Contains(typeof(TInterface)));

            return types.ToArray();
        }

        internal static Type[] ScanAssembly<TInterface, TAttributeType>(Assembly assembly) where TAttributeType : Attribute
        {
            var types = assembly.GetTypes()
                                .Where(t => t.GetInterfaces().Contains(typeof(TInterface)))
                                .Where(t => t.GetCustomAttributes(typeof(TAttributeType)).Any());
            return types.ToArray();
        }
    }


}
