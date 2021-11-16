using BotCord.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Models
{
    public struct ControllerInfo
    {
        public readonly ControllerBase Controller;

        /// <summary>
        /// The higher the BootPriority, the earlier the controller is initialized.
        /// </summary>
        public readonly int InitPriority;

        /// <summary>
        /// The higher the ShutDownPriority, the earlier the controller is shutdown.
        /// </summary>
        public readonly int ShutDownPriority;

        /// <summary>
        /// Shutdowns the entire application if the controller failed to initialize.
        /// </summary>
        public readonly bool ShutDownOnInitFail;

        /// <summary>
        /// Indicates whether the controller requires a working connection to Discord, 
        /// so the controller will only be initialized if the bot is connected to 
        /// Discord and will automatically shutdown if the connection is lost.
        /// </summary>
        public readonly bool CanOnlyRunIfConnectedToDiscord;

        public ControllerInfo(ControllerBase controller, int initPriority = 0, bool shutDownOnInitFail = false, int shutDownPriority = 0, bool canOnlyRunIfConnectedToDiscord = false)
        {
            Controller = controller;
            InitPriority = initPriority;
            ShutDownOnInitFail = shutDownOnInitFail;
            ShutDownPriority = shutDownPriority;
            CanOnlyRunIfConnectedToDiscord = canOnlyRunIfConnectedToDiscord;
        }
    }
}
