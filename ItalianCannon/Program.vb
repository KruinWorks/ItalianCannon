Imports System.Security.Cryptography.X509Certificates

Module Program
    Sub Main(args As String())
        'Prevent output errors.
        Constants.CurrentConfigurations = New Configurations.ConfObj()
        With Constants.CurrentConfigurations
            .TeaCupTargets.RemoveAt(0)
            .ExtraHTTPHeaders.RemoveAt(0)
            .EnableAnimations = False
            .SilentMode = False
        End With

        'Setup colors for all outputs.
        Out("Initiating color profiles...", , , False)
        dpreColor = Console.ForegroundColor
        dbakColor = Console.BackgroundColor

        'Setup wizard.
        For Each Arg As String In args
            If Arg.ToLower = "--setup" Then
                Out("Starting Setup Wizard...")
                Try
                    RunSetup()
                Catch ex As Exception
                    Out("An error occurred in Setup Wizard: " & "",, LogLevels.EXCEPTION)
                    Environment.Exit(1)
                End Try
                Out("Setup Wizard complete! Restart ItalianCannon to confirm the configurations.")
                Environment.Exit(0)
            End If
        Next

        'Default startup.
        Configurations.Initiate()
        Console.WriteLine(Constants.ASCIIART)
        Out("ItalianCannon version " & Constants.AppVer, , , False)
        'dotnet run performance notification.
        Out("Notice: Using 'dotnet run' may significantly slow ItalianCannon down, for further information, see https://see.wtf/OtImc")
        'Apply animations.
        If Constants.CurrentConfigurations.EnableAnimations Then
            Dim thrAnimations As New Threading.Thread(AddressOf ThreadAnimations)
            thrAnimations.Start()
            Dim thrSpeed As New Threading.Thread(AddressOf ThreadSpeedCalc)
            thrSpeed.Start()
        End If
        'Generate random post data.
        If Constants.CurrentConfigurations.RandomPOST Then
            Out("Generating random POST data (" & Constants.CurrentConfigurations.RandomPOSTLength & "-bytes) ...")
            Dim CharPicker As New Random()
            For i = 0 To Constants.CurrentConfigurations.RandomPOSTLength
                Constants.RandomPOSTData &= Convert.ToChar(CharPicker.Next(32, 126))
            Next
        End If
        'Ignore invalid SSL Certificates.
        If Constants.CurrentConfigurations.DisableSSLValidation Then
            System.Net.ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf FakeCertCallBack)
            Out("SSL Certificate Validation has been disabled.")
        End If
        Dim TotalThreads As Integer = 0
        For Each AU As Configurations.AtkUrl In Constants.CurrentConfigurations.TeaCupTargets
            TotalThreads += AU.Threads
        Next
        Out("Starting threads... Your current threads number (total) was " & TotalThreads & ".", "AU")
        Constants.SW.Start()
        'Start for each AtkUrls.
        If Constants.CurrentConfigurations.TeaCupTargets.Count = 0 Then
            Out("NO URL SPECIFIED!", "AU", LogLevels.EXCEPTION, , True)
            Environment.Exit(1)
        End If
        If Constants.CurrentConfigurations.TeaCupTargets.Count = 1 Then
            'Single URL Mode.
            Dim AU As Configurations.AtkUrl = Constants.CurrentConfigurations.TeaCupTargets(0)
            Constants.TeaCupTarget = AU.Url 'Update url to pass to threads.
            For i = 1 To AU.Threads
                Dim thread As New Threading.Thread(AddressOf ThreadRun)
                thread.Start()
            Next
            Threading.Thread.Sleep(500)
        Else
            Out("Multi-URL mode enabled.", "AU")
            For Each AU As Configurations.AtkUrl In Constants.CurrentConfigurations.TeaCupTargets
                Threading.Thread.Sleep(500) 'Wait cooldown.
                Out("===== TID range BEGINS at " & Constants.ThrId & " for URL: " & AU.Url)
                Constants.TeaCupTarget = AU.Url 'Update url to pass to threads.
                For i = 1 To AU.Threads
                    Dim thread As New Threading.Thread(AddressOf ThreadRun)
                    thread.Start()
                Next
                Threading.Thread.Sleep(500) 'Wait cooldown.
                Out("===== TID range ENDS at " & Constants.ThrId & " for URL: " & AU.Url)
            Next
        End If
        Out("Dispatching READY signal...")
        Constants.ThreadsReady = True
        Dim input As String = ""
        Out("Started. Type 'exit' to stop.")
