using EventBusRabbitMQ.Connection;
using EventBusRabbitMQ.Connection.Abstract;
using EventBusRabbitMQ.Producers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Reports.Application;
using Reports.Consumers;
using Reports.Extensions;
using Reports.Infrastructure;
using Reports.Infrastructure.Constants;
using Reports.Infrastructure.Settings;

namespace Reports
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
            services.AddControllers();

            #region Add Infrasucture Middleware
            services.Configure<ReportDbSettings>(Configuration.GetSection(DbSettingsConstants.DATABASE_CONFIG_NAME));
            services.AddInfrastructure(Configuration);
            #endregion

            #region Add Application Middleware
            services.AddAutoMapper(typeof(Startup));
            services.AddApplication();
            #endregion

            #region Swagger Dependencies
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Report API", Version = "v1" });
            });
            #endregion

            #region EventBus Dependencies
            services.AddSingleton<IRabbitMQPersistentConnection>(sp => {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:HostName"]
                };

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:UserName"]))
                {
                    factory.UserName = Configuration["EventBus:UserName"];
                }

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:Password"]))
                {
                    factory.Password = Configuration["EventBus:Password"];
                }

                var retryCount = 5;
                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:RetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBus:RetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            services.AddSingleton<RequestReportQueueConsumer>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region "EventBus Dependencies"
            app.UseRabbitMQListener();
            #endregion

            #region Swagger Dependencies
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report API V1");
            }); 
            #endregion
        }
    }
}
