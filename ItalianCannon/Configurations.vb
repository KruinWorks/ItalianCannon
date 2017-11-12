Public Class Configurations
    Public Class ConfObj
        Public Note As String
        Public TeaCupTarget As String
        Public Threads As Integer
        Public IntervalPerThread As Integer
        Public MaxRequestsPerThread As Long
        Public UserAgent As String
        Public AppearsToBeDefault As Boolean
        Sub New()
            Note = "Please change 'AppearsToBeDefault' to False after changing settings. ItalianCannon will ignore this configuration entry."
            TeaCupTarget = "https://www.baidu.com"
            Threads = 1
            IntervalPerThread = 500
            MaxRequestsPerThread = 1000
            UserAgent = "Mozilla/5.0 (Linux) AppleWebKit/888.88 (KHTML, like Gecko) Chrome/66.6.2333.66 Safari/233.33"
            AppearsToBeDefault = True
        End Sub
    End Class
    Public Shared Sub Initiate()
        'Find file, if does not exist, create one.
        Out("Loading configurations...", "CONF")
        If Not IO.File.Exists(Constants.ConfFile) Then
            Out("Configurations file not found. Creating a new one.", "CONF", LogLevels.WARN)
            Constants.CurrentConfigurations = New ConfObj
            SaveConf()
            WaitEdit() 'Wait for edit then reload and exit.
        Else
            Try
                ReadConf()
                If Constants.CurrentConfigurations.AppearsToBeDefault Then
                    Out("Configurations appear to be default.", "CONF", LogLevels.WARN)
                    WaitEdit() 'Wait for edit then reload and exit.
                    Exit Sub 'Since the waitedit sub itself will output a "Complete", so I just let it "go".
                End If
                Out("Complete.", "CONF")
            Catch ex As Exception
                Out("Cannot read configuration: " & ex.ToString, "CONF", LogLevels.EXCEPTION)
                Environment.Exit(1)
            End Try
        End If
    End Sub

    Public Shared Sub SaveConf()
        Constants.CurrentConfigurations = Constants.CurrentConfigurations
        Dim text As String = Newtonsoft.Json.JsonConvert.SerializeObject(Constants.CurrentConfigurations, Newtonsoft.Json.Formatting.Indented)
        Dim writer As New IO.StreamWriter(IO.File.Create(Constants.ConfFile), System.Text.Encoding.UTF8)
        writer.Write(text)
        writer.Flush()
        writer.Close()
    End Sub

    Public Shared Sub ReadConf()
        Dim settingsText As String = IO.File.ReadAllText(Constants.ConfFile)
        Constants.CurrentConfigurations = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ConfObj)(settingsText)
    End Sub

    Public Shared Sub WaitEdit()
        If Constants.CurrentCommandLine.GenConf Then Environment.Exit(0)
                If Constants.CurrentCommandLine.VerboseMode Then
                    Console.WriteLine("Please change the configurations and try again.")
                    Environment.Exit(0)
                End If
        Out("You can now edit the configurations. Press any key to reload.", "CONF")
        Console.ReadKey(True)
        Out("Looping to reload...", "CONF")
        Initiate()
    End Sub
End Class
