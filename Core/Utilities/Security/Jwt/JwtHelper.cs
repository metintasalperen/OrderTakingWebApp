using System;
using System.Collections.Generic;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Core.Entities.Concrete;
using Core.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public AccessToken CreateToken(User user, List<OperationClaims> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }
        public AccessToken CreateTokenForCustomer(int tableId, string role, int waiterId)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, tableId, role, waiterId, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaims> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(user, operationClaims),
                signingCredentials: signingCredentials);
            return jwt;
        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, int tableId, string role, int waiterId,
            SigningCredentials signingCredentials)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimsForCustomer(tableId, role, waiterId),
                signingCredentials: signingCredentials);
            return jwt;
        }
        public IEnumerable<Claim> GetClaims(User user, List<OperationClaims> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddFirstName(user.FirstName);
            claims.AddLastName(user.LastName);
            claims.AddNameIdentifier(user.UserId.ToString());
            claims.AddRole(operationClaims.Select(c => c.Name).ToArray());
            return claims;
        }
        public IEnumerable<Claim> GetClaimsForCustomer(int tableId, string role, int waiterId)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(tableId.ToString());
            claims.AddRole(role);
            claims.AddWaiter(waiterId.ToString());
            return claims;
        }
    }
}