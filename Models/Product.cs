using System.ComponentModel.DataAnnotations;

namespace BackendAPIDevelopmentTask.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
