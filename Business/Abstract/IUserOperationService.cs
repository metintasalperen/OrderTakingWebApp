using Core.Entities.Concrete;

namespace Business.Abstract
{
    public interface IUserOperationService
    {
        void Add(UserOperationClaims userOperationClaims);
    }
}