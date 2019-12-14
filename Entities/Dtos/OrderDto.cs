using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Dtos
{
    public class OrderDto : IDto
    {
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int WaiterId { get; set; }
        public bool IsDelivered { get; set; }
    }
}
