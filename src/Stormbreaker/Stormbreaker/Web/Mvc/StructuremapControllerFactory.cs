using System;
using System.Web.Mvc;
using System.Web.Routing;
using Stormbreaker.Models;
using StructureMap;

namespace Stormbreaker.Web.Mvc {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        #region public override IController CreateController(RequestContext requestContext, string controllerName)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                object dashboardControllerType;
                if (requestContext.RouteData.DataTokens.TryGetValue("controllerType", out dashboardControllerType))
                {
                    var item = requestContext.RouteData.DataTokens["item"] as IContentItem;
                    if (item != null)
                    {
                        var genericType = ((Type)dashboardControllerType).MakeGenericType(item.GetType());
                        return ObjectFactory.GetInstance(genericType) as IController;
                    }
                }
                var controllerType = base.GetControllerType(requestContext, controllerName);
                return ObjectFactory.GetInstance(controllerType) as IController;
            }
            catch (Exception)
            {
                //Use the default logic
                return base.CreateController(requestContext, controllerName);
            }
        }
        #endregion
    }
}