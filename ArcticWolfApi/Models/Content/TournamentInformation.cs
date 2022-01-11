using System.Collections.Generic;

namespace ArcticWolfApi.Models.Content
{
    public class TournamentInformation
    {
        public string _type = "Tournaments Info";

        public List<Tournament> tournaments = new();
    }
}
