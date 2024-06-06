using System.ComponentModel.DataAnnotations;

namespace BetaCycle4.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ShoppingId { get; set; }

        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;


    }
}
