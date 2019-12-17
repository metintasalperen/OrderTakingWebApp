using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class AdminPanelMenuAddModel
    {
        public MenuItem MenuItem { get; set; }
        [Display(Name = "File")]
        public IFormFile Image { get; set; }
    }
}
