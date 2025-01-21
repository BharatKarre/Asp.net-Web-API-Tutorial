using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Loggings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ILogger = CollegeApp.Loggings.ILogger;

namespace CollegeApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //We also default logging method provided by Asp.net Core, however it logs in 4 places 1. Console 2. Debug
            var builder = WebApplication.CreateBuilder(args);
            //If you want to limit the loggings place you can clear the logging
            builder.Logging.ClearProviders();
            //Add the logging place you want
            builder.Logging.AddConsole();
            //or
            builder.Logging.AddDebug(); 

            // Add services to the container.
            // Registered NewtonsoftJson library for HttpPatch API
            builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //We have 3 type of server to use DI 
            //1. AddScoped
            //2. AddSingleton
            //3. AddTransient
            //We need to configuration the Interface and the class we want in order to know the mechanism type we using for logging.
            //Now DI will do its Job and create insatnce of logtofile wherever Ilogger Intereface is used.
            // I can change the loggin mechanism by passing the 2nd parameter as a Log to DB or Log to Server memory instead of Log to File.
            builder.Services.AddScoped<ILogger, LogToFile>();
            builder.Services.AddSingleton<ILogger, LogToFile>();
            builder.Services.AddTransient<ILogger, LogToFile>();

            //Connection string to connect SQL server database
            builder.Services.AddDbContext<CollegeDBContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("Lala"));
            });

            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}