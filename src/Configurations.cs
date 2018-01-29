// ItalianCannon.Configurations
using ItalianCannon;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ItalianCannon
{
    public class Configurations
    {
        public class ConfObj
        {
            public string Note;

            public List<AtkUrl> TeaCupTargets;

            public int IntervalPerThread;

            public long MaxRequestsPerThread;

            public string UserAgent;

            public bool AppearsToBeDefault;

            public bool DisableSSLValidation;

            public bool IgnoreHTTPError;

            public List<Header> ExtraHTTPHeaders;

            public bool EnableAnimations;

            public bool EnableColors;

            public bool VerboseMode;

            public bool RandomPOST;

            public byte RandomPOSTRate;

            public long RandomPOSTLength;

            public bool RandomHEAD;

            public byte RandomHEADRate;

            public ConfObj()
            {
                this.Note = "Please change 'AppearsToBeDefault' to False after changing settings. ItalianCannon will ignore this configuration entry. For headers help, see https://github.com/dotnet/corefx/blob/master/src/System.Net.WebHeaderCollection/src/System/Net/HttpRequestHeader.cs";
                this.IntervalPerThread = 500;
                this.MaxRequestsPerThread = 1000L;
                this.UserAgent = "Mozilla/5.0 (Linux) AppleWebKit/888.88 (KHTML, like Gecko) Chrome/66.6.2333.66 Safari/233.33";
                this.AppearsToBeDefault = true;
                this.DisableSSLValidation = false;
                this.IgnoreHTTPError = false;
                this.TeaCupTargets = new List<AtkUrl>
                {
                    new AtkUrl()
                };
                this.ExtraHTTPHeaders = new List<Header>
                {
                    new Header()
                };
                this.EnableAnimations = false;
                this.EnableColors = true;
                this.VerboseMode = false;
                this.RandomPOST = false;
                this.RandomPOSTRate = 1;
                this.RandomPOSTLength = 128L;
                this.RandomHEAD = false;
                this.RandomHEADRate = 1;
            }
        }

        public class Header
        {
            public HttpRequestHeader HType;

            public string Content;

            public Header()
            {
                this.HType = HttpRequestHeader.Allow;
                this.Content = "GET";
            }
        }

        public class AtkUrl
        {
            public string Url;

            public int Threads;

            public AtkUrl()
            {
                this.Url = "https://www.baidu.com";
                this.Threads = 1;
            }
        }

        public static void Initiate()
        {
            Constants.CurrentConfigurations = new ConfObj();
            SOPT.Out("Loading configurations...", "CONF", SOPT.LogLevels.INFO, true, false);
            if (!File.Exists(Constants.ConfFile))
            {
                SOPT.Out("Configurations file not found. Creating a new one at " + Constants.ConfFile + ".", "CONF", SOPT.LogLevels.WARN, true, true);
                Constants.CurrentConfigurations = new ConfObj();
                Configurations.SaveConf();
                Configurations.WaitEdit();
            }
            else
            {
                try
                {
                    Configurations.ReadConf();
                    if (Constants.CurrentConfigurations.AppearsToBeDefault)
                    {
                        SOPT.Out("Configurations appear to be default.", "CONF", SOPT.LogLevels.WARN, true, true);
                        Configurations.WaitEdit();
                    }
                    else
                    {
                        if (!Configurations.CheckConflictConf())
                        {
                            SOPT.Out("Conflict options detected.", "CONF", SOPT.LogLevels.EXCEPTION, true, true);
                            Environment.Exit(1);
                        }
                        SOPT.Out("Complete.", "CONF", SOPT.LogLevels.INFO, true, false);
                    }
                }
                catch (Exception ex2)
                {
                    ProjectData.SetProjectError(ex2);
                    Exception ex = ex2;
                    SOPT.Out("Cannot read configuration: " + ex.ToString(), "CONF", SOPT.LogLevels.EXCEPTION, true, true);
                    Environment.Exit(1);
                    ProjectData.ClearProjectError();
                }
            }
        }

        public static void SaveConf()
        {
            Constants.CurrentConfigurations = Constants.CurrentConfigurations;
            string text = JsonConvert.SerializeObject(Constants.CurrentConfigurations, Formatting.Indented);
            StreamWriter streamWriter = new StreamWriter(File.Create(Constants.ConfFile), Encoding.UTF8);
            streamWriter.Write(text);
            streamWriter.Flush();
            streamWriter.Close();
        }

        public static void ReadConf()
        {
            Constants.CurrentConfigurations = JsonConvert.DeserializeObject<ConfObj>(File.ReadAllText(Constants.ConfFile));
            ConfObj currentConfigurations = Constants.CurrentConfigurations;
            currentConfigurations.TeaCupTargets.RemoveAt(0);
            currentConfigurations.ExtraHTTPHeaders.RemoveAt(0);
        }

        public static void WaitEdit()
        {
            SOPT.Out("Please change the configurations and try again.", "CONF", SOPT.LogLevels.WARN, true, true);
            Environment.Exit(0);
        }

        public static bool CheckConflictConf()
        {
            if (Constants.CurrentConfigurations.VerboseMode & Constants.CurrentConfigurations.EnableAnimations)
            {
                return false;
            }
            return true;
        }
    }
}