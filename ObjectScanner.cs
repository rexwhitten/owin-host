using System;
using System.Linq;

namespace apistation.owin
{
    using System.Reflection;

    public class ObjectScanner
    {
        #region Ctor

        internal ConstructorInfo DefaultConstructor(Type implementation)
        {
            foreach (var ctor in implementation.GetConstructors())
            {
                var pars = ctor.GetParameters();
                if (pars.Length == 0)
                {
                    return ctor;
                }
            }

            return null;
        }

        internal ConstructorInfo ParameterizedConstructor(Type implementation)
        {
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

        #endregion Ctor

        
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

        /// <summary>
        /// simple liskov search type inheritance
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <param name="liskovSearch"></param>
        /// <returns></returns>
        internal static bool InheritsType(Type child, Type parent, bool liskovSearch)
        {
            var check = child.BaseType == parent;
            if (check == true || liskovSearch == false) return check;
            return InheritsType(child.BaseType, parent, liskovSearch);
        }
    }
}