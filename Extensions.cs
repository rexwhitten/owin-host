using System;

namespace apistation.owin
{
    using System.Collections;

    public static class Extensons
    {
        private static string GetDeepMessage(Exception e)
        {
            if (e.InnerException != null)
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

        #endregion Hashtable functions

        #region Type Selector

        public static string ToTypeIdentifier(this Type type)
        {
            return type.FullName.Replace(".", "/");
        }

        #endregion Type Selector
    }
}