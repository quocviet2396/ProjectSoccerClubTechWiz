using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectModels
{
    [Table("Team")]
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? TeamName { get; set; }
        [Required]
        public string? CoachName { get; set; }
        [Required]
        public string? TeamCity { get; set; }
        [Required]
        public string? TeamStadium { get; set; }
        [Required]
        public string? Logo { get; set; }
        public string? Website { get; set; }


        public List<Player>? Player { get; set; } // Danh sách cầu thủ của đội bóng
    }
}
