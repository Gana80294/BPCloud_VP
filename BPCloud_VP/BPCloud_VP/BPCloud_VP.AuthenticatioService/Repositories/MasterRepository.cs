using BPCloud_VP.AuthenticatioService.Repositories;
using BPCloud_VP.AuthenticatioService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BPCloud_VP.AuthenticatioService.DBContexts;
using System.Data.SqlClient;
using System.Data;

namespace BPCloud_VP.AuthenticatioService.Repositories
{
    public class MasterRepository : IMasterRepository
    {
        private readonly AuthContext _dbContext;
        IConfiguration _configuration;
        private int _tokenTimespan = 0;

        public MasterRepository(AuthContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
            try
            {
                var span = "30";
                if (span != "")
                    _tokenTimespan = Convert.ToInt32(span.ToString());
                if (_tokenTimespan <= 0)
                {
                    _tokenTimespan = 30;
                }
            }
            catch
            {
                _tokenTimespan = 30;
            }
        }

        #region Authentication

        public Client FindClient(string clientId)
        {
            try
            {
                var client = _dbContext.Clients.Find(clientId);
                return client;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/FindClient", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/FindClient", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/FindClient : - ", ex);
                return null;
            }
        }

        public AuthenticationResult AuthenticateUser(string UserName, string Password)
        {
            try
            {
                AuthenticationResult authenticationResult = new AuthenticationResult();
                List<string> MenuItemList = new List<string>();
                string MenuItemNames = "";
                string ProfilesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");
                string Profile = "Empty";
                User user = null;
                string isChangePasswordRequired = "No";
                string DefaultPassword = "Exalca@123";
                //string DefaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];


                if (UserName.Contains('@') && UserName.Contains('.'))
                {
                    user = (from tb in _dbContext.Users
                            where tb.Email == UserName && tb.IsActive
                            select tb).FirstOrDefault();
                }
                else
                {
                    user = (from tb in _dbContext.Users
                            where tb.UserName == UserName && tb.IsActive
                            select tb).FirstOrDefault();
                }

                if (user != null)
                {
                    bool isValidUser = false;

                    string DecryptedPassword = Decrypt(user.Password, true);
                    isValidUser = DecryptedPassword == Password;
                    if (isValidUser)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        if (Password == DefaultPassword)
                        {
                            isChangePasswordRequired = "Yes";
                        }
                        //MasterController MasterController = new MasterController();
                        //await MasterController.LoginHistory(user.UserID, user.UserName);
                        Role userRole = (from tb1 in _dbContext.Roles
                                         join tb2 in _dbContext.UserRoleMaps on tb1.RoleID equals tb2.RoleID
                                         join tb3 in _dbContext.Users on tb2.UserID equals tb3.UserID
                                         where tb3.UserID == user.UserID && tb1.IsActive && tb2.IsActive && tb3.IsActive
                                         select tb1).FirstOrDefault();

                        if (userRole != null)
                        {
                            MenuItemList = (from tb1 in _dbContext.Apps
                                            join tb2 in _dbContext.RoleAppMaps on tb1.AppID equals tb2.AppID
                                            where tb2.RoleID == userRole.RoleID && tb1.IsActive && tb2.IsActive
                                            select tb1.AppName).ToList();
                            foreach (string item in MenuItemList)
                            {
                                if (MenuItemNames == "")
                                {
                                    MenuItemNames = item;
                                }
                                else
                                {
                                    MenuItemNames += "," + item;
                                }
                            }
                        }
                        authenticationResult.IsSuccess = true;
                        authenticationResult.UserID = user.UserID;
                        authenticationResult.UserName = user.UserName;
                        authenticationResult.DisplayName = user.UserName;
                        authenticationResult.EmailAddress = user.Email;
                        authenticationResult.UserRole = userRole != null ? userRole.RoleName : string.Empty;
                        authenticationResult.MenuItemNames = MenuItemNames;
                        authenticationResult.IsChangePasswordRequired = isChangePasswordRequired;
                        authenticationResult.Profile = string.IsNullOrEmpty(Profile) ? "Empty" : Profile;
                    }
                    else
                    {
                        authenticationResult.IsSuccess = false;
                        authenticationResult.Message = "The user name or password is incorrect.";
                    }
                }
                else
                {
                    authenticationResult.IsSuccess = false;
                    authenticationResult.Message = "The user name or password is incorrect.";
                }

                return authenticationResult;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/AuthenticateUser", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/AuthenticateUser", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/AuthenticateUser : - ", ex);
                return null;
            }
        }

        #endregion


        #region attempts

        #endregion
        public int Attempts(string UserName)
        {
            try
            {
                var result = (from tb1 in _dbContext.Users
                              where tb1.UserName == UserName && tb1.IsActive
                              select tb1.Attempts).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/Attempts", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/Attempts", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/Attempts : - ", ex);
                return 0;
            }
        }

        #region User

        #region plantmap

        public List<UserPlantMap_withuser> GetAllPlant()
        {
            var result = (from tb in _dbContext.Users
                          join tb1 in _dbContext.UserPlantMaps on tb.UserID equals tb1.UserID
                          where tb.IsActive && tb1.IsActive
                          select new UserPlantMap_withuser()
                          {
                              PlantID = tb1.PlantID,
                              UserID = tb1.UserID,
                              IsActive = tb1.IsActive,
                              CreatedOn = tb1.CreatedOn,
                              CreatedBy = tb1.CreatedBy,
                              ModifiedOn = tb1.ModifiedOn,
                              ModifiedBy = tb1.ModifiedBy,
                              UserName = tb.UserName,
                              Email = tb.Email


                          }

                          ).ToList();

            return result;
        }

        

        #endregion


        public List<UserWithRole> GetAllUsers()
        {

            try
            {
                var result = (from tb in _dbContext.Users
                              join tb1 in _dbContext.UserRoleMaps on tb.UserID equals tb1.UserID
                              where tb.IsActive && tb1.IsActive
                              select new
                              {
                                  tb.UserID,
                                  tb.UserName,
                                  tb.DisplayName,
                                  tb.Email,
                                  tb.ContactNumber,
                                  tb.Password,
                                  tb.IsActive,
                                  tb.CreatedOn,
                                  tb.ModifiedOn,
                                  tb1.RoleID,
                              }).ToList();

                List<UserWithRole> UserWithRoleList = new List<UserWithRole>();

                result.ForEach(record =>
                {
                    UserWithRoleList.Add(new UserWithRole()
                    {
                        UserID = record.UserID,
                        UserName = record.UserName,
                        Email = record.Email,
                        ContactNumber = record.ContactNumber,
                        Password = Decrypt(record.Password, true),
                        IsActive = record.IsActive,
                        CreatedOn = record.CreatedOn,
                        ModifiedOn = record.ModifiedOn,
                        DisplayName = record.DisplayName,
                        RoleID = record.RoleID,
                        Plants = (from tb in _dbContext.UserPlantMaps where tb.UserID == record.UserID select tb.PlantID).ToList(),
                        Companies = (from tb in _dbContext.UserCompanyMaps where tb.UserID == record.UserID select tb.Company).ToList(),
                        ReasonCodes = (from tb in _dbContext.UserSupportMasterMaps where tb.UserID == record.UserID select tb.ReasonCode).ToList(),
                    });

                });
                return UserWithRoleList;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllUsers", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllUsers", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllUsers : - ", ex);
                return null;
            }
        }

