using System;

namespace BrickPile.Core.Exceptions
{
    internal sealed class DuplicateUrlException : Exception
    {
        public DuplicateUrlException(string message) : base(message) { }
    }
}
