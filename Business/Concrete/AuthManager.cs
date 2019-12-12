using Business.Abstract;
using Business.Constants;
using Core.Utilities.Hashing;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            if (UserExist(userForRegisterDto.UserName)) return new ErrorDataResult<User>(Messages.RegisterFailed);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                UserName = userForRegisterDto.UserName,
                Role = userForRegisterDto.Role
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.RegisterSuccess);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByUserName(userForLoginDto.UserName);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.LoginFailed);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash,
                userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.LoginFailed);
            }
            return new SuccessDataResult<User>(userToCheck, Messages.LoginSuccess);
        }

        public bool UserExist(string userName)
        {
            return _userService.GetByUserName(userName) != null;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims =_userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken,Messages.AccessTokenCreated);
        }
    }
}