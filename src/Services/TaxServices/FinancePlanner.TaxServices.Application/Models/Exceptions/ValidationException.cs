using System;
using System.Runtime.Serialization;

namespace FinancePlanner.TaxServices.Application.Models.Exceptions
{
    [Serializable]
    public class ValidationException : ApplicationException
    {
        public ValidationException() { }

        public ValidationException(Type type) : base($"{type} is missing") { }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
