using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class TournamentInformation
    {
        public string _type = "Tournaments Info";
        public List<Tournament> tournaments = new();
    }
}
