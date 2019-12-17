using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Entities;
using Core.Entities.Concrete;

namespace Entities.Concrete
{
    public class Order:IEntity
    {
        [Key]
        [Required]
        public int OrderId { get; set; }
        [Required]
        [ForeignKey("Table")]
        public int TableId { get; set; }
        public Table Table { get; set; }
        [Required]
        [ForeignKey("MenuItem")]
        public int ItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        [Required]
        public int Quantity { get; set; }
        [ForeignKey("User")]
        public int WaiterId { get; set; }
        public User User { get; set; }
        [Required]
        public bool IsDelivered { get; set; }
        public bool IsDummy { get; set; }
    }
}
