Module Program
    Sub Main(args As String())
        'Arguments specified
        If Not args.Count = 0 Then
            ResolveArguments(args)
            'Read resolved arguments
            If Constants.CurrentCommandLine.GenConf Then
                Configurations.Initiate()
                Environment.Exit(0)
            End If
            If Constants.CurrentCommandLine.DisplayHelp Then
                Console.WriteLine(Constants.CommandLineHelp)
                Environment.Exit(0)
            End If
        End If

        'Proceed other stuff.
        If Not Constants.CurrentCommandLine.VerboseMode Then Console.WriteLine(Constants.ASCIIART)
        Out("ItalianCannon version " & Constants.AppVer, , , False)
        Out("Initiating color profiles...", , , False)
        dpreColor = Console.ForegroundColor
        dbakColor = Console.BackgroundColor
        Configurations.Initiate()
        'Apply -c.
        If Constants.CurrentCommandLine.NoSingleThrLimit Then
            Constants.CurrentConfigurations.MaxRequestsPerThread = 0
        End If
        Out("Starting threads... Your current threads number was " & Constants.CurrentConfigurations.Threads & ".")
        Constants.SW.Start()
        For i = 1 To Constants.CurrentConfigurations.Threads
            Dim thread As New Threading.Thread(AddressOf ThreadRun)
            thread.Start()
        Next
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
        Out("Starting...", "THR" & ThrId)
        Dim i As Long = 1
        Do Until i = Constants.CurrentConfigurations.MaxRequestsPerThread
            Try
                Dim client As New Net.WebClient()
                client.Headers.Add(Net.HttpRequestHeader.UserAgent, Constants.CurrentConfigurations.UserAgent)
                client.DownloadData(Constants.CurrentConfigurations.TeaCupTarget)
                client.Dispose()
                Constants.Total += 1
                Out("REQ OK", Constants.SW.Elapsed.ToString & "/" & i & "thr./" & Constants.Total & "ts" & "/THR" & ThrId)
            Catch ex As Exception
                Out("REQ ERR: " & ex.Message, Constants.SW.Elapsed.ToString & "/" & i & "thr./" & Constants.Total & "ts" & "/THR" & ThrId, LogLevels.EXCEPTION)
            End Try
            Threading.Thread.Sleep(Constants.CurrentConfigurations.IntervalPerThread)
            i += 1
        Loop
        Out("Max requests limit exceeded. Stopped.", Constants.SW.Elapsed.ToString & "/" & Constants.CurrentConfigurations.MaxRequestsPerThread & "thr./" & Constants.Total & "ts" & "/THR" & ThrId)
    End Sub

    Sub ResolveArguments(args As String())
        Dim fullArgs As String = ""
        For Each s As String In args
            fullArgs &= s & " "
        Next
        If fullArgs.ToLower().Contains("-v") Then
            Constants.CurrentCommandLine.VerboseMode = True
        Else
            Constants.CurrentCommandLine.VerboseMode = False
        End If
        If fullArgs.ToLower().Contains("-c") Then
            Constants.CurrentCommandLine.NoSingleThrLimit = True
        Else
            Constants.CurrentCommandLine.NoSingleThrLimit = False
        End If
        If fullArgs.ToLower().Contains("--genconf") Then
            Constants.CurrentCommandLine.GenConf = True
        Else
            Constants.CurrentCommandLine.GenConf = False
        End If
        If fullArgs.ToLower().Contains("--help") Then
            Constants.CurrentCommandLine.DisplayHelp = True
        Else
            Constants.CurrentCommandLine.DisplayHelp = False
        End If
        If fullArgs.ToLower().Contains("-g") Then
            Constants.CurrentCommandLine.DisableColor = True
        Else
            Constants.CurrentCommandLine.DisableColor = False
        End If
    End Sub
End Module
