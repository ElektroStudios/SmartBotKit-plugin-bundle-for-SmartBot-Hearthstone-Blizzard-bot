
#Region " Imports "

Imports SmartBot.Plugins

#End Region

#Region " SmartBotEvent "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies a SmartBot's process event.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum SmartBotEvent

        ''' <summary>
        ''' Called at SmartBot's starup, when the plugin is built.
        ''' <p></p>
        ''' Method name: OnPluginCreated
        ''' </summary>
        Startup

        ''' <summary>
        ''' Called when the bot is started.
        ''' <p></p>
        ''' Method name: OnStarted
        ''' </summary>
        BotStart

        ''' <summary>
        ''' Called when the bot is stopped.
        ''' <p></p>
        ''' Method name: OnStopped
        ''' </summary>
        BotStop

        ''' <summary>
        ''' Called when a game begins.
        ''' <p></p>
        ''' Method name: OnGameBegin
        ''' </summary>
        GameBegin

        ''' <summary>
        ''' Called when a game ends.
        ''' <p></p>
        ''' Method name: OnGameEnd
        ''' </summary>
        GameEnd

        ''' <summary>
        ''' Called when a turn begins.
        ''' <p></p>
        ''' Method name: OnTurnBegin
        ''' </summary>
        TurnBegin

        ''' <summary>
        ''' Called when a turn ends.
        ''' <p></p>
        ''' Method name: OnTurnEnd
        ''' </summary>
        TurnEnd

        ''' <summary>
        ''' Called when the bot timer is ticked, every 300 milliseconds.
        ''' <p></p>
        ''' Method name: OnTick
        ''' </summary>
        TimerTick

        ''' <summary>
        ''' Called before SmartBot's process termination.
        ''' <p></p>
        ''' Method name: Dispose
        ''' </summary>
        [Exit]

    End Enum

End Namespace

#End Region
