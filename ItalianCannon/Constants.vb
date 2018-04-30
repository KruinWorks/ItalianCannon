﻿''' <summary>
''' Not all constants :)
''' </summary>
Public Class Constants
    ''' <summary>
    ''' Startup ASCII art.
    ''' </summary>
    Public Const ASCIIART As String = "                                            
____________       __________               
____  _/_  /______ ___  /__(_)_____ _______ 
 __  / _  __/  __ `/_  /__  /_  __ `/_  __ \
__/ /  / /_ / /_/ /_  / _  / / /_/ /_  / / /
/___/  \__/ \__,_/ /_/  /_/  \__,_/ /_/ /_/ 
                                            
_________                                   
__  ____/_____ ___________________________  
_  /    _  __ `/_  __ \_  __ \  __ \_  __ \ 
/ /___  / /_/ /_  / / /  / / / /_/ /  / / / 
\____/  \__,_/ /_/ /_//_/ /_/\____//_/ /_/  
(ASCII Art Generated by patorjk.com)
"
    ''' <summary>
    ''' Application version
    ''' </summary>
    Public Const AppVer As String = "1.0.8-bt-b32"
    ''' <summary>
    ''' Appliation configuration file's name.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property ConfFile As String = IO.Path.Combine((New IO.FileInfo(Reflection.Assembly.GetExecutingAssembly.Location)).DirectoryName, "ItalianCannon.json")
    ''' <summary>
    ''' Current configuration.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property CurrentConfigurations As Configurations.ConfObj
    ''' <summary>
    ''' Current Thread ID.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property ThrId As Integer = 0
    ''' <summary>
    ''' Requests with 1xx/2xx/3xx responses.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property Total As UInt64 = 0
    ''' <summary>
    ''' Requests with 4xx/5xx responses. Or just timed out.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property TotalFail As UInt64 = 0
    ''' <summary>
    ''' Attack stopwatch.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property SW As New Stopwatch
    ''' <summary>
    ''' Total downloaded. Measured as "bytes".
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property TotalDownloaded As ULong = 0
    ''' <summary>
    ''' Download delta.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property DLDelta As Single = 0
    ''' <summary>
    ''' Download speed output string.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property DLSpeed As String = "[?]"
    ''' <summary>
    ''' Oops
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property TeaCupTarget As String
    ''' <summary>
    ''' Tells the thread to run or not
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property ThreadsReady As Boolean = False
    ''' <summary>
    ''' Content to be served when posting.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property RandomPOSTData As String
End Class
