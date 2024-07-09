using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public interface IBankRepository
    {
        List<BPCFactBank> GetAllBanks();
        List<BPCFactBank> GetBanksByPartnerID(string PartnerID);
        Task<BPCFactBank> CreateBank(BPCFactBank Bank);
        Task<BPCFactBank> CreateBanks(List<BPCFactBank> Banks);
        Task<BPCFactBankSupport> CreateSupportBank(BPCFactBankSupport Bank);
        Task CreateBanks(List<BPCFactBank> Banks, string PartnerID);
        Task<BPCFactBank> UpdateBank(BPCFactBank Bank);
        Task<BPCFactBank> DeleteBank(BPCFactBank Fact);
        Task DeleteBankByPartnerID(string PartnerID);
        Task CreateBanks(List<BPCFactBankXLSX> Banks);
        List<BPCFactBankSupport> GetSupportBanks(string partnerID);

    }
}
