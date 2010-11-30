using System;
using System.Linq;

namespace Stormbreaker.Extensions {
    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class TypeExtensions {
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        #region internal static T GetAttribute<T>(this Type type)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static T GetAttribute<T>(this Type type) where T : Attribute
        {
            T attribute = null;

            object[] attributes = type.GetCustomAttributes(true);
            foreach (object attributeInType in attributes)
            {
                if (typeof(T).IsAssignableFrom(attributeInType.GetType()))
                    attribute = (T)attributeInType;
            }

            return attribute;
        }
        #endregion
        #region public static bool HasAction(this Type type, string actionName)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static bool HasAction(this Type type, string actionName)
        {
            return type.GetMethods().SingleOrDefault(x => x.Name.ToLowerInvariant() == actionName.ToLower()) != null;
        }
        #endregion
    }
}
