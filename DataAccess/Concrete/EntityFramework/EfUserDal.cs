using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, OrderingContext>, IUserDal
    {
        public List<OperationClaims> GetClaims(User user)
        {
            using (var context = new OrderingContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaims in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaims.OperationClaimId
                             where userOperationClaims.UserId == user.UserId
                             select new OperationClaims
                             {
                                 Id = operationClaim.Id,
                                 Name = operationClaim.Name
                             };
                return result.ToList();
            }
        }
    }
}
