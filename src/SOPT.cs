// ItalianCannon.SOPT
using ItalianCannon;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.IO;

namespace ItalianCannon
{
    [StandardModule]
    public sealed class SOPT
    {
        public enum LogLevels
        {
            INFO,
            WARN,
            EXCEPTION,
            UKNN
        }

        public static ConsoleColor dpreColor = Console.ForegroundColor;

        public static ConsoleColor dbakColor = Console.BackgroundColor;

        public static void Out(string text, string sourceModule = "MAIN", LogLevels level = LogLevels.INFO, bool colorEnabled = true, bool IgnoreAnimations = false)
        {
            if (!IgnoreAnimations)
            {
                if (Constants.CurrentConfigurations.VerboseMode)
                {
                    return;
                }
                if (Constants.CurrentConfigurations.EnableAnimations)
                {
                    return;
                }
            }
            string timeStr = SOPT.GetTimePrefix();
            string lvlStr;
            ConsoleColor preColor;
            ConsoleColor bakColor;
            switch (level)
            {
            case LogLevels.INFO:
                lvlStr = "[INFO]";
                preColor = SOPT.dpreColor;
                bakColor = SOPT.dbakColor;
                break;
            case LogLevels.WARN:
                lvlStr = "[WARN]";
                preColor = ConsoleColor.Black;
                bakColor = ConsoleColor.Yellow;
                break;
            case LogLevels.EXCEPTION:
                lvlStr = "[EROR]";
                preColor = ConsoleColor.White;
                bakColor = ConsoleColor.DarkRed;
                break;
            default:
                lvlStr = "[UKNN]";
                preColor = ConsoleColor.White;
                bakColor = ConsoleColor.White;
                break;
            }
            string output = timeStr + lvlStr + "[" + sourceModule + "]: " + text;
            if (colorEnabled && Constants.CurrentConfigurations.EnableColors)
            {
                Console.BackgroundColor = bakColor;
                Console.ForegroundColor = preColor;
            }
            if (level == LogLevels.EXCEPTION)
            {
                Stream errStream = Console.OpenStandardError();
                StreamWriter streamWriter = new StreamWriter(errStream);
                streamWriter.AutoFlush = true;
                streamWriter.NewLine = Environment.NewLine;
                streamWriter.WriteLine(output);
                streamWriter.Close();
                errStream.Close();
            }
            else
            {
                Console.WriteLine(output);
            }
            if (colorEnabled && Constants.CurrentConfigurations.EnableColors)
            {
                Console.BackgroundColor = SOPT.dbakColor;
                Console.ForegroundColor = SOPT.dpreColor;
            }
        }

        public static string GetTimePrefix()
        {
            return "[" + DateTime.Now.ToString() + "]";
        }
    }
}