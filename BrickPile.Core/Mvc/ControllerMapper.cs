using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web.Mvc;

namespace BrickPile.Core.Mvc
{
    /// <summary>
    ///     Represents a map of all registred controllers in BrickPile.
    /// </summary>
    internal class ControllerMapper : IControllerMapper
    {
        private static readonly ConcurrentDictionary<Type, string> ControllerMap =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<string, string[]> ControllerActionMap =
            new ConcurrentDictionary<string, string[]>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ControllerMapper" /> class.
        /// </summary>
        public ControllerMapper()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsClass)
                    {
                        continue;
                    }
                    if (!type.IsSubclassOf(typeof (Controller))) continue;
                    ControllerMap.TryAdd(type, type.Name.Replace("Controller", ""));
                    var methodNames = type.GetMethods().Select(x => x.Name).ToArray();
                    ControllerActionMap.TryAdd(type.Name.Replace("Controller", ""), methodNames);
                }
            }
        }

        /// <summary>
        ///     Gets the name of the controller.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetControllerName(Type type)
        {
            string name;
            ControllerMap.TryGetValue(type, out name);
            return name;
        }

        /// <summary>
        ///     Controllers the has action.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        public bool ControllerHasAction(string controllerName, string actionName)
        {
            return ControllerActionMap.ContainsKey(controllerName) &&
                   ControllerActionMap[controllerName].Any(
                       action => String.Equals(action, actionName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}