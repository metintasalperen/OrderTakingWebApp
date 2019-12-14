using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;
using Entities.Dtos;

namespace WebUI.Models
{
    public class MenuViewModel
    {
        public List<MenuItem> Menu { get; set; }
        public List<string> Categories { get; set; }
        public string CurrentCategory { get; set; }
        public List<MenuItemBasketDto> Basket { get; set; }
        public int TableNumber { get; set; }
    }
}
