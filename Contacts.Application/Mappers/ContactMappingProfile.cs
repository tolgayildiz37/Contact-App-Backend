using AutoMapper;
using Contacts.Application.Commands.Abstract;
using Contacts.Application.Commands.AddContact;
using Contacts.Application.Commands.AddContactInfo;
using Contacts.Application.Commands.DeleteAllContactInfo;
using Contacts.Application.Commands.DeleteContact;
using Contacts.Application.Commands.DeleteContactInfo;
using Contacts.Application.Commands.UpdateContact;
using Contacts.Application.Responses;
using Contacts.Domain.Entities;

namespace Contacts.Application.Mappers
{
    public class ContactMappingProfile : Profile
    {
        public ContactMappingProfile()
        {
            CreateMap<Contact, AddContactCommand>().ReverseMap();
            CreateMap<Contact, AddContactInfoCommand>().ReverseMap();
            CreateMap<Contact, UpdateContactCommand>().ReverseMap();
            CreateMap<Contact, DeleteContactCommand>().ReverseMap();
            CreateMap<Contact, DeleteContactInfoCommand>().ReverseMap();
            CreateMap<Contact, DeleteAllContactInfoCommand>().ReverseMap();
            CreateMap<Contact, ContactResponse>().ReverseMap();
            CreateMap<ContactInfo, ContactInfoCommand>().ReverseMap();
            CreateMap<ContactInfo, ContactInfoResponse>().ReverseMap();
        }
    }
}
