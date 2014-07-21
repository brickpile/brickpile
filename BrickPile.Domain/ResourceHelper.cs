using System;
using System.Reflection;

namespace BrickPile.Domain {
    /// <summary>
    /// 
    /// </summary>
    public class ResourceHelper {
        /// <summary>
        /// Gets the resource lookup.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static string GetResourceLookup(Type resourceType, string resourceName) {
            if ((resourceType != null) && (resourceName != null)) {
                PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);
                if (property == null) {
                    throw new InvalidOperationException(string.Format("Resource Type Does Not Have Property"));
                }
                if (property.PropertyType != typeof(string)) {
                    throw new InvalidOperationException(string.Format("Resource Property is Not String Type"));
                }
                return (string) property.GetValue(null, null);
            }
            return null;
        }
    }
}