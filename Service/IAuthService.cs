using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Note.Service
{
    public interface IAuthService
    {
        Task<ActionResult<string>> Login(string username, string password);
        Task<ActionResult<string>> Register(string username, string password);
    }
}