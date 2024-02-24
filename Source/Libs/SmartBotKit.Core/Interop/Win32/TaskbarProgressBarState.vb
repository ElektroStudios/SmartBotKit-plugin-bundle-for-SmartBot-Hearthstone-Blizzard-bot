
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Taskbar ProgressBar State "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies a thumbnail progress bar state.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd391697%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum TaskbarProgressBarState

        ''' <summary>
        ''' No progress is displayed.
        ''' </summary>
        NoProgress = 0

        ''' <summary>
        ''' The progress is indeterminate (marquee).
        ''' </summary>
        Indeterminate = &H1

        ''' <summary>
        ''' Normal progress is displayed.
        ''' </summary>
        Normal = &H2

        ''' <summary>
        ''' An error occurred (red).
        ''' </summary>
        [Error] = &H4

        ''' <summary>
        ''' The operation is paused (yellow).
        ''' </summary>
        Paused = &H8

    End Enum

End Namespace

#End Region
