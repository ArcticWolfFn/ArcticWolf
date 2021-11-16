using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Extensions
{
    public static class SocketUserExtension
    {
        public static bool IsStaff(this SocketUser user)
        {
            if (user == null)
            {
                return false;
            }

            bool userHasStaffRole = Controllers.DiscordController.DiscordClient.GetGuild(Controllers.ConfigController.Config.MainDiscordGuildID)
                .GetUser(user.Id).Roles.Where(x => x.Id == Controllers.ConfigController.Config.StaffRoleID).Any();

            return userHasStaffRole;
        }
    }
}
