using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.DataAccess.EntityFramework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOperationClaimsDal : EfEntityRepositoryBase<OperationClaims, OrderingContext>, IOperationClaimsDal
    {
        
    }
}