using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.exceptions
{
    public class PasswordChangeFailedException : Exception
    {
        public PasswordChangeFailedException():base("Yeni şifre oluşturuken bir hata oluştu")
        {
        }

        public PasswordChangeFailedException(string? message) : base(message)
        {
        }

        public PasswordChangeFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PasswordChangeFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
