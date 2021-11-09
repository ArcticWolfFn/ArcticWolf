using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class TournamentInformationEntry : BasePagesEntry
    {
        public TournamentInformationEntry(params object[] motds)
          : base("tournamentinformation")
        {
        }

        public TournamentInformation tournament_info = new();
    }
}
