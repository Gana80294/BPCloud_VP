using BPCloud_VP.AuthenticationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BPCloud_VP.AuthenticationService.Repositories;

namespace BPCloud_VP.AuthenticationService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterRepository _masterRepository;
        IConfiguration _configuration;
        public MasterController(IMasterRepository masterRepository, IConfiguration configuration)
        {
            _masterRepository = masterRepository;
            _configuration = configuration;
        }

        #region Authentication

        [HttpGet]
        public Client FindClient(string clientId)
        {
            try
            {
                var client = _masterRepository.FindClient(clientId);
                return client;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/FindClient : - ", ex);
                return null;
            }
        }
        [HttpGet]
        public AuthenticationResult AuthenticateUser(string UserName, string Password)
        {
            try
            {
                var result = _masterRepository.AuthenticateUser(UserName, Password);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/FindClient : - ", ex);
                return null;
            }
        }

        #endregion

        #region User
        //[Authorize]
        [HttpGet]
        public List<UserWithRole> GetAllUsers()
        {
            try
            {
                var userWithRole = _masterRepository.GetAllUsers();
                return userWithRole;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllUsers", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserWithRole userWithRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateUser(userWithRole);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateUser", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserWithRole userWithRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.UpdateUser(userWithRole);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateUser", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(UserWithRole userWithRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _masterRepository.DeleteUser(userWithRole);
                return new OkResult();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteUser", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateVendorUser(VendorUser vendorUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateVendorUser(vendorUser);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateUser", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetApplicationTourDisabled(ApplicationTour applicationTour)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _masterRepository.SetApplicationTourDisabled(applicationTour);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SetApplicationTourDisabled", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTermsAndConditionStatus(UserView userView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _masterRepository.UpdateTermsAndConditionStatus(userView);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/UpdateTermsAndConditionStatus", ex);
                return BadRequest(ex.Message);
            }
        }



        #region  plant map with user
        [HttpGet]
        public List<UserPlantMap_withuser> GetAllPlant()
        {
            try
            {
                var result = _masterRepository.GetAllPlant();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllPlant", ex);
                return null;
            }
        }

        #endregion


        #endregion

        #region
        //public int Attempts(string UserName)
        //{
        //    try
        //    {
        //        var result = _masterRepository.Attempts(UserName);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.WriteToFile("Master/Attempts", ex);
        //        return 0;
        //    }
        //}
        #endregion

        #region Role

        //[Authorize]
        [HttpGet]
        public List<RoleWithApp> GetAllRoles()
        {
            try
            {
                var result = _masterRepository.GetAllRoles();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllRoles", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleWithApp roleWithApp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateRole(roleWithApp);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateRole", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleWithApp roleWithApp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.UpdateRole(roleWithApp);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateRole", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(RoleWithApp roleWithApp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _masterRepository.DeleteRole(roleWithApp);
                return new OkResult();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteRole", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region App

        [HttpGet]
        public List<App> GetAllApps()
        {
            try
            {
                var result = _masterRepository.GetAllApps();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllApps", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateApp(App app)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateApp(app);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateApp", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApp(App app)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.UpdateApp(app);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateApp", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApp(App app)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _masterRepository.DeleteApp(app);
                return new OkResult();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteApp", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region AppUsage

        [HttpGet]
        public List<AppUsageView> GetAllAppUsages()
        {
            try
            {
                var result = _masterRepository.GetAllAppUsages();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllAppUsages", ex);
                return null;
            }
        }

        [HttpGet]
        public List<AppUsageView> GetAppUsagesByUser(Guid UserID)
        {
            try
            {
                var result = _masterRepository.GetAppUsagesByUser(UserID);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAppUsagesByUser", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult GetBuyerPlant(Guid UserID)
        {
            try
            {
                var result = _masterRepository.GetBuyerPlant(UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetBuyerPlant", ex);
                return null;
            }
        }

        [HttpGet]
        public List<string> GetHelpDeskAdminPlants(Guid UserID)
        {
            try
            {
                var result = _masterRepository.GetHelpDeskAdminPlants(UserID);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetHelpDeskAdminPlants", ex);
                return null;
            }
        }

        [HttpGet]
        public List<string> GetHelpDeskAdminCompanies(Guid UserID)
        {
            try
            {
                var result = _masterRepository.GetHelpDeskAdminCompanies(UserID);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetHelpDeskAdminCompanies", ex);
                return null;
            }
        }

        [HttpGet]
        public List<string> GetHelpDeskAdminReasonCodes(Guid UserID)
        {
            try
            {
                var result = _masterRepository.GetHelpDeskAdminReasonCodes(UserID);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetHelpDeskAdminReasonCodes", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppUsage(AppUsage AppUsage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateAppUsage(AppUsage);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateAppUsage", ex);
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> UpdateAppUsage(AppUsage AppUsage)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        else
        //        {
        //            var result = await _masterRepository.UpdateAppUsage(AppUsage);
        //            return Ok(result);
        //        }
        //    }
        //    {

        //        ErrorLog.WriteToFile("Master/UpdateAppUsage", ex);
        //        return BadRequest(ex.Message);
        //    }
        //    //return Ok();
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteAppUsage(AppUsage AppUsage)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        var result = await _masterRepository.DeleteAppUsage(AppUsage);
        //        return new OkResult();
        //    }
        //    {
        //        ErrorLog.WriteToFile("Master/DeleteAppUsage", ex);
        //        return BadRequest(ex.Message);
        //    }
        //}

        #endregion


        #region SessionMaster

        [HttpGet]
        public SessionMaster GetSessionMasterByProject(string ProjectName)
        {
            try
            {
                var result = _masterRepository.GetSessionMasterByProject(ProjectName);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetSessionMasterByProject", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SessionMaster> GetAllSessionMasters()
        {
            try
            {
                var result = _masterRepository.GetAllSessionMasters();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllSessionMasters", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SessionMaster> GetAllSessionMastersByProject(string ProjectName)
        {
            try
            {
                var result = _masterRepository.GetAllSessionMastersByProject(ProjectName);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllSessionMastersByProject", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSessionMaster(SessionMaster SessionMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateSessionMaster(SessionMaster);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateSessionMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSessionMaster(SessionMaster SessionMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.UpdateSessionMaster(SessionMaster);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateSessionMaster", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSessionMaster(SessionMaster SessionMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _masterRepository.DeleteSessionMaster(SessionMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteSessionMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region LogInAndChangePassword

        [HttpPost]
        public IActionResult LoginHistory(Guid UserID, string Username)
        {
            try
            {
                var result = _masterRepository.LoginHistory(UserID, Username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/LoginHistory : - ", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public List<UserLoginHistory> GetAllUsersLoginHistory()
        {
            try
            {
                var result = _masterRepository.GetAllUsersLoginHistory();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllUsersLoginHistory : - ", ex);
                return null;
            }
        }
        [HttpGet]
        public List<UserLoginHistory> GetCurrentUserLoginHistory(Guid UserID)
        {
            try
            {
                var result = _masterRepository.GetCurrentUserLoginHistory(UserID);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetCurrentUserLoginHistory : - ", ex);
                return null;
            }
        }
        [HttpGet]
        public async Task<IActionResult> SignOut(Guid UserID)
        {
            try
            {
                var result = await _masterRepository.SignOut(UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SignOut : - ", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public List<UserLoginHistory> FilterLoginHistory(string UserName = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                var result = _masterRepository.FilterLoginHistory(UserName, FromDate, ToDate);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/FilterLoginHistory : - ", ex);
                return null;
            }
        }

        #endregion

        #region ChangePassword

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            try
            {
                var result = await _masterRepository.ChangePassword(changePassword);
                if (result != null)
                    return Ok(result);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/ChangePassword : - ", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendResetLinkToMail(EmailModel emailModel)
        {
            try
            {
                var result = await _masterRepository.SendResetLinkToMail(emailModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SendResetLinkToMail : - ", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var result = await _masterRepository.ForgotPassword(forgotPassword);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest("New Password Should Not Same As Default Or Previous 5 Passwords");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/ForgotPassword : - ", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region UserPreferencesTheme

        [HttpGet]
        public UserPreference GetUserPreferenceByUserID(Guid UserID)
        {
            try
            {
                var UserPreference = _masterRepository.GetUserPrefercences(UserID);
                return UserPreference;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetUserPreferenceByUserID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetUserPreference(UserPreference UserPreference)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.SetUserPreference(UserPreference);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/SetUserPreference", ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpGet]
        public List<UserWithRole> GetSupportDeskUsersByRoleName(string RoleName)
        {
            try
            {
                var userWithRole = _masterRepository.GetSupportDeskUsersByRoleName(RoleName);
                return userWithRole;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetSupportDeskUsersByRoleName", ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateActionLog(ActionLog log)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateActionLog(log);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateActionLog", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAllActionLogs()
        {
            try
            {

                var result = _masterRepository.GetAllActionLogs();
                return Ok(result);

            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/GetAllActionLogs", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public List<ActionLog> FilterASNList(string UserName = null, string AppName = null, DateTime? UsedDate = null)
        {
            try
            {
                var data = _masterRepository.FilterASNList(UserName, AppName, UsedDate);
                return data;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/FilterASNList", ex);
                return null;
            }
        }
        [HttpGet]
        public List<string> GetUserPlants(Guid UserID)
        {
            try
            {
                var data = _masterRepository.GetUserPlants(UserID);
                return data;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetUserPlants", ex);
                return null;
            }
        }
        [HttpGet]
        public User BuyerFactByPlant(string plant)
        {
            try
            {
                var data = _masterRepository.GetBuyerFactByPlant(plant);
                return data;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/BuyerFactByPlant", ex);
                return null;
            }
        }

        [HttpGet]
        public List<UserView> HelpdeskUserByCompany(string Company)
        {
            try
            {
                var data = _masterRepository.HelpdeskUserByCompany(Company);
                return data;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/HelpdeskUserByCompany", ex);
                return null;
            }
        }

        [HttpPost]
        public List<UserView> GetUserViewsByPatnerIDs([FromBody] List<string> PatnerIDs)
        {
            try
            {
                var data = _masterRepository.GetUserViewsByPatnerIDs(PatnerIDs);
                return data;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetUserViewsByPatnerIDs", ex);
                return null;
            }
        }

        [HttpGet]
        public UserView GetUserViewByPatnerID(string PatnerID)
        {
            try
            {
                var data = _masterRepository.GetUserViewByPatnerID(PatnerID);
                return data;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetUserViewByPatnerID", ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateSuperUSer(SuperUser user)
        {
            try
            {
                Boolean saved = _masterRepository.CreateSuperUSer(user);
                return Ok(saved);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateSuperUSer", ex);
                return BadRequest();
            }
        }




        //[HttpGet]
        //public UserPlantMap GetOfPlantsDetails(Guid userID)
        //{
        //    try
        //    {
        //        var data = _masterRepository.GetOfPlantsDetails(userID);
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.WriteToFile("Master/BuyerFactByPlant", ex);
        //        return null;
        //    }
        //}

    }
}
