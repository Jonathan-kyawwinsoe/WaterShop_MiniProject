namespace water_shop.Entity
{
    public class VoucherDetail
    {
        public int Id { get; set; }
        public int? VoucherId { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public virtual Voucher? Voucher { get; set; }
        public virtual Products? Product { get; set; }
    }
}
