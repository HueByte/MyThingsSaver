using System;
using System.Collections.Generic;

namespace MTS.Core.Entities
{
    public class HandledExceptionList : Exception
    {
        public List<string> ExceptionMessages { get; set; }
        public HandledExceptionList(List<string> exceptionMessages)
        {
            ExceptionMessages = exceptionMessages;
        }
    }

    public class HandledException : Exception
    {
        public HandledException(string ExceptionMessage) : base(ExceptionMessage) { }
    }

    public class TokenException : Exception
    {
        public TokenException(string ExceptionMessage) : base(ExceptionMessage) { }
    }
}