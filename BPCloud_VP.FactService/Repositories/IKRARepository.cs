using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public interface IKRARepository
    {
        List<BPCKRA> GetAllKRAs();
        List<BPCKRA> GetKRAsByPartnerID(string PartnerID);
        Task<BPCKRA> CreateKRA(BPCKRA KRA);
        Task<BPCKRA> CreateKRAs(List<BPCKRA> KRAs);
        Task CreateKRAs(List<BPCKRA> KRAs,string PartnerID);
        Task<BPCKRA> UpdateKRA(BPCKRA KRA);
        Task<BPCKRA> DeleteKRA(BPCKRA KRA);
        Task DeleteKRAByPartnerID(string PartnerID);
    }
}
