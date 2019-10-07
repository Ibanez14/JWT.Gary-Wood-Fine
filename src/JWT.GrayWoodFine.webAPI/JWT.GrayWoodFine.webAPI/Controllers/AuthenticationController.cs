using JWT.GrayWoodFine.webAPI.Models;
using JWT.GrayWoodFine.webAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.GrayWoodFine.webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController:ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            this._authenticateService = authenticateService;
        }



        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Tested");
        }





        [HttpPost]      
        [Route("Request")]
        public ActionResult RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();


            string token;

            if (_authenticateService.IsAuthenticated(request, out token))
                return Ok(token);

            return BadRequest();
        }
    }
}
