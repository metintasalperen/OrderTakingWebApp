using System.Collections.Generic;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IMenuService
    {
        List<Menu> GetAll();
    }
}
