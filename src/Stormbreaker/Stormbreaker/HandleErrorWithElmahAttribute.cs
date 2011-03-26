using System;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace Stormbreaker {
    /// <summary>
    /// 
    /// </summary>
    public class HandleErrorWithElmahAttribute : HandleErrorAttribute {
        /// <summary>
        /// Called when [exception].
        /// </summary>
        /// <param name="context">The context.</param>
        public override void OnException(ExceptionContext context) {
            base.OnException(context);

            var e = context.Exception;
            if (!context.ExceptionHandled   // if unhandled, will be logged anyhow
                    || RaiseErrorSignal(e)      // prefer signaling, if possible
                    || IsFiltered(context))     // filtered?
                return;

            LogException(e);
        }
        /// <summary>
        /// Raises the error signal.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private static bool RaiseErrorSignal(Exception e) {
            var context = HttpContext.Current;
            if (context == null)
                return false;
            var signal = ErrorSignal.FromContext(context);
            if (signal == null)
                return false;
            signal.Raise(e, context);
            return true;
        }
        /// <summary>
        /// Determines whether the specified context is filtered.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the specified context is filtered; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsFiltered(ExceptionContext context) {
            var config = context.HttpContext.GetSection("elmah/errorFilter") as ErrorFilterConfiguration;

            if (config == null)
                return false;

            var testContext = new ErrorFilterModule.AssertionHelperContext(context.Exception, HttpContext.Current);

            return config.Assertion.Test(testContext);
        }
        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        private static void LogException(Exception e) {
            var context = HttpContext.Current;
            ErrorLog.GetDefault(context).Log(new Elmah.Error(e, context));
        }
    }
}