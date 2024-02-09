using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.Repositories.BasketItemT;
using ETicaretAPI.Application.Repositories.BasketT;
using ETicaretAPI.Application.Repositories.CompletedOrderT;
using ETicaretAPI.Application.Repositories.CustomerT;
using ETicaretAPI.Application.Repositories.EndpointT;
using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Application.Repositories.IncoiceFileT;
using ETicaretAPI.Application.Repositories.MenuT;
using ETicaretAPI.Application.Repositories.OrderT;
using ETicaretAPI.Application.Repositories.ProductIeFileT;
using ETicaretAPI.Application.Repositories.ProductImageFileT;
using ETicaretAPI.Application.Repositories.ProductT;
using ETicaretAPI.Domain.Entites.Idetity;
using ETicaretAPI.Persistence.Context;
using ETicaretAPI.Persistence.Repositories.BasketItemT;
using ETicaretAPI.Persistence.Repositories.BasketT;
using ETicaretAPI.Persistence.Repositories.CompletedOrderT;
using ETicaretAPI.Persistence.Repositories.CustomerT;
using ETicaretAPI.Persistence.Repositories.EndpointT;
using ETicaretAPI.Persistence.Repositories.File;
using ETicaretAPI.Persistence.Repositories.IncoiceFileT;
using ETicaretAPI.Persistence.Repositories.MenuT;
using ETicaretAPI.Persistence.Repositories.OrderT;
using ETicaretAPI.Persistence.Repositories.ProductImageFileT;
using ETicaretAPI.Persistence.Repositories.ProductT;
using ETicaretAPI.Persistence.Services;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<ETicaretAPIDbContext>(options => options.UseNpgsql(Configuration.ConnectionString) );
            services.AddIdentity<AppUser, AppRole>(option =>
            {
                option.Password.RequiredLength = 3;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireDigit = false;
                
            }).AddEntityFrameworkStores<ETicaretAPIDbContext>().AddDefaultTokenProviders();

            //.AddDefaultTokenProviders();=>user password reset ederken bu servisi eklememez gerekiyor

            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IProductReadRepository,ProductReadRepository>();
            services.AddScoped<IProductWriteRepository,ProductWriteRepository>();

            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepositpory,FileWriteRepository>();
            services.AddScoped<IInvoiceFileReadRepository, InvoiceReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceWriteRepository>();
            services.AddScoped<IProductImageFileReadRepository,ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();

            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();

            services.AddScoped<ICompletedOrderReadRepository,CompletedOrderReadRepository>();
            services.AddScoped<ICompletedOrderWriteRepository, CompletedOrderWriteRepository>();

            services.AddScoped<IEndpointReadRepository,EndpointReadRepository>();
            services.AddScoped<IEndpointWriteRepository,EndpointWriteRepository>();

            services.AddScoped<IMenuReadRepository,MenuReadRepository>();
            services.AddScoped<IMenuWriteRepository,MenuWriteRepository>();

            services.AddScoped<IBasketService, BasketService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IExternalAuthentication, AuthService>();
            services.AddScoped<IInternalAuthentication, AuthService>();

            services.AddScoped <IOrderService, OrderService>();
         
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthorizationEndpointService, AuthorizationEndpointService>();

            //services.AddSingleton<ICustomerReadRepository, CustomerReadRepository>();
            //services.AddDbContext<ETicaretAPIDbContext>(options => options.UseNpgsql(Configuration.ConnectionString),
            //    ServiceLifetime.Singleton
            //    );
            //services.AddSingleton<ICustomerReadRepository, CustomerReadRepository>();
        }
    }
}
