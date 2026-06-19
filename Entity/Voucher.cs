namespace water_shop.Entity
{
    public class Voucher
    {
        public int Id { get; set; }
        public string VoucherName { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public int? DeliveryId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Status VoucherStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public virtual Customer? Customer { get; set; }
        public virtual Delivery? Delivery { get; set; }
        public virtual ICollection<VoucherDetail> VoucherDetails { get; set; } = new List<VoucherDetail>();
        public virtual ICollection<EmptyBottleTransaction> EmptyBottleTransactions { get; set; } = new List<EmptyBottleTransaction>();
    }
}
