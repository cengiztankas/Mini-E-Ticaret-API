﻿using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Infrastructure.enums;
using ETicaretAPI.Infrastructure.Services;
using ETicaretAPI.Infrastructure.Services.Configurations;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection) {

            //serviceCollection.AddScoped<IFileService, FileService>();

            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler,TokenHandler>();
            serviceCollection.AddScoped<IMailService,Mailservice>();
            serviceCollection.AddScoped<IApplicationService,ApplicationService>();
            serviceCollection.AddScoped<IQRCodeService,QRCodeService>();
        }
        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage,IStorage 
        {
            serviceCollection.AddScoped<IStorage, T>();
          
        }
        //bir önceki metodun zengin halidir

        //public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType) 
        //{
        //    switch (storageType)
        //    {
        //        case StorageType.Local:
        //            serviceCollection.AddScoped<IStorage, LocalStorage>();
        //            break;
        //        case StorageType.Azure:
        //          //  serviceCollection.AddScoped<IStorage, AzureStorage>();
        //            break;
        //        case StorageType.AWS:
        //           // serviceCollection.AddScoped<IStorage, AwsStorage>();
        //            break;
        //        default:
        //            serviceCollection.AddScoped<IStorage, LocalStorage>();
        //            break;
        //    }

        //}
    }
}