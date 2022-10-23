using System;
using API.Configuration;
using API.Measurement.Repository;
using API.Measurement.Service;
using API.Consumers;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MassTransit;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mongoDbConfiguration = Configuration.GetSection("MongoDb").Get<MongoDbConfiguration>();
            var rabbitConfiguration = Configuration.GetSection("RabbitMQ").Get<RabbitMQConfiguration>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CoreTemperatureConsumer>();
                x.AddConsumer<PowerGeneratedConsumer>();
                x.AddConsumer<WaterUsageConsumer>();
                x.AddConsumer<TurbineRpmConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitConfiguration.ServerAddress), settings =>
                    {
                        settings.Username(rabbitConfiguration.Username);
                        settings.Password(rabbitConfiguration.Password);
                    });
                    
                    cfg.ReceiveEndpoint(QueueNames.CORE_TEMPERATURE, ep =>
                    {
                        ep.ConfigureConsumer<CoreTemperatureConsumer>(context);
                    });
                    cfg.ReceiveEndpoint(QueueNames.POWER_GENERATED, ep =>
                    {
                        ep.ConfigureConsumer<PowerGeneratedConsumer>(context);
                    });
                    cfg.ReceiveEndpoint(QueueNames.WATER_USAGE, ep =>
                    {
                        ep.ConfigureConsumer<WaterUsageConsumer>(context);
                    });
                    cfg.ReceiveEndpoint(QueueNames.TURBINE_RPM, ep =>
                    {
                        ep.ConfigureConsumer<TurbineRpmConsumer>(context);
                    });
                });
            });

            var mongoSettings = MongoClientSettings.FromConnectionString(mongoDbConfiguration.Connection);
            mongoSettings.ConnectTimeout = TimeSpan.FromSeconds(3);
            mongoSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(3);
            
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" }); });
            
            services.AddSingleton(new MongoClient(mongoSettings));
            services.AddScoped<MeasurementRepository>();
            services.AddScoped<MeasurementService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}