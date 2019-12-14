using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;
using Entities.Dtos;

namespace WebUI.Models
{
    public class OrderViewModel
    {
        public int TableNumber { get; set; }
        public List<Order> DeliveredOrders { get; set; }
        public List<Order> NotDeliveredOrders { get; set; }
    }
}
