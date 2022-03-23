using System;

namespace FullFraim.Services.Exceptions
{
    public class UniqueNameException : Exception
    {
        public UniqueNameException(string message) : base(message) { }
    }
}
