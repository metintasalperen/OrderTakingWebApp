using Core.DataAccess;
using Entities.Concrete;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IMenuDal : IEntityRepository<MenuItem>
    {
        // Custom Operations will be here.
        List<string> GetCategories();
    }
}
