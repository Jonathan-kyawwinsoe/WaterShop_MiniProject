using System.Formats.Asn1;

namespace water_shop.Entity
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int? CategoryId { get; set; } 
        public decimal UnitsPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsReturnAble { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public virtual Category? Category { get; set; }
        public virtual List<VoucherDetail> VoucherDetails { get; set; } = new List<VoucherDetail>();
        public virtual List<EmptyBottleTransaction> EmptyBottleTransaction { get; set; } = new List<EmptyBottleTransaction>();
    }
}
