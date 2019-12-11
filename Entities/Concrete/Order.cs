using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class Order:IEntity
    {
        [Key]
        public int OrderId { get; set; }
        public Table Table { get; set; }
        public Menu Menu { get; set; }
        public int Quantity { get; set; }
        public int WaiterId { get; set; }
        public bool IsDelivered { get; set; }
    }
}
