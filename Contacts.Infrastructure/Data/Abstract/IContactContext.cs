using Contacts.Domain.Entities;
using MongoDB.Driver;

namespace Contacts.Infrastructure.Data.Abstract
{
    public interface IContactContext : IContext
    {
        IMongoCollection<Contact> Contacts { get; }
    }
}
