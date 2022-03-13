using AutoMapper;
using Contacts.Application.Commands.Abstract;
using Contacts.Application.Commands.AddContact;
using Contacts.Application.Responses;
using Contacts.Domain.Entities;

namespace Contacts.Application.Mappers
{
    public class ContactMappingProfile : Profile
    {
        public ContactMappingProfile()
        {
            CreateMap<Contact, AddContactCommand>().ReverseMap();
            CreateMap<Contact, ContactResponse>().ReverseMap();
            CreateMap<ContactInfo, ContactInfoCommand>().ReverseMap();
            CreateMap<ContactInfo, ContactInfoResponse>().ReverseMap();
        }
    }
}
