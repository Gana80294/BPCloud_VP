using BPCloud_VP.AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.AuthenticationService.Repositories
{
    public interface IAuthRepository
    {
        Client FindClient(string clientId);
        Task<AuthenticationResult> AuthenticateUser(string UserName, string Password);
     

    }
}
