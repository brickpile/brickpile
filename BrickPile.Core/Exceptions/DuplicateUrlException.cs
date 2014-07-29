using System;

namespace BrickPile.Core.Exceptions
{
    public class DuplicateUrlException : Exception
    {
        public DuplicateUrlException(string message) : base(message) { }
    }
}
