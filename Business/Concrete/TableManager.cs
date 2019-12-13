using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class TableManager:ITableService
    {

        private ITableDal _tableDal;

        public TableManager(ITableDal tableDal)
        {
            _tableDal = tableDal;
        }

        public Table GetByTableId(int tableId)
        {
            return _tableDal.Get(u => u.TableId == tableId);
        }
        public List<Table> GetAll()
        {
            return _tableDal.GetList();
        }
        public void Update(Table table)
        {
            _tableDal.Update(table);
        }
    }
}
