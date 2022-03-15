using Contacts.Domain.Entities;
using Contacts.Domain.Repositories.Abstract;
using Contacts.Infrastructure.Data.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contacts.Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IContactContext _context;

        public ContactRepository(IContactContext context)
        {
            _context = context;
        }

        public async Task<Contact> Add(Contact entity)
        {
            await _context.Contacts.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> AddInformation(Contact entity)
        {
            var contact = await _context.Contacts.Find(c => c.Id.Equals(entity.Id)).FirstOrDefaultAsync();
            if (contact != null)
            {
                contact.ContactInformation.AddRange(entity.ContactInformation);

                return await Update(contact);
            }
            return false;
        }

        public async Task<bool> Delete(Contact entity)
        {
            var filter = Builders<Contact>.Filter.Eq(q => q.Id, entity.Id);
            var result = await _context.Contacts.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> DeleteInformation(Contact entity)
        {
            var contact = await _context.Contacts.Find(c => c.Id.Equals(entity.Id)).FirstOrDefaultAsync();
            if (contact != null)
            {
                foreach (var info in entity.ContactInformation)
                {
                    var deleted = contact.ContactInformation.Find(q => q.Info.Equals(info.Info) && q.InfoType == info.InfoType);
                    contact.ContactInformation.Remove(deleted);
                }

                return await Update(contact);
            }
            return false;
        }

        public async Task<bool> DeleteAllInformation(string id)
        {
            var contact = await _context.Contacts.Find(c => c.Id.Equals(id)).FirstOrDefaultAsync();
            if (contact != null)
            {
                contact.ContactInformation.Clear();

                return await Update(contact);
            }
            return false;
        }

        public async Task<Contact> Get(Expression<Func<Contact, bool>> filter)
        {
            return await _context.Contacts.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Contact>> GetAll(Expression<Func<Contact, bool>> filter = null)
        {
            return filter == null ?
                await _context.Contacts.Find(m => true).ToListAsync() :
                await _context.Contacts.Find(filter).ToListAsync();
        }

        public async Task<bool> Update(Contact entity)
        {
            var oldEntity = await Get(x => x.Id.Equals(entity.Id));
            if(oldEntity != null)
            {
                entity.CreatedTime = oldEntity.CreatedTime;
                entity.UpdatedTime = DateTime.Now;
            }
            var result = await _context.Contacts.ReplaceOneAsync(filter: q => q.Id.Equals(entity.Id), replacement: entity);
            return result.IsAcknowledged && result.MatchedCount > 0;
        }
    }
}
