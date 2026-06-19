namespace water_shop.Entity
{
    public class Customer
    {
        public int Id { get; set; } 
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int  EmptyBottleOwed { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual List<Voucher> Vouchers { get; set; } = new List<Voucher>();
        public virtual List<EmptyBottleTransaction> EmptyBottleTransactions { get; set; } = new List<EmptyBottleTransaction>();
    }
}
