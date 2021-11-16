using FNitePlusBot.Models.API.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNitePlusBot.Models.API.Motd
{
    public class ContentFields
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("buttonTextOverride")]
        public string ButtonTextOverride { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("secondaryButtonTextOverride")]
        public object SecondaryButtonTextOverride { get; set; }

        [JsonProperty("entryType")]
        public string EntryType { get; set; }

        [JsonProperty("image")]
        public List<Image> Image { get; set; }

        [JsonProperty("offerId")]
        public string OfferId { get; set; }

        [JsonProperty("tabTitleOverride")]
        public string TabTitleOverride { get; set; }

        [JsonProperty("tileImage")]
        public List<Image> TileImage { get; set; }

        [JsonProperty("videoAutoplay")]
        public object VideoAutoplay { get; set; }

        [JsonProperty("videoLoop")]
        public object VideoLoop { get; set; }

        [JsonProperty("videoMute")]
        public object VideoMute { get; set; }

        [JsonProperty("videoStreamingEnabled")]
        public object VideoStreamingEnabled { get; set; }

        [JsonProperty("videoUID")]
        public object VideoUID { get; set; }

        [JsonProperty("videoVideoString")]
        public object VideoVideoString { get; set; }

        [JsonProperty("videoFullscreen")]
        public object VideoFullscreen { get; set; }

        [JsonProperty("offerAction")]
        public object OfferAction { get; set; }

        [JsonProperty("challengeCategoryTag")]
        public object ChallengeCategoryTag { get; set; }

        [JsonProperty("eventid")]
        public object Eventid { get; set; }

        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }

        [JsonProperty("islandCode")]
        public string IslandCode { get; set; }

        [JsonProperty("websiteButtonText")]
        public object WebsiteButtonText { get; set; }

        [JsonProperty("websiteURL")]
        public object WebsiteURL { get; set; }
    }
}
