using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Extensions
{
    public static class SocketMessageExtension
    {
        public static bool IsDm(this SocketMessage msg)
        {
            if ((msg.Channel as SocketGuildChannel) == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the guild of the message.
        /// Returns <see langword="null"/> if no guild was found.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static SocketGuild GetGuild(this SocketMessage msg)
        {
            if (!msg.IsDm())
            {
                return null;
            }

            return (msg.Channel as SocketGuildChannel).Guild;
        }

        public static Task<RestUserMessage> Reply(this SocketMessage msg, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null)
        {
            MessageReference messageReference = new(msg.Id, msg.Channel.Id);

            return msg.Channel.SendMessageAsync(text, isTTS, embed, options, allowedMentions, messageReference);
        }

        public static Task<RestUserMessage> ReplyWithFile(this SocketMessage msg, string filePath, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, bool isSpoiler = false, AllowedMentions allowedMentions = null)
        {
            MessageReference messageReference = new(msg.Id, msg.Channel.Id);
            return msg.Channel.SendFileAsync(filePath, text, isTTS, embed, options, isSpoiler, allowedMentions, messageReference);
        }
    }
}
