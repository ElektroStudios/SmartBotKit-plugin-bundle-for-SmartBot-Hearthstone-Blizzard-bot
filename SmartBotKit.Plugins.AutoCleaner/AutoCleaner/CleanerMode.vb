
#Region " Imports "

Imports SmartBot.Plugins

#End Region

#Region " CleanerMode "

Namespace AutoCleaner

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies when the garbage should be cleaned.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum CleanerMode

        ''' <summary>
        ''' Clean garbage on SmartBot's starup.
        ''' </summary>
        OnStartup = 0

        ''' <summary>
        ''' Clean garbage on a call to <see cref="Plugin.OnStopped()"/>.
        ''' </summary>
        OnBotStop = 1

        ''' <summary>
        ''' Clean garbage on SmartBot's exit.
        ''' </summary>
        OnExit = 2

    End Enum

End Namespace

#End Region
