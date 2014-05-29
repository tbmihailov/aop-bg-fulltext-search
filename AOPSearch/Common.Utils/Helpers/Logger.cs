
namespace Common.Utils.Logging
{
    using System;
    using log4net;
    using log4net.Config;

    public static class Logger
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Logger));

        static Logger()
        {
            XmlConfigurator.Configure();
        }

        public static void Log(LogLevel logLevel, String log)
        {
            if (logLevel.Equals(LogLevel.DEBUG))
            {
                logger.Debug(log);
            }
            else if (logLevel.Equals(LogLevel.ERROR))
            {
                logger.Error(log);
            }
            else if (logLevel.Equals(LogLevel.FATAL))
            {
                logger.Fatal(log);
            }
            else if (logLevel.Equals(LogLevel.INFO))
            {
                logger.Info(log);
            }
            else if (logLevel.Equals(LogLevel.WARNING))
            {
                logger.Warn(log);
            }
        }

        public enum LogLevel
        {
            DEBUG = 1,

            ERROR = 4,

            FATAL = 3,

            INFO = 0,

            WARNING = 2
        }
    }
}
