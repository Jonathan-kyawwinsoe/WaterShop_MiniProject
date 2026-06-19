namespace water_shop.Entity
{
    public class Delivery
    {
        public int Id { get; set; }
        public string DeliveryName { get; set; } = string.Empty;
        public string DeliveryPhone { get; set; } = string.Empty;
        public string VehicleNumber { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public virtual List<Voucher> Vouchers { get; set; } = new List<Voucher>();
    }
}
