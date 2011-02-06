using System;

namespace Stormbreaker.Exceptions {
    /// <summary>
    /// Represents an exception that has occured within Stormbreaker
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class StormbreakerException : Exception {
        /// <summary>
        /// Create a new exception
        /// </summary>
        /// <param name="message"></param>
        public StormbreakerException(string message) : base(message) { }
        /// <summary>
        /// Create a new exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public StormbreakerException(string message, Exception innerException) : base(message, innerException) { }        
    }
}