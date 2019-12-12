using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMenuDal : EfEntityRepositoryBase<MenuItem, OrderingContext>, IMenuDal
    {
        private OrderingContext _context = new OrderingContext();
        public List<string> GetCategories()
        {
            using (var context = new OrderingContext())
            {
                var result = context.MenuItems.Select(m => m.Category).Distinct().ToList();
                return result;
            }
            throw new NotImplementedException();
        }
    }
}
