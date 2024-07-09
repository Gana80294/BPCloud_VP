using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IAIACTRepository
    {
        List<BPCOFAIACT> GetAllAIACTs();
        List<BPCOFAIACT> GetAIACTsByPartnerID(string PartnerID);
        List<BPCOFAIACT> GetActionsByPartnerID(string PartnerID); 
        List<BPCOFAIACT> GetNotificationsByPartnerID(string PartnerID);
        Task<BPCOFAIACT> UpdateNotification(BPCOFAIACT AIACT);
        Task<BPCOFAIACT> CreateAIACT(BPCOFAIACT AIACT);
        Task CreateAIACTs(List<BPCOFAIACT> AIACTs, string PartnerID);
        Task<BPCOFAIACT> UpdateAIACT(BPCOFAIACT AIACT);
        Task<BPCOFAIACT> DeleteAIACT(BPCOFAIACT AIACT);
        Task DeleteAIACTByPartnerID(string PartnerID);
        Task<BPCOFAIACT> AcceptAIACT(BPCOFAIACT AIACT);
        Task<BPCOFAIACT> AcceptAIACTs(List<BPCOFAIACT> AIACTs);
        Task<BPCOFAIACT> RejectAIACT(BPCOFAIACT AIACT);
    }
}
