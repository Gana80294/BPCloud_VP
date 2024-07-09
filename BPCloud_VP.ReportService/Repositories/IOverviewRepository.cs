using BPCloud_VP.ReportService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public interface IOverviewRepository
    {
        List<BPCReportOV> GetOverviewReports(string PartnerID);
        List<BPCReportOV> GetOverviewReportByDate(OverviewReportOption overviewReportOption);
        List<BPCReportOV> GetOverviewReportByOption(OverviewReportOption overviewReportOption);
        List<BPCReportOV> GetOverviewReportByStatus(OverviewReportOption overviewReportOption);

    }
}
