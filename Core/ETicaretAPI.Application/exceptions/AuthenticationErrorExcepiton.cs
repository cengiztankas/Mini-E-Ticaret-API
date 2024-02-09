using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.exceptions
{
    public class AuthenticationErrorExcepiton : Exception
    {
        public AuthenticationErrorExcepiton():base("Kimlik doğrulama hatası!")
        {
        }

        public AuthenticationErrorExcepiton(string? message) : base(message)
        {
        }

        public AuthenticationErrorExcepiton(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AuthenticationErrorExcepiton(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
