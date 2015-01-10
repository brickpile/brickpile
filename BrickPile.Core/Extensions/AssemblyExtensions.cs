using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BrickPile.Core.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> TryGetTypes(this Assembly assm)
        {
            try
            {
                return assm.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
