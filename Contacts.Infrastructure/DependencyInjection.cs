using Contacts.Domain.Repositories.Abstract;
using Contacts.Infrastructure.Data;
using Contacts.Infrastructure.Data.Abstract;
using Contacts.Infrastructure.Repositories;
using Contacts.Infrastructure.Settings;
using Contacts.Infrastructure.Settings.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Contacts.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database Configuration Dependencies
            services.AddSingleton<IContactDatabaseSettings>(sp => sp.GetRequiredService<IOptions<ContactDatabaseSettings>>().Value);
            #endregion

            #region DTO Configuration Dependencies
            services.AddTransient<IContactContext, ContactContext>();
            services.AddTransient<IContactRepository, ContactRepository>();
            #endregion

            return services;
        }
    }
}
