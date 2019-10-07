using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.GrayWoodFine.webAPI.Services
{
    public interface IUserMangementService
    {
        bool IsValidUser(string username, string password);
    }
}
