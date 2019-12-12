using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;


namespace Business.Concrete
{
    public class MenuManager:IMenuService
    {
        private IMenuDal _menuDal;

        public MenuManager(IMenuDal menuDal)
        {
            _menuDal = menuDal;
        }

        public List<MenuItem> GetAll()
        {
            return _menuDal.GetList();
        }

        public List<string> GetCategories()
        {
            return _menuDal.GetCategories();
        }
    }
}
