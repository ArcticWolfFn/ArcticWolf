using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class PlaylistInformationEntry : BasePagesEntry
    {
        public PlaylistInformationEntry(params object[] motds)
          : base("playlistinformation")
        {
        }

        public bool is_tile_hidden = false;
        public bool show_ad_violator = false;

        public string frontend_matchmaking_header_style = "Basic";
        public string frontend_matchmaking_header_text = "Frontend Matchmaking Header Text";
        public string frontend_matchmaking_header_text_description = "Frontend Matchmaking Header Description";

        public PlaylistInformation playlist_info = new();
    }
}
