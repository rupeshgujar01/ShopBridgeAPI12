using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Models
{
        public class ItemDTO
        {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int? Discount { get; set; }
        public bool? OnSale { get; set; }
    }
}
