using System;

namespace insurance_management.Exceptions
{
    public class PolicyNotFoundException : Exception
    {
        public PolicyNotFoundException(string message)
            : base($"Policy Not Found: {message}")  // Adding a default prefix to the custom message
        { }

        // a default constructor with a generic message
        public PolicyNotFoundException()
            : base("Policy Not Found.")  // Default message 
        { }
    }
}
