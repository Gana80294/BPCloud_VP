using BPCloud_VP_POService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IGateRepository
    {
        Task CreateHoveringVechicles(BPCGateHoveringVechicles GateHV);
        Task CreateAllHoveringVechicles(List<BPCGateHoveringVechicles> GateHV);
        Task<BPCGateEntry> CreateGateEntryByAsnList(ASNListView Asn);
        Task CancelGateEntryByAsnList(ASNListView Asn);
        List<BPCGateHoveringVechicles> GetHoveringVechicleByPartnerId(string PartnerId);

    }
}
