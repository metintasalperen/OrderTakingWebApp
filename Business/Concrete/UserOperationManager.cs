using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class UserOperationManager : IUserOperationService
    {
        private IUserOperationClaimsDal _userOperationClaimsDal;

        public UserOperationManager(IUserOperationClaimsDal userOperationClaimsDal)
        {
            _userOperationClaimsDal = userOperationClaimsDal;
        }

        public void Add(UserOperationClaims userOperationClaims)
        {
            _userOperationClaimsDal.Add(userOperationClaims);
        }
    }
}