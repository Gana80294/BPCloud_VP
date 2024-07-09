using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using Microsoft.Extensions.Configuration;

namespace BPCloud_VP_POService.Repositories
{
    public class MasterRepository : IMasterRepository
    {
        private readonly POContext _dbContext;
        PORepository poRepository;
        IConfiguration _configuration;

        public MasterRepository(POContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            poRepository = new PORepository(_dbContext, _configuration);
        }
        public List<BPCCountryMaster> GetAllBPCCountryMasters()
        {
            try
            {
                return _dbContext.BPCCountryMasters.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetAllBPCCountryMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetAllBPCCountryMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCCurrencyMaster> GetAllBPCCurrencyMasters()
        {
            try
            {
                return _dbContext.BPCCurrencyMasters.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetAllBPCCurrencyMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetAllBPCCurrencyMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCDocumentCenterMaster> GetAllDocumentCenterMaster()
        {
            try
            {
                return _dbContext.BPCDocumentCenterMasters.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetAllDocumentCenterMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetAllDocumentCenterMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region  user palnt getting
        public List<BPCPlantMaster> GetAllPlant()
        {
            try
            {
                return _dbContext.BPCPlantMasters.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetAllPlant", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetAllPlant", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCPlantMaster> CreatePlantMaster(BPCPlantMaster BPCPlantMaster)
        {
            BPCPlantMaster BPCPlantMasterResult = new BPCPlantMaster();
            try
            {
                BPCPlantMaster Master1 = (from tb in _dbContext.BPCPlantMasters
                                          where tb.IsActive && tb.PlantCode == BPCPlantMaster.PlantCode
                                          select tb).FirstOrDefault();
                if (Master1 == null)
                {
                    BPCPlantMaster.CreatedOn = DateTime.Now;
                    BPCPlantMaster.IsActive = true;
                    var result = _dbContext.BPCPlantMasters.Add(BPCPlantMaster);
                    await _dbContext.SaveChangesAsync();

                    BPCPlantMasterResult = result.Entity;
                }
                else
                {
                    throw new Exception("Plant with same code already exists");
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreatePlantMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreatePlantMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreatePlantMaster : - ", ex);
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return BPCPlantMasterResult;
        }
        public async Task<BPCPlantMaster> UpdatePlantMaster(BPCPlantMaster BPCPlantMaster)
        {
            BPCPlantMaster BPCPlantMasterResult = new BPCPlantMaster();
            try
            {
                BPCPlantMaster BPCPlantMaster2 = (from tb in _dbContext.BPCPlantMasters
                                                  where tb.IsActive && tb.PlantCode == BPCPlantMaster.PlantCode
                                                  select tb).FirstOrDefault();
                if (BPCPlantMaster2 != null)
                {
                    BPCPlantMaster2.PlantText = BPCPlantMaster.PlantText;
                    BPCPlantMaster2.AddressLine1 = BPCPlantMaster.AddressLine1;
                    BPCPlantMaster2.AddressLine2 = BPCPlantMaster.AddressLine2;
                    BPCPlantMaster2.City = BPCPlantMaster.City;
                    BPCPlantMaster2.State = BPCPlantMaster.State;
                    BPCPlantMaster2.Country = BPCPlantMaster.Country;
                    BPCPlantMaster2.PinCode = BPCPlantMaster.PinCode;
                    BPCPlantMaster2.IsActive = true;
                    BPCPlantMaster2.ModifiedOn = DateTime.Now;
                    BPCPlantMaster2.ModifiedBy = BPCPlantMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    BPCPlantMasterResult = BPCPlantMaster2;
                }
                else
                {
                    return BPCPlantMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/UpdatePlantMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/UpdatePlantMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdatePlantMaster : - ", ex);
                //throw ex;
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return BPCPlantMasterResult;
        }
        public async Task<BPCPlantMaster> DeletePlantMaster(BPCPlantMaster BPCPlantMaster)
        {
            BPCPlantMaster BPCPlantMasterResult = new BPCPlantMaster();
            try
            {
                BPCPlantMaster BPCPlantMaster1 = (from tb in _dbContext.BPCPlantMasters
                                                  where tb.IsActive && tb.PlantCode == BPCPlantMaster.PlantCode
                                                  select tb).FirstOrDefault();
                if (BPCPlantMaster1 != null)
                {
                    var res = _dbContext.BPCPlantMasters.Remove(BPCPlantMaster1);
                    await _dbContext.SaveChangesAsync();
                    BPCPlantMasterResult = res.Entity;
                }
                else
                {
                    return BPCPlantMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/DeletePlantMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/DeletePlantMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeletePlantMaster : - ", ex);
                //throw ex;
                return null;
            }
            return BPCPlantMasterResult;
        }

        #endregion
        public async Task<BPCDocumentCenterMaster> CreateDocumentCenterMaster(BPCDocumentCenterMaster BPCDocumentCenterMaster)
        {
            BPCDocumentCenterMaster BPCDocumentCenterMasterResult = new BPCDocumentCenterMaster();
            try
            {
                BPCDocumentCenterMaster Master1 = (from tb in _dbContext.BPCDocumentCenterMasters
                                                   where tb.IsActive && tb.DocumentType == BPCDocumentCenterMaster.DocumentType
                                                   select tb).FirstOrDefault();
                if (Master1 == null)
                {
                    int ID = (from tb in _dbContext.BPCDocumentCenterMasters
                              orderby tb.AppID descending
                              select tb.AppID).FirstOrDefault();
                    BPCDocumentCenterMaster.AppID = ID + 1;
                    BPCDocumentCenterMaster.CreatedOn = DateTime.Now;
                    BPCDocumentCenterMaster.IsActive = true;
                    var result = _dbContext.BPCDocumentCenterMasters.Add(BPCDocumentCenterMaster);
                    await _dbContext.SaveChangesAsync();

                    BPCDocumentCenterMasterResult.DocumentType = BPCDocumentCenterMaster.DocumentType;
                    BPCDocumentCenterMasterResult.Text = BPCDocumentCenterMaster.Text;
                    BPCDocumentCenterMasterResult.Mandatory = BPCDocumentCenterMaster.Mandatory;
                    BPCDocumentCenterMasterResult.Extension = BPCDocumentCenterMaster.Extension;
                    BPCDocumentCenterMasterResult.SizeInKB = BPCDocumentCenterMaster.SizeInKB;
                    BPCDocumentCenterMasterResult.ForwardMail = BPCDocumentCenterMaster.ForwardMail;
                }
                else
                {
                    return BPCDocumentCenterMasterResult;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateDocumentCenterMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateDocumentCenterMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateDocumentCenterMaster : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return BPCDocumentCenterMasterResult;
        }
        public async Task<BPCDocumentCenterMaster> UpdateDocumentCenterMaster(BPCDocumentCenterMaster BPCDocumentCenterMaster)
        {
            BPCDocumentCenterMaster BPCDocumentCenterMasterResult = new BPCDocumentCenterMaster();
            try
            {
                BPCDocumentCenterMaster BPCDocumentCenterMaster1 = (from tb in _dbContext.BPCDocumentCenterMasters
                                                                    where tb.IsActive && tb.DocumentType == BPCDocumentCenterMaster.DocumentType && tb.AppID != BPCDocumentCenterMaster.AppID
                                                                    select tb).FirstOrDefault();
                if (BPCDocumentCenterMaster1 == null)
                {
                    BPCDocumentCenterMaster BPCDocumentCenterMaster2 = (from tb in _dbContext.BPCDocumentCenterMasters
                                                                        where tb.IsActive && tb.AppID == BPCDocumentCenterMaster.AppID
                                                                        select tb).FirstOrDefault();
                    //BPCDocumentCenterMaster2.DocumentType = BPCDocumentCenterMaster.DocumentType;
                    BPCDocumentCenterMaster2.Text = BPCDocumentCenterMaster.Text;
                    BPCDocumentCenterMaster2.Mandatory = BPCDocumentCenterMaster.Mandatory;
                    BPCDocumentCenterMaster2.Extension = BPCDocumentCenterMaster.Extension;
                    BPCDocumentCenterMaster2.SizeInKB = BPCDocumentCenterMaster.SizeInKB;
                    BPCDocumentCenterMaster2.ForwardMail = BPCDocumentCenterMaster.ForwardMail;
                    BPCDocumentCenterMaster2.IsActive = true;
                    BPCDocumentCenterMaster2.ModifiedOn = DateTime.Now;
                    BPCDocumentCenterMaster2.ModifiedBy = BPCDocumentCenterMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    BPCDocumentCenterMasterResult.AppID = BPCDocumentCenterMaster.AppID;
                    BPCDocumentCenterMasterResult.DocumentType = BPCDocumentCenterMaster.DocumentType;
                    BPCDocumentCenterMasterResult.Text = BPCDocumentCenterMaster.Text;
                    BPCDocumentCenterMasterResult.Mandatory = BPCDocumentCenterMaster.Mandatory;
                    BPCDocumentCenterMasterResult.Extension = BPCDocumentCenterMaster.Extension;
                    BPCDocumentCenterMasterResult.SizeInKB = BPCDocumentCenterMaster.SizeInKB;
                    BPCDocumentCenterMasterResult.ForwardMail = BPCDocumentCenterMaster.ForwardMail;
                }
                else
                {
                    return BPCDocumentCenterMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/UpdateDocumentCenterMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/UpdateDocumentCenterMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdateDocumentCenterMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return BPCDocumentCenterMasterResult;
        }
        public async Task<BPCDocumentCenterMaster> DeleteDocumentCenterMaster(BPCDocumentCenterMaster BPCDocumentCenterMaster)
        {
            BPCDocumentCenterMaster BPCDocumentCenterMasterResult = new BPCDocumentCenterMaster();
            try
            {
                BPCDocumentCenterMaster BPCDocumentCenterMaster1 = (from tb in _dbContext.BPCDocumentCenterMasters
                                                                    where tb.IsActive && tb.AppID == BPCDocumentCenterMaster.AppID
                                                                    select tb).FirstOrDefault();
                if (BPCDocumentCenterMaster1 != null)
                {
                    _dbContext.BPCDocumentCenterMasters.Remove(BPCDocumentCenterMaster1);
                    await _dbContext.SaveChangesAsync();
                    BPCDocumentCenterMasterResult.AppID = BPCDocumentCenterMaster.AppID;
                    BPCDocumentCenterMasterResult.DocumentType = BPCDocumentCenterMaster.DocumentType;
                    BPCDocumentCenterMasterResult.Text = BPCDocumentCenterMaster.Text;
                    BPCDocumentCenterMasterResult.Mandatory = BPCDocumentCenterMaster.Mandatory;
                    BPCDocumentCenterMasterResult.Extension = BPCDocumentCenterMaster.Extension;
                    BPCDocumentCenterMasterResult.SizeInKB = BPCDocumentCenterMaster.SizeInKB;
                    BPCDocumentCenterMasterResult.ForwardMail = BPCDocumentCenterMaster.ForwardMail;
                }
                else
                {
                    return BPCDocumentCenterMasterResult;
                }
                //BPCDocumentCenterMaster1.IsActive = false;
                //BPCDocumentCenterMaster1.ModifiedOn = DateTime.Now;
                //BPCDocumentCenterMaster1.ModifiedBy = BPCDocumentCenterMaster.ModifiedBy;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/DeleteDocumentCenterMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/DeleteDocumentCenterMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/DeleteDocumentCenterMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return BPCDocumentCenterMasterResult;
        }
        public List<BPCReasonMaster> GetAllReasonMaster()
        {
            try
            {
                return _dbContext.BPCReasonMasters.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetAllReasonMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetAllReasonMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCExpenseTypeMaster> GetExpenseTypeMasters()
        {
            try
            {
                return _dbContext.BPCExpenseTypeMasters.Distinct().ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetExpenseTypeMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetExpenseTypeMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCExpenseTypeMaster> CreateExpenseTypeMaster(BPCExpenseTypeMaster BPCExpenseTypeMaster)
        {
            BPCExpenseTypeMaster BPCExpenseTypeMasterResult = new BPCExpenseTypeMaster();
            try
            {
                BPCExpenseTypeMaster Master1 = (from tb in _dbContext.BPCExpenseTypeMasters
                                                where tb.IsActive && tb.ExpenseType == BPCExpenseTypeMaster.ExpenseType
                                                select tb).FirstOrDefault();
                if (Master1 == null)
                {
                    BPCExpenseTypeMaster.CreatedOn = DateTime.Now;
                    BPCExpenseTypeMaster.IsActive = true;
                    var result = _dbContext.BPCExpenseTypeMasters.Add(BPCExpenseTypeMaster);
                    await _dbContext.SaveChangesAsync();

                    BPCExpenseTypeMasterResult.ExpenseType = BPCExpenseTypeMaster.ExpenseType;
                    BPCExpenseTypeMasterResult.MaxAmount = BPCExpenseTypeMaster.MaxAmount;
                    BPCExpenseTypeMasterResult.Client = BPCExpenseTypeMaster.Client;
                    BPCExpenseTypeMasterResult.Company = BPCExpenseTypeMaster.Company;
                    BPCExpenseTypeMasterResult.Type = BPCExpenseTypeMaster.Type;
                    BPCExpenseTypeMasterResult.PatnerID = BPCExpenseTypeMaster.PatnerID;
                }
                else
                {
                    return BPCExpenseTypeMasterResult;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateExpenseTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateExpenseTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateExpenseTypeMaster : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return BPCExpenseTypeMasterResult;
        }
        public async Task<BPCExpenseTypeMaster> UpdateExpenseTypeMaster(BPCExpenseTypeMaster BPCExpenseTypeMaster)
        {
            BPCExpenseTypeMaster BPCExpenseTypeMasterResult = new BPCExpenseTypeMaster();
            try
            {
                BPCExpenseTypeMaster BPCExpenseTypeMaster2 = (from tb in _dbContext.BPCExpenseTypeMasters
                                                              where tb.IsActive && tb.ExpenseType == BPCExpenseTypeMaster.ExpenseType
                                                              select tb).FirstOrDefault();
                if (BPCExpenseTypeMaster2 != null)
                {
                    BPCExpenseTypeMaster2.ExpenseType = BPCExpenseTypeMaster.ExpenseType;
                    BPCExpenseTypeMaster2.MaxAmount = BPCExpenseTypeMaster.MaxAmount;
                    BPCExpenseTypeMaster2.Client = BPCExpenseTypeMaster.Client;
                    BPCExpenseTypeMaster2.Company = BPCExpenseTypeMaster.Company;
                    BPCExpenseTypeMaster2.Type = BPCExpenseTypeMaster.Type;
                    BPCExpenseTypeMaster2.PatnerID = BPCExpenseTypeMaster.PatnerID;
                    BPCExpenseTypeMaster2.IsActive = true;
                    BPCExpenseTypeMaster2.ModifiedOn = DateTime.Now;
                    BPCExpenseTypeMaster2.ModifiedBy = BPCExpenseTypeMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    BPCExpenseTypeMasterResult.ExpenseType = BPCExpenseTypeMaster.ExpenseType;
                    BPCExpenseTypeMasterResult.MaxAmount = BPCExpenseTypeMaster.MaxAmount;
                    BPCExpenseTypeMasterResult.Client = BPCExpenseTypeMaster.Client;
                    BPCExpenseTypeMasterResult.Company = BPCExpenseTypeMaster.Company;
                    BPCExpenseTypeMasterResult.Type = BPCExpenseTypeMaster.Type;
                    BPCExpenseTypeMasterResult.PatnerID = BPCExpenseTypeMaster.PatnerID;
                }
                else
                {
                    return BPCExpenseTypeMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/UpdateExpenseTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/UpdateExpenseTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdateExpenseTypeMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return BPCExpenseTypeMasterResult;
        }
        public async Task<BPCExpenseTypeMaster> DeleteExpenseTypeMaster(BPCExpenseTypeMaster BPCExpenseTypeMaster)
        {
            BPCExpenseTypeMaster BPCExpenseTypeMasterResult = new BPCExpenseTypeMaster();
            try
            {
                BPCExpenseTypeMaster BPCExpenseTypeMaster1 = (from tb in _dbContext.BPCExpenseTypeMasters
                                                              where tb.IsActive && tb.ExpenseType == BPCExpenseTypeMaster.ExpenseType
                                                              select tb).FirstOrDefault();
                if (BPCExpenseTypeMaster1 != null)
                {
                    _dbContext.BPCExpenseTypeMasters.Remove(BPCExpenseTypeMaster1);
                    await _dbContext.SaveChangesAsync();
                    BPCExpenseTypeMasterResult.ExpenseType = BPCExpenseTypeMaster.ExpenseType;
                    BPCExpenseTypeMasterResult.MaxAmount = BPCExpenseTypeMaster.MaxAmount;
                    BPCExpenseTypeMasterResult.Client = BPCExpenseTypeMaster.Client;
                    BPCExpenseTypeMasterResult.Company = BPCExpenseTypeMaster.Company;
                    BPCExpenseTypeMasterResult.Type = BPCExpenseTypeMaster.Type;
                    BPCExpenseTypeMasterResult.PatnerID = BPCExpenseTypeMaster.PatnerID;
                }
                else
                {
                    return BPCExpenseTypeMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/DeleteExpenseTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/DeleteExpenseTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteExpenseTypeMaster : - ", ex);
            }
            return BPCExpenseTypeMasterResult;
        }

        public List<BPCTaxTypeMaster> GetTaxTypeMasters()
        {
            try
            {
                return _dbContext.BPCTaxTypeMasters.Distinct().ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetTaxTypeMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetTaxTypeMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCTaxTypeMaster> CreateTaxTypeMaster(BPCTaxTypeMaster BPCTaxTypeMaster)
        {
            BPCTaxTypeMaster BPCTaxTypeMasterResult = new BPCTaxTypeMaster();
            try
            {
                BPCTaxTypeMaster Master1 = (from tb in _dbContext.BPCTaxTypeMasters
                                            where tb.IsActive && tb.TaxType == BPCTaxTypeMaster.TaxType
                                            select tb).FirstOrDefault();
                if (Master1 == null)
                {
                    BPCTaxTypeMaster.CreatedOn = DateTime.Now;
                    BPCTaxTypeMaster.IsActive = true;
                    var result = _dbContext.BPCTaxTypeMasters.Add(BPCTaxTypeMaster);
                    await _dbContext.SaveChangesAsync();

                    BPCTaxTypeMasterResult.TaxType = BPCTaxTypeMaster.TaxType;
                    //BPCTaxTypeMasterResult.MaxAmount = BPCTaxTypeMaster.MaxAmount;
                    //BPCTaxTypeMasterResult.Client = BPCTaxTypeMaster.Client;
                    //BPCTaxTypeMasterResult.Company = BPCTaxTypeMaster.Company;
                    //BPCTaxTypeMasterResult.Type = BPCTaxTypeMaster.Type;
                    //BPCTaxTypeMasterResult.PatnerID = BPCTaxTypeMaster.PatnerID;
                }
                else
                {
                    return BPCTaxTypeMasterResult;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateTaxTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateTaxTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateTaxTypeMaster : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return BPCTaxTypeMasterResult;
        }
        public async Task<BPCTaxTypeMaster> UpdateTaxTypeMaster(BPCTaxTypeMaster BPCTaxTypeMaster)
        {
            BPCTaxTypeMaster BPCTaxTypeMasterResult = new BPCTaxTypeMaster();
            try
            {
                BPCTaxTypeMaster BPCTaxTypeMaster2 = (from tb in _dbContext.BPCTaxTypeMasters
                                                      where tb.IsActive && tb.TaxType == BPCTaxTypeMaster.TaxType
                                                      select tb).FirstOrDefault();
                if (BPCTaxTypeMaster2 != null)
                {
                    BPCTaxTypeMaster2.TaxType = BPCTaxTypeMaster.TaxType;
                    //BPCTaxTypeMaster2.MaxAmount = BPCTaxTypeMaster.MaxAmount;
                    //BPCTaxTypeMaster2.Client = BPCTaxTypeMaster.Client;
                    //BPCTaxTypeMaster2.Company = BPCTaxTypeMaster.Company;
                    //BPCTaxTypeMaster2.Type = BPCTaxTypeMaster.Type;
                    //BPCTaxTypeMaster2.PatnerID = BPCTaxTypeMaster.PatnerID;
                    BPCTaxTypeMaster2.IsActive = true;
                    BPCTaxTypeMaster2.ModifiedOn = DateTime.Now;
                    BPCTaxTypeMaster2.ModifiedBy = BPCTaxTypeMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    BPCTaxTypeMasterResult.TaxType = BPCTaxTypeMaster.TaxType;
                    //BPCTaxTypeMasterResult.MaxAmount = BPCTaxTypeMaster.MaxAmount;
                    //BPCTaxTypeMasterResult.Client = BPCTaxTypeMaster.Client;
                    //BPCTaxTypeMasterResult.Company = BPCTaxTypeMaster.Company;
                    //BPCTaxTypeMasterResult.Type = BPCTaxTypeMaster.Type;
                    //BPCTaxTypeMasterResult.PatnerID = BPCTaxTypeMaster.PatnerID;
                }
                else
                {
                    return BPCTaxTypeMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/UpdateTaxTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/UpdateTaxTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdateTaxTypeMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return BPCTaxTypeMasterResult;
        }
        public async Task<BPCTaxTypeMaster> DeleteTaxTypeMaster(BPCTaxTypeMaster BPCTaxTypeMaster)
        {
            BPCTaxTypeMaster BPCTaxTypeMasterResult = new BPCTaxTypeMaster();
            try
            {
                BPCTaxTypeMaster BPCTaxTypeMaster1 = (from tb in _dbContext.BPCTaxTypeMasters
                                                      where tb.IsActive && tb.TaxType == BPCTaxTypeMaster.TaxType
                                                      select tb).FirstOrDefault();
                if (BPCTaxTypeMaster1 != null)
                {
                    _dbContext.BPCTaxTypeMasters.Remove(BPCTaxTypeMaster1);
                    await _dbContext.SaveChangesAsync();
                    BPCTaxTypeMasterResult.TaxType = BPCTaxTypeMaster.TaxType;
                    //BPCTaxTypeMasterResult.MaxAmount = BPCTaxTypeMaster.MaxAmount;
                    //BPCTaxTypeMasterResult.Client = BPCTaxTypeMaster.Client;
                    //BPCTaxTypeMasterResult.Company = BPCTaxTypeMaster.Company;
                    //BPCTaxTypeMasterResult.Type = BPCTaxTypeMaster.Type;
                    //BPCTaxTypeMasterResult.PatnerID = BPCTaxTypeMaster.PatnerID;
                }
                else
                {
                    return BPCTaxTypeMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/DeleteTaxTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/DeleteTaxTypeMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteTaxTypeMaster : - ", ex);
            }
            return BPCTaxTypeMasterResult;
        }


        public List<BPCHSNMaster> GetAllHSNMasters()
        {
            try
            {
                return _dbContext.BPCHSNMasters.Distinct().ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetAllHSNMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetAllHSNMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCHSNMaster> CreateHSNMaster(BPCHSNMaster BPCHSNMaster)
        {
            BPCHSNMaster BPCHSNMasterResult = new BPCHSNMaster();
            try
            {
                BPCHSNMaster Master1 = (from tb in _dbContext.BPCHSNMasters
                                        where tb.IsActive && tb.HSNCode == BPCHSNMaster.HSNCode
                                        select tb).FirstOrDefault();
                if (Master1 == null)
                {
                    BPCHSNMaster.CreatedOn = DateTime.Now;
                    BPCHSNMaster.IsActive = true;
                    var result = _dbContext.BPCHSNMasters.Add(BPCHSNMaster);
                    await _dbContext.SaveChangesAsync();

                    return result.Entity;
                }
                else
                {
                    return BPCHSNMasterResult;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateHSNMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateHSNMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateHSNMaster : - ", ex);
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task CreateHSNMasters(List<BPCHSNMaster> BPCHSNMasters)
        {
            var log = new BPCLog();
            try
            {
                log = await poRepository.CreateBPCLog("CreateHSNMasters", BPCHSNMasters.Count);
                foreach (var BPCHSNMaster in BPCHSNMasters)
                {
                    BPCHSNMaster Master1 = (from tb in _dbContext.BPCHSNMasters
                                            where tb.IsActive && tb.HSNCode == BPCHSNMaster.HSNCode
                                            select tb).FirstOrDefault();
                    if (Master1 == null)
                    {
                        BPCHSNMaster.CreatedOn = DateTime.Now;
                        BPCHSNMaster.IsActive = true;
                        var result = _dbContext.BPCHSNMasters.Add(BPCHSNMaster);
                        await _dbContext.SaveChangesAsync();

                    }
                    else
                    {
                        Master1.Description = BPCHSNMaster.Description;
                        Master1.IsActive = true;
                        Master1.ModifiedOn = DateTime.Now;
                        Master1.ModifiedBy = BPCHSNMaster.ModifiedBy;
                    }
                }
                if (log != null)
                {
                    await poRepository.UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Master/CreateHSNMaster--" + "Unable to generate Log");
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateHSNMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateHSNMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateHSNMaster : - ", ex);
                if (log != null)
                {
                    await poRepository.UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Master/CreateHSNMaster--" + "Unable to generate Log");
                }
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<BPCHSNMaster> UpdateHSNMaster(BPCHSNMaster BPCHSNMaster)
        {
            BPCHSNMaster BPCHSNMasterResult = new BPCHSNMaster();
            try
            {
                BPCHSNMaster BPCHSNMaster2 = (from tb in _dbContext.BPCHSNMasters
                                              where tb.IsActive && tb.HSNCode == BPCHSNMaster.HSNCode
                                              select tb).FirstOrDefault();
                if (BPCHSNMaster2 != null)
                {
                    BPCHSNMaster2.Description = BPCHSNMaster.Description;
                    BPCHSNMaster2.IsActive = true;
                    BPCHSNMaster2.ModifiedOn = DateTime.Now;
                    BPCHSNMaster2.ModifiedBy = BPCHSNMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();

                    return BPCHSNMaster2;
                }
                else
                {
                    return BPCHSNMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/UpdateHSNMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/UpdateHSNMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdateHSNMaster : - ", ex);
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<BPCHSNMaster> DeleteHSNMaster(BPCHSNMaster BPCHSNMaster)
        {
            BPCHSNMaster BPCHSNMasterResult = new BPCHSNMaster();
            try
            {
                BPCHSNMaster BPCHSNMaster1 = (from tb in _dbContext.BPCHSNMasters
                                              where tb.IsActive && tb.HSNCode == BPCHSNMaster.HSNCode
                                              select tb).FirstOrDefault();
                if (BPCHSNMaster1 != null)
                {
                    var result = _dbContext.BPCHSNMasters.Remove(BPCHSNMaster1);
                    await _dbContext.SaveChangesAsync();
                    return result.Entity;
                }
                else
                {
                    return BPCHSNMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/DeleteHSNMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/DeleteHSNMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteHSNMaster : - ", ex);
                throw ex;
            }
        }

        public List<BPCProfitCentreMaster> GetAllProfitCentreMasters()
        {
            try
            {
                return _dbContext.BPCProfitCentreMasters.Distinct().ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetAllProfitCentreMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetAllProfitCentreMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCProfitCentreMaster> CreateProfitCentreMaster(BPCProfitCentreMaster BPCProfitCentreMaster)
        {
            BPCProfitCentreMaster BPCProfitCentreMasterResult = new BPCProfitCentreMaster();
            try
            {
                BPCProfitCentreMaster Master1 = (from tb in _dbContext.BPCProfitCentreMasters
                                                 where tb.IsActive && tb.ProfitCentre == BPCProfitCentreMaster.ProfitCentre
                                                 select tb).FirstOrDefault();
                if (Master1 == null)
                {
                    BPCProfitCentreMaster.CreatedOn = DateTime.Now;
                    BPCProfitCentreMaster.IsActive = true;
                    var result = _dbContext.BPCProfitCentreMasters.Add(BPCProfitCentreMaster);
                    await _dbContext.SaveChangesAsync();

                    return result.Entity;
                }
                else
                {
                    return BPCProfitCentreMasterResult;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateProfitCentreMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateProfitCentreMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateProfitCentreMaster : - ", ex);
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task CreateProfitCentreMasters(List<BPCProfitCentreMaster> BPCProfitCentreMasters)
        {
            var log = new BPCLog();
            try
            {
                log = await poRepository.CreateBPCLog("CreateProfitCentreMasters", BPCProfitCentreMasters.Count);
                foreach (var BPCProfitCentreMaster in BPCProfitCentreMasters)
                {
                    BPCProfitCentreMaster Master1 = (from tb in _dbContext.BPCProfitCentreMasters
                                                     where tb.IsActive && tb.ProfitCentre == BPCProfitCentreMaster.ProfitCentre
                                                     select tb).FirstOrDefault();
                    if (Master1 == null)
                    {
                        BPCProfitCentreMaster.CreatedOn = DateTime.Now;
                        BPCProfitCentreMaster.IsActive = true;
                        var result = _dbContext.BPCProfitCentreMasters.Add(BPCProfitCentreMaster);
                        await _dbContext.SaveChangesAsync();

                    }
                    else
                    {
                        Master1.Description = BPCProfitCentreMaster.Description;
                        Master1.Language = BPCProfitCentreMaster.Language;
                        Master1.IsActive = true;
                        Master1.ModifiedOn = DateTime.Now;
                        Master1.ModifiedBy = BPCProfitCentreMaster.ModifiedBy;
                    }
                }
                if (log != null)
                {
                    await poRepository.UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Master/CreateProfitCentreMaster--" + "Unable to generate Log");
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateProfitCentreMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateProfitCentreMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateProfitCentreMaster : - ", ex);
                if (log != null)
                {
                    await poRepository.UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Master/CreateProfitCentreMaster--" + "Unable to generate Log");
                }
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<BPCProfitCentreMaster> UpdateProfitCentreMaster(BPCProfitCentreMaster BPCProfitCentreMaster)
        {
            BPCProfitCentreMaster BPCProfitCentreMasterResult = new BPCProfitCentreMaster();
            try
            {
                BPCProfitCentreMaster BPCProfitCentreMaster2 = (from tb in _dbContext.BPCProfitCentreMasters
                                                                where tb.IsActive && tb.ProfitCentre == BPCProfitCentreMaster.ProfitCentre
                                                                select tb).FirstOrDefault();
                if (BPCProfitCentreMaster2 != null)
                {
                    BPCProfitCentreMaster2.Description = BPCProfitCentreMaster.Description;
                    BPCProfitCentreMaster2.Language = BPCProfitCentreMaster.Language;
                    BPCProfitCentreMaster2.IsActive = true;
                    BPCProfitCentreMaster2.ModifiedOn = DateTime.Now;
                    BPCProfitCentreMaster2.ModifiedBy = BPCProfitCentreMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();

                    return BPCProfitCentreMaster2;
                }
                else
                {
                    return BPCProfitCentreMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/UpdateProfitCentreMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/UpdateProfitCentreMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdateProfitCentreMaster : - ", ex);
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<BPCProfitCentreMaster> DeleteProfitCentreMaster(BPCProfitCentreMaster BPCProfitCentreMaster)
        {
            BPCProfitCentreMaster BPCProfitCentreMasterResult = new BPCProfitCentreMaster();
            try
            {
                BPCProfitCentreMaster BPCProfitCentreMaster1 = (from tb in _dbContext.BPCProfitCentreMasters
                                                                where tb.IsActive && tb.ProfitCentre == BPCProfitCentreMaster.ProfitCentre
                                                                select tb).FirstOrDefault();
                if (BPCProfitCentreMaster1 != null)
                {
                    var result = _dbContext.BPCProfitCentreMasters.Remove(BPCProfitCentreMaster1);
                    await _dbContext.SaveChangesAsync();
                    return result.Entity;
                }
                else
                {
                    return BPCProfitCentreMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/DeleteProfitCentreMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/DeleteProfitCentreMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteProfitCentreMaster : - ", ex);
                throw ex;
            }
        }


        public List<BPCCompanyMaster> GetAllCompanyMasters()
        {
            try
            {
                return _dbContext.BPCCompanyMasters.Distinct().ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetAllCompanyMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetAllCompanyMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCCompanyMaster> CreateCompanyMaster(BPCCompanyMaster BPCCompanyMaster)
        {
            BPCCompanyMaster BPCCompanyMasterResult = new BPCCompanyMaster();
            try
            {
                BPCCompanyMaster Master1 = (from tb in _dbContext.BPCCompanyMasters
                                                 where tb.IsActive && tb.Company == BPCCompanyMaster.Company
                                            select tb).FirstOrDefault();
                if (Master1 == null)
                {
                    BPCCompanyMaster.CreatedOn = DateTime.Now;
                    BPCCompanyMaster.IsActive = true;
                    var result = _dbContext.BPCCompanyMasters.Add(BPCCompanyMaster);
                    await _dbContext.SaveChangesAsync();

                    return result.Entity;
                }
                else
                {
                    return BPCCompanyMasterResult;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateCompanyMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateCompanyMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateCompanyMaster : - ", ex);
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task CreateCompanyMasters(List<BPCCompanyMaster> BPCCompanyMasters)
        {
            var log = new BPCLog();
            try
            {
                log = await poRepository.CreateBPCLog("CreateCompanyMasters", BPCCompanyMasters.Count);
                foreach (var BPCCompanyMaster in BPCCompanyMasters)
                {
                    BPCCompanyMaster Master1 = (from tb in _dbContext.BPCCompanyMasters
                                                     where tb.IsActive && tb.Company == BPCCompanyMaster.Company
                                                     select tb).FirstOrDefault();
                    if (Master1 == null)
                    {
                        BPCCompanyMaster.CreatedOn = DateTime.Now;
                        BPCCompanyMaster.IsActive = true;
                        var result = _dbContext.BPCCompanyMasters.Add(BPCCompanyMaster);
                        await _dbContext.SaveChangesAsync();

                    }
                    else
                    {
                        Master1.CompanyName = BPCCompanyMaster.CompanyName;
                        Master1.IsActive = true;
                        Master1.ModifiedOn = DateTime.Now;
                        Master1.ModifiedBy = BPCCompanyMaster.ModifiedBy;
                    }
                }
                if (log != null)
                {
                    await poRepository.UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Master/CreateCompanyMaster--" + "Unable to generate Log");
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateCompanyMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateCompanyMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateCompanyMaster : - ", ex);
                if (log != null)
                {
                    await poRepository.UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Master/CreateCompanyMaster--" + "Unable to generate Log");
                }
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<BPCCompanyMaster> UpdateCompanyMaster(BPCCompanyMaster BPCCompanyMaster)
        {
            BPCCompanyMaster BPCCompanyMasterResult = new BPCCompanyMaster();
            try
            {
                BPCCompanyMaster BPCCompanyMaster2 = (from tb in _dbContext.BPCCompanyMasters
                                                                where tb.IsActive && tb.Company == BPCCompanyMaster.Company
                                                                select tb).FirstOrDefault();
                if (BPCCompanyMaster2 != null)
                {
                    BPCCompanyMaster2.CompanyName = BPCCompanyMaster.CompanyName;
                    BPCCompanyMaster2.IsActive = true;
                    BPCCompanyMaster2.ModifiedOn = DateTime.Now;
                    BPCCompanyMaster2.ModifiedBy = BPCCompanyMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();

                    return BPCCompanyMaster2;
                }
                else
                {
                    return BPCCompanyMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/UpdateCompanyMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/UpdateCompanyMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdateCompanyMaster : - ", ex);
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<BPCCompanyMaster> DeleteCompanyMaster(BPCCompanyMaster BPCCompanyMaster)
        {
            BPCCompanyMaster BPCCompanyMasterResult = new BPCCompanyMaster();
            try
            {
                BPCCompanyMaster BPCCompanyMaster1 = (from tb in _dbContext.BPCCompanyMasters
                                                                where tb.IsActive && tb.Company == BPCCompanyMaster.Company
                                                                select tb).FirstOrDefault();
                if (BPCCompanyMaster1 != null)
                {
                    var result = _dbContext.BPCCompanyMasters.Remove(BPCCompanyMaster1);
                    await _dbContext.SaveChangesAsync();
                    return result.Entity;
                }
                else
                {
                    return BPCCompanyMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/DeleteCompanyMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/DeleteCompanyMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteCompanyMaster : - ", ex);
                throw ex;
            }
        }
        public List<BPTicketStatus> GetAllTicketStatus()
        {
            try
            {
                var result =  _dbContext.BPTicketStatus.ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/UpdateCompanyMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/UpdateCompanyMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdateCompanyMaster : - ", ex);
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
           
        }
    }
}
