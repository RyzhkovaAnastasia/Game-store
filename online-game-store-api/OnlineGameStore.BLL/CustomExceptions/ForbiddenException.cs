using System;
using System.Runtime.Serialization;

namespace OnlineGameStore.BLL.CustomExceptions
{
    [Serializable]
    public class ForrbidenException : Exception
    {
        public ForrbidenException() : base() { }
        public ForrbidenException(string message) : base(message) { }
        public ForrbidenException(string message, Exception innerException) : base(message, innerException) { }
        protected ForrbidenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
