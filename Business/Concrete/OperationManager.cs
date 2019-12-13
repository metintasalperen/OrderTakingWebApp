using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class OperationManager : IOperationService
    {
        private IOperationClaimsDal _operationClaimsDal;

        public OperationManager(IOperationClaimsDal operationClaimsDal)
        {
            _operationClaimsDal = operationClaimsDal;
        }

        public OperationClaims GetByName(string name)
        {
            return _operationClaimsDal.Get(u => u.Name == name);
        }
    }
}