using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMenuDal : EfEntityRepositoryBase<Menu, OrderingContext>, IMenuDal
    {

    }
}
