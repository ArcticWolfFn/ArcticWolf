using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Models
{
    public class Config
    {
        public string DiscordBotToken = null;
        public ulong MainDiscordGuildID = 0;
        public ulong StaffRoleID = 0;
        public string BotPrefix = "%";
        public ulong LogChannelID = 0;
    }
}
