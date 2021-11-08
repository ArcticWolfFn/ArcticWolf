using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Storage.Constants
{
    public class Fortnite
    {
        public static readonly List<decimal> Versions = new()
        {
            15.30M
        };

        public static readonly List<FnSeason> Seasons = new()
        {
            new FnSeason(
                seasonNum: 14,
                startTime: new DateTime(year: 2020, month: 8, day: 27, hour: 6, minute: 0, second: 0, DateTimeKind.Utc),
                endTime: new DateTime(year: 2020, month: 12, day: 2, hour: 4, minute: 59, second: 59, DateTimeKind.Utc)
                ),
            new FnSeason(
                seasonNum: 15,
                startTime: new DateTime(year: 2020, month: 12, day: 2, hour: 5, minute: 0, second: 0, DateTimeKind.Utc),
                endTime: new DateTime(year: 2021, month: 3, day: 16, hour: 3, minute: 59, second: 59, DateTimeKind.Utc)
                ),
            new FnSeason(
                seasonNum: 16,
                startTime: new DateTime(year: 2021, month: 3, day: 16, hour: 4, minute: 0, second: 0, DateTimeKind.Utc),
                endTime: new DateTime(year: 2021, month: 6, day: 8, hour: 5, minute: 59, second: 59, DateTimeKind.Utc)
                ),
            new FnSeason(
                seasonNum: 17,
                startTime: new DateTime(year: 2021, month: 6, day: 8, hour: 6, minute: 0, second: 0, DateTimeKind.Utc),
                endTime: new DateTime(year: 2021, month: 9, day: 13, hour: 5, minute: 59, second: 59, DateTimeKind.Utc)
                ),
            new FnSeason(
                seasonNum: 18,
                startTime: new DateTime(year: 2021, month: 6, day: 8, hour: 6, minute: 0, second: 0, DateTimeKind.Utc),
                endTime: new DateTime(year: 2021, month: 12, day: 31, hour: 5, minute: 59, second: 59, DateTimeKind.Utc)
                ),
        };
    }
}
