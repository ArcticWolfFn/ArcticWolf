using BotCord.Models.Enums;
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
    /// The LogController handles the logging system.
    /// </summary>
    public class LogController : ControllerBase
    {
        private readonly string _logFolderPath = "../Logs/";
        private StreamWriter _streamWriter = null;
        private Timer _writeNextLogLineTimer = null;
        private static readonly List<StoredLogMessage> _logMessageStorage = new();

        public enum LogType
        {
            Info,
            Debug,
            Warning,
            Error,
            Success,
        }

        private struct StoredLogMessage
        {
            public String message;
            public LogType logType;
        }

        /// <summary>
        /// Initializes the controller. Only called by the MainController.
        /// </summary>
        /// <returns>Controller Initialization Status</returns>
        public override StatusReport Init()
        {
            _writeNextLogLineTimer = new Timer
            {
                Interval = 100
            };
            _writeNextLogLineTimer.Elapsed += WriteNextLogLineTimer_Elapsed;
            _writeNextLogLineTimer.Start();

            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }

            //create log file
            try
            {
                _streamWriter = new StreamWriter(File.Create(_logFolderPath + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".log"));
            }
            catch (Exception ex)
            {
                LogController.WriteLine("Couldn't create LogFile. Aborting...\nDetailed Information: " + ex.Message, LogType.Error);
                return StatusReport.FatalError;
            }

            InitDone = true;
            return StatusReport.OK;
        }

        private void WriteNextLogLineTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _writeNextLogLineTimer.Stop();
            if (_logMessageStorage.Count == 0)
            {
                _writeNextLogLineTimer.Start();
                return;
            }

            List<StoredLogMessage> LMS_Dump = new(_logMessageStorage);
            _logMessageStorage.Clear();

            foreach (StoredLogMessage slm in LMS_Dump)
            {
                //detect and set console color and log type
                string ltype;
                ConsoleColor consoleColor;
                switch (slm.logType)
                {
                    case LogType.Error:
                        ltype = "ERROR";
                        consoleColor = ConsoleColor.Red;
                        break;
                    case LogType.Warning:
                        ltype = "WARNING";
                        consoleColor = ConsoleColor.Yellow;
                        break;
                    case LogType.Debug:
                        if (!Bot.Debug)
                        {
                            _writeNextLogLineTimer.Start();
                            return;
                        }
                        ltype = "DEBUG";
                        consoleColor = ConsoleColor.Gray;
                        break;
                    case LogType.Success:
                        ltype = "SUCCESS";
                        consoleColor = ConsoleColor.Green;
                        break;
                    default:
                        ltype = "INFO";
                        consoleColor = ConsoleColor.Blue;
                        break;
                }
                if (consoleColor != ConsoleColor.Black)
                    Console.ForegroundColor = consoleColor;

                //fix alignment
                if (slm.logType == LogType.Success)
                {
                    Console.Write("[" + ltype + "]\t");
                }
                else
                {
                    Console.Write("[" + ltype + "]\t\t");
                }
                Console.ResetColor(); // only change the color of the log type

                Console.WriteLine(slm.message);

                //only write to the streamwriter if the controller is initialized
                if (InitDone)
                {
                    _streamWriter.WriteLine("[" + ltype + "]\t" + slm.message);
                    _streamWriter.Flush();
                }
            }

            _writeNextLogLineTimer.Start();
        }

        /// <summary>
        /// Writes the specified string to the console and to the log file if InitDone is true
        /// </summary>
        /// <param name="message">The log message</param>
        /// <param name="logType">The log type of the message (default is "Info")</param>
        public static void WriteLine(string message, LogType logType = LogType.Info)
        {
            StoredLogMessage storedLogMessage;
            storedLogMessage.logType = logType;
            storedLogMessage.message = "<" + DateTime.Now + "> " + message;
            _logMessageStorage.Add(storedLogMessage);
        }

        /// <summary>
        /// Shutdowns the controller. Only called by the MainController.
        /// </summary>
        /// <returns>Controller Shutdown Status</returns>
        public override StatusReport ShutDown()
        {
            if (!InitDone)
            {
                return StatusReport.InitNotDone;
            }

            WriteNextLogLineTimer_Elapsed(null, null);

            _writeNextLogLineTimer.Stop();

            _streamWriter.Close();

            InitDone = false;

            return StatusReport.OK;
        }
    }
}
