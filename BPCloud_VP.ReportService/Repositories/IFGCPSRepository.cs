using BPCloud_VP.ReportService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public interface IFGCPSRepository
    {
        List<BPCReportFGCPS> GetAllReportFGCPSByPartnerID(string PartnerID);
        List<BPCReportFGCPS> GetFilteredReportFGCPSByPartnerID(string PartnerID, string Material = null, string MaterialText = null);
    }
}
