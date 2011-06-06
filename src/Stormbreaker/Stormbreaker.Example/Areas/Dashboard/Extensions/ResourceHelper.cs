using System;
using System.Globalization;
using System.Resources;

namespace Stormbreaker.Dashboard.Extensions {
    public static class ResourceHelper {
        public static string GetString(Type resourceType, string resourceName) {
            return new ResourceManager(resourceType.FullName, resourceType.Assembly)
                .GetString(resourceName);
        }

        public static string GetString(Type resourceType, string resourceName, CultureInfo culture) {
            return new ResourceManager(resourceType.FullName, resourceType.Assembly)
                .GetString(resourceName, culture);
        }

        public static object GetObject(Type resourceType, string resourceName) {
            return new ResourceManager(resourceType.FullName, resourceType.Assembly)
                .GetObject(resourceName);
        }

        public static object GetObject(Type resourceType, string resourceName, CultureInfo culture) {
            return new ResourceManager(resourceType.FullName, resourceType.Assembly)
                .GetObject(resourceName, culture);
        }
    }

}