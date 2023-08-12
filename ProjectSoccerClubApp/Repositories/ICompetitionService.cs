using System;
using ProjectModels;

namespace ProjectSoccerClubApp.Repositories
{
	public interface ICompetitionService
	{
        Task<IEnumerable<Competition>> GetCompetition();
        Task<bool> addCompetition(Competition newCompetition);
        Task<bool> editCompetition(Competition newCompetition);

        Task<Competition> GetCompetitionById(int id);
    }
}

