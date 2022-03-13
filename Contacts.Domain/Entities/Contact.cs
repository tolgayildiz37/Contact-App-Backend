using Contacts.Domain.Entities.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Contacts.Domain.Entities
{
    public class Contact : Entity
    {
        [BsonElement("Name")]
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<ContactInfo> ContactInformation { get; set; }
    }
}
