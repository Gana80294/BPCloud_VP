using BPCloud_VP.ReportService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public interface IPPMRepository
    {
        List<BPCReportPPMHeader> GetPPMReports(string PartnerID);
        List<BPCReportPPMHeader> GetPPMReportByDate(PPMReportOption ppmReportOption);
        List<BPCReportPPMItem> GetPPMItemReportByPeriod(string PartnerID, DateTime? period = null);
        List<BPCReportPPMHeader> GetPPMReportByStatus(PPMReportOption ppmReportOption);
    }
}
