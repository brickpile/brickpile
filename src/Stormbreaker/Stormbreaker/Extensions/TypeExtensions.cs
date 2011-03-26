using System;

namespace Stormbreaker.Extensions {
    /// <summary>
    /// Extension methods for Type objects
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class TypeExtensions {
        /// <summary>
        /// Get the attribute of a specific type ( returns null if not exists
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type) where T : Attribute {
            T attribute = null;

            var attributes = type.GetCustomAttributes(true);
            foreach (var attributeInType in attributes)
            {
                if (typeof(T).IsAssignableFrom(attributeInType.GetType()))
                    attribute = (T)attributeInType;
            }

            return attribute;
        }
    }
}
