using System.Collections.Generic;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IMenuService
    {
        MenuItem GetById(int id);
        List<MenuItem> GetAll();
        List<string> GetCategories();
        List<MenuItem> GetByCategory(string category);
        void Update(MenuItem item);
        void Add(MenuItem item);
        void Delete(int itemId);
    }
}
