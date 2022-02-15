using ArcticWolf.Storage;
using ArcticWolf.Storage.Constants;
using System;

namespace ArcticWolf
{
    public static class DateTimeExtension
    {
        public static FnSeason GetSeason(this DateTime dateTime)
        {
            return Fortnite.Seasons.Find(x => x.StartTime <= dateTime && x.EndTime >= dateTime);
        }
    }
}
