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
using System.Web;
using System.Web.Mvc;
using Elmah;
using Raven.Client;

namespace BrickPile.UI {
    /// <summary>
    /// 
    /// </summary>
    public class HandleErrorWithElmahAttribute : HandleErrorAttribute {
        private readonly IDocumentSession _session;
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