using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public interface IAIACTRepository
    {
        List<BPCAIACT> GetAllAIACTs();
        List<BPCAIACT> GetAIACTsByPartnerID(string PartnerID);
        List<BPCAIACT> GetActionsByPartnerID(string PartnerID);
        List<BPCAIACT> GetNotificationsByPartnerID(string PartnerID);
        Task<BPCAIACT> CreateAIACT(BPCAIACT AIACT);
        Task<BPCAIACT> CreateAIACTDetails(List<BPCAIACT> AIACTs);
        Task CreateAIACTs(List<BPCAIACT> AIACTs, string PartnerID);
        Task<BPCAIACT> UpdateAIACT(BPCAIACT AIACT);
        Task UpdateAIACTs(List<int> SeqNos);
        Task<BPCAIACT> DeleteAIACT(BPCAIACT AIACT);
        Task DeleteAIACTByPartnerID(string PartnerID);
        Task<BPCAIACT> AcceptAIACT(BPCAIACT AIACT);
        Task<BPCAIACT> AcceptAIACTs(List<BPCAIACT> AIACTs);
        Task<BPCAIACT> RejectAIACT(BPCAIACT AIACT);
    }
}
