using ArcticWolfApi.Models.Calendar;
using ArcticWolfApi.Models.Calendar.States;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ArcticWolfApi.Controllers
{
    [Route("fortnite/api/[controller]/v1/")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        [HttpGet("timeline")]
        public ActionResult<Timeline> GetTimeline()
        {
            int seasonNumber = Request.GetSeasonNumber();
            decimal buildVersion = Request.GetBuildVersion();

            // set lobby season for battle pass etc
            string str = seasonNumber == 2 ? "LobbyWinterDecor" : string.Format("LobbySeason{0}", seasonNumber);

            Timeline timeline1 = new();
            Timeline timeline2 = timeline1;

            Dictionary<string, TimelineChannel> dictionary1 = new Dictionary<string, TimelineChannel>
            {
                ["standalone-store"] = new TimelineChannel(new ChannelState[1]
                {
                    new ChannelState()
                    {
                        ActiveEvents = new List<ChannelEvent>(),
                        State = (object) new StandaloneStoreState()
                    }
                }),

                ["client-events"] = new TimelineChannel(new ChannelState[1]
                {
                    new ChannelState()
                    {
                        ActiveEvents = new List<ChannelEvent>()
                        {
                            new ChannelEvent("EventFlag." + str),
                            new ChannelEvent("EventFlag.LTQ_S15_Legendary_Week_01"),
                        },

                        State = new ClientEventsState(seasonNumber)
                    }
                })
            };

            Dictionary<string, TimelineChannel> dictionary2 = dictionary1;

            timeline2.Channels = dictionary2;

            Timeline timeline3 = timeline1;
            if (seasonNumber != 13)
            {
                return timeline3;
            }

            int eventFlagWLId;
            if (buildVersion < 13.30M)
            {
                if (buildVersion < 13.10M)
                {
                    if (buildVersion >= 13.00M)
                    {
                        eventFlagWLId = 7;
                        goto label_10;
                    }
                }
                else if (buildVersion >= 13.20M)
                {
                    eventFlagWLId = 6;
                    goto label_10;
                }
                eventFlagWLId = 7;
            }
            else
            {
                eventFlagWLId = buildVersion < 13.40M ? 4 : 0;
            }

            label_10:
            timeline3.Channels["client-events"].States[0].ActiveEvents.Add(new ChannelEvent(string.Format("EventFlag.WL{0}", eventFlagWLId)));

            return timeline3;
        }
    }
}
