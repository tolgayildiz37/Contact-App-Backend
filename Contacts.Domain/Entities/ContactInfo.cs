using Contacts.Domain.Entities.Abstract;

namespace Contacts.Domain.Entities
{
    public class ContactInfo : Entity
    {
        public int InfoType { get; set; }
        public string Info { get; set; }
    }
}
