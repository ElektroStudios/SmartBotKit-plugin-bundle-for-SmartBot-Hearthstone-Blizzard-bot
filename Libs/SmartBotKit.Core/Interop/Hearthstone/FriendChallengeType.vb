

' ****************************************************************************
' This source-code belongs to the 'Challenge Notifier' plugin, which is discontinued.
' ****************************************************************************


' ReSharper disable once CheckNamespace
Namespace SmartBotKit.Interop


    Public Enum FriendChallengeType

        ''' <summary>
        ''' A normal friend challenge.
        ''' </summary>
        Normal = 0

        ''' <summary>
        ''' A 80 gold quest friend challenge.
        ''' </summary>
        GoldQuest = 1

        ''' <summary>
        ''' Undetermined.
        ''' <para></para>
        ''' Useful for debug purposes, in scenarios on which <see cref="HearthMirrorFriendlyChallengeProvider.DialogVisible"/> is False.
        ''' </summary>
        Undetermined = -1

    End Enum

End Namespace