using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class Table:IEntity
    {
        [Key]
        [Required]
        public int TableId { get; set; }
        [Required]
        public bool IsEmpty { get; set; }
    }
}
