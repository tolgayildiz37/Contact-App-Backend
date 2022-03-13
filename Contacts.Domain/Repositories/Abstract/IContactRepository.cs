﻿using Contacts.Domain.Entities;
using System.Threading.Tasks;

namespace Contacts.Domain.Repositories.Abstract
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<bool> AddInformation(Contact entity);
        Task<bool> DeleteInformation(Contact entity);
        Task<bool> DeleteAllInformation(string id);
    }
}
