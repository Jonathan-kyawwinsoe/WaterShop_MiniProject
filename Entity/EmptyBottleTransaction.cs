using System.ComponentModel.DataAnnotations;

namespace water_shop.Entity
{
    public class EmptyBottleTransaction
    {
        
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? VoucherId { get; set; }
        public DateTime TransactDate { get; set; }
        public int QuantityIn { get; set; }
        public int QuantityOut { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Products? Product { get; set; }
        public virtual Voucher? Voucher { get; set; }
    }
}
