using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DellyBelly.Domain.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual Customer User { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public DateTime AddedAt { get; set; }
    }
}
