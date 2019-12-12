using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }
        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public User GetByFirstName(string firstName)
        {
            return _userDal.Get(u => u.FirstName == firstName);
        }

        public User GetByLastName(string lastName)
        {
            return _userDal.Get(u => u.LastName == lastName);
        }

        public User GetByUserName(string userName)
        {
            return _userDal.Get(u => u.UserName == userName);
        }
        public List<OperationClaims> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }
    }
}
