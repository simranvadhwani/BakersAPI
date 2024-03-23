﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakers.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int  productId { get; set; } 
        public string? Name { get; set; }
        public string? Discription { get; set; }
        public decimal Price { get; set; }   

    }
}
