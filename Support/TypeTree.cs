﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin.Support
{
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class TypeTreeExtensions
    {
        
    }

    public class TypeTree
    {
        /// <summary>
        /// Fine Implementations of Types
        /// </summary>
        /// <param name="type"></param>
        /// <param name="deepScan">Liskov search mode (locates any valid implementation or inheritance)</param>
        /// <returns></returns>
        static Type[] PerformScan(Assembly asm, Type checkType, bool deepScan)
        {
            var typeSet = new Collection<Type>();

            #region build type set
            var typeResultSet = (Assembly.GetExecutingAssembly()).GetTypes();

            foreach (var rtype in typeResultSet)
            {
                if(rtype.BaseType == checkType)
                {
                    typeSet.Add(rtype);
                }

                if(rtype.GetInterface(checkType.Name) != null)
                {
                    typeSet.Add(rtype);
                }
            }
            #endregion

            return typeSet.ToArray();
        }

        private static bool HasBaseType(Type rtype, Type type)
        {
            bool result = false;

            if(rtype.BaseType != typeof(System.Object))
            {
                if(rtype.BaseType == type) {
                    return true;
                }
                else {
                    return HasBaseType(rtype.BaseType, type);
                }
            }

            return result;
        }

        /// <summary>
        /// Fine Implementations of Types
        /// </summary>
        /// <param name="type"></param>
        /// <param name="deepScan">Liskov search mode (locates any valid implementation or inheritance)</param>
        /// <returns></returns>
        internal static Type[] Scan(Type type, bool deepScan)
        {
            return PerformScan(Assembly.GetExecutingAssembly(), type, deepScan);
        }
    }
}
