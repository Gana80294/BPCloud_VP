using BPCloud_VP.AuthenticatioService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.AuthenticatioService.Repositories
{
    public interface IMasterRepository
    {

        #region Authentication
        Client FindClient(string clientId);
        AuthenticationResult AuthenticateUser(string UserName, string Password);

        #endregion

        #region User

        List<UserWithRole> GetAllUsers();
        Task<UserWithRole> CreateUser(UserWithRole userWithRole);
        Task<UserWithRole> UpdateUser(UserWithRole userWithRole);
        Task<UserWithRole> DeleteUser(UserWithRole userWithRole);
        Task<UserWithRole> CreateVendorUser(VendorUser vendorUser);
        Task<UserWithRole> SetApplicationTourDisabled(ApplicationTour applicationTour);
        Task<User> UpdateTermsAndConditionStatus(UserView userView);
        #endregion

        #region Role

        List<RoleWithApp> GetAllRoles();
        Task<RoleWithApp> CreateRole(RoleWithApp roleWithApp);
        Task<RoleWithApp> UpdateRole(RoleWithApp roleWithApp);
        Task<RoleWithApp> DeleteRole(RoleWithApp roleWithApp);

        #endregion

        #region App

        List<App> GetAllApps();
        Task<App> CreateApp(App app);
        Task<App> UpdateApp(App app);
        Task<App> DeleteApp(App app);

        #endregion

        #region AppUsage

        List<AppUsageView> GetAllAppUsages();
        List<AppUsageView> GetAppUsagesByUser(Guid UserID);
        string GetBuyerPlant(Guid UserID);
        List<string> GetHelpDeskAdminPlants(Guid UserID);
        List<string> GetHelpDeskAdminCompanies(Guid UserID);
        List<string> GetHelpDeskAdminReasonCodes(Guid UserID);
        Task<AppUsage> CreateAppUsage(AppUsage AppUsage);
        //Task<AppUsage> UpdateAppUsage(AppUsage AppUsage);
        //Task<AppUsage> DeleteAppUsage(AppUsage AppUsage);

        #endregion

        #region SessionMaster

        SessionMaster GetSessionMasterByProject(string ProjectName);
        List<SessionMaster> GetAllSessionMasters();
        List<SessionMaster> GetAllSessionMastersByProject(string ProjectName);
        Task<SessionMaster> CreateSessionMaster(SessionMaster SessionMaster);
        Task<SessionMaster> UpdateSessionMaster(SessionMaster SessionMaster);
        Task<SessionMaster> DeleteSessionMaster(SessionMaster SessionMaster);

        #endregion

        #region LogInAndChangePassword

        Task<UserLoginHistory> LoginHistory(Guid UserID, string Username);
        List<UserLoginHistory> GetAllUsersLoginHistory();
        List<UserLoginHistory> GetCurrentUserLoginHistory(Guid UserID);
        Task<UserLoginHistory> SignOut(Guid UserID);
        List<UserLoginHistory> FilterLoginHistory(string UserName = null, DateTime? FromDate = null, DateTime? ToDate = null);
        #endregion

        #region ChangePassword

        Task<User> ChangePassword(ChangePassword changePassword);
        Task<TokenHistory> SendResetLinkToMail(EmailModel emailModel);
        Task<TokenHistory> ForgotPassword(ForgotPassword forgotPassword);


        #endregion

        #region sendMail

        Task<bool> SendMail(string code, string UserName, string toEmail, string userID, string siteURL);
        Task<bool> SendMailToVendor(string toEmail, string UserName, string password);

        #endregion

        #region EncryptAndDecrypt
        string Decrypt(string Password, bool UseHashing);
        string Encrypt(string Password, bool useHashing);

        #endregion

        #region UserPreferences

        UserPreference GetUserPrefercences(Guid userID);
        Task<UserPreference> SetUserPreference(UserPreference UserPreference);
        #endregion

        List<UserWithRole> GetSupportDeskUsersByRoleName(string RoleName);

        Task<ActionLog> CreateActionLog(ActionLog log);
        User GetBuyerFactByPlant(string plant);
       // UserPlantMap GetOfPlantsDetails(Guid userID);
        List<UserView> HelpdeskUserByCompany(string Company);
        List<ActionLog> GetAllActionLogs();
        List<ActionLog> FilterASNList(string UserName, string AppName = null, DateTime? UsedDate = null);
        #region plantmaps 
        List<UserPlantMap_withuser> GetAllPlant();
     
        #endregion
        
        List<string> GetUserPlants(Guid UserID);
        List<UserView> GetUserViewsByPatnerIDs([FromBody] List<string> PatnerIDs);
        UserView GetUserViewByPatnerID(string PatnerID);
        Boolean CreateSuperUSer(SuperUser user);
        #region
        int Attempts(string UserName);
        #endregion
    }
}
