using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface ITableDal:IEntityRepository<Table>
    {
        // Custom Operations will be here.
    }
}
