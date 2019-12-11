using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IMenuDal : IEntityRepository<MenuItem>
    {
        // Custom Operations will be here.
    }
}
