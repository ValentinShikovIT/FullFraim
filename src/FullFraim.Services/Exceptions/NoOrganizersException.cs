using System;

namespace FullFraim.Services.Exceptions
{
    public class NoOrganizersException : Exception
    {
        public NoOrganizersException(string message) : base(message) { }
    }
}
