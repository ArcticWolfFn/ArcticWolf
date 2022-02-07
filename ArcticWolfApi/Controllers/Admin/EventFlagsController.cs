using ArcticWolf.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcticWolfApi.Controllers.Admin
{
    /// <summary>
    /// Used for a custom data dashboard
    /// </summary>
    [Route("/admin/[controller]/")]
    [ApiController]
    public class EventFlagsController : ControllerBase
    {
        [HttpGet]
        public List<EventFlagMinified> GetUserSetting()
        {
            List<EventFlagMinified> returnedFlags = new();

            List<FnEventFlag> foundFlags = Program.Database.FnEventFlags.Include(x => x.TimeSpans).ToList();


            foreach (FnEventFlag flag in foundFlags)
            {
                EventFlagMinified flagMin = new();
                flagMin.Id = flag.Event;

                List<string> seasons = new();

                foreach (FnEventFlagTimeSpan ts in flag.TimeSpans)
                {
                    foreach (FnSeason season in ts.Seasons)
                    {
                        if (!seasons.Contains("S" + season.SeasonNumber))
                        {
                            seasons.Add("S" + season.SeasonNumber);
                        }
                    }
                }

                flagMin.Seasons = string.Join(", ", seasons);

                if (flag.TimeSpans.Any())
                {
                    flagMin.FirstUsage = flag.TimeSpans.OrderBy(x => x.StartTime).First().StartTime;
                    flagMin.LastUsage = flag.TimeSpans.OrderBy(x => x.EndTime).First().EndTime;
                }

                returnedFlags.Add(flagMin);
            }

            return returnedFlags;
        }

        /// <summary>
        /// Model for returning minimal information about the event flag
        /// </summary>
        public class EventFlagMinified
        {
            public string Id {get; set;}
            public string Seasons { get; set; }
            public DateTime FirstUsage { get; set; }
            public DateTime LastUsage { get; set; }
        }
    }
}
