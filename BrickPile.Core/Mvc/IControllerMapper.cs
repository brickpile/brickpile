using System;

namespace BrickPile.Core.Mvc
{
    public  interface IControllerMapper
    {
        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        string GetControllerName(Type type);

        /// <summary>
        /// Controllers the has action.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        bool ControllerHasAction(string controllerName, string actionName);
    }
}
