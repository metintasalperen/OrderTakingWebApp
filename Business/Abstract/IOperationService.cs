using Core.Entities.Concrete;

namespace Business.Abstract
{
    public interface IOperationService
    {
        OperationClaims GetByName(string name);
    }
}