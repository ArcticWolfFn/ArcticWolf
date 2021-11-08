using ArcticWolf.DataMiner.Constants;
using ArcticWolf.DataMiner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Storage
{
    public class FnEventFlagTimeSpan
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        [NotMapped]
        public FnSeason StartsInSeason
        {
            get {
                if (_startsInSeason.SeasonNumber == 0)
                {
                    _startsInSeason = Fortnite.Seasons.Find(x => x.StartTime <= StartTime && x.EndTime >= StartTime);
                }

                return _startsInSeason;
            }
        }
        private FnSeason _startsInSeason;

        public DateTime EndTime { get; set; }

        [NotMapped]
        public FnSeason EndsInSeason
        {
            get
            {
                if (_endsInSeason.SeasonNumber == 0)
                {
                    _endsInSeason = Fortnite.Seasons.Find(x => x.StartTime <= EndTime && x.EndTime >= EndTime);
                }

                return _endsInSeason;
            }
        }
        private FnSeason _endsInSeason;
    }
}