        public async Task<UserWithRole> CreateUser(UserWithRole userWithRole)
        {
            UserWithRole userResult = new UserWithRole();
            try
            {
                // Creating User
                User user1 = (from tb1 in _dbContext.Users
                              where tb1.UserName == userWithRole.UserName && tb1.IsActive
                              select tb1).FirstOrDefault();

                if (user1 == null)
                {
                    //User user2 = (from tb1 in _dbContext.Users
                    //              where tb1.Email == userWithRole.Email && tb1.IsActive
                    //              select tb1).FirstOrDefault();
                    //if (user2 == null)
                    //{
                    //string DefaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
                    string DefaultPassword = _configuration["DefaultPassword"];
                    User user = new User();
                    user.UserID = Guid.NewGuid();
                    user.UserName = userWithRole.UserName;
                    user.Email = userWithRole.Email;
                    user.Password = Encrypt(DefaultPassword, true);
                    user.ContactNumber = userWithRole.ContactNumber;
                    user.DisplayName = userWithRole.DisplayName;
                    user.CreatedBy = userWithRole.CreatedBy;
                    user.IsActive = true;
                    user.CreatedOn = DateTime.Now;
                    user.Attempts = 0;
                    user.IsBlocked = false;
                    var result = _dbContext.Users.Add(user);
                    //_dbContext.Users.Add(user);
                    await _dbContext.SaveChangesAsync();

                    UserRoleMap UserRole = new UserRoleMap()
                    {
                        RoleID = userWithRole.RoleID,
                        UserID = user.UserID,
                        IsActive = true,
                        CreatedOn = DateTime.Now
                    };
                    var result1 = _dbContext.UserRoleMaps.Add(UserRole);
                    await _dbContext.SaveChangesAsync();

                    //storing in userplantmap table
                    //var res1 = (from tb1 in _dbContext.UserPlantMaps
                    //            where tb1.UserID == userWithRole.UserID && tb1.IsActive
                    //            select tb1).FirstOrDefault();

                    if (userWithRole.Plants != null)
                    {
                        foreach (string plant in userWithRole.Plants)
                        {
                            UserPlantMap roleApp = new UserPlantMap()
                            {
                                UserID = user.UserID,
                                IsActive = true,
                                CreatedOn = DateTime.Now,
                                PlantID = plant
                            };
                            _dbContext.UserPlantMaps.Add(roleApp);
                        }
                    }
                    await _dbContext.SaveChangesAsync();

                    if (userWithRole.Companies != null)
                    {
                        foreach (string comp in userWithRole.Companies)
                        {
                            UserCompanyMap companyMap = new UserCompanyMap()
                            {
                                UserID = user.UserID,
                                IsActive = true,
                                CreatedOn = DateTime.Now,
                                Company = comp
                            };
                            _dbContext.UserCompanyMaps.Add(companyMap);
                        }
                    }
                    await _dbContext.SaveChangesAsync();


                    if (userWithRole.ReasonCodes != null)
                    {
                        foreach (string reason in userWithRole.ReasonCodes)
                        {
                            UserSupportMasterMap supportMasterMap = new UserSupportMasterMap()
                            {
                                UserID = user.UserID,
                                ReasonCode = reason,
                                IsActive = true,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                            };
                            _dbContext.UserSupportMasterMaps.Add(supportMasterMap);
                        }
                    }
                    await _dbContext.SaveChangesAsync();


                    //

                    userResult.UserName = user.UserName;
                    userResult.Email = user.Email;
                    userResult.ContactNumber = user.ContactNumber;
                    userResult.UserID = user.UserID;
                    userResult.Password = user.Password;
                    userResult.RoleID = UserRole.RoleID;
                    userResult.DisplayName = user.DisplayName;
                    // Attachment
                    //}
                    //else
                    //{
                    //    return userResult;
                    //    //return BadRequest("User with same email address already exist");
                    //}
                }
                else
                {
                    //return userResult;
                    throw new Exception("User with same name already exist");
                }
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/CreateUser", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/CreateUser", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateUser : - ", ex);
                throw ex;
            }
            return userResult;
            //_dbContext.Users.Add(entity);
            //_dbContext.SaveChanges();
        }

