using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP_POService.Models;
using BPCloud_VP_POService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPCloud_VP_POService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterRepository _MasterRepository;

        public MasterController(IMasterRepository MasterRepository)
        {
            _MasterRepository = MasterRepository;
        }

        [HttpGet]
        public List<BPCCountryMaster> GetAllBPCCountryMasters()
        {
            try
            {
                var CountryMasters = _MasterRepository.GetAllBPCCountryMasters();
                return CountryMasters;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllBPCCountryMasters", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCCurrencyMaster> GetAllBPCCurrencyMasters()
        {
            try
            {
                var CurrencyMasters = _MasterRepository.GetAllBPCCurrencyMasters();
                return CurrencyMasters;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllBPCCurrencyMasters", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCDocumentCenterMaster> GetAllDocumentCenterMaster()
        {
            try
            {
                var AllDocumentCenterMaster = _MasterRepository.GetAllDocumentCenterMaster();
                return AllDocumentCenterMaster;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllDocumentCenterMaster", ex);
                return null;
            }
        }
        //#region  user palnt getting
        [HttpGet]
        public List<BPCPlantMaster> GetAllPlant()
        {
            try
            {
                var result = _MasterRepository.GetAllPlant();
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllPlant", ex);
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreatePlantMaster(BPCPlantMaster PlantMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _MasterRepository.CreatePlantMaster(PlantMaster);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/CreatePlantMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePlantMaster(BPCPlantMaster PlantMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _MasterRepository.UpdatePlantMaster(PlantMaster);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdatePlantMaster", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeletePlantMaster(BPCPlantMaster PlantMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.DeletePlantMaster(PlantMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeletePlantMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        //#endregion 

        [HttpPost]
        public async Task<IActionResult> CreateDocumentCenterMaster(BPCDocumentCenterMaster BPCDocumentCenterMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.CreateDocumentCenterMaster(BPCDocumentCenterMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateDocumentCenterMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDocumentCenterMaster(BPCDocumentCenterMaster BPCDocumentCenterMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.UpdateDocumentCenterMaster(BPCDocumentCenterMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateDocumentCenterMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocumentCenterMaster(BPCDocumentCenterMaster BPCDocumentCenterMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.DeleteDocumentCenterMaster(BPCDocumentCenterMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteDocumentCenterMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public List<BPCReasonMaster> GetAllReasonMaster()
        {
            try
            {
                var AllReasonMaster = _MasterRepository.GetAllReasonMaster();
                return AllReasonMaster;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllReasonMaster", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCExpenseTypeMaster> GetExpenseTypeMasters()
        {
            try
            {
                var CountryMasters = _MasterRepository.GetExpenseTypeMasters();
                return CountryMasters;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("POFlip/GetExpenseTypeMasters", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpenseTypeMaster(BPCExpenseTypeMaster BPCExpenseTypeMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.CreateExpenseTypeMaster(BPCExpenseTypeMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateExpenseTypeMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateExpenseTypeMaster(BPCExpenseTypeMaster BPCExpenseTypeMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.UpdateExpenseTypeMaster(BPCExpenseTypeMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateExpenseTypeMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExpenseTypeMaster(BPCExpenseTypeMaster BPCExpenseTypeMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.DeleteExpenseTypeMaster(BPCExpenseTypeMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteExpenseTypeMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public List<BPCTaxTypeMaster> GetTaxTypeMasters()
        {
            try
            {
                var CountryMasters = _MasterRepository.GetTaxTypeMasters();
                return CountryMasters;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("POFlip/GetTaxTypeMasters", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaxTypeMaster(BPCTaxTypeMaster BPCTaxTypeMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.CreateTaxTypeMaster(BPCTaxTypeMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateTaxTypeMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTaxTypeMaster(BPCTaxTypeMaster BPCTaxTypeMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.UpdateTaxTypeMaster(BPCTaxTypeMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateTaxTypeMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTaxTypeMaster(BPCTaxTypeMaster BPCTaxTypeMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.DeleteTaxTypeMaster(BPCTaxTypeMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteTaxTypeMaster", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public List<BPCHSNMaster> GetAllHSNMasters()
        {
            try
            {
                var CountryMasters = _MasterRepository.GetAllHSNMasters();
                return CountryMasters;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("POFlip/GetAllHSNMasters", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateHSNMaster(BPCHSNMaster BPCHSNMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.CreateHSNMaster(BPCHSNMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateHSNMaster", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateHSNMasters([FromBody] List<BPCHSNMaster> BPCHSNMasters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _MasterRepository.CreateHSNMasters(BPCHSNMasters);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateHSNMasters", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHSNMaster(BPCHSNMaster BPCHSNMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.UpdateHSNMaster(BPCHSNMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateHSNMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHSNMaster(BPCHSNMaster BPCHSNMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.DeleteHSNMaster(BPCHSNMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteHSNMaster", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public List<BPCProfitCentreMaster> GetAllProfitCentreMasters()
        {
            try
            {
                var CountryMasters = _MasterRepository.GetAllProfitCentreMasters();
                return CountryMasters;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("POFlip/GetAllProfitCentreMasters", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfitCentreMaster(BPCProfitCentreMaster BPCProfitCentreMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.CreateProfitCentreMaster(BPCProfitCentreMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateProfitCentreMaster", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProfitCentreMasters([FromBody] List<BPCProfitCentreMaster> BPCProfitCentreMasters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _MasterRepository.CreateProfitCentreMasters(BPCProfitCentreMasters);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateProfitCentreMasters", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfitCentreMaster(BPCProfitCentreMaster BPCProfitCentreMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.UpdateProfitCentreMaster(BPCProfitCentreMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateProfitCentreMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfitCentreMaster(BPCProfitCentreMaster BPCProfitCentreMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.DeleteProfitCentreMaster(BPCProfitCentreMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteProfitCentreMaster", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public List<BPCCompanyMaster> GetAllCompanyMasters()
        {
            try
            {
                var CountryMasters = _MasterRepository.GetAllCompanyMasters();
                return CountryMasters;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("POFlip/GetAllCompanyMasters", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyMaster(BPCCompanyMaster BPCCompanyMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.CreateCompanyMaster(BPCCompanyMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateCompanyMaster", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompanyMasters([FromBody] List<BPCCompanyMaster> BPCCompanyMasters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _MasterRepository.CreateCompanyMasters(BPCCompanyMasters);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateCompanyMasters", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompanyMaster(BPCCompanyMaster BPCCompanyMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.UpdateCompanyMaster(BPCCompanyMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateCompanyMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompanyMaster(BPCCompanyMaster BPCCompanyMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MasterRepository.DeleteCompanyMaster(BPCCompanyMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteCompanyMaster", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public List<BPTicketStatus> GetAllTicketStatus()
        {
            try
            {
                var result = _MasterRepository.GetAllTicketStatus();
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteCompanyMaster", ex);
                return null;
            }
        }

    }
}