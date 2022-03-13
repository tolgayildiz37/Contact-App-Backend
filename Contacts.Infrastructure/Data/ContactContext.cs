using Contacts.Domain.Entities;
using Contacts.Infrastructure.Data.Abstract;
using Contacts.Infrastructure.Settings.Abstract;
using MongoDB.Driver;

namespace Contacts.Infrastructure.Data
{
    public class ContactContext : IContactContext
    {
        public ContactContext(IContactDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Contacts = database.GetCollection<Contact>(settings.CollectionName);
            ContactContextSeed.SeedData(Contacts);
        }

        public IMongoCollection<Contact> Contacts { get; }
    }
}
