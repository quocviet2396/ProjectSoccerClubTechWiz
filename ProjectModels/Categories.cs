using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectModels
{
	[Table("Categories")]
	public class Categories
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public bool status { get; set; }
        public ICollection<Products> Products { get; set; }
        public ICollection<Carts> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
        

    }
}

