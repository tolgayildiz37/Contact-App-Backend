using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Reports.Domain.Repositories.Abstract;
using Reports.Infrastructure.Data;
using Reports.Infrastructure.Data.Abstract;
using Reports.Infrastructure.Repositories;
using Reports.Infrastructure.Settings;
using Reports.Infrastructure.Settings.Abstract;

namespace Reports.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database Configuration Dependencies
            services.AddSingleton<IReportDbSettings>(sp => sp.GetRequiredService<IOptions<ReportDbSettings>>().Value);
            #endregion

            #region DTO Configuration Dependencies
            services.AddTransient<IReportContext, ReportContext>();
            services.AddTransient<IReportRepository, ReportRepository>();
            #endregion

            return services;
        }
    }
}
