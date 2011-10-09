using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web.Mvc;

namespace BrickPile.UI.Web.Mvc {
    public class ControllerMapper : IControllerMapper {

        static readonly ConcurrentDictionary<Type, string> ControllerMap = new ConcurrentDictionary<Type, string>();
        static readonly ConcurrentDictionary<string, string[]> ControllerActionMap = new ConcurrentDictionary<string, string[]>();

        public string GetControllerName(Type type) {
            
            string name;
            ControllerMap.TryGetValue(type, out name);
            return name;

        }

        public bool ControllerHasAction(string controllerName, string actionName) {
            if (!ControllerActionMap.ContainsKey(controllerName))
                return false;

            foreach (var action in ControllerActionMap[controllerName]) {
                if (String.Equals(action, actionName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;            
        }

        public ControllerMapper() {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.FullName.Contains("Truffler"));

            foreach (var assembly in assemblies) {
                foreach (var type in assembly.GetTypes()) {
                    if (!type.IsClass) {
                        continue;
                    }
                    if (type.IsSubclassOf(typeof(Controller))) {
                        ControllerMap.TryAdd(type, type.Name);
                        var methodNames = type.GetMethods().Select(x => x.Name).ToArray();
                        ControllerActionMap.TryAdd(type.Name,methodNames);
                    }
                }
            }

        }

    }
}