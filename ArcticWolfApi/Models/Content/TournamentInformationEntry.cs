namespace ArcticWolfApi.Models.Content
{
    public class TournamentInformationEntry : BasePagesEntry
    {
        public TournamentInformation tournament_info = new();

        public TournamentInformationEntry(params object[] motds) : base("tournamentinformation")
        {
        }
    }
}
