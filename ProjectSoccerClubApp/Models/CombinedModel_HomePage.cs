namespace ProjectSoccerClubApp.Models
{
    public class CombinedModel_HomePage
    {
        public IEnumerable<ProjectModels.Player> Top10Players { get; set; }    
        public IEnumerable<ProjectModels.News>News { get; set; }
        
    }
}
