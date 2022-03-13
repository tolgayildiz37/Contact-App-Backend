using System.Collections.Generic;

namespace Contacts.Application.Commands.Abstract
{
    public class ContactCommand
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<ContactInfoCommand> ContactInformation { get; set; }
    }
}
