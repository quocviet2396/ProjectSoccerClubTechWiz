using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectModels
{
	[Table("Order")]
	public class Order
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string TrackingNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string PaymentId { get; set; }
        public Products Product { get; set; } 

        [Required]
        public string ZipCode{ get; set; }

        public User User { get; set; }
        public Categories Category { get; set; }
    }
}

