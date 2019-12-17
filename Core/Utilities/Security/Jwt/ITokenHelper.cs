using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities.Concrete;
using Microsoft.IdentityModel.Tokens;

namespace Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaims> operationClaims);
        AccessToken CreateTokenForCustomer(int tableId, string role, int waiterId);

        JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaims> operationClaims);

        JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, int tableId, string role, int waiterId,
            SigningCredentials signingCredentials);

        IEnumerable<Claim> GetClaims(User user, List<OperationClaims> operationClaims);
        IEnumerable<Claim> GetClaimsForCustomer(int tableId, string role, int waiterId);
    }
}