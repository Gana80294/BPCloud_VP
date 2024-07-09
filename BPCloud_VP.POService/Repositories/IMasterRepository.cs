using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IMasterRepository
    {
        List<BPCCountryMaster> GetAllBPCCountryMasters();
        List<BPCCurrencyMaster> GetAllBPCCurrencyMasters();
        List<BPCDocumentCenterMaster> GetAllDocumentCenterMaster();
        Task<BPCDocumentCenterMaster> CreateDocumentCenterMaster(BPCDocumentCenterMaster BPCDocumentCenterMaster);
        Task<BPCDocumentCenterMaster> UpdateDocumentCenterMaster(BPCDocumentCenterMaster BPCDocumentCenterMaster);
        Task<BPCDocumentCenterMaster> DeleteDocumentCenterMaster(BPCDocumentCenterMaster BPCDocumentCenterMaster);
        List<BPCReasonMaster> GetAllReasonMaster();
        List<BPCExpenseTypeMaster> GetExpenseTypeMasters();
        Task<BPCExpenseTypeMaster> CreateExpenseTypeMaster(BPCExpenseTypeMaster bPCExpenseTypeMaster);
        Task<BPCExpenseTypeMaster> UpdateExpenseTypeMaster(BPCExpenseTypeMaster bPCExpenseTypeMaster);
        Task<BPCExpenseTypeMaster> DeleteExpenseTypeMaster(BPCExpenseTypeMaster bPCExpenseTypeMaster);
        List<BPCTaxTypeMaster> GetTaxTypeMasters();
        Task<BPCTaxTypeMaster> CreateTaxTypeMaster(BPCTaxTypeMaster bPCTaxTypeMaster);
        Task<BPCTaxTypeMaster> UpdateTaxTypeMaster(BPCTaxTypeMaster bPCTaxTypeMaster);
        Task<BPCTaxTypeMaster> DeleteTaxTypeMaster(BPCTaxTypeMaster bPCTaxTypeMaster);

        List<BPCHSNMaster> GetAllHSNMasters();
        Task<BPCHSNMaster> CreateHSNMaster(BPCHSNMaster bPCHSNMaster);
        Task CreateHSNMasters(List<BPCHSNMaster> BPCHSNMasters);
        Task<BPCHSNMaster> UpdateHSNMaster(BPCHSNMaster bPCHSNMaster);
        Task<BPCHSNMaster> DeleteHSNMaster(BPCHSNMaster bPCHSNMaster);

        List<BPCProfitCentreMaster> GetAllProfitCentreMasters();
        Task<BPCProfitCentreMaster> CreateProfitCentreMaster(BPCProfitCentreMaster bPCProfitCentreMaster);
        Task CreateProfitCentreMasters(List<BPCProfitCentreMaster> BPCProfitCentreMasters);
        Task<BPCProfitCentreMaster> UpdateProfitCentreMaster(BPCProfitCentreMaster bPCProfitCentreMaster);
        Task<BPCProfitCentreMaster> DeleteProfitCentreMaster(BPCProfitCentreMaster bPCProfitCentreMaster);


        List<BPCCompanyMaster> GetAllCompanyMasters();
        Task<BPCCompanyMaster> CreateCompanyMaster(BPCCompanyMaster bPCCompanyMaster);
        Task CreateCompanyMasters(List<BPCCompanyMaster> BPCCompanyMasters);
        Task<BPCCompanyMaster> UpdateCompanyMaster(BPCCompanyMaster bPCCompanyMaster);
        Task<BPCCompanyMaster> DeleteCompanyMaster(BPCCompanyMaster bPCCompanyMaster);

        List<BPTicketStatus> GetAllTicketStatus();
        #region user palnt getting
        List<BPCPlantMaster> GetAllPlant();
        Task<BPCPlantMaster> CreatePlantMaster(BPCPlantMaster BPCPlantMaster);
        Task<BPCPlantMaster> UpdatePlantMaster(BPCPlantMaster BPCPlantMaster);
        Task<BPCPlantMaster> DeletePlantMaster(BPCPlantMaster BPCPlantMaster);

        #endregion

    }
}
