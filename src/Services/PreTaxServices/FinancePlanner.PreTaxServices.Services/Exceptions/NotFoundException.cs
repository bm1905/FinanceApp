using System;
using System.Runtime.Serialization;

namespace FinancePlanner.PreTaxServices.Services.Exceptions
{
    [Serializable]
    public class NotFoundException : ApplicationException
    {
        public NotFoundException() { }

        public NotFoundException(Type type) : base($"{type} is missing") { }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}