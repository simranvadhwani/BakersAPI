using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakers.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTime CreatedDate {  get; set; }  
    }
}
