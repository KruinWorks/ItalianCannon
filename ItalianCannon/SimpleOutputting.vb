''' <summary>
''' Simple output framework(no)(
''' </summary>
Public Module SOPT
    Public dpreColor As ConsoleColor
    Public dbakColor As ConsoleColor
    ''' <summary>
    ''' Output something with prefix added.
    ''' </summary>
    Public Sub Out(text As String, Optional sourceModule As String = "MAIN", Optional level As LogLevels = LogLevels.INFO, Optional colorEnabled As Boolean = True)
        'Combination structure:
        '[TIME][LEVEL][MODULE]: text
        '
        If Constants.CurrentCommandLine.VerboseMode Then
            Exit Sub
        End If


        Dim timeStr As String = GetTimePrefix() '[TIME] prefix, [] included.
        Dim lvlStr As String '[LEVEL] prefix, [] included.

        Dim output As String

        Dim preColor As ConsoleColor
        Dim bakColor As ConsoleColor
        Select Case level
            Case LogLevels.INFO
                lvlStr = "[INFO]"
                preColor = dpreColor
                bakColor = dbakColor
            Case LogLevels.WARN
                lvlStr = "[WARN]"
                preColor = ConsoleColor.Black
                bakColor = ConsoleColor.Yellow
            Case LogLevels.EXCEPTION
                lvlStr = "[EROR]"
                preColor = ConsoleColor.White
                bakColor = ConsoleColor.DarkRed
            Case Else
                lvlStr = "[UKNN]"
                preColor = ConsoleColor.White
                bakColor = ConsoleColor.White
        End Select

        'Combine strings
        output = timeStr & lvlStr & "[" & sourceModule & "]: " & text

        'Apply color and output
        If colorEnabled Then
            If Not Constants.CurrentCommandLine.DisableColor Then
                Console.BackgroundColor = bakColor
                Console.ForegroundColor = preColor
            End If
        End If
        'Output
        If level = LogLevels.EXCEPTION Then
            Dim errStream As IO.Stream = Console.OpenStandardError()
            Dim errWriter As New IO.StreamWriter(errStream) With {.AutoFlush = True, .NewLine = Environment.NewLine}
            errWriter.WriteLine(output)
            errWriter.Close()
            errStream.Close()
        Else
            Console.WriteLine(output)
        End If

        'Reverse color changes.
        If colorEnabled Then
            If Not Constants.CurrentCommandLine.DisableColor Then
                Console.BackgroundColor = dbakColor
                Console.ForegroundColor = dpreColor
            End If
        End If
    End Sub
    ''' <summary>
    ''' Indicates the seriousness of logging entries.
    ''' </summary>
    Public Enum LogLevels
        ''' <summary>
        ''' Works fine. Nothing wrong.
        ''' </summary>
        INFO = 0
        ''' <summary>
        ''' Something is wrong, but program can still run.
        ''' </summary>
        WARN = 1
        ''' <summary>
        ''' Something is wrong and program can not continue.
        ''' </summary>
        EXCEPTION = 2
        ''' <summary>
        ''' We don't know the exact status of this entry.
        ''' </summary>
        UKNN = 3
    End Enum
    ''' <summary>
    ''' Get current time string.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetTimePrefix() As String
        Return "[" & Date.Now.ToString & "]"
    End Function
End Module
