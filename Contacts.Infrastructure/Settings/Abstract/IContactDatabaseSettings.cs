namespace Contacts.Infrastructure.Settings.Abstract
{
    public interface IContactDatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; }
    }
}
