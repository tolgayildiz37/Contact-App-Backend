using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reports.Consumers;

namespace Reports.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static RequestReportQueueConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitMQListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<RequestReportQueueConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(Stopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void Stopping()
        {
            Listener.Disconnect();
        }
    }
}
