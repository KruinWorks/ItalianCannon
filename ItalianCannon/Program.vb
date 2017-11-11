Module Program
    Sub Main(args As String())
        'No arguments specified
        Console.WriteLine(Constants.ASCIIART)
        Out("ItalianCannon version " & Constants.AppVer)
        Configurations.Initiate()
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
        GoTo Read
    End Sub

    Sub ThreadRun()
        Constants.ThrId += 1
        Dim ThrId As Integer = Constants.ThrId
        Out("Starting...", "THR" & ThrId)
        For i = 1 To Constants.CurrentConfigurations.MaxRequestsPerThread Step 1
            Try
                Dim client As New Net.WebClient()
                client.Headers.Add(Net.HttpRequestHeader.UserAgent, Constants.CurrentConfigurations.UserAgent)
                client.DownloadData(Constants.CurrentConfigurations.TeaCupTarget)
                client.Dispose()
                Constants.Total += 1
                Out("REQ OK", Constants.SW.Elapsed.ToString & "/" & i & "thr./" & Constants.Total & "ts" & "/THR" & ThrId)
            Catch ex As Exception
                Out(ex.ToString)
                Out("REQ ERR", Constants.SW.Elapsed.ToString & "/" & i & "thr./" & Constants.Total & "ts" & "/THR" & ThrId)
            End Try
            Threading.Thread.Sleep(Constants.CurrentConfigurations.IntervalPerThread)
        Next
        Out("Max requests limit exceeded. Stopped.", Constants.SW.Elapsed.ToString & "/" & Constants.CurrentConfigurations.MaxRequestsPerThread & "thr./" & Constants.Total & "ts" & "/THR" & ThrId)
    End Sub
End Module