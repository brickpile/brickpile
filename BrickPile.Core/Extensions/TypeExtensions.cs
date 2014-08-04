using System;
using System.Linq;

namespace BrickPile.Core.Extensions
{
    /// <summary>
    ///     Provides BrickPile <see cref="Type" /> helper methods.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///     Gets the attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static T GetAttribute<T>(this Type type) where T : Attribute
        {
            T attribute = null;

            var attributes = type.GetCustomAttributes(true);
            foreach (var attributeInType in attributes.OfType<T>())
            {
                attribute = attributeInType;
            }

            return attribute;
        }
    }
}