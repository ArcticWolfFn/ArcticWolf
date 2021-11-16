using BotCord.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BotCord.Controllers
{
    /// <summary>
    /// Handles the configuration file.
    /// </summary>
    public class ConfigController : ControllerBase
    {
        private readonly string LOG_PREFIX = $"[{nameof(ConfigController)}] ";
        private readonly string CONFIG_FILE_PATH = "./config.json";

        public static Models.Config Config = null;
        private static readonly Timer _configAutoSaveTimer = new(30000);

        /// <summary>
        /// Initializes the controller. Called by the MainController.
        /// </summary>
        /// <returns>Controller Initialization Status</returns>
        public override StatusReport Init()
        {
            if (!File.Exists(CONFIG_FILE_PATH))
            {
                try
                {
                    File.WriteAllText(CONFIG_FILE_PATH, JsonConvert.SerializeObject(new Models.Config()));
                }
                catch (Exception ex)
                {
                    LogController.WriteLine(LOG_PREFIX + "Couldn't create config file. Error: " + ex.Message, LogController.LogType.Error);
                    return StatusReport.FatalError;
                }
            }

            string ConfigFileContent = "";
            try
            {
                ConfigFileContent = File.ReadAllText(CONFIG_FILE_PATH, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogController.WriteLine(LOG_PREFIX + "Couldn't read config file. Error: " + ex.Message, LogController.LogType.Error);
                return StatusReport.FatalError;
            }

            if (string.IsNullOrWhiteSpace(ConfigFileContent))
            {
                Config = new Models.Config();
            }
            else
            {
                try
                {
                    Config = JsonConvert.DeserializeObject<Models.Config>(ConfigFileContent);
                }
                catch (Exception ex)
                {
                    LogController.WriteLine(LOG_PREFIX + "Couldn't convert config file to JSON object. Error: " + ex.Message, LogController.LogType.Error);
                    return StatusReport.FatalError;
                }
            }

            _configAutoSaveTimer.Elapsed += ConfigAutoSaveTimer_Elapsed;
            _configAutoSaveTimer.Start();

            InitDone = true;

            if (Config.MainDiscordGuildID == 0)
            {
                LogController.WriteLine(LOG_PREFIX + "MainDiscordGuildID can't be 0", LogController.LogType.Error);
                return StatusReport.FatalError;
            }

            return StatusReport.OK;
        }

        private void ConfigAutoSaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SaveConfig();
        }

        /// <summary>
        /// Shutdowns the controller. Called by the MainController.
        /// </summary>
        /// <returns>Controller Shutdown Status</returns>
        public override StatusReport ShutDown()
        {
            if (!InitDone)
            {
                return StatusReport.InitNotDone;
            }

            _configAutoSaveTimer.Stop();
            SaveConfig();

            return StatusReport.OK;
        }

        private void SaveConfig()
        {
            var ConfigJSON = JsonConvert.SerializeObject(Config);
            File.WriteAllText(CONFIG_FILE_PATH, ConfigJSON);
        }
    }
}
