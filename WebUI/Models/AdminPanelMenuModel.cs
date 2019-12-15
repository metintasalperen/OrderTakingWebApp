using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class AdminPanelMenuModel
    {
        public List<MenuItem> Menu { get; set; }
        public List<string> Categories { get; set; }
        public string CurrentCategory { get; set; }
    }
}
