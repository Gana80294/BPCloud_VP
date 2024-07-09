using BPCloud_VP.ReportService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public interface IGRRRepository
    {
        List<BPCReportGRR> GetAllReportGRRByPartnerID(string PartnerID);
        List<BPCReportGRR> GetFilteredReportGRRByPartnerID(string PartnerID, string Material = null);
        //List<BPCReportGRR> GRRecieptListByPartnerID(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null);

    }
}
