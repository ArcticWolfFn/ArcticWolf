using BotCord.Models.Enums;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Commands
{
    /// <summary>
    /// Base class for bot commands.
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// Indicates whether the command was successfully initialized.
        /// </summary>
        protected bool InitDone = false;

        public abstract void Handle(SocketMessage msg, string[] msg_args);

        public abstract StatusReport Init();

        public abstract StatusReport ShutDown();
    }
}
