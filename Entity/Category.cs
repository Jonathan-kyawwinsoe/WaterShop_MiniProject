namespace water_shop.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public virtual ICollection<Products> Products { get; set; } = new List<Products>();
    }
}
