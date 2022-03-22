using Contacts.Domain.Entities;
using Contacts.Infrastructure.Constants;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Contacts.Infrastructure.Data
{
    public class ContactContextSeed
    {
        public static void SeedData(IMongoCollection<Contact> contacts)
        {
            bool isExistContact = contacts.Find(p => true).Any();
            if (!isExistContact)
                contacts.InsertManyAsync(GetConfigureContacts());
        }

        private static IEnumerable<Contact> GetConfigureContacts()
        {
            return new List<Contact>()
            {
                new Contact()
                {
                    Name = "Name1",
                    Surname = "Surname1",
                    Company = "Company1",
                    ContactInformation = null
                },
                new Contact()
                {
                    Name = "Name2",
                    Surname = "Surname2",
                    Company = "Company2",
                    ContactInformation = new List<ContactInfo>()
                    {
                        new ContactInfo()
                        {
                            InfoType = (int)ContactInfoTypes.Types.PhoneNumber,
                            Info = "+905554443322"
                        },
                        new ContactInfo()
                        {
                            InfoType = (int)ContactInfoTypes.Types.Location,
                            Info = "İstanbul"
                        }
                    }
                },
                new Contact()
                {
                    Name = "Name3",
                    Surname = "Surname3",
                    Company = "Company3",
                    ContactInformation = new List<ContactInfo>()
                    {
                        new ContactInfo()
                        {
                            InfoType = (int)ContactInfoTypes.Types.PhoneNumber,
                            Info = "+905554443301"
                        },
                        new ContactInfo()
                        {
                            InfoType = (int)ContactInfoTypes.Types.PhoneNumber,
                            Info = "+905554443302"
                        },
                        new ContactInfo()
                        {
                            InfoType = (int)ContactInfoTypes.Types.Location,
                            Info = "Kastamonu"
                        }
                    }
                },
                new Contact()
                {
                    Name = "Name4",
                    Surname = "Surname4",
                    Company = "Company4",
                    ContactInformation = new List<ContactInfo>()
                    {
                        new ContactInfo()
                        {
                            InfoType = (int)ContactInfoTypes.Types.Location,
                            Info = "Kastamonu"
                        }
                    }
                }
            };
        }
    }
}
