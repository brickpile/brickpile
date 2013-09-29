/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web.Mvc;

namespace BrickPile.UI.Web.Mvc {
    public class ControllerMapper : IControllerMapper {
        static readonly ConcurrentDictionary<Type, string> ControllerMap = new ConcurrentDictionary<Type, string>();
        static readonly ConcurrentDictionary<string, string[]> ControllerActionMap = new ConcurrentDictionary<string, string[]>();
        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetControllerName(Type type) {
            
            string name;
            ControllerMap.TryGetValue(type, out name);
            return name;

        }
        /// <summary>
        /// Controllers the has action.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        public bool ControllerHasAction(string controllerName, string actionName) {
            if (!ControllerActionMap.ContainsKey(controllerName))
                return false;

            foreach (var action in ControllerActionMap[controllerName]) {
                if (String.Equals(action, actionName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;            
        }

        /// <summary>
        /// Controllers the exists.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        public bool ControllerExists(string controllerName) {
            return ControllerMap.Any(x => x.Key.Name.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerMapper"/> class.
        /// </summary>
        public ControllerMapper() {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

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