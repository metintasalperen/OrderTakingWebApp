using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class Menu:IEntity
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
        public int Sold { get; set; }
        public string Explanation { get; set; }
        public decimal Price { get; set; }
    }
}
