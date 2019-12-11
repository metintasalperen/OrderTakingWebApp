using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class MenuItem:IEntity
    {
        [Key]
        public int ItemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public int Sold { get; set; }
        public string Explanation { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        [Required]
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }
}
