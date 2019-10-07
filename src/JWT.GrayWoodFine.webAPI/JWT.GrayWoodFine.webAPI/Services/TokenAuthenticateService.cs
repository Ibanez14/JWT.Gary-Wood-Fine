using JWT.GrayWoodFine.webAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT.GrayWoodFine.webAPI.Services
{

    public class TokenAuthenticateService : IAuthenticateService
    {
        private readonly IUserMangementService _userMangementService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticateService(IUserMangementService userMangementService,
                                        IOptionsMonitor<TokenManagement> options)
        {
            _userMangementService = userMangementService;
            _tokenManagement = options.CurrentValue;
        }

        public IUserMangementService UserMangementService => _userMangementService;

        public bool IsAuthenticated(TokenRequest request, out string token)
        {
            token = string.Empty;

            if (!_userMangementService.IsValidUser(request.UserName, request.Password))
                return false;

            // claims to add to JWT Token
            var claims = new Claim[] { new Claim(ClaimTypes.Name, request.UserName) };

            // key to add in SignInCredentials
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenManagement.Secret));

            // credential to add in JWTT Token
            var credentials = new SigningCredentials(key: key, algorithm: SecurityAlgorithms.HmacSha256);


            // Creating JWT Token
            var jwtToken = 
                new JwtSecurityToken(
                    issuer: _tokenManagement.Issuer,
                    audience: _tokenManagement.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                    signingCredentials: credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}
