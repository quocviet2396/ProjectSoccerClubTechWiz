using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectModels
{
    [Table("Player")]
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? PlayerName { get; set; }
        
        public string? PlayerPosition { get; set; }
        public DateTime PlayerDOB { get; set; }
        public string? PlayerNationality { get; set; }
        public string? PlayerImg { get; set; }
        public string? PlayerOVR { get; set; }
        public int TeamId { get; set; } // Khóa ngoại đến đội bóng
        public Team? Team { get; set; } // Đội bóng của cầu thủ

    }
}