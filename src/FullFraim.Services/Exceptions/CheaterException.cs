using System;

namespace FullFraim.Services.Exceptions
{
    public class CheaterException : InvalidOperationException
    {
        public CheaterException(string message) : base(message) { }
    }
}
