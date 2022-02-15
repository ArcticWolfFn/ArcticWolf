using ArcticWolf.Storage.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Storage
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
                    _startsInSeason = StartTime.GetSeason();
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
                    _endsInSeason = EndTime.GetSeason();
                }

                return _endsInSeason;
            }
        }
        private FnSeason _endsInSeason;

        [NotMapped]
        public IEnumerable<FnSeason> Seasons
        {
            get
            {
                if (_seasons == null)
                {
                    _seasons = Fortnite.Seasons.Where(x =>
                        // starts in season
                        (x.StartTime <= StartTime && x.EndTime >= StartTime) ||

                        // ends in season
                        (x.StartTime <= EndTime && x.EndTime >= EndTime) ||

                        // during season
                        (x.StartTime >= StartTime && x.EndTime <= StartTime)
                        );
                }
                return _seasons;
            }
        }
        private IEnumerable<FnSeason> _seasons = null;
    }
}
