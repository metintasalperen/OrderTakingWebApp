using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserOperationClaims: EfEntityRepositoryBase<UserOperationClaims, OrderingContext>, IUserOperationClaims
    {
       
    }
}