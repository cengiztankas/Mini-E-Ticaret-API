using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Infrastructure.enums;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Application;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using ETicaretAPI.API.Configuration.ColumnWriters;
using Serilog.Context;
using Microsoft.AspNetCore.HttpLogging;
using ETicaretAPI.API.Extentions;
using ETicaretAPI.SignalR;
using Microsoft.AspNetCore.Builder;
using ETicaretAPI.API.Filter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();//context.User.Identity.Name 
                                          //    Client'tan gelen request neticvesinde oluşturulan HttpContext nesnesine
                                          //  katmanlardaki class'lar üzerinden(busineess logic) erişebilmemizi saðlayan bir servistir.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddStorage<AzureStorage>();
builder.Services.AddSignalRServices();

//builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage(StorageType.Local);
//cors policy
builder.Services.AddCors(option => option.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
//seri Logs
Logger log= new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File("logs/log.txt")
                    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSql"),"logs",needAutoCreateTable:true,
                    columnOptions:new Dictionary<string, ColumnWriterBase>
                    {
                        { "message", new RenderedMessageColumnWriter() },
                        { "message_template",new MessageTemplateColumnWriter()},
                        {"level", new LevelColumnWriter() },
                        {"time_stamp",new TimestampColumnWriter()},
                        {"exception",new ExceptionColumnWriter()},
                        {"log_event",new LogEventSerializedColumnWriter() },
                        {"user_name",new UsernameColumnWriter()}, //bunu biz kendimiz oluşturduk
                    })
                    .Enrich.FromLogContext() //aşagıda yazdığımız contexten yararlanmasını istiyoruz
                    .MinimumLevel.Information()
                    .CreateLogger();
builder.Host.UseSerilog(log);


builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

//--------Validation Filter----
builder.Services.AddControllers(op =>
{
    op.Filters.Add<ValidationFilter>();
    op.Filters.Add<RolePermissionFilter>();
})
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer("Admin", option =>
    {
        option.TokenValidationParameters = new()
        {
            ValidateAudience = true,//oluşturulacak token değerini kimlerin / hangi orjinlerin / hangi sitelerin kullanacağını belirlediğimiz değerdir.
            ValidateIssuer = true, //oluşturduğumuz token değerini kimin dağıtacağını ifade edeceğimiz alandır.
            ValidateLifetime = true, //oluşturulan token değerinin süresini kontrol edecek olan doğrulamadır.
            ValidateIssuerSigningKey = true,// üretilecek token içerisine koyulan securtyKey dir.


            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securtyToken, validatoinParameters) => expires != null ? expires > DateTime.UtcNow : false, //token süresi bitince token ı durdurur. 
            //log
            NameClaimType=ClaimTypes.Name //jwt üzerinde NameClaimn e karşılık gelen değeri user.Identity.Name propertysinden elde edebiliriz
        ,};
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Exceptionları handle ettik
app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

//LOG
app.UseSerilogRequestLogging(); //bundan önceki olaylar loglanmayacak. sadece bundan sonraki olaylar loglanack
app.UseHttpLogging();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
//LOG =>burada  Authentication ve Authorization gerçekleştikten sonra username i alıp log context e push ediyoruz
app.Use(async (context,next) => {
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);
    await next();
});
app.UseStaticFiles();
app.MapControllers();
app.MapHups();
app.Run();
