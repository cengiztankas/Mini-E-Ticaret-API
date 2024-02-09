using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application
{
    public static class ServiceRegistration
    {
        public  static void AddApplicationServices(this IServiceCollection collection)
        {
            collection.AddMediatR(typeof (ServiceRegistration));
            collection.AddHttpClient(); //facebook işlemlerinde kullandık
        }
        //Bu Yapı ile asembly deki tüm servisleri IOC Container a eklemiş oluyoruz..
    }
}
