
#Region " SmartBotEvent "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies a computer power state.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum ComputerState

        ''' <summary>
        ''' Does not change the current state of the computer.
        ''' </summary>
        NoChange = 0

        ''' <summary>
        ''' Hibernates the computer.
        ''' </summary>
        Hibernate = 1

        ''' <summary>
        ''' Suspends the computer.
        ''' </summary>
        Suspend = 2

        ''' <summary>
        ''' Shutdowns the computer.
        ''' </summary>
        Shutdown = 3

    End Enum

End Namespace

#End Region
