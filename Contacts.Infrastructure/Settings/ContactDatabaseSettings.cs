using Contacts.Infrastructure.Constants;
using Contacts.Infrastructure.Settings.Abstract;

namespace Contacts.Infrastructure.Settings
{
    public class ContactDatabaseSettings : IContactDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; }

        public ContactDatabaseSettings()
        {
            CollectionName = DbSettingsConstants.COLLECTION_CONTACT;
        }
    }
}
