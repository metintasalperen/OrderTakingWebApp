using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class Menu:IEntity
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
        public int Sold { get; set; }
        public string Explanation { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
    }
}
