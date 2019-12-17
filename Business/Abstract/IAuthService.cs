using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);

        bool UserExist(string userName);
        IDataResult<AccessToken> CreateAccessToken(User user);
        IDataResult<AccessToken> CreateAccessTokenForCustomer(int tableId, string role, int waiterId)
    }
}