using System;
using System.Text;

using NLog;
using NLog.Config;
using NLog.Targets;

namespace Common
{
    public class Logger
    {
        private static NLog.Logger logger;

        static Logger()
        {
            var config = new LoggingConfiguration();

            var infoFileTarget = new FileTarget("infoTarget")
            {
                Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${level} ${message} ${newline}",
                FileName = "${baseDir}/logs/${level}/${shortdate}.log",
                Encoding = Encoding.UTF8
            };

            config.AddTarget(infoFileTarget);

            var errorFileTarget = new FileTarget("errorTarget")
            {
                Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${level} ${message}${newline}${exception:format=tostring}${newline}",
                FileName = "${baseDir}/logs/${level}/${shortdate}.log",
                Encoding = Encoding.UTF8
            };
            config.AddTarget(errorFileTarget);

            config.AddRuleForOneLevel(LogLevel.Info, infoFileTarget);
            config.AddRuleForOneLevel(LogLevel.Error, errorFileTarget);

            LogManager.Configuration = config;

            logger = LogManager.GetLogger("NLogger");

        }

        public static void Error(string msg, Exception ex)
        {
            logger.Error(ex, msg);
        }

        public static void Info(string msg)
        {
            logger.Info(msg);
        }
    }
}
