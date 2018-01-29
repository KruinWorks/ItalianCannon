// ItalianCannon.Constants
using ItalianCannon;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ItalianCannon
{
    public class Constants
    {
        public const string ASCIIART = "                                            \r\n                      \r\n  /  /   /()  \r\n   /   /   `/  /  /   `/   \\\r\n/ /  / / / // /  /   / / // /  / / /\r\n//  \\/ \\,/ //  //  \\,/ // // \r\n                                            \r\n                                   \r\n  /   \r\n  /       `/   \\   \\   \\   \\ \r\n/ /  / // /  / / /  / / / // /  / / / \r\n\\/  \\,/ // //// //\\/// //  \r\n(ASCII Art Generated by patorjk.com)\r\n";

        public const string AppVer = "1.0.6-rs-b27";

        
        public static string ConfFile = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "ItalianCannon.json");

        public static Configurations.ConfObj CurrentConfigurations
        {
            get;
            set;
        }

        public static int ThrId
        {
            get;
            set;
        } = 0;


        public static ulong Total
        {
            get;
            set;
        } = 0uL;


        public static ulong TotalFail
        {
            get;
            set;
        } = 0uL;


        public static Stopwatch SW
        {
            get;
            set;
        } = new Stopwatch();


        public static ulong TotalDownloaded
        {
            get;
            set;
        } = 0uL;


        public static float DLDelta
        {
            get;
            set;
        } = 0f;


        public static string DLSpeed
        {
            get;
            set;
        } = "[?]";


        public static string TeaCupTarget
        {
            get;
            set;
        }

        public static bool ThreadsReady
        {
            get;
            set;
        } = false;


        public static string RandomPOSTData
        {
            get;
            set;
        }
    }
}