        public async Task<UserWithRole> UpdateUser(UserWithRole userWithRole)
        {
            UserWithRole userResult = new UserWithRole();
            try
            {

                User user1 = (from tb1 in _dbContext.Users
                              where tb1.UserName == userWithRole.UserName && tb1.IsActive && tb1.UserID != userWithRole.UserID
                              select tb1).FirstOrDefault();

                if (user1 == null)
                {
                    //User user2 = (from tb1 in _dbContext.Users
                    //              where tb1.Email == userWithRole.Email && tb1.IsActive && tb1.UserID != userWithRole.UserID
                    //              select tb1).FirstOrDefault();
                    //if (user2 == null)
                    //{
                    //Updating User details
                    var user = (from tb in _dbContext.Users
                                where tb.IsActive &&
                                tb.UserID == userWithRole.UserID
                                select tb).FirstOrDefault();
                    user.UserName = userWithRole.UserName;
                    user.Email = userWithRole.Email;
                    //user.Password = Encrypt(userWithRole.Password, true);
                    user.ContactNumber = userWithRole.ContactNumber;
                    user.DisplayName = userWithRole.DisplayName;
                    user.IsActive = true;
                    user.ModifiedOn = DateTime.Now;
                    user.ModifiedBy = userWithRole.ModifiedBy;
                    await _dbContext.SaveChangesAsync();

                    UserRoleMap OldUserRole = _dbContext.UserRoleMaps.Where(x => x.UserID == userWithRole.UserID && x.IsActive).FirstOrDefault();
                    if (OldUserRole.RoleID != userWithRole.RoleID)
                    {
                        //Delete old role related to the user
                        _dbContext.UserRoleMaps.Remove(OldUserRole);
                        _dbContext.SaveChanges();

                        //Add new roles for the user
                        UserRoleMap UserRole = new UserRoleMap()
                        {
                            RoleID = userWithRole.RoleID,
                            UserID = user.UserID,
                            IsActive = true,
                            CreatedBy = userWithRole.ModifiedBy,
                            CreatedOn = DateTime.Now,
                        };
                        var r = _dbContext.UserRoleMaps.Add(UserRole);
                        await _dbContext.SaveChangesAsync();



                        userResult.UserName = user.UserName;
                        userResult.Email = user.Email;
                        userResult.ContactNumber = user.ContactNumber;
                        userResult.UserID = user.UserID;
                        userResult.Password = user.Password;
                        userResult.RoleID = UserRole.RoleID;
                        userResult.DisplayName = user.DisplayName;
                    }
                    //storing in userplantmap table
                    var res1 = (from tb1 in _dbContext.UserPlantMaps
                                where tb1.UserID == userWithRole.UserID && tb1.IsActive
                                select tb1).ToList();
                    //var res2 = (from tb1 in _dbContext.UserPlantMaps
                    //            where tb1.UserID == userWithRole.UserID && tb1.IsActive
                    //            select tb1).FirstOrDefault();


                    if (res1 != null)
                    {
                        foreach (UserPlantMap res in res1)
                        {
                            _dbContext.UserPlantMaps.Remove(res);
                        }
                        _dbContext.SaveChanges();

                        if (userWithRole.Plants != null)
                        {
                            foreach (string plant in userWithRole.Plants)
                            {
                                UserPlantMap roleApp = new UserPlantMap()
                                {
                                    UserID = user.UserID,
                                    IsActive = true,
                                    CreatedOn = DateTime.Now,
                                    PlantID = plant
                                };
                                _dbContext.UserPlantMaps.Add(roleApp);
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                    //

                    //storing in userplantmap table


                    var res11 = (from tb1 in _dbContext.UserCompanyMaps
                                 where tb1.UserID == userWithRole.UserID && tb1.IsActive
                                 select tb1).ToList();
                    //var res2 = (from tb1 in _dbContext.UserPlantMaps
                    //            where tb1.UserID == userWithRole.UserID && tb1.IsActive
                    //            select tb1).FirstOrDefault();


                    if (res11 != null)
                    {
                        foreach (UserCompanyMap res in res11)
                        {
                            _dbContext.UserCompanyMaps.Remove(res);
                        }
                        _dbContext.SaveChanges();

                        if (userWithRole.Companies != null)
                        {
                            foreach (string comp in userWithRole.Companies)
                            {
                                UserCompanyMap companyMap = new UserCompanyMap()
                                {
                                    UserID = user.UserID,
                                    IsActive = true,
                                    CreatedOn = DateTime.Now,
                                    Company = comp
                                };
                                _dbContext.UserCompanyMaps.Add(companyMap);
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                    }


                    var res2 = (from tb1 in _dbContext.UserSupportMasterMaps
                                where tb1.UserID == userWithRole.UserID && tb1.IsActive
                                select tb1).ToList();
                    //var res2 = (from tb1 in _dbContext.UserPlantMaps
                    //            where tb1.UserID == userWithRole.UserID && tb1.IsActive
                    //            select tb1).FirstOrDefault();


                    if (res2 != null)
                    {
                        foreach (UserSupportMasterMap res in res2)
                        {
                            _dbContext.UserSupportMasterMaps.Remove(res);
                        }
                        _dbContext.SaveChanges();

                        if (userWithRole.ReasonCodes != null)
                        {
                            foreach (string reason in userWithRole.ReasonCodes)
                            {
                                UserSupportMasterMap supportMasterMap = new UserSupportMasterMap()
                                {
                                    UserID = user.UserID,
                                    ReasonCode = reason,
                                    IsActive = true,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now,
                                };
                                _dbContext.UserSupportMasterMaps.Add(supportMasterMap);
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                    }

                    //}
                    //else
                    //{
                    //    return userResult;
                    //    //return Content(HttpStatusCode.BadRequest, "User with same email address already exist");
                    //}
                }
                else
                {
                    return userResult;
                    //return Content(HttpStatusCode.BadRequest, "User with same name already exist");
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateUser", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateUser", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/UpdateUser : - ", ex);
                return null;
            }
            return userResult;
        }

        public async Task<UserWithRole> DeleteUser(UserWithRole userWithRole)
        {
            UserWithRole userResult = new UserWithRole();
            try
            {
                var result = (from tb in _dbContext.Users
                              where tb.IsActive &&
                              tb.UserID == userWithRole.UserID
                              select tb).FirstOrDefault();
                if (result == null)
                {
                    return userResult;
                }
                else
                {
                    //result.IsActive = false;
                    //result.ModifiedOn = DateTime.Now;
                    //result.ModifiedBy = userWithRole.ModifiedBy;
                    _dbContext.Users.Remove(result);
                    await _dbContext.SaveChangesAsync();

                    //Changing the Status of role related to the user
                    UserRoleMap UserRole = _dbContext.UserRoleMaps.Where(x => x.UserID == userWithRole.UserID && x.IsActive).FirstOrDefault();
                    //UserRole.IsActive = false;
                    //UserRole.ModifiedOn = DateTime.Now;
                    //UserRole.ModifiedBy = userWithRole.ModifiedBy;
                    _dbContext.UserRoleMaps.Remove(UserRole);
                    await _dbContext.SaveChangesAsync();
                    var res1 = (from tb1 in _dbContext.UserPlantMaps
                                where tb1.UserID == userWithRole.UserID && tb1.IsActive
                                select tb1).ToList();
                    if (res1 != null)
                    {
                        foreach (UserPlantMap res in res1)
                        {
                            _dbContext.UserPlantMaps.Remove(res);
                        }
                        _dbContext.SaveChanges();

                    }


                    userResult.UserName = result.UserName;
                    userResult.Email = result.Email;
                    userResult.ContactNumber = result.ContactNumber;
                    userResult.UserID = result.UserID;
                    userResult.Password = result.Password;
                    userResult.RoleID = UserRole.RoleID;
                    return userResult;
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/DeleteUser", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/DeleteUser", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteUser : - ", ex);
                return null;
            }
        }

        public async Task<UserWithRole> CreateVendorUser(VendorUser vendorUser)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                UserWithRole userResult = new UserWithRole();
                try
                {
                    // Creating User
                    User user1 = (from tb1 in _dbContext.Users
                                  where tb1.UserName == vendorUser.UserName && tb1.IsActive
                                  select tb1).FirstOrDefault();

                    if (user1 == null)
                    {
                        //User user2 = (from tb1 in _dbContext.Users
                        //              where tb1.Email == vendorUser.Email && tb1.IsActive
                        //              select tb1).FirstOrDefault();
                        //if (user2 == null)
                        //{
                        //string DefaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
                        string DefaultPassword = vendorUser.Phone ?? _configuration["DefaultPassword"];
                        User user = new User();
                        user.UserID = Guid.NewGuid();
                        user.UserName = vendorUser.UserName;
                        user.Email = vendorUser.Email;
                        user.Password = Encrypt(DefaultPassword, true);
                        user.ContactNumber = vendorUser.Phone;
                        user.DisplayName = vendorUser.DisplayName;
                        user.CreatedBy = vendorUser.Email;
                        user.IsBlocked = false;
                        user.Attempts = 0;

                        user.IsActive = true;
                        user.CreatedOn = DateTime.Now;
                        var result = _dbContext.Users.Add(user);
                        //_dbContext.Users.Add(user);
                        await _dbContext.SaveChangesAsync();

                        var VendorRoleID = _dbContext.Roles.Where(x => x.RoleName.ToLower() == "vendor").Select(y => y.RoleID).FirstOrDefault();

                        UserRoleMap UserRole = new UserRoleMap()
                        {
                            RoleID = VendorRoleID,
                            UserID = user.UserID,
                            IsActive = true,
                            CreatedOn = DateTime.Now
                        };
                        var result1 = _dbContext.UserRoleMaps.Add(UserRole);
                        await _dbContext.SaveChangesAsync();

                        userResult.UserName = user.UserName;
                        userResult.Email = user.Email;
                        userResult.ContactNumber = user.ContactNumber;
                        userResult.UserID = user.UserID;
                        userResult.Password = user.Password;
                        userResult.RoleID = UserRole.RoleID;
                        userResult.DisplayName = user.DisplayName;
                        userResult.IsBlocked = user.IsBlocked;

                        if (vendorUser.Email != null)
                        {
                            await SendMailToVendor(vendorUser.Email, vendorUser.UserName, vendorUser.Phone);
                        }
                        transaction.Commit();
                        transaction.Dispose();
                        return userResult;
                        // Attachment
                        //}
                        //else
                        //{
                        //    //return userResult;
                        //    transaction.Rollback();
                        //    transaction.Dispose();
                        //    ErrorLog.WriteToFile("Master/CreateUser : - User with same email address already exist");
                        //    throw new Exception("User with same email address already exist");
                        //}
                    }
                    else
                    {
                        //return userResult;
                        // transaction.Rollback();
                        // transaction.Dispose();
                        ErrorLog.WriteToFile("Master/CreateVendorUser : - User with same vendor code already exist");
                        throw new Exception("User with same vendor code already exist");
                    }
                }
                catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/CreateVendorUser", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/CreateVendorUser", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    ErrorLog.WriteToFile("Master/CreateVendorUser : - ", ex);
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public async Task<UserWithRole> SetApplicationTourDisabled(ApplicationTour applicationTour)
        {
            UserWithRole userResult = new UserWithRole();
            try
            {
                var result = (from tb in _dbContext.Users
                              where tb.IsActive &&
                              tb.UserID == applicationTour.UserId
                              select tb).FirstOrDefault();
                if (result == null)
                {
                    return userResult;
                }
                else
                {
                    result.TourStatus = applicationTour.TourStatus;
                    await _dbContext.SaveChangesAsync();

                    return userResult;
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/SetApplicationTourDisabled", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/SetApplicationTourDisabled", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SetApplicationTourDisabled : - ", ex);
                return null;
            }
        }

        public async Task<User> UpdateTermsAndConditionStatus(UserView userView)
        {
            try
            {
                var result = (from tb in _dbContext.Users
                              where tb.IsActive &&
                              tb.UserID == userView.UserID
                              select tb).FirstOrDefault();
                if (result == null)
                {
                    throw new Exception("User does not exist");
                }
                else
                {
                    result.IsAccepted = true;
                    result.AcceptedOn = DateTime.Now;
                    result.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();

                    return result;
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateTermsAndConditionStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateTermsAndConditionStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/UpdateTermsAndConditionStatus : - ", ex);
                return null;
            }
        }

        #endregion

        #region Role

        public List<RoleWithApp> GetAllRoles()
        {
            try
            {
                List<RoleWithApp> RoleWithAppList = new List<RoleWithApp>();
                List<Role> RoleList = (from tb in _dbContext.Roles
                                       where tb.IsActive
                                       select tb).ToList();
                foreach (Role rol in RoleList)
                {
                    RoleWithAppList.Add(new RoleWithApp()
                    {
                        RoleID = rol.RoleID,
                        RoleName = rol.RoleName,
                        IsActive = rol.IsActive,
                        CreatedOn = rol.CreatedOn,
                        ModifiedOn = rol.ModifiedOn,
                        AppIDList = _dbContext.RoleAppMaps.Where(x => x.RoleID == rol.RoleID && x.IsActive).Select(r => r.AppID).ToArray()
                    });
                }
                return RoleWithAppList;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllRoles", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllRoles", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllRoles : - ", ex);
                return null;

            }
        }

        public async Task<RoleWithApp> CreateRole(RoleWithApp roleWithApp)
        {
            RoleWithApp roleResult = new RoleWithApp();
            try
            {
                Role role1 = (from tb in _dbContext.Roles
                              where tb.IsActive && tb.RoleName == roleWithApp.RoleName
                              select tb).FirstOrDefault();
                if (role1 == null)
                {
                    Role role = new Role();
                    role.RoleID = Guid.NewGuid();
                    role.RoleName = roleWithApp.RoleName;
                    role.CreatedOn = DateTime.Now;
                    role.CreatedBy = roleWithApp.CreatedBy;
                    role.IsActive = true;
                    var result = _dbContext.Roles.Add(role);
                    await _dbContext.SaveChangesAsync();

                    foreach (int AppID in roleWithApp.AppIDList)
                    {
                        RoleAppMap roleApp = new RoleAppMap()
                        {
                            AppID = AppID,
                            RoleID = role.RoleID,
                            IsActive = true,
                            CreatedOn = DateTime.Now
                        };
                        _dbContext.RoleAppMaps.Add(roleApp);
                    }
                    await _dbContext.SaveChangesAsync();
                    roleResult.RoleName = roleWithApp.RoleName;
                    roleResult.RoleID = roleWithApp.RoleID;
                    roleResult.AppIDList = roleWithApp.AppIDList;
                }
                else
                {
                    return roleResult;
                    //return Content(HttpStatusCode.BadRequest, "Role with same name already exist");
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/CreateRole", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/CreateRole", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateRole : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return roleResult;
        }

        public async Task<RoleWithApp> UpdateRole(RoleWithApp roleWithApp)
        {
            RoleWithApp roleResult = new RoleWithApp();
            try
            {
                Role role = (from tb in _dbContext.Roles
                             where tb.IsActive && tb.RoleName == roleWithApp.RoleName && tb.RoleID != roleWithApp.RoleID
                             select tb).FirstOrDefault();
                if (role == null)
                {
                    Role role1 = (from tb in _dbContext.Roles
                                  where tb.IsActive && tb.RoleID == roleWithApp.RoleID
                                  select tb).FirstOrDefault();
                    role1.RoleName = roleWithApp.RoleName;
                    role1.IsActive = true;
                    role1.ModifiedOn = DateTime.Now;
                    role1.ModifiedBy = roleWithApp.ModifiedBy;
                    await _dbContext.SaveChangesAsync();

                    List<RoleAppMap> OldRoleAppList = _dbContext.RoleAppMaps.Where(x => x.RoleID == roleWithApp.RoleID && x.IsActive).ToList();
                    List<RoleAppMap> NeedToRemoveRoleAppList = OldRoleAppList.Where(x => !roleWithApp.AppIDList.Any(y => y == x.AppID)).ToList();
                    List<int> NeedToAddAppList = roleWithApp.AppIDList.Where(x => !OldRoleAppList.Any(y => y.AppID == x)).ToList();

                    //Delete Old RoleApps which is not exist in new List
                    NeedToRemoveRoleAppList.ForEach(x =>
                    {
                        _dbContext.RoleAppMaps.Remove(x);
                    });
                    await _dbContext.SaveChangesAsync();

                    //Create New RoleApps
                    foreach (int AppID in NeedToAddAppList)
                    {
                        RoleAppMap roleApp = new RoleAppMap()
                        {
                            AppID = AppID,
                            RoleID = role1.RoleID,
                            IsActive = true,
                            CreatedOn = DateTime.Now,
                        };
                        _dbContext.RoleAppMaps.Add(roleApp);
                    }
                    await _dbContext.SaveChangesAsync();
                    roleResult.RoleName = roleWithApp.RoleName;
                    roleResult.RoleID = roleWithApp.RoleID;
                    roleResult.AppIDList = roleWithApp.AppIDList;
                }
                else
                {
                    return roleResult;
                    //return Content(HttpStatusCode.BadRequest, "Role with same name already exist");
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateRole", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateRole", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateRole : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return roleResult;
        }

        public async Task<RoleWithApp> DeleteRole(RoleWithApp roleWithApp)
        {
            RoleWithApp roleResult = new RoleWithApp();
            try
            {
                Role role1 = (from tb in _dbContext.Roles
                              where tb.IsActive && tb.RoleID == roleWithApp.RoleID
                              select tb).FirstOrDefault();
                if (role1 == null)
                {
                    return roleResult;
                }
                else
                {
                    //role1.IsActive = false;
                    //role1.ModifiedOn = DateTime.Now;
                    _dbContext.Roles.Remove(role1);
                    await _dbContext.SaveChangesAsync();

                    //Change the status of the RoleApps related to the role
                    List<RoleAppMap> RoleAppList = _dbContext.RoleAppMaps.Where(x => x.RoleID == roleWithApp.RoleID && x.IsActive).ToList();
                    RoleAppList.ForEach(x =>
                    {
                        //x.IsActive = false;
                        //x.ModifiedOn = DateTime.Now;
                        //x.ModifiedBy = roleWithApp.ModifiedBy;
                        _dbContext.RoleAppMaps.Remove(x);
                    });
                    await _dbContext.SaveChangesAsync();
                    roleResult.RoleName = role1.RoleName;
                    roleResult.RoleID = role1.RoleID;

                    return roleResult;
                }
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/DeleteRole", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/DeleteRole", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/DeleteRole : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region App

        public List<App> GetAllApps()
        {
            try
            {
                var result = (from tb in _dbContext.Apps
                              where tb.IsActive
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllApps", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllApps", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllApps : - ", ex);
                return null;

            }
        }

        public async Task<App> CreateApp(App App)
        {
            App appResult = new App();
            try
            {
                App App1 = (from tb in _dbContext.Apps
                            where tb.IsActive && tb.AppName == App.AppName
                            select tb).FirstOrDefault();
                if (App1 == null)
                {
                    App.CreatedOn = DateTime.Now;
                    App.IsActive = true;
                    var result = _dbContext.Apps.Add(App);
                    await _dbContext.SaveChangesAsync();

                    appResult.AppName = App.AppName;
                    appResult.AppID = App.AppID;
                }
                else
                {
                    return appResult;
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/CreateApp", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/CreateApp", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateApp : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return appResult;
        }

        public async Task<App> UpdateApp(App App)
        {
            App appResult = new App();
            try
            {
                App App1 = (from tb in _dbContext.Apps
                            where tb.IsActive && tb.AppName == App.AppName && tb.AppID != App.AppID
                            select tb).FirstOrDefault();
                if (App1 == null)
                {
                    App App2 = (from tb in _dbContext.Apps
                                where tb.IsActive && tb.AppID == App.AppID
                                select tb).FirstOrDefault();
                    App2.AppName = App.AppName;
                    App2.IsActive = true;
                    App2.ModifiedOn = DateTime.Now;
                    App2.ModifiedBy = App.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    appResult.AppName = App.AppName;
                    appResult.AppID = App.AppID;
                }
                else
                {
                    return appResult;
                }
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateApp", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateApp", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateApp : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return appResult;
        }

        public async Task<App> DeleteApp(App App)
        {
            App appResult = new App();
            try
            {
                App App1 = (from tb in _dbContext.Apps
                            where tb.IsActive && tb.AppID == App.AppID
                            select tb).FirstOrDefault();
                if (App1 != null)
                {
                    _dbContext.Apps.Remove(App1);
                    await _dbContext.SaveChangesAsync();
                    appResult.AppName = App1.AppName;
                    appResult.AppID = App1.AppID;
                }
                else
                {
                    return appResult;
                }
                //App1.IsActive = false;
                //App1.ModifiedOn = DateTime.Now;
                //App1.ModifiedBy = App.ModifiedBy;

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/DeleteApp", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/DeleteApp", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/DeleteApp : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return appResult;
        }

        #endregion


        #region AppUsage

        public List<AppUsageView> GetAllAppUsages()
        {
            try
            {
                var result = (from tb in _dbContext.AppUsages
                              join tb1 in _dbContext.Users on tb.UserID equals tb1.UserID
                              join tb2 in _dbContext.UserRoleMaps on tb1.UserID equals tb2.UserID
                              join tb3 in _dbContext.Roles on tb2.RoleID equals tb3.RoleID
                              where tb.IsActive
                              select new AppUsageView()
                              {
                                  ID = tb.ID,
                                  UserID = tb.UserID,
                                  UserName = tb1.UserName,
                                  UserRole = tb3.RoleName,
                                  AppName = tb.AppName,
                                  UsageCount = tb.UsageCount,
                                  LastUsedOn = tb.LastUsedOn,
                                  IsActive = tb.IsActive,
                                  CreatedOn = tb.CreatedOn,
                                  CreatedBy = tb.CreatedBy,
                                  ModifiedOn = tb.ModifiedOn,
                                  ModifiedBy = tb.ModifiedBy
                              }).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllAppUsages", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllAppUsages", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllAppUsages : - ", ex);
                return null;

            }
        }

        public List<AppUsageView> GetAppUsagesByUser(Guid UserID)
        {
            try
            {
                var result = (from tb in _dbContext.AppUsages
                              join tb1 in _dbContext.Users on tb.UserID equals tb1.UserID
                              join tb2 in _dbContext.UserRoleMaps on tb1.UserID equals tb2.UserID
                              join tb3 in _dbContext.Roles on tb2.RoleID equals tb3.RoleID
                              where tb.IsActive && tb.UserID == UserID
                              select new AppUsageView()
                              {
                                  ID = tb.ID,
                                  UserID = tb.UserID,
                                  UserName = tb1.UserName,
                                  UserRole = tb3.RoleName,
                                  AppName = tb.AppName,
                                  UsageCount = tb.UsageCount,
                                  LastUsedOn = tb.LastUsedOn,
                                  IsActive = tb.IsActive,
                                  CreatedOn = tb.CreatedOn,
                                  CreatedBy = tb.CreatedBy,
                                  ModifiedOn = tb.ModifiedOn,
                                  ModifiedBy = tb.ModifiedBy
                              }).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetAppUsagesByUser", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetAppUsagesByUser", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAppUsagesByUser : - ", ex);
                return null;

            }
        }

        public string GetBuyerPlant(Guid UserID)
        {
            try
            {
                var result = (from tb1 in _dbContext.Users
                              join tb2 in _dbContext.UserPlantMaps on tb1.UserID equals tb2.UserID
                              where tb1.IsActive && tb1.UserID == UserID
                              select tb2.PlantID).First();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetBuyerPlant", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetBuyerPlant", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetBuyerPlant : - ", ex);
                return null;

            }
        }

        public List<string> GetHelpDeskAdminPlants(Guid UserID)
        {
            try
            {
                var result = (from tb1 in _dbContext.Users
                              join tb2 in _dbContext.UserPlantMaps on tb1.UserID equals tb2.UserID
                              where tb1.IsActive && tb1.UserID == UserID
                              select tb2.PlantID).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetHelpDeskAdminPlants", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetHelpDeskAdminPlants", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetHelpDeskAdminPlants : - ", ex);
                return null;

            }
        }

        public List<string> GetHelpDeskAdminCompanies(Guid UserID)
        {
            try
            {
                var result = (from tb1 in _dbContext.Users
                              join tb2 in _dbContext.UserCompanyMaps on tb1.UserID equals tb2.UserID
                              where tb1.IsActive && tb1.UserID == UserID
                              select tb2.Company).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetHelpDeskAdminCompanies", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetHelpDeskAdminCompanies", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetHelpDeskAdminCompanies : - ", ex);
                return null;

            }
        }

        public List<string> GetHelpDeskAdminReasonCodes(Guid UserID)
        {
            try
            {
                var result = (from tb1 in _dbContext.Users
                              join tb2 in _dbContext.UserSupportMasterMaps on tb1.UserID equals tb2.UserID
                              where tb1.IsActive && tb1.UserID == UserID
                              select tb2.ReasonCode).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetHelpDeskAdminReasonCodes", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetHelpDeskAdminReasonCodes", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetHelpDeskAdminReasonCodes : - ", ex);
                return null;

            }
        }

        public async Task<AppUsage> CreateAppUsage(AppUsage AppUsage)
        {
            AppUsage AppUsageResult = new AppUsage();
            try
            {
                AppUsage AppUsage1 = (from tb in _dbContext.AppUsages
                                      where tb.IsActive && tb.UserID == AppUsage.UserID && tb.AppName == AppUsage.AppName
                                      select tb).FirstOrDefault();
                if (AppUsage1 == null)
                {
                    AppUsage.UsageCount = 1;
                    AppUsage.LastUsedOn = DateTime.Now;
                    AppUsage.CreatedOn = DateTime.Now;
                    AppUsage.IsActive = true;
                    var result = _dbContext.AppUsages.Add(AppUsage);
                    await _dbContext.SaveChangesAsync();
                    return result.Entity;
                }
                else
                {
                    AppUsage1.UsageCount += 1;
                    AppUsage1.LastUsedOn = DateTime.Now;
                    AppUsage1.ModifiedOn = DateTime.Now;
                    AppUsage1.ModifiedBy = AppUsage.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    return AppUsage1;
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/CreateAppUsage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/CreateAppUsage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateAppUsage : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //public async Task<AppUsage> UpdateAppUsage(AppUsage AppUsage)
        //{
        //    AppUsage AppUsageResult = new AppUsage();
        //    try
        //    {
        //        AppUsage AppUsage1 = (from tb in _dbContext.AppUsages
        //                    where tb.IsActive && tb.AppUsageName == AppUsage.AppUsageName && tb.ID != AppUsage.ID
        //                    select tb).FirstOrDefault();
        //        if (AppUsage1 == null)
        //        {
        //            AppUsage AppUsage2 = (from tb in _dbContext.AppUsages
        //                        where tb.IsActive && tb.ID == AppUsage.ID
        //                        select tb).FirstOrDefault();
        //            AppUsage2.AppUsageName = AppUsage.AppUsageName;
        //            AppUsage2.IsActive = true;
        //            AppUsage2.ModifiedOn = DateTime.Now;
        //            AppUsage2.ModifiedBy = AppUsage.ModifiedBy;
        //            await _dbContext.SaveChangesAsync();
        //            AppUsageResult.AppUsageName = AppUsage.AppUsageName;
        //            AppUsageResult.ID = AppUsage.ID;
        //        }
        //        else
        //        {
        //            return AppUsageResult;
        //        }
        //    }
        //    catch (SqlException ex){ ErrorLog.WriteToFile("MasterRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){ErrorLog.WriteToFile("MasterRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
        //    {

        //        ErrorLog.WriteToFile("Master/UpdateAppUsage : - ", ex);
        //        //return Content(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //    return AppUsageResult;
        //}

        //public async Task<AppUsage> DeleteAppUsage(AppUsage AppUsage)
        //{
        //    AppUsage AppUsageResult = new AppUsage();
        //    try
        //    {
        //        AppUsage AppUsage1 = (from tb in _dbContext.AppUsages
        //                    where tb.IsActive && tb.ID == AppUsage.ID
        //                    select tb).FirstOrDefault();
        //        if (AppUsage1 != null)
        //        {
        //            _dbContext.AppUsages.Remove(AppUsage1);
        //            await _dbContext.SaveChangesAsync();
        //            AppUsageResult.AppUsageName = AppUsage1.AppUsageName;
        //            AppUsageResult.ID = AppUsage1.ID;
        //        }
        //        else
        //        {
        //            return AppUsageResult;
        //        }
        //        //AppUsage1.IsActive = false;
        //        //AppUsage1.ModifiedOn = DateTime.Now;
        //        //AppUsage1.ModifiedBy = AppUsage.ModifiedBy;

        //    }
        //    catch (SqlException ex){ ErrorLog.WriteToFile("MasterRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){ErrorLog.WriteToFile("MasterRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
        //    {

        //        ErrorLog.WriteToFile("Master/DeleteAppUsage : - ", ex);
        //        //return Content(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //    return AppUsageResult;
        //}

        #endregion

        #region SessionMaster

        public SessionMaster GetSessionMasterByProject(string ProjectName)
        {
            try
            {
                SessionMaster sessionMasters = (from tb in _dbContext.SessionMasters
                                                where tb.IsActive && tb.ProjectName.ToLower() == ProjectName.ToLower()
                                                select tb).FirstOrDefault();

                return sessionMasters;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetSessionMasterByProject", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetSessionMasterByProject", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetSessionMasterByProject : - ", ex);
                return null;
            }
        }

        public List<SessionMaster> GetAllSessionMasters()
        {
            try
            {
                var result = (from tb in _dbContext.SessionMasters
                              where tb.IsActive
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllSessionMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllSessionMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllSessionMasters : - ", ex);
                return null;

            }
        }

        public List<SessionMaster> GetAllSessionMastersByProject(string ProjectName)
        {
            try
            {
                var result = (from tb in _dbContext.SessionMasters
                              where tb.IsActive && tb.ProjectName.ToLower() == ProjectName.ToLower()
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllSessionMastersByProject", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllSessionMastersByProject", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllSessionMastersByProject : - ", ex);
                return null;

            }
        }


        public async Task<SessionMaster> CreateSessionMaster(SessionMaster SessionMaster)
        {
            SessionMaster SessionMasterResult = new SessionMaster();
            try
            {
                SessionMaster SessionMaster1 = (from tb in _dbContext.SessionMasters
                                                where tb.IsActive && tb.ProjectName == SessionMaster.ProjectName
                                                select tb).FirstOrDefault();
                if (SessionMaster1 == null)
                {
                    SessionMaster.CreatedOn = DateTime.Now;
                    SessionMaster.IsActive = true;
                    var result = _dbContext.SessionMasters.Add(SessionMaster);
                    await _dbContext.SaveChangesAsync();

                    SessionMasterResult.ID = SessionMaster.ID;
                    SessionMasterResult.ProjectName = SessionMaster.ProjectName;
                    SessionMasterResult.SessionTimeOut = SessionMaster.SessionTimeOut;
                }
                else
                {
                    return SessionMasterResult;
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/CreateSessionMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/CreateSessionMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateSessionMaster : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return SessionMasterResult;
        }

        public async Task<SessionMaster> UpdateSessionMaster(SessionMaster SessionMaster)
        {
            SessionMaster SessionMasterResult = new SessionMaster();
            try
            {
                SessionMaster SessionMaster1 = (from tb in _dbContext.SessionMasters
                                                where tb.IsActive && tb.ProjectName == SessionMaster.ProjectName && tb.ID != SessionMaster.ID
                                                select tb).FirstOrDefault();
                if (SessionMaster1 == null)
                {
                    SessionMaster SessionMaster2 = (from tb in _dbContext.SessionMasters
                                                    where tb.IsActive && tb.ID == SessionMaster.ID
                                                    select tb).FirstOrDefault();
                    SessionMaster2.SessionTimeOut = SessionMaster.SessionTimeOut;
                    SessionMaster2.IsActive = true;
                    SessionMaster2.ModifiedOn = DateTime.Now;
                    SessionMaster2.ModifiedBy = SessionMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    SessionMasterResult.ID = SessionMaster.ID;
                    SessionMasterResult.ProjectName = SessionMaster.ProjectName;
                    SessionMasterResult.SessionTimeOut = SessionMaster.SessionTimeOut;
                }
                else
                {
                    return SessionMasterResult;
                }
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateSessionMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/UpdateSessionMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateSessionMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return SessionMasterResult;
        }

        public async Task<SessionMaster> DeleteSessionMaster(SessionMaster SessionMaster)
        {
            SessionMaster SessionMasterResult = new SessionMaster();
            try
            {
                SessionMaster SessionMaster1 = (from tb in _dbContext.SessionMasters
                                                where tb.IsActive && tb.ID == SessionMaster.ID
                                                select tb).FirstOrDefault();
                if (SessionMaster1 != null)
                {
                    _dbContext.SessionMasters.Remove(SessionMaster1);
                    await _dbContext.SaveChangesAsync();
                    SessionMasterResult.ID = SessionMaster.ID;
                    SessionMasterResult.ProjectName = SessionMaster.ProjectName;
                    SessionMasterResult.SessionTimeOut = SessionMaster.SessionTimeOut;
                }
                else
                {
                    return SessionMasterResult;
                }
                //SessionMaster1.IsActive = false;
                //SessionMaster1.ModifiedOn = DateTime.Now;
                //SessionMaster1.ModifiedBy = SessionMaster.ModifiedBy;

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/DeleteSessionMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/DeleteSessionMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/DeleteSessionMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return SessionMasterResult;
        }

        #endregion

        #region LogInAndChangePassword

        public async Task<UserLoginHistory> LoginHistory(Guid UserID, string Username)
        {
            try
            {
                UserLoginHistory loginData = new UserLoginHistory();
                loginData.UserID = UserID;
                loginData.UserName = Username;
                loginData.LoginTime = DateTime.Now;
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                _dbContext.UserLoginHistory.Add(loginData);
                await _dbContext.SaveChangesAsync();
                return loginData;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/LoginHistory", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/LoginHistory", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/LoginHistory : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
                return null;
            }

        }

        public List<UserLoginHistory> GetAllUsersLoginHistory()
        {
            try
            {
                var UserLoginHistoryList = (from tb1 in _dbContext.UserLoginHistory
                                            orderby tb1.LoginTime descending
                                            select tb1).ToList();

                return UserLoginHistoryList;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllUsersLoginHistory", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllUsersLoginHistory", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllUsersLoginHistory : ", ex);
                return null;
            }
        }

        public List<UserLoginHistory> GetCurrentUserLoginHistory(Guid UserID)
        {
            try
            {
                var UserLoginHistoryList = (from tb1 in _dbContext.UserLoginHistory
                                            where tb1.UserID == UserID
                                            orderby tb1.LoginTime descending
                                            select tb1).ToList();
                return UserLoginHistoryList;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetCurrentUserLoginHistory", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetCurrentUserLoginHistory", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetCurrentUserLoginHistory : ", ex);
                return null;
            }
        }

        public async Task<UserLoginHistory> SignOut(Guid UserID)
        {
            try
            {
                var result = _dbContext.UserLoginHistory.Where(data => data.UserID == UserID).OrderByDescending(d => d.LoginTime).FirstOrDefault();
                result.LogoutTime = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/SignOut", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/SignOut", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SignOut : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        public List<UserLoginHistory> FilterLoginHistory(string UserName = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                bool IsUserName = !string.IsNullOrEmpty(UserName);
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                var result = (from tb in _dbContext.UserLoginHistory
                              where (!IsUserName || tb.UserName.ToLower().Contains(UserName.ToLower()))
                              && (!IsFromDate || (tb.LoginTime.Date >= FromDate.Value.Date))
                              && (!IsToDate || (tb.LoginTime.Date <= ToDate.Value.Date))
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/FilterLoginHistory", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/FilterLoginHistory", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ChangePassword

        public async Task<User> ChangePassword(ChangePassword changePassword)
        {
            User userResult = new User();
            try
            {
                User user = (from tb in _dbContext.Users
                             where tb.UserName == changePassword.UserName && tb.IsActive
                             select tb).FirstOrDefault();
                if (user != null)
                {
                    string DecryptedPassword = Decrypt(user.Password, true);
                    if (DecryptedPassword == changePassword.CurrentPassword)
                    {
                        string DefaultPassword = _configuration["DefaultPassword"];
                        if (changePassword.NewPassword == DefaultPassword || user.Pass1 != null && Decrypt(user.Pass1, true) == changePassword.NewPassword || user.Pass2 != null && Decrypt(user.Pass2, true) == changePassword.NewPassword ||
                            user.Pass3 != null && Decrypt(user.Pass3, true) == changePassword.NewPassword || user.Pass4 != null && Decrypt(user.Pass4, true) == changePassword.NewPassword || user.Pass5 != null && Decrypt(user.Pass5, true) == changePassword.NewPassword)
                        {
                            userResult = null;
                            return userResult;
                        }
                        else
                        {
                            var previousPWD = user.Password;
                            user.Password = Encrypt(changePassword.NewPassword, true);
                            var index = user.LastChangedPassword;
                            var lastchangedIndex = 0;

                            //To find lastchangedpassword
                            if (!string.IsNullOrEmpty(index))
                            {
                                if (user.Pass1 != null)
                                {
                                    var strings = "user.Pass1";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 2;

                                    }
                                }
                                if (user.Pass2 != null)
                                {
                                    var strings = "user.Pass2";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 3;

                                    }
                                }
                                if (user.Pass3 != null)
                                {
                                    var strings = "user.Pass3";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 4;

                                    }
                                }
                                if (user.Pass4 != null)
                                {
                                    var strings = "user.Pass4";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 5;

                                    }
                                }
                                if (user.Pass5 != null)
                                {
                                    var strings = "user.Pass5";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 1;

                                    }
                                }
                            }

                            if (lastchangedIndex <= 0)
                            {
                                lastchangedIndex = 1;
                            }
                            // TO change previous password
                            if (lastchangedIndex == 1)
                            {
                                user.Pass1 = previousPWD;
                            }
                            else if (lastchangedIndex == 2)
                            {
                                user.Pass2 = previousPWD;
                            }
                            else if (lastchangedIndex == 3)
                            {
                                user.Pass3 = previousPWD;
                            }
                            else if (lastchangedIndex == 4)
                            {
                                user.Pass4 = previousPWD;
                            }
                            else if (lastchangedIndex == 5)
                            {
                                user.Pass5 = previousPWD;
                            }

                            user.LastChangedPassword = lastchangedIndex.ToString();
                            user.IsActive = true;
                            user.ModifiedOn = DateTime.Now;
                            user.ExpiringOn = DateTime.Now.AddDays(90);
                            await _dbContext.SaveChangesAsync();
                            userResult = user;
                        }
                    }
                    else
                    {
                        //return Content(HttpStatusCode.BadRequest, "Current password is incorrect.");
                        return userResult;
                    }
                }
                else
                {
                    return userResult;
                    //return Content(HttpStatusCode.BadRequest, "The user name or password is incorrect.");
                }
            }

            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/ChangePassword", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/ChangePassword", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/ChangePassword : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return userResult;
        }

        public async Task<TokenHistory> SendResetLinkToMail(EmailModel emailModel)
        {
            TokenHistory tokenHistoryResult = new TokenHistory();
            try
            {
                DateTime ExpireDateTime = DateTime.Now.AddMinutes(_tokenTimespan);
                User user = (from tb in _dbContext.Users
                             where tb.UserName == emailModel.UserName && tb.IsActive
                             select tb).FirstOrDefault();

                if (user != null)
                {
                    string code = Encrypt(user.UserID.ToString() + '|' + user.UserName + '|' + ExpireDateTime, true);

                    bool sendresult = await SendMail(HttpUtility.UrlEncode(code), user.UserName, user.Email, user.UserID.ToString(), emailModel.siteURL);
                    if (sendresult)
                    {
                        try
                        {
                            TokenHistory history1 = (from tb in _dbContext.TokenHistories
                                                     where tb.UserID == user.UserID && !tb.IsUsed
                                                     select tb).FirstOrDefault();
                            if (history1 == null)
                            {
                                TokenHistory history = new TokenHistory()
                                {
                                    UserID = user.UserID,
                                    Token = code,
                                    EmailAddress = user.Email,
                                    CreatedOn = DateTime.Now,
                                    ExpireOn = ExpireDateTime,
                                    IsUsed = false,
                                    Comment = "Token sent successfully"
                                };
                                var result = _dbContext.TokenHistories.Add(history);
                            }
                            else
                            {
                                ErrorLog.WriteToFile("Master/MasterRepository : Token already present, updating new token to the user " + user.UserName);
                                history1.Token = code;
                                history1.CreatedOn = DateTime.Now;
                                history1.ExpireOn = ExpireDateTime;
                            }
                            await _dbContext.SaveChangesAsync();

                            tokenHistoryResult = history1;
                        }
                        catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/SendResetLinkToMail", ex); throw new Exception("Something went wrong"); }
                        catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/SendResetLinkToMail", ex); throw new Exception("Something went wrong"); }
                        catch (Exception ex)
                        {
                            ErrorLog.WriteToFile("Master/SendResetLinkToMail : Add record to TokenHistories - ", ex);
                        }
                        return tokenHistoryResult;
                        //return Content(HttpStatusCode.OK, string.Format("Reset password link sent successfully to {0}", user.Email));
                    }
                    else
                    {
                        //return tokenHistoryResult;
                        //throw new Exception("Sorry! There is some problem on sending mail");
                        throw new Exception("Something went wrong");
                        //return Content(HttpStatusCode.BadRequest, "Sorry! There is some problem on sending mail");
                    }
                }
                else
                {
                    //return tokenHistoryResult;
                    throw new Exception($"User name {emailModel.UserName} is not registered!");
                    //return Content(HttpStatusCode.BadRequest, "Your email address is not registered!");
                }

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/SendResetLinkToMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/SendResetLinkToMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("MasterRepository/SendResetLinkToMail : - ", ex);
                //return null;
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<TokenHistory> ForgotPassword(ForgotPassword forgotPassword)
        {
            string[] decryptedArray = new string[3];
            string result = string.Empty;
            TokenHistory tokenHistoryResult = new TokenHistory();
            try
            {
                try
                {
                    result = Decrypt(forgotPassword.Token, true);
                }
                catch
                {
                    return tokenHistoryResult;
                    //return Content(HttpStatusCode.BadRequest, "Invalid token!");
                    //var errors = new string[] { "Invalid token!" };
                    //IHttpActionResult errorResult = GetErrorResult(new IdentityResult(errors));
                    //return errorResult;
                }
                if (result.Contains('|') && result.Split('|').Length == 3)
                {
                    decryptedArray = result.Split('|');
                }
                else
                {
                    return tokenHistoryResult;
                    //return Content(HttpStatusCode.BadRequest, "Invalid token!");
                }

                if (decryptedArray.Length == 3)
                {
                    DateTime date = DateTime.Parse(decryptedArray[2].Replace('+', ' '));
                    if (DateTime.Now > date)// Convert.ToDateTime(decryptedarray[2]))
                    {
                        throw new Exception("Token Expired");

                        //return tokenHistoryResult;
                        //return Content(HttpStatusCode.BadRequest, "Reset password link expired!");
                    }
                    var DecryptedUserID = decryptedArray[0];

                    User user = (from tb in _dbContext.Users
                                 where tb.UserID.ToString() == DecryptedUserID && tb.IsActive
                                 select tb).FirstOrDefault();

                    if (user.UserName == decryptedArray[1] && forgotPassword.UserID == user.UserID)
                    {
                        try
                        {
                            string DefaultPassword = _configuration["DefaultPassword"];

                            TokenHistory history = _dbContext.TokenHistories.Where(x => x.UserID == user.UserID && !x.IsUsed && x.Token == forgotPassword.Token).Select(r => r).FirstOrDefault();
                            if (history != null)
                            {

                                var index = user.LastChangedPassword;
                                var lastchangedIndex = 0;
                                var previousPWD = user.Password;
                                if (forgotPassword.NewPassword == DefaultPassword || user.Pass1 != null && Decrypt(user.Pass1, true) == forgotPassword.NewPassword || user.Pass2 != null && Decrypt(user.Pass2, true) == forgotPassword.NewPassword ||
                                    user.Pass3 != null && Decrypt(user.Pass3, true) == forgotPassword.NewPassword || user.Pass4 != null && Decrypt(user.Pass4, true) == forgotPassword.NewPassword || user.Pass5 != null && Decrypt(user.Pass5, true) == forgotPassword.NewPassword)
                                {
                                    return null;
                                }
                                else
                                {
                                    //To find lastchangedpassword
                                    if (!string.IsNullOrEmpty(index))
                                    {
                                        if (user.Pass1 != null)
                                        {
                                            var strings = "user.Pass1";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 2;
                                            }
                                        }
                                        if (user.Pass2 != null)
                                        {
                                            var strings = "user.Pass2";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 3;
                                            }
                                        }
                                        if (user.Pass3 != null)
                                        {
                                            var strings = "user.Pass3";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 4;

                                            }
                                        }
                                        if (user.Pass4 != null)
                                        {
                                            var strings = "user.Pass4";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 5;

                                            }
                                        }
                                        if (user.Pass5 != null)
                                        {
                                            var strings = "user.Pass5";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 1;

                                            }
                                        }
                                    }

                                    if (lastchangedIndex <= 0)
                                    {
                                        lastchangedIndex = 1;
                                    }
                                    // TO change previous password
                                    if (lastchangedIndex == 1)
                                    {
                                        user.Pass1 = previousPWD;
                                    }
                                    else if (lastchangedIndex == 2)
                                    {
                                        user.Pass2 = previousPWD;
                                    }
                                    else if (lastchangedIndex == 3)
                                    {
                                        user.Pass3 = previousPWD;
                                    }
                                    else if (lastchangedIndex == 4)
                                    {
                                        user.Pass4 = previousPWD;
                                    }
                                    else if (lastchangedIndex == 5)
                                    {
                                        user.Pass5 = previousPWD;
                                    }

                                    user.LastChangedPassword = lastchangedIndex.ToString();
                                    // Updating Password
                                    user.Password = Encrypt(forgotPassword.NewPassword, true);
                                    user.IsActive = true;
                                    user.ModifiedOn = DateTime.Now;
                                    user.ExpiringOn = DateTime.Now.AddDays(90);
                                    await _dbContext.SaveChangesAsync();

                                    // Updating TokenHistory
                                    history.UsedOn = DateTime.Now;
                                    history.IsUsed = true;
                                    history.Comment = "Token Used successfully";
                                    await _dbContext.SaveChangesAsync();

                                    tokenHistoryResult = history;
                                }
                            }
                            else
                            {
                                throw new Exception("Token Might have Already Used!");

                                //return tokenHistoryResult;
                                //return Content(HttpStatusCode.BadRequest, "Token might have already used or wrong token");
                            }
                        }
                        catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/ForgotPassword", ex); throw new Exception("Something went wrong"); }
                        catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/ForgotPassword", ex); throw new Exception("Something went wrong"); }
                        catch (Exception ex)
                        {
                            ErrorLog.WriteToFile("MasterRepository/ForgotPassword : Getting TokenHistory - ", ex);
                            return null;
                            //return Content(HttpStatusCode.InternalServerError, ex.Message);
                        }

                    }
                    else
                    {
                        throw new Exception("invalid Token");

                        //return tokenHistoryResult;
                        //return Content(HttpStatusCode.BadRequest, "Invalid token!");
                    }


                }
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/ForgotPassword", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/ForgotPassword", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("MasterRepository/ForgotPassword : - ", ex);
                //return null;
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return tokenHistoryResult;
        }

        #endregion

        #region sendMail

        public async Task<bool> SendMail(string code, string UserName, string toEmail, string userID, string siteURL)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];  // Comment this line for Emami dev deployement
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                sb.Append(@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> 
                            <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> <h2>Emami Vendor Portal</h2> 
                            </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> 
                            <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> 
                            <p>Dear concern,</p> <p>We have received a request to reset your password, you can reset it now by clicking reset button</p> 
                            <div style='text-align: end;'>" + "<a href=\"" + siteURL + "?token=" + code + "&Id=" + userID + "\"" + ">" +
                            "<button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Reset</button></a></div> " +
                            "Note that the link will expire in 30 minutes, so be sure to use it right away." +
                            "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>");
                subject = "Reset password";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;  // Change this to false for Emami dev deployement
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;  // Comment this line for Emami dev deployement
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);  // Comment this line for Emami dev deployement
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                ErrorLog.WriteToFile($"AuthenticationService-MasterRepository Reset link has been sent successfully to {UserName} - {toEmail}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> SendMailToVendor(string toEmail, string UserName, string password)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"]; // Comment this line for Emami dev deployement
                string SMTPPort = STMPDetailsConfig["Port"];
                string SiteURL = _configuration["SiteURL"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                //sb.Append(string.Format("Dear {0},<br/>", toEmail));
                //sb.Append("<p>Thank you for subscribing to BP Cloud.</p>");
                //sb.Append("<p>Please Login by clicking <a href=\"" + SiteURL + "/#/auth/login\">here</a></p>");
                //sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
                //sb.Append(string.Format("<p>Password: {0}</p>", password));
                //sb.Append("<p>Regards,</p><p>Admin</p>");

                sb.Append($@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> 
                                <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> 
                                 <p> <h2>Emami Supplier Portal</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> 
                                 <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear Supplier,</p> 
                                 <p>Thank you for subscribing to BP Cloud VP by Emami Limited, Request you to proceed with Login.</p> 
                                 <div style='text-align: end;'>" + "<a href=\"" + SiteURL + "/#/auth/login\"" + ">" +
                                 "<button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Login</button></a> </div> " +
                                 $"<p>User name: {UserName}</p> " +
                                 $"<p>Password: {password}</p> " +
                                 "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>");

                subject = "BP Cloud VP Vendor Creation";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false; // Change this to false for Emami dev deployement
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false; // Comment this line for Emami dev deployement
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim()); // Comment this line for Emami dev deployement
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                return true;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SendMailToVendor : - ", ex);
                throw ex;
            }
        }

        #endregion


        #region EncryptAndDecrypt

        public string Decrypt(string Password, bool UseHashing)
        {
            string EncryptionKey = "Exalca";
            byte[] KeyArray;
            byte[] ToEncryptArray = Convert.FromBase64String(Password);
            if (UseHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                hashmd5.Clear();
            }
            else
            {
                KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = KeyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 ToEncryptArray, 0, ToEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public string Encrypt(string Password, bool useHashing)
        {
            //string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
            string EncryptionKey = "Exalca";
            byte[] KeyArray;
            byte[] ToEncryptArray = UTF8Encoding.UTF8.GetBytes(Password);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                hashmd5.Clear();
            }
            else
                KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = KeyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray =
              cTransform.TransformFinalBlock(ToEncryptArray, 0,
              ToEncryptArray.Length);

            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        #endregion

        #region Userpreferences
        public UserPreference GetUserPrefercences(Guid userID)
        {
            try
            {
                UserPreference result = _dbContext.UserPreferences.FirstOrDefault(v => v.UserID == userID);
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/Decrypt", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/Decrypt", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllUserPrefercences : - ", ex);
                return null;

            }
        }

        public async Task<UserPreference> SetUserPreference(UserPreference UserPreference)
        {
            try
            {
                UserPreference UserPreference1 = new UserPreference();
                UserPreference existing = _dbContext.UserPreferences.FirstOrDefault(v => v.UserID == UserPreference.UserID);

                if (existing == null)
                {
                    UserPreference.CreatedOn = DateTime.Now;
                    UserPreference.ModifiedOn = DateTime.Now;
                    var result = _dbContext.UserPreferences.Add(UserPreference);
                }

                else
                {
                    existing.NavbarPrimaryBackground = UserPreference.NavbarPrimaryBackground;
                    existing.NavbarSecondaryBackground = UserPreference.NavbarSecondaryBackground;
                    existing.ToolbarBackground = UserPreference.ToolbarBackground;
                    existing.ModifiedOn = DateTime.Now;
                }

                await _dbContext.SaveChangesAsync();
                return UserPreference;

            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/SetUserPreference", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/SetUserPreference", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateUserPreference:-", ex);
                return null;
            }
        }


        #endregion


        public List<UserWithRole> GetSupportDeskUsersByRoleName(string RoleName)
        {
            try
            {
                List<UserWithRole> RoleWithAppList = new List<UserWithRole>();
                Role Role = (from tb in _dbContext.Roles
                             where tb.IsActive && tb.RoleName.ToLower() == RoleName.ToLower()
                             select tb).FirstOrDefault();
                var result = (from tb in _dbContext.Users
                              join tb1 in _dbContext.UserRoleMaps on tb.UserID equals tb1.UserID
                              where tb.IsActive && tb1.IsActive && tb1.RoleID == Role.RoleID
                              select new
                              {
                                  tb.UserID,
                                  tb.UserName,
                                  tb.Email,
                                  tb.ContactNumber,
                                  tb.Password,
                                  tb.IsActive,
                                  tb.CreatedOn,
                                  tb.ModifiedOn,
                                  tb1.RoleID,
                              }).ToList();
                List<UserWithRole> UserWithRoleList = new List<UserWithRole>();
                if (result != null)
                {
                    result.ForEach(record =>
                    {
                        UserWithRoleList.Add(new UserWithRole()
                        {
                            UserID = record.UserID,
                            UserName = record.UserName,
                            Email = record.Email,
                            ContactNumber = record.ContactNumber,
                            Password = Decrypt(record.Password, true),
                            IsActive = record.IsActive,
                            CreatedOn = record.CreatedOn,
                            ModifiedOn = record.ModifiedOn,
                            RoleID = record.RoleID
                        });

                    });
                }
                return UserWithRoleList;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetSupportDeskUsersByRoleName", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetSupportDeskUsersByRoleName", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetSupportDeskUsersByRoleName : - ", ex);
                return null;
            }
        }

        public async Task<ActionLog> CreateActionLog(ActionLog log)
        {
            try
            {
                ActionLog ALog = new ActionLog();

                ALog.UserID = log.UserID;
                ALog.AppName = log.AppName;
                ALog.Action = log.Action;
                ALog.ActionText = log.ActionText;
                ALog.UsedOn = DateTime.Now;
                ALog.IsActive = true;
                ALog.CreatedOn = DateTime.Now;
                ALog.CreatedBy = log.CreatedBy;
                _dbContext.ActionLogs.Add(ALog);
                await _dbContext.SaveChangesAsync();
                return log;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/CreateActionLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/CreateActionLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ActionLog> GetAllActionLogs()
        {
            try
            {
                var result = _dbContext.ActionLogs.ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllActionLogs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetAllActionLogs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ActionLog> FilterASNList(string UserName = null, string AppName = null, DateTime? UsedDate = null)
        {
            try
            {
                bool IsUserName = !string.IsNullOrEmpty(UserName);
                bool IsAppName = !string.IsNullOrEmpty(AppName);
                bool IsUsedDate = UsedDate.HasValue;
                var result = (from tb in _dbContext.ActionLogs
                              where (!IsUserName || tb.CreatedBy == UserName) && (!IsAppName || tb.AppName == AppName)
                                && (!IsUsedDate || tb.UsedOn.Date == UsedDate)
                              orderby tb.UsedOn
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/FilterASNList", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/FilterASNList", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GetUserPlants(Guid UserID)
        {
            try
            {
                var result = (from tb in _dbContext.UserPlantMaps
                              where tb.UserID == UserID
                              select tb.PlantID).ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetUserPlants", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetUserPlants", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserView> GetUserViewsByPatnerIDs([FromBody] List<string> PatnerIDs)
        {
            try
            {
                var result = (from tb in _dbContext.Users
                              where PatnerIDs.Any(x => x == tb.UserName)
                              select new UserView()
                              {
                                  UserID = tb.UserID,
                                  UserName = tb.UserName,
                                  Email = tb.Email
                              }).Distinct().ToList();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserView GetUserViewByPatnerID(string PatnerID)
        {
            try
            {
                var result = (from tb in _dbContext.Users
                              where tb.UserName == PatnerID
                              select new UserView()
                              {
                                  UserID = tb.UserID,
                                  UserName = tb.UserName,
                                  Email = tb.Email
                              }).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetUserViewByPatnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetUserViewByPatnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public User GetBuyerFactByPlant(string plant)
        {
            try
            {
                var result = (from tb1 in _dbContext.UserPlantMaps
                              where tb1.PlantID == plant
                              join tb2 in _dbContext.UserRoleMaps on tb1.UserID equals tb2.UserID
                              join tb3 in _dbContext.Roles on tb2.RoleID equals tb3.RoleID
                              where (tb3.RoleName == "Buyer")
                              join tb4 in _dbContext.Users on tb2.UserID equals tb4.UserID
                              select tb4).ToList();

                return result[0];
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetBuyerFactByPlant", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetBuyerFactByPlant", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public UserPlantMap GetOfPlantsDetails(Guid userID)
        //{
        //    try
        //    {
        //        var Plants = (from tb1 in _dbContext.UserPlantMaps where tb1.UserID == userID).ToList();
        //        return Plants;
        //    }
        //    catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/GetBuyerFactByPlant", ex); throw new Exception("Something went wrong"); }
        //    catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/GetBuyerFactByPlant", ex); throw new Exception("Something went wrong"); }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
            
        //}


        public List<UserView> HelpdeskUserByCompany(string Company)
        {
            try
            {
                var result = (from tb1 in _dbContext.UserCompanyMaps
                              join tb2 in _dbContext.UserRoleMaps on tb1.UserID equals tb2.UserID
                              join tb3 in _dbContext.Roles on tb2.RoleID equals tb3.RoleID
                              join tb4 in _dbContext.Users on tb2.UserID equals tb4.UserID
                              where tb1.Company == Company && (tb3.RoleName.ToLower().Contains("helpdesk") || tb3.RoleName.ToLower().Contains("help desk"))
                              select new UserView()
                              {
                                  UserID = tb4.UserID,
                                  UserName = tb4.UserName,
                                  Email = tb4.Email,
                              }).GroupBy(x => x.UserID).Select(x => x.FirstOrDefault()).ToList();

                return result;
            }
            catch (SqlException ex) { ErrorLog.WriteToFile("MasterRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { ErrorLog.WriteToFile("MasterRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  Boolean CreateSuperUSer(SuperUser user)
        {
            try
            {
                Guid userId ;
                Role user1 = _dbContext.Roles.Where(x => x.RoleName == user.UserRole).FirstOrDefault();
                User user2 = (from tb1 in _dbContext.Users
                              where tb1.UserName == user.UserName && tb1.IsActive && tb1.ContactNumber == user.MobileNumber && tb1.IsActive && tb1.Email == user.Mail && tb1.IsActive
                              select tb1).FirstOrDefault();
                bool saved = false;
                if (user1 != null && user2== null)
                {
                    User superuser = new User();
                    superuser.UserID  = userId = Guid.NewGuid();
                    superuser.UserName = user.UserName;
                    superuser.Email = user.Mail;
                    superuser.Password = Encrypt(user.MobileNumber, true);
                    superuser.ContactNumber = user.MobileNumber;
                    superuser.DisplayName = user.UserName;
                    superuser.CreatedBy = "Admin";
                    superuser.IsActive = true;
                    superuser.CreatedOn = DateTime.Now;
                    superuser.Attempts = 0;
                    superuser.IsBlocked = false;
                    foreach (string plant in user.Plant)
                    {
                        UserPlantMap roleApp = new UserPlantMap()
                        {
                            UserID = superuser.UserID,
                            IsActive = true,
                            CreatedOn = DateTime.Now,
                            PlantID = plant
                        };
                        _dbContext.UserPlantMaps.Add(roleApp);
                    }
                    _dbContext.Users.Add(superuser);
                    //_dbContext.SaveChangesAsync();
                    UserRoleMap UserRole = new UserRoleMap();
                    UserRole.RoleID = user1.RoleID;
                    UserRole.UserID = superuser.UserID;
                    UserRole.IsActive = true;
                    UserRole.CreatedOn = DateTime.Now;
                   _dbContext.UserRoleMaps.Add(UserRole);
                    _dbContext.SaveChanges();
                    SendMail(superuser.UserID);
                    return saved = true;
                }
                else
                {
                    return saved;
                }
                
            }

            //catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASNApprovalStatus", ex); throw new Exception("Something went wrong"); }
            //catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASNApprovalStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                //WriteLog.WriteToFile("ASNFieldMasterRepository/UpdateASNApprovalStatus : - ", ex);
                throw ex;
            }
            


        }
        public async Task<bool> SendMail(Guid userID)
        {

            try
            {
                User toEmail = _dbContext.Users.Where(x => x.UserID == userID).FirstOrDefault();
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string siteURL = _configuration.GetValue<string>("SiteURL");

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                sb.Append($@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> 
                                <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> 
                                 <p> <h2>Emami Supplier Portal</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> 
                                 <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear Sir/Madam</p> 
                                 <p>Please find the superuser credentials to login.</p> 
                                 <div style='text-align: end;'>" + "<a href=\"" + siteURL + "/#/auth/login\"" + ">" +
                               "<button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Login</button></a> </div> " +
                               $"<p>User name: {toEmail.UserName}.</p> " +
                               $"<p>Password: {toEmail.ContactNumber}.</p> " +
                               "<p>Regards,</p> <p>Admin.</p> </div> </div> </div></body></html>");
                subject = "BP Cloud VP Super User Creation";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false; // Change this to false for Emami dev deployement
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false; // Comment this line for Emami dev deployement
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim()); // Comment this line for Emami dev deployement
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail.Email, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                ErrorLog.WriteToFile("Super mail send");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        //WriteLog.WriteToFile("ASNRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        //WriteLog.WriteToFile("ASNRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                //WriteLog.WriteToFile("ASNRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                //WriteLog.WriteToFile("ASNRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex)
            {
                //WriteLog.WriteToFile("ASNRepository/SendMail", ex); 
                throw new Exception("Something went wrong");
            }
            catch (InvalidOperationException ex)
            {
                //WriteLog.WriteToFile("ASNRepository/SendMail", ex); 
                throw new Exception("Something went wrong");
            }
            catch (Exception ex)
            {
                //WriteLog.WriteToFile("ASNRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }





        }


    }

}
