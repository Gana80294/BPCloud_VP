using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface ISubconRepository
    {
        List<BPCOFSubcon> GetSubconByDocAndPartnerID(string DocNumber, string PartnerID);
        List<BPCOFSubcon> GetSubconBySLAndPartnerID(string DocNumber, string Item, string SlLine, string PartnerID);
        Task CreateSubcon(List<BPCOFSubcon> subcons);
        Task DeleteSubcon(BPCOFSubcon subcon);
        List<BPCOFSubconView> GetSubconViewByDocAndPartnerID(string DocNumber, string PartnerID);
    }
}
