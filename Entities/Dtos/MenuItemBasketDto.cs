using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Dtos
{
    public class MenuItemBasketDto: IDto
    {
        public int quantity { get; set; }
        public int itemId { get; set; }
        public Decimal price { get; set; }
        public string itemName { get; set; }
        public int isUpdate { get; set; }
    }
}
