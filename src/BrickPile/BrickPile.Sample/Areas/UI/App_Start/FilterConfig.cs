using System.Web.Mvc;

namespace BrickPile.UI2 {
    /// <summary>
    /// 
    /// </summary>
    public class FilterConfig {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}