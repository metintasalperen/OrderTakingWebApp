using Entities.Concrete;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class OrderListViewModel
    {
        public List<Order> Orders { get; internal set; }
        public int WaiterId { get; internal set; }
        public List<MenuItem> Menu { get; internal set; }
        public List<Table> Table { get; internal set; }
        public StringValues CurrentCategory { get; internal set; }
        public List<Order> TableOrders { get; internal set; }
    }
}
