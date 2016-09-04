using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin
{
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;


    public static class Extensons
    {
        static string GetDeepMessage(Exception e)
        {
            if(e.InnerException != null)
            {
                return GetDeepMessage(e.InnerException);
            }

            return e.Message;
        }

        #region Hashtable functions
        public static Hashtable ToHashtable(this Exception exp)
        {
            var h = new Hashtable();

            h.Add("message", GetDeepMessage(exp));
            
            return h;
        }
        #endregion

        #region Type Selector 
        public static string ToTypeIdentifier(this Type type)
        {
            return type.FullName.Replace(".","/");
        }
        #endregion
    }
}
