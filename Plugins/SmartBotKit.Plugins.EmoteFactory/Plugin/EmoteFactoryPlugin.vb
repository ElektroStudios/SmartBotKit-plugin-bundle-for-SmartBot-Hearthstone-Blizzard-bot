
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics
Imports System.Threading
Imports System.Threading.Tasks

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Bot

Imports SmartBotKit.ReservedUse

#End Region

#Region " EmoteFactoryPlugin "

' ReSharper disable once CheckNamespace

Namespace EmoteFactory


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that builds configurable rule conditions to send or answer to opponent emotes..
    ''' </summary>
    ''' <example></example>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class EmoteFactoryPlugin : Inherits Plugin

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin's data container.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The plugin's data container.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shadows ReadOnly Property DataContainer As EmoteFactoryPluginData
            Get
                Return DirectCast(MyBase.DataContainer, EmoteFactoryPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A random secuence generator.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly Rng As Random

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="EmoteFactoryPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the current emote replies.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private replyCount As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A <see cref="Boolean"/> flag that determine whether the plugin already has sent a emote on first hero's turn.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private emotedFirstTurn As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A <see cref="Boolean"/> flag that determine whether the plugin already has squelched/muted the enemy.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private isEnemySquelched As Boolean

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="EmoteFactoryPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            UpdateUtil.RunUpdaterExecutable()

            Me.Rng = New Random(Seed:=Environment.TickCount)
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="EmoteFactoryPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Emote Factory] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="EmoteFactoryPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Emote Factory] -> Plugin enabled.")
                Else
                    Bot.Log("[Emote Factory] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameBegin()
            Me.replyCount = 0
            Me.isEnemySquelched = False
            Me.emotedFirstTurn = False
            MyBase.OnGameBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a turn begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTurnBegin()
            If (Me.DataContainer.Enabled) AndAlso (Bot.IsBotRunning) Then
                Me.MaybeDoEmoteOnFirstTurn()
            End If
            MyBase.OnTurnBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the player receives an emote from the enemy.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepperBoundary>
        Public Overrides Sub OnReceivedEmote(ByVal emoteType As EmoteType)
            If (Me.DataContainer.Enabled) AndAlso (Bot.IsBotRunning) AndAlso Not (Me.isEnemySquelched) Then
                Me.MaybeDoReplyEmote(emoteType)
                Me.MaybeSquelchEnemy()
            End If
            MyBase.OnReceivedEmote(emoteType)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot determines the hero has a lethal move to perform in the current turn.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnLethal()
            If (Me.DataContainer.Enabled) AndAlso (Bot.IsBotRunning) Then
                Me.MaybeDoEmoteOnLethal()
            End If
            MyBase.OnLethal()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game is conceded by our hero.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnConcede()
            If (Me.DataContainer.Enabled) AndAlso (Bot.IsBotRunning) Then
                Me.MaybeDoEmoteOnConcede()
            End If
            MyBase.OnConcede()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when our hero is defeated by the opponent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDefeat()
            If (Me.DataContainer.Enabled) AndAlso (Bot.IsBotRunning) Then
                Me.MaybeDoEmoteOnDefeat()
            End If
            MyBase.OnDefeat()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="EmoteFactoryPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Randomly decides to send a emote on the first turn.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Async Sub MaybeDoEmoteOnFirstTurn()
            If Not (Me.emotedFirstTurn) AndAlso
                   (Me.DataContainer.EmoteOnFirstTurn) AndAlso
                   (Bot.CurrentBoard.TurnCount < 2) Then

                Me.emotedFirstTurn = True
                Dim emote As EmoteType = Me.DataContainer.EmoteOnFirstTurnType

                Dim t As Task = Task.Factory.StartNew(
                    Sub()
                        Dim queued As Boolean = Me.MaybeSendEmote(Me.DataContainer.SendEmoteOnConditionsPercent, emote)
                        If (queued) Then
                            Bot.Log(
                                $"[Emote Factory] -> Sending emote '{emote.ToString()}' due to condition: '{ _
                                       NameOf(Me.DataContainer.EmoteOnFirstTurn)}'.")
                        End If
                    End Sub, TaskCreationOptions.LongRunning)

                Await t
                t.Dispose()
            End If
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Randomly decides to send a emote when the bot detects a lethal move.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Async Sub MaybeDoEmoteOnLethal()
            If (Me.DataContainer.EmoteOnLethal) Then
                Dim emote As EmoteType = Me.DataContainer.EmoteOnLethalType

                Dim t As Task = Task.Factory.StartNew(
                    Sub()
                        Dim queued As Boolean = Me.MaybeSendEmote(Me.DataContainer.SendEmoteOnConditionsPercent, emote)
                        If (queued) Then
                            Bot.Log(
                                $"[Emote Factory] -> Sending emote '{emote.ToString()}' due to condition: '{ _
                                       NameOf(Me.DataContainer.EmoteOnLethal)}'.")
                        End If
                    End Sub, TaskCreationOptions.LongRunning)

                Await t
                t.Dispose()
            End If
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Randomly decides to send a emote when the bot concedes the game.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Async Sub MaybeDoEmoteOnConcede()
            If (Me.DataContainer.EmoteOnConcede) Then
                Dim emote As EmoteType = Me.DataContainer.EmoteOnConcedeType

                Dim t As Task = Task.Factory.StartNew(
                    Sub()
                        Dim queued As Boolean = Me.MaybeSendEmote(Me.DataContainer.SendEmoteOnConditionsPercent, emote)
                        If (queued) Then
                            Bot.Log(
                                $"[Emote Factory] -> Sending emote '{emote.ToString()}' due to condition: '{ _
                                       NameOf(Me.DataContainer.EmoteOnConcede)}'.")
                        End If
                    End Sub, TaskCreationOptions.LongRunning)

                Await t
                t.Dispose()
            End If
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Randomly decides to send a emote when the bot concedes the game.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Async Sub MaybeDoEmoteOnDefeat()
            If (Me.DataContainer.EmoteOnDefeat) Then
                Dim emote As EmoteType = Me.DataContainer.EmoteOnDefeatType

                Dim t As Task = Task.Factory.StartNew(
                    Sub()
                        Dim queued As Boolean = Me.MaybeSendEmote(Me.DataContainer.SendEmoteOnConditionsPercent, emote)
                        If (queued) Then
                            Bot.Log(
                                $"[Emote Factory] -> Sending emote '{emote.ToString()}' due to condition: '{ _
                                       NameOf(Me.DataContainer.EmoteOnDefeat)}'.")
                        End If
                    End Sub, TaskCreationOptions.LongRunning)

                Await t
                t.Dispose()
            End If
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Randomly decides to reply a enemy's emote.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Async Sub MaybeDoReplyEmote(ByVal emoteReceived As EmoteType)

            If (Interlocked.Increment(Me.replyCount) <= Me.DataContainer.MaxReplies) Then

                Dim emoteToSend As EmoteType
                Select Case emoteReceived
                    Case EmoteType.Greetings
                        ' Fixes an issue on which the plugin can send two times the "Greetings" emote on the first turn.
                        ' https://smartbot.pw/index.php?/topic/9071-plugin-emote-factory/&do=findComment&comment=54069
                        If (Bot.CurrentBoard.TurnCount < 2) AndAlso (Me.emotedFirstTurn) Then
                            Exit Sub
                        End If
                        emoteToSend = Me.DataContainer.EmoteToReplyGrettings

                    Case EmoteType.Oops
                        emoteToSend = Me.DataContainer.EmoteToReplyOops

                    Case EmoteType.Thanks
                        emoteToSend = Me.DataContainer.EmoteToReplyThanks

                    Case EmoteType.Threaten
                        emoteToSend = Me.DataContainer.EmoteToReplyThreaten

                    Case EmoteType.WellPlayed
                        emoteToSend = Me.DataContainer.EmoteToReplyWellPlayed

                    Case EmoteType.Wow
                        emoteToSend = Me.DataContainer.EmoteToReplyWow
                End Select

                ' Dim emoteToSend As EmoteType = DirectCast(Me.Rng.Next(1, 5), EmoteType) ' Random emote.

                Dim t As Task = Task.Factory.StartNew(
                    Sub()
                        Dim queued As Boolean = Me.MaybeSendEmote(Me.DataContainer.SendEmoteOnConditionsPercent, emoteToSend)
                        If (queued) Then
                            Bot.Log(
                                $"[Emote Factory] -> Sending emote '{emoteToSend.ToString() _
                                       }' as a reply to enemy emote: '{emoteReceived.ToString()}'.")
                        End If
                    End Sub, TaskCreationOptions.LongRunning)

                Await t
                t.Dispose()
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Randomly decides to send a emote to the enemy.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="emote">
        ''' The type of emote to send.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepperBoundary>
        Private Function MaybeSendEmote(ByVal percent As Integer, ByVal emote As EmoteType) As Boolean

            Dim chance As Integer = Me.Rng.Next(1, 101)
            Dim isWinner As Boolean = (chance <= percent)

            If (isWinner) Then
                Thread.Sleep(Me.Rng.Next(1500, Me.DataContainer.MaxDelay))
                Bot.SendEmote(emote)
            End If

            Return isWinner
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Randomly decides to squelch/mute the enemy.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepperBoundary>
        Private Function MaybeSquelchEnemy() As Boolean
            Dim chance As Integer = Me.Rng.Next(1, 101)
            Dim isWinner As Boolean = (chance <= Me.DataContainer.SquelchEnemnyPercent)

            If (isWinner) Then
                Me.isEnemySquelched = True
                Thread.Sleep(Me.Rng.Next(2000, 5000))
                Bot.Squelch()
                Bot.Log("[Emote Factory] -> Enemy squelched.")
            End If

            Return isWinner
        End Function

#End Region

    End Class

End Namespace

#End Region
