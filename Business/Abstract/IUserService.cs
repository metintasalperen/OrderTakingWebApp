using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaims> GetClaims(User user);
        void Add(User user);
        User GetByFirstName(string firstName);
        User GetByLastName(string lastName);    
        User GetByUserName(string userName);
        List<User> GetByRole(string role);
    }
}
