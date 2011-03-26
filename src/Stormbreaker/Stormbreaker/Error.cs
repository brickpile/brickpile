using System;
using Elmah;

namespace Stormbreaker {
    /// <summary>
    /// This class logs exception in ELMAH
    /// </summary>
    /// <example>
    /// try {
    ///    .....
    /// } catch (Exception ex) {
    ///     Error.Log("this is a custom error",ex);
    /// }
    /// </example>
    public static class Error {
        /// <summary>
        /// Logs the exception in ELMAH with a custom message
        /// </summary>
        /// <param name="message">Custom message</param>
        /// <param name="ex">Exception object</param>
        public static void Log(string message, Exception ex) {
            ErrorSignal.FromCurrentContext().Raise(new Elmah.ApplicationException(message, ex));
        }
        /// <summary>
        /// Logs the message in ELMAH
        /// </summary>
        /// <param name="message">Custom message</param>
        public static void Log(string message) {
            ErrorSignal.FromCurrentContext().Raise(new Elmah.ApplicationException(message));
        }
        /// <summary>
        /// Logs the exception in ELMAH
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(Exception ex) {
            ErrorSignal.FromCurrentContext().Raise(ex);
        }
    }
}