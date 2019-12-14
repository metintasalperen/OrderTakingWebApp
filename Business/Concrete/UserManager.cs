using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Core.Entities.Concrete;
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

        public List<User> GetByRole(string role)
        {
            return _userDal.GetList(p => p.Role == role);
        }
        public List<OperationClaims> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public User GetByUserId(int userId)
        {
            return _userDal.Get(u => u.UserId == userId);
        }

        public void Delete(int userId)
        {
            _userDal.Delete(new User { UserId = userId });
        }

        public void Update(User user)
        {
            _userDal.Update(user);
        }
    }
}