Read:
        input = Console.ReadLine().ToLower()
        If input = "exit" Then Environment.Exit(0)
        Out("You can type 'exit' or press ^C to stop.", , LogLevels.WARN)
        GoTo Read
    End Sub

    Sub ThreadRun()
        Constants.ThrId += 1
        Dim ThrId As Integer = Constants.ThrId
        Dim TeaCupTarget As String = Constants.TeaCupTarget
        Out("Starting... / URL: " & TeaCupTarget, "THR" & ThrId)
        While(Not Constants.ThreadsReady)
            Threading.Thread.Sleep(100)
        End While
        Dim i As Long = 1
        Do Until i = Constants.CurrentConfigurations.MaxRequestsPerThread
            Try
                Dim HITRequest As String = "GET"
                'Random HEAD.
                If (Constants.CurrentConfigurations.RandomHEAD And RandomHIT(Constants.CurrentConfigurations.RandomHEADRate)) Then
                    'Send HEAD Request and jump out.
                    Dim TReq As System.Net.WebRequest = System.Net.WebRequest.Create(New Uri(TeaCupTarget))
                    TReq.Method = "HEAD"
                    TReq.GetResponse()
                    Constants.Total += 1
                    Out("HEAD OK", Constants.SW.Elapsed.ToString & "/" & i & "thr./" & Constants.Total & "ts" & "/THR" & ThrId)
                    Exit Try
                End If
                Using client As New Net.WebClient()
                    client.Headers.Add(Net.HttpRequestHeader.UserAgent, Constants.CurrentConfigurations.UserAgent)
                    For Each tHeader As Configurations.Header In Constants.CurrentConfigurations.ExtraHTTPHeaders
                        client.Headers.Add(tHeader.HType, tHeader.Content)
                    Next
                    'Random POST
                    Dim Size As Long
                    If (Constants.CurrentConfigurations.RandomPOST And RandomHIT(Constants.CurrentConfigurations.RandomPOSTRate)) Then
                        HITRequest = "POST"
                        Size = Constants.CurrentConfigurations.RandomPOSTLength
                        client.UploadData(TeaCupTarget, System.Text.Encoding.UTF8.GetBytes(Constants.RandomPOSTData))
                    Else
                        Size = client.DownloadData(TeaCupTarget).Length
                    End If
                    Constants.TotalDownloaded += Size
                End Using
                Constants.Total += 1
                Out(HITRequest & " OK", Constants.SW.Elapsed.ToString & "/" & i & "thr./" & Constants.Total & "ts" & "/THR" & ThrId)
            Catch sEx As System.Net.WebException 'Proceed server-side errors (4xx / 5xx)
                If Constants.CurrentConfigurations.IgnoreHTTPError = False Then
                    Constants.TotalFail += 1
                    Out("GET ERR: " & sEx.Message, Constants.SW.Elapsed.ToString & "/" & i & "thr./" & Constants.Total & "ts" & "/THR" & ThrId, LogLevels.EXCEPTION)
                    Exit Try
                End If
                Dim RCode As Short
                If sEx.Status = System.Net.WebExceptionStatus.ProtocolError Then
                    RCode = CType(sEx.Response, System.Net.HttpWebResponse).StatusCode
                End If
                Constants.Total += 1
                Out("GET NG: HTTP/" & RCode, Constants.SW.Elapsed.ToString & "/" & i & "thr./" & Constants.Total & "ts" & "/THR" & ThrId)
            Catch ex As Exception
                Constants.TotalFail += 1
                Out("REQ ERR: " & ex.Message, Constants.SW.Elapsed.ToString & "/" & i & "thr./" & Constants.Total & "ts" & "/THR" & ThrId, LogLevels.EXCEPTION)
            End Try
            Threading.Thread.Sleep(Constants.CurrentConfigurations.IntervalPerThread)
            i += 1
        Loop
        Out("Max requests limit exceeded. Stopped.", Constants.SW.Elapsed.ToString & "/" & Constants.CurrentConfigurations.MaxRequestsPerThread & "thr./" & Constants.Total & "ts" & "/THR" & ThrId)
        If Constants.CurrentConfigurations.EnableAnimations Then Constants.ThrId -= 1
    End Sub

    Sub ThreadSpeedCalc()
        Constants.DLSpeed = "[Pending...]"
        Dim PrevDL As ULong = Constants.TotalDownloaded
        Threading.Thread.Sleep(1000)
        Do Until 233 = 2333
            Constants.DLDelta = Constants.TotalDownloaded - PrevDL
            'Judge size unit.
            Dim Speed As String = "[" & JudgeSizeUnit(Constants.DLDelta * 2) & "/s / " & JudgeSizeUnit(Constants.TotalDownloaded) & "]"
            Constants.DLSpeed = Speed
            PrevDL = Constants.TotalDownloaded
            Threading.Thread.Sleep(500)
        Loop
    End Sub

    Sub ThreadAnimations()
        '[###       ][00:00:00.000000][500THR][100TS/1FL][MAX0]
        Dim count As UInt64 = 0
        Dim prevLength As Short = 0
        Dim TotalThreads As Integer = 0
        For Each AU As Configurations.AtkUrl In Constants.CurrentConfigurations.TeaCupTargets
            TotalThreads += AU.Threads
        Next
        Console.Write("Starting...")
        Threading.Thread.Sleep(1000)
        For i = 1 To 11 Step 1
            Console.Write(vbBack)
        Next
        Do Until 233 = 2333
            If Not prevLength = 0 Then 'If not first time, check and delete.
                For i = 0 To prevLength
                    Console.Write(vbBack)
                Next
            End If
            'Console overflow.
            Dim overflow As Boolean = False
            'Backup color
            Dim prevFore As ConsoleColor = Console.ForegroundColor
            Dim max As String = Constants.CurrentConfigurations.MaxRequestsPerThread
            If max = 0 Then max = vbBack & vbBack & vbBack & "UNLIMITED"
            Dim sb0 As String = ""
            Dim sb1 As String = GetAnimationChar(count Mod 13)
            Dim sb2 As String = "[" & Constants.SW.Elapsed.ToString & "]"
            Dim sb3 As String = "[" & Constants.ThrId & "/" & TotalThreads & "THR]"
            Dim sb4 As String = "[" & Constants.Total & "TS/" & Constants.TotalFail & "FL]"
            Dim sb5 As String = "[MAX" & max & "]"
            Dim sb6 As String = Constants.DLSpeed
            Dim sb7 As String = ""
            Dim sb8 As String = "          " 'Clearing.
            If Constants.ThrId = 0 Then
                sb7 = "[NO THREADS ALIVE]"
                sb0 = "[!]"
            Else
                sb0 = GetAnimationBlock(count Mod 8)
            End If
            'Preprocessing before console overflow.
            If ("[OVERFLOW!]" & sb0 & sb1 & sb2 & sb3 & sb4 & sb5 & sb6 & sb7 & sb8).Length >= Console.WindowWidth
                overflow = True
            Else
                overflow = False
            End If
            prevLength = (sb0 & sb1 & sb2 & sb3 & sb4 & sb5 & sb6 & sb7 & sb8).Length 'Set length for next deletion.
            If overflow Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("[OVERFLOW!]")
            End If
            Console.ForegroundColor = ConsoleColor.Cyan
            Console.Write(sb0)
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.Write(sb1)
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write(sb2)
            Console.ForegroundColor = ConsoleColor.Cyan
            Console.Write(sb3)
            Console.ForegroundColor = ConsoleColor.White
            Console.Write(sb4)
            Console.ForegroundColor = ConsoleColor.Red
            Console.Write(sb5)
            Console.ForegroundColor = ConsoleColor.White
            Console.Write(sb6)
            Console.ForegroundColor = ConsoleColor.Magenta
            Console.Write(sb7)
            Console.ForegroundColor = prevFore 'Trash code #2nd.
            Console.Write(sb8)
            count += 1
            Threading.Thread.Sleep(250)
        Loop
    End Sub

    ''' <summary>
    ''' Get animation block char, from 0 to 12.
    ''' Case Else, equals to 12.
    ''' </summary>
    ''' <returns></returns>
    Function GetAnimationBlock(percentage As Integer) As String
        Select Case percentage 'Trash code #1st.
            Case 0
                Return "[/]"
            Case 1
                Return "[-]"
            Case 2
                Return "[\]"
            Case 3
                Return "[|]"
            Case 4
                Return "[/]"
            Case 5
                Return "[-]"
            Case 6
                Return "[\]"
            Case 7
                Return "[|]"
            Case Else
                Return "[/]"
        End Select
    End Function

    ''' <summary>
    ''' Get animation block char, from 0 to 12.
    ''' Case Else, equals to 12.
    ''' </summary>
    ''' <returns></returns>
    Function GetAnimationChar(percentage As Integer) As String
        Select Case percentage 'Trash code #1st.
            Case 0
                Return "[*         ]"
            Case 1
                Return "[**        ]"
            Case 2
                Return "[***       ]"
            Case 3
                Return "[ ***      ]"
            Case 4
                Return "[  ***     ]"
            Case 5
                Return "[   ***    ]"
            Case 6
                Return "[    ***   ]"
            Case 7
                Return "[     ***  ]"
            Case 8
                Return "[      *** ]"
            Case 9
                Return "[       ***]"
            Case 10
                Return "[        **]"
            Case 11
                Return "[         *]"
            Case Else
                Return "[          ]"
        End Select
    End Function

    Function JudgeSizeUnit(Size As Double) As String
        Dim SizeUnit As String = "B"
        If Size >= 1000 Then
            Size /= 1000
            SizeUnit = "KiB"
            If Size >= 1024 Then
                SizeUnit = "MiB"
                Size /= 1024
                If Size >= 1024 Then
                    SizeUnit = "GiB"
                    Size /= 1024
                    If Size >= 1024 Then
                        SizeUnit = "TiB"
                        Size /= 1024
                        If Size >= 1024 Then
                            SizeUnit = "PiB"
                            Size /= 1024
                            If Size >= 1024 Then
                                SizeUnit = "EiB"
                                Size /= 1024
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Return Math.Round(Size, 2) & SizeUnit
    End Function

    'Callback of ignorance of invalid ssl certificates.
    Function FakeCertCallBack(ByVal Sender As Object, ByVal Cert As X509Certificate, ByVal Chain As X509Chain, ByVal [error] As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function

    Function RandomHIT(Probability As Byte) As Boolean
        Dim Result As Short = (New Random).Next(0, 101)
        If (Result > 0 And Result <= Probability) Then
            Return True
        Else
            Return False
        End If
    End Function

    Sub RunSetup()
        Out("Welcome to ItalianCannon!", "SETUP")
        Dim Conf As New Configurations.ConfObj()
        With Conf
            .TeaCupTargets.RemoveAt(0)
            .ExtraHTTPHeaders.RemoveAt(0)
        End With
        'Adding sites.
AddSite:
        Out(":: Which site would you like to test?", "SETUP")
        Try
            Dim Site As New Uri(Console.ReadLine())
            Out(":: How many threads would you like to start with this site? [5]", "SETUP")
            Dim Threads As UInt32
            Try
                Threads = Console.ReadLine()
                If Threads = 0 Then Throw New ArgumentException("0 is now allowed.")
                Out("Setting threads to " & Threads & ".", "SETUP")
            Catch ex As Exception
                Out("Setting threads to 5.", "SETUP", LogLevels.WARN)
                Threads = 5
            End Try
            Conf.TeaCupTargets.Add(New Configurations.AtkUrl() With {.Url = Site.ToString(), .Threads = Threads})
            Out("Added! Would you like to add another URL? [true/FALSE]", "SETUP")
            Dim ContinueAdding As Boolean = False
            Try
                ContinueAdding = Console.ReadLine()
                If ContinueAdding Then GoTo AddSite
            Catch ex As Exception
            End Try
        Catch ex As Exception
            Out("Error parsing URL, are you using the correct format? (Must begin with 'http://' or 'https://')", "SETUP", LogLevels.EXCEPTION)
            GoTo AddSite
        End Try

        Out(":: How many requests should each thread make? [1000]", "SETUP")
        Dim MaximumReq As Integer = 1000
        Try
            MaximumReq = Console.ReadLine()
            Out("Setting requests limit to " & MaximumReq & ".", "SETUP")
        Catch ex As Exception
            Out("Setting requests limit to 1000", "SETUP", LogLevels.WARN)
        End Try
        Conf.MaxRequestsPerThread = MaximumReq

        Out(":: Do you want to disable SSL certificate validation? [true/FALSE]", "SETUP")
        Dim DisableSSLValidation As Boolean = False
        Try
            DisableSSLValidation = Console.ReadLine()
        Catch ex As Exception
        End Try
        Conf.DisableSSLValidation = DisableSSLValidation

        Out(":: Ignore server errors? (Like 4xx and 5xx, and other non-1xx/2xx/3xx status codes.) [true/FALSE]", "SETUP")
        Dim IgnoreServerError As Boolean = False
        Try
            IgnoreServerError = Console.ReadLine()
        Catch ex As Exception
        End Try
        Conf.IgnoreHTTPError = IgnoreServerError
        Conf.AppearsToBeDefault = False

        Out("Saving configurations...", "SETUP")
        Constants.CurrentConfigurations = Conf
        Configurations.SaveConf()

        Out("=================================================================", "SETUP")
        Out("Basic configurations are finished! You can run ItalianCannon now.", "SETUP")
        Out("For further configurations, see https://see.wtf/6nhoB", "SETUP")
        Out("To improve performance, see https://see.wtf/OtImc", "SETUP")
        Out("Thanks for using!", "SETUP")
        Out("=================================================================", "SETUP")

    End Sub
End Module
