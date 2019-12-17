using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class AdminPanelMenuUpdateModel
    {
        public MenuItem MenuItem { get; set; }
        public IFormFile Image { get; set; }
    }
}
