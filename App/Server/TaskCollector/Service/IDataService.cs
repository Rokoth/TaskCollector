using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public interface IAuthService
    {                
        Task<ClaimsIdentity> Auth(ClientIdentity login, CancellationToken token);
        Task<ClaimsIdentity> Auth(UserIdentity login, CancellationToken token);        
    }   
}