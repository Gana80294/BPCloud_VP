using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IFLIPCostRepository
    {
        List<BPCFLIPCost> GetAllFLIPCosts();
        List<BPCFLIPCost> GetFLIPCostsByFLIPID(string FLIPID);
        Task<BPCFLIPCost> CreateFLIPCost(BPCFLIPCost FLIPCost);
        Task CreateFLIPCosts(List<BPCFLIPCost> FLIPCosts, string FLIPID);
        Task<BPCFLIPCost> UpdateFLIPCost(BPCFLIPCost FLIPCost);
        Task<BPCFLIPCost> DeleteFLIPCost(BPCFLIPCost FLIPCost);
        Task DeleteFLIPCostByFLIPID(string FLIPID);
    }
}
