using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectModels
{
	[Table("Products")]
	public class Products
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int Id { get; set; }
	
		public int CategoryId { get; set; }

        public string? Name { get; set; }

        public string? Slug { get; set; }

        public string? Photo { get; set; }

        public int Quantity { get; set; }


        public string? Descrption { get; set; }

		public string? OriginalPrice { get; set; }

		public string? SellingPrice { get; set; }

		public bool Trending { get; set; }

		public bool Featured { get; set; }

		public bool status { get; set; }

        public Categories Category { get; set; }

        public ICollection<Carts> Carts { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}

