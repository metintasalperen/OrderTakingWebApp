using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class Table:IEntity
    {
        public int TableId { get; set; }
        public bool IsEmpty { get; set; }
    }
}
