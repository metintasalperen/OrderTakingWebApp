using System.Collections.Generic;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IMenuService
    {
        List<MenuItem> GetAll();
        List<string> GetCategories();
    }
}
