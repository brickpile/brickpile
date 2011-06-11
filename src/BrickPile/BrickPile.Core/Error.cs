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

using Elmah;

namespace BrickPile.Core {
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
        public static void Log(string message, System.Exception ex) {
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
        public static void Log(System.Exception ex) {
            ErrorSignal.FromCurrentContext().Raise(ex);
        }
    }
}