using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectModels
{
    [Table("Match")]
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CompetitionId { get; set; }
      

        [Required]
        public int HomeTeamId { get; set; } // Khóa ngoại đến đội nhà
        [Required]
        public int AwayTeamId { get; set; } // Khóa ngoại đến đội khách

        [Required]
        public DateTime MatchTime { get; set; }
        [Required]
        public string Stadium { get; set; }

        public string? Result { get; set; }

        public Team? HomeTeam { get; set; } // Quan hệ với đội nhà
        public Team? AwayTeam { get; set; } // Quan hệ với đội khách
        public Competition? Competition { get; set; }
    }
}
