using System;
using System.Runtime.Serialization;

namespace OnlineGameStore.BLL.CustomExceptions
{
    [Serializable]
    public class ModelException : Exception
    {
        public ModelException() : base() { }
        public ModelException(string message) : base(message) { }
        public ModelException(string message, Exception innerException) : base(message, innerException) { }
        protected ModelException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
