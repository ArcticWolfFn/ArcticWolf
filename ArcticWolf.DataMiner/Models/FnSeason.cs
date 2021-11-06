using System;

namespace ArcticWolf.DataMiner.Models
{
    public struct FnSeason
    {
        public readonly int SeasonNumber;

        public readonly DateTime StartTime;
        public readonly DateTime EndTime;

        public FnSeason(int seasonNum, DateTime startTime, DateTime endTime)
        {
            SeasonNumber = seasonNum;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
