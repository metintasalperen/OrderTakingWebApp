using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class Order:IEntity
    {
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public int WaiterId { get; set; }
        public bool IsDelivered { get; set; }
    }
}
