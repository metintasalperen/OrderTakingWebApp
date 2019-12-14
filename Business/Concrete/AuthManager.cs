using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
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
        private IUserOperationService _userOperationService;
        private IOperationService _operationService;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IUserOperationService userOperationService, IOperationService operationService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _userOperationService = userOperationService;
            _operationService = operationService;
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
            var operation = new UserOperationClaims
            {
                OperationClaims = _operationService.GetByName(user.Role),
                User = _userService.GetByUserName(user.UserName)
            };
            _userOperationService.Add(operation);
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
        public IDataResult<AccessToken> CreateAccessTokenForCustomer(int tableId, string role)
        {
            var accessToken = _tokenHelper.CreateTokenForCustomer(tableId, role);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }
    }
}