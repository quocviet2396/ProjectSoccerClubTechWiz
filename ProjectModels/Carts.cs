using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectModels
{
    [Table("Carts")]
    public class Carts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
      

        public int ProductId { get; set; }
        public Products Product { get; set; } // Thêm thuộc tính Product để định nghĩa mối quan hệ
        public Categories Category { get; set; } // Thêm thuộc tính Product để định nghĩa mối quan hệ
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
