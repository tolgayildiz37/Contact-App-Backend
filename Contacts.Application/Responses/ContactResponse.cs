﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Application.Responses
{
    public class ContactResponse
    {
        public string Id { get; protected set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<ContactInfoResponse> ContactInformation { get; set; }
        public DateTime CreatedTime { get; protected set; }
    }
}
