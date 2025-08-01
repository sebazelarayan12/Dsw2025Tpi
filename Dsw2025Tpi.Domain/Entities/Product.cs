using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Product : EntityBase
    {
        public Product()
        {
        }
        public Product(string sku, string name, decimal price, string internalCode, string description, int stock)
        {
            Sku = sku;
            Name = name;
            CurrentUnitPrice = price;
            InternalCode = internalCode;
            Description = description;
            StockQuantity = stock;
            IsActive = true;
        }
        public string? Sku { get; set; }
        public string? Name { get; set; }
        public decimal CurrentUnitPrice { get; set; }
        public string InternalCode { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    }
}
