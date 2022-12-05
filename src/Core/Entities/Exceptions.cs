using System;
using System.Collections.Generic;

namespace MTS.Core.Entities
{
    public class EndpointExceptionList : Exception
    {
        public List<string> ExceptionMessages { get; set; }
        public EndpointExceptionList(List<string> exceptionMessages)
        {
            ExceptionMessages = exceptionMessages;
        }
    }

    public class EndpointException : Exception
    {
        public EndpointException(string ExceptionMessage) : base(ExceptionMessage) { }
    }
}