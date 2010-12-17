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
        internal static T GetAttribute<T>(this Type type) where T : Attribute
        {
            T attribute = null;

            var attributes = type.GetCustomAttributes(true);
            foreach (var attributeInType in attributes)
            {
                if (typeof(T).IsAssignableFrom(attributeInType.GetType()))
                    attribute = (T)attributeInType;
            }

            return attribute;
        }
        public static bool ContainsAction(this Type type, string actionName)
        {
            return type.GetMethods().SingleOrDefault(x => x.Name.ToLowerInvariant() == actionName.ToLower()) != null;
        }
    }
}
