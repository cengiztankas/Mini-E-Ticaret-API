using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.exceptions
{
    public class NotFoundUserExeption : Exception
    {
        public NotFoundUserExeption():base("Bu Kullanıcı Bulunamadı.")
        {
        }

        public NotFoundUserExeption(string? message) : base(message)
        {
        }

        protected NotFoundUserExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}