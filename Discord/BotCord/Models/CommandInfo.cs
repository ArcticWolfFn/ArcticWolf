using BotCord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Models
{
    public struct CommandInfo
    {
        public readonly CommandBase Command;

        /// <summary>
        /// The higher the BootPriority, the earlier the command is initialized.
        /// </summary>
        public readonly int InitPriority;

        /// <summary>
        /// The higher the ShutDownPriority, the earlier the command is shutdown.
        /// </summary>
        public readonly int ShutDownPriority;

        /// <summary>
        /// The command without the bot prefix (for example "help")
        /// </summary>
        public readonly string ChatCommand;

        /// <summary>
        /// Contains a small text explaining what the command does
        /// </summary>
        public readonly string CommandDetails;

        /// <summary>
        /// Indicates if a command can only be used by VCC staff
        /// </summary>
        public readonly bool IsStaffOnly;

        public CommandInfo(CommandBase command, int initPriority, int shutDownPriority, string chatCommand, string commandDetails, bool isStaffOnly = false)
        {
            Command = command;
            InitPriority = initPriority;
            ShutDownPriority = shutDownPriority;
            ChatCommand = chatCommand;
            CommandDetails = commandDetails;
            IsStaffOnly = isStaffOnly;
        }
    }
}
