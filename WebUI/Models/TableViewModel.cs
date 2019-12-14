using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class TableViewModel
    {
        public List<Table> Tables { get; set; }
        public string Action { get; set; }
        public Table ChosenTable { get; set; }
    }
}
