using BPCloud_VP.ReportService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public interface IIPRepository
    {
        List<BPCReportIP> GetAllReportIPByPartnerID(string PartnerID);
        List<BPCReportIP> GetFilteredReportIPByPartnerID(string PartnerID, string Material = null, string Method = null);

        Task CreateBPCPAYH(List<BPCPayment> data);
        Task UpdateBPCPAYH(List<BPCPayment> data);

        Task CreateBPCSCSTK(List<BPCSCSTK> data);
        Task UpdateBPCSCSTK(List<BPCSCSTK> data);
        List<BPCPayment> GetBPCPAYHByPartnerId(string partnerID);
    }
}
