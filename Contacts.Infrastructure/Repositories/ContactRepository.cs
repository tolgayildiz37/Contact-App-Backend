using Contacts.Domain.Entities;
using Contacts.Domain.Repositories.Abstract;
using Contacts.Infrastructure.Data.Abstract;
using Contacts.Infrastructure.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contacts.Infrastructure.Repositories
{
    public class ContactRepository : MongoDbRepository<Contact>, IContactRepository
    {
        private readonly IContactContext _context;

        public ContactRepository(IContactContext context) : base(context.Contacts)
        {
            _context = context;
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

        public async Task<IEnumerable<ReportData>> CreateReport()
        {
            List<ReportData> result = new List<ReportData>();
            var addFieldsStage = BsonDocument.Parse("{$addFields:{country:" +
                "{$arrayElemAt:[{$filter:{input:\"$ContactInformation\",as:\"info\"," +
                "cond:{$eq:[\"$$info.InfoType\",3]}}},0]}," +
                "ContactInformation:{$ifNull:[\"$ContactInformation\",[]]}}}");

            var groupStage = BsonDocument.Parse("{$group:" +
                "{_id:\"$country.Info\",personCount:{$sum:1}," +
                "phoneNumberCount:{$sum:{$size:{$filter:{input:\"$ContactInformation\",as:\"info\",cond:{$eq:[\"$$info.InfoType\",1]}}}}}}}");

            var pipeline = new BsonDocument[]
            {
                addFieldsStage,
                groupStage
            };

            var queryResult = await _context.Contacts.Aggregate<BsonDocument>(pipeline).ToListAsync();
            if (queryResult.Count > 0)
            {
                foreach (var item in queryResult)
                {
                    if (!item.GetValue("_id", "").IsBsonNull)
                    {
                        result.Add(new ReportData()
                        {
                            Location = item.GetValue("_id", "").ToString(),
                            PersonCount = item.GetValue("personCount", 0).ToInt32(),
                            PhoneNumberCount = item.GetValue("phoneNumberCount", 0).ToInt32()
                        });
                    }
                }
            }

            return result;
        }
    }
}
