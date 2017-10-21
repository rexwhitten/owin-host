using System;
using System.Collections.Generic;

namespace apistation.owin
{
    /// <summary>
    /// ObjectFactory v1
    /// </summary>
    internal class ObjectFactory
    {
        private static IDictionary<Type, Type> _registry = new Dictionary<Type, Type>();
        private static IDictionary<string, Dictionary<Type, Object>> _instances = new Dictionary<string, Dictionary<Type, Object>>();

        #region Utility

        private static bool IsRegistered<TInterface, TImplementation>()
        {
            var intType = typeof(TInterface);
            var impType = typeof(TImplementation);

            if (_registry.ContainsKey(intType)) return true;
            return false;
        }

        #endregion Utility

        #region Type Resolution

        internal static object Resolve(Type interfaceType)
        {
            // Argument Validation
            if (interfaceType == null) throw new ArgumentException("InterfaceType");

            if (!_registry.ContainsKey(interfaceType)) throw new TypeInitializationException(interfaceType.FullName, new ArgumentException("InterfaceType was not registered"));

            return ResolveType(interfaceType);
        }

        internal static TInterface Resolve<TInterface>()
        {
            Type interfaceType = typeof(TInterface);
            // Argument Validation
            if (interfaceType == null) throw new ArgumentException("InterfaceType");

            if (!_registry.ContainsKey(interfaceType)) throw new TypeInitializationException(interfaceType.FullName, new ArgumentException("InterfaceType was not registered"));

            return (TInterface)ResolveType(interfaceType);
        }

        private static object ResolveType(Type interfaceType)
        {
            var rtype = interfaceType;

            var ctors = rtype.GetConstructors();
            foreach (var ctor in ctors)
            {
                var pars = ctor.GetParameters();
                if (pars.Length == 0)
                {
                    // we found a default do nothing
                }
                else
                {
                    int argIndex = 0;
                    object[] args = new object[pars.Length];
                    foreach (var par in pars)
                    {
                        args[argIndex] = Resolve(par.ParameterType);
                    }
                    return ActivateType(interfaceType, args);
                }
            }

            return ActivateType(interfaceType, null);
        }

        internal static TInterface Activate<TInterface>(Type implementation)
        {
            return (TInterface)ActivateType(implementation);
        }

        private static object ActivateType(Type interfaceType, params object[] args)
        {
            return Activator.CreateInstance(_registry[interfaceType], args);
        }

        #endregion Type Resolution

        #region Type Registration

        internal static void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (_registry.ContainsKey(typeof(TInterface)))
            {
                _registry.Add(typeof(TInterface), typeof(TImplementation));
            }
            else
            {
                _registry[typeof(TInterface)] = typeof(TImplementation);
            }
        }

        #endregion Type Registration

        #region Instance Management

        internal static void Instance<TInterface, TImplementation>(string name) where TImplementation : TInterface
        {
            if (name == null || name == string.Empty) throw new ArgumentException("name");

            if (!_instances.ContainsKey(name))
            {
                _instances.Add(name, new Dictionary<Type, object>());
            }

            if (!_instances[name].ContainsKey(typeof(TInterface)))
            {
                ObjectFactory.Register<TInterface, TImplementation>();

                _instances[name].Add(typeof(TInterface), ObjectFactory.Resolve<TInterface>());
            }
        }

        internal static TInterface Instance<TInterface>(string name)
        {
            if (name == null || name == string.Empty) throw new ArgumentException("name");

            if (!_instances.ContainsKey(name))
            {
                throw new ArgumentException(name + " was not found in the instance dictionary.");
            }

            if (!_instances[name].ContainsKey(typeof(TInterface)))
            {
                throw new ArgumentException(typeof(TInterface).Name + " was not found in the instance dictionary section " + name);
            }

            return (TInterface)_instances[name][typeof(TInterface)];
        }

        #endregion Instance Management
    }
}