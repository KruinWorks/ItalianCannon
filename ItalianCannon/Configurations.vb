Public Class Configurations
    Public Class ConfObj
        Public Note As String
        Public TeaCupTargets As List(Of AtkUrl)
        Public IntervalPerThread As Integer
        Public MaxRequestsPerThread As Long
        Public UserAgent As String
        Public AppearsToBeDefault As Boolean
        Public DisableSSLValidation As Boolean
        Public IgnoreHTTPError As Boolean
        Public ExtraHTTPHeaders As List(Of Header)
        Public EnableAnimations As Boolean
        Public EnableColors As Boolean
        Public SilentMode As Boolean
        Public RandomPOST As Boolean
        Public RandomPOSTRate As Byte
        Public RandomPOSTLength As Long
        Public RandomHEAD As Boolean
        Public RandomHEADRate As Byte
        Public RandomQuery As Boolean
        Sub New()
            Note = "Please change 'AppearsToBeDefault' to False after changing settings. ItalianCannon will ignore this configuration entry. For headers help, see https://see.wtf/O5vFp"
            IntervalPerThread = 500
            MaxRequestsPerThread = 1000
            UserAgent = "Mozilla/5.0 (Linux) AppleWebKit/888.88 (KHTML, like Gecko) Chrome/66.6.2333.66 Safari/233.33"
            AppearsToBeDefault = True
            DisableSSLValidation = False
            IgnoreHTTPError = False
            Dim SampleTCTCol As New List(Of AtkUrl)
            SampleTCTCol.Add(New AtkUrl)
            TeaCupTargets = SampleTCTCol
            Dim SampleHeaderCol As New List(Of Header)
            SampleHeaderCol.Add(New Header)
            ExtraHTTPHeaders = SampleHeaderCol
            EnableAnimations = False
            EnableColors = True
            SilentMode = False
            RandomPOST = False
            RandomPOSTRate = 1
            RandomPOSTLength = 128
            RandomHEAD = False
            RandomHEADRate = 1
            RandomQuery = False
        End Sub
    End Class

    Public Class Header
        Public HType As System.Net.HttpRequestHeader
        Public Content As String
        Sub New()
            HType = System.Net.HttpRequestHeader.Allow
            Content = "GET"
        End Sub
    End Class

    Public Class AtkUrl
        Public Url As String
        Public Threads As Integer
        Sub New()
            Url = "https://www.contoso.com"
            Threads = 1
        End Sub
    End Class

    Public Shared Sub Initiate()
        'Use default conf for failsafe.
        Constants.CurrentConfigurations = New ConfObj()
        'Find file, if does not exist, create one.
        Out("Loading configurations...", "CONF")
        If Not IO.File.Exists(Constants.ConfFile) Then
            Out("Configurations file not found. Creating a new one at " & Constants.ConfFile & ".", "CONF", LogLevels.WARN,, True)
            Constants.CurrentConfigurations = New ConfObj
            SaveConf()
            WaitEdit() 'Display message and exit.
        Else
            Try
                ReadConf()
                If Constants.CurrentConfigurations.AppearsToBeDefault Then
                    Out("Configurations appear to be default.", "CONF", LogLevels.WARN,, True)
                    WaitEdit()
                    Exit Sub 'Since the waitedit sub itself will output a "Complete", so I just let it "go".
                End If
                If Not CheckConflictConf() Then
                    Out("Conflict options detected.", "CONF", LogLevels.EXCEPTION,, True)
                    Environment.Exit(1)
                End If
                Out("Complete.", "CONF")
            Catch ex As Exception
                Out("Cannot read configuration: " & ex.ToString, "CONF", LogLevels.EXCEPTION,, True)
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
        With Constants.CurrentConfigurations
            .TeaCupTargets.RemoveAt(0)
            .ExtraHTTPHeaders.RemoveAt(0)
        End With
    End Sub

    Public Shared Sub WaitEdit()
        Out("Please change the configurations and try again.", "CONF", LogLevels.WARN,, True)
        Environment.Exit(0)
    End Sub

    Public Shared Function CheckConflictConf() As Boolean
        If (Constants.CurrentConfigurations.SilentMode And Constants.CurrentConfigurations.EnableAnimations) Then Return False
        Return True
    End Function
End Class
