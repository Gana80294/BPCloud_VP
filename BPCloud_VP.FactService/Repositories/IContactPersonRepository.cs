using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public interface IContactPersonRepository
    {
        List<BPCFactContactPerson> GetAllContactPersons();
        List<BPCFactContactPerson> GetContactPersonsByPartnerID(string PartnerID);
        Task<BPCFactContactPerson> CreateContactPerson(BPCFactContactPerson FactContactPerson);
        Task<BPCFactContactPerson> CreateContactPersons(List<BPCFactContactPerson> FactContactPerson);
        Task CreateContactPersons(List<BPCFactContactPerson> FactContactPersons, string PartnerID);
        Task<BPCFactContactPerson> UpdateContactPerson(BPCFactContactPerson FactContactPerson);
        Task<BPCFactContactPerson> DeleteContactPerson(BPCFactContactPerson FactContactPerson);
        Task DeleteContactPersonByPartner(string PartnerID);
    }
}
