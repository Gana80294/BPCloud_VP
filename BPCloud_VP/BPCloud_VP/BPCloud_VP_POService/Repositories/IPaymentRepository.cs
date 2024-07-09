using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IPaymentRepository
    {
        Task<BPCPayAccountStatement> CreateAccountStatements(List<BPCPayAccountStatement> AccountStatements);
        Task<BPCPayAccountStatement> UpdateAccountStatement(BPCPayAccountStatement AccountStatement);
        List<BPCPayAccountStatement> GetAccountStatementByPartnerID(string PartnerID);
        List<BPCPayAccountStatement> FilterAccountStatementByPartnerID(string PartnerID, string DocumentID = null, string ProfitCenter = null, DateTime? FromDate = null, DateTime? ToDate = null);
        List<BPCPayPayable> GetPayableByPartnerID(string PartnerID);
        List<BPCPayPayable> FilterPayableByPartnerID(string PartnerID, string AccountStatement = null, DateTime? FromDate = null, DateTime? ToDate = null);
        List<BPCPayPayment> GetPaymentByPartnerID(string PartnerID);
        List<BPCPayPayment> FilterPaymentByPartnerID(string PartnerID,DateTime? FromDate = null, DateTime? ToDate = null);
        List<BPCPayTDS> GetTDSByPartnerID(string PartnerID);
        List<BPCPayTDS> FilterTDSByPartnerID(string PartnerID, DateTime? FromDate = null, DateTime? ToDate = null);

        Task<List<BPCPayAccountStatement>> AcceptBC(List<BPCPayAccountStatement> accountStatement);
    }
}
