using System.Collections.Generic;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ITableService
    {
        List<Table> GetAll();
        void Update(Table table);
        Table GetByTableId(int tableId);
    }
}
