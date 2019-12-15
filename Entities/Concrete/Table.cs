using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Entities;
using Core.Entities.Concrete;

namespace Entities.Concrete
{
    public class Table:IEntity
    {
        [Key]
        [Required]
        public int TableId { get; set; }
        [Required]
        public bool IsEmpty { get; set; }
        public string Token { get; set; }
    }
}
