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
    ''' Help text for --help argument.
    ''' </summary>
    Public Const CommandLineHelp As String = "ItalianCannon Commandline Help
-v           Verbose mode, outputs nothing.
-c           Ignore single-thread requests limit.
             *Equals to 'MaxRequestsPerThread = 0' in the conf. file.
-g           Disable color-ized console output.
--help       Display this help message and exit.
             *-v will be ignored.
--genconf    Generate configurations file and exit.
             *If a conf. file doesn't exist or is invalid.
             *If --help specified at the same time, it will be ignored.

[i] All the arguments are not case-sensitive."
    ''' <summary>
    ''' Application version
    ''' </summary>
    Public Const AppVer As String = "1.0.1-bt-b15"
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
    ''' Current command line.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property CurrentCommandLine As CommandLineOptions = New CommandLineOptions
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
    ''' Attack stopwatch.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property SW As New Stopwatch

    Public Class CommandLineOptions
        ''' <summary>
        ''' Silent mode. Outputs nothing.
        ''' Usage: -v, compatible with everything.
        ''' </summary>
        ''' <returns></returns>
        Public Property VerboseMode As Boolean
        ''' <summary>
        ''' No single-thread limit, equals to `"MaxRequestsPerThread": 0`
        ''' Usage: -c
        ''' </summary>
        ''' <returns></returns>
        Public Property NoSingleThrLimit As Boolean
        ''' <summary>
        ''' Generate default configurations file and exit.
        ''' Usage: --genconf, not compatible with any other args.
        ''' </summary>
        ''' <returns></returns>
        Public Property GenConf As Boolean
        ''' <summary>
        ''' Display command line help and exit.
        ''' Usage: --help, not compatible with any other args.
        ''' </summary>
        ''' <returns></returns>
        Public Property DisplayHelp As Boolean
        ''' <summary>
        ''' Disable output color.
        ''' Usage: -g
        ''' </summary>
        ''' <returns></returns>
        Public Property DisableColor As Boolean
        Sub New()
            VerboseMode = False
            NoSingleThrLimit = False
            GenConf = False
            DisplayHelp = False
            DisableColor = False
        End Sub
    End Class
End Class
