
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

#End Region

#Region " UltimateEmoterPlugin "

Namespace UltimateEmoter

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' This plugin automatically sends emotes to enemies on certain conditions defined by the user.
    ''' </summary>
    ''' <example></example>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class UltimateEmoterPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As UltimateEmoterPluginData
            Get
                Return DirectCast(MyBase.DataContainer, UltimateEmoterPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A random secuence generator.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly Rng As Random

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="UltimateEmoterPluginData.Enabled"/> value.
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
        ''' A <see cref="Boolean"/> flag that determine whether the plugin already has sent a emote on first hero's or enemy's turn.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private emotedFirstTurn As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A <see cref="Boolean"/> flag that determine whether the plugin already has squelched/muted the enemy.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private isEnemySquelched As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="UltimateEmoterPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            SmartBotKit.ReservedUse.UpdateUtil.RunUpdaterExecutable()

            Me.Rng = New Random(Seed:=Environment.TickCount)
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="UltimateEmoterPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[UltimateEmoter] Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="UltimateEmoterPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[UltimateEmoter] Plugin enabled.")
                Else
                    Bot.Log("[UltimateEmoter] Plugin disabled.")
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
            If (Me.DataContainer.Enabled) AndAlso (Bot.IsBotRunning) Then
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
        ''' Releases all the resources used by this <see cref="UltimateEmoterPlugin"/> instance.
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
                Dim emote As Bot.EmoteType = Me.DataContainer.EmoteOnFirstTurnType

                Dim t As Task = Task.Factory.StartNew(
                    Sub()
                        Dim queued As Boolean = Me.MaybeSendEmote(Me.DataContainer.SendEmoteOnConditionsPercent, emote)
                        If (queued) Then
                            Bot.Log(String.Format("[UltimateEmoter] Sending emote '{0}' due to condition: '{1}'.", emote.ToString(), NameOf(Me.DataContainer.EmoteOnFirstTurn)))
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
                Dim emote As Bot.EmoteType = Me.DataContainer.EmoteOnLethalType

                Dim t As Task = Task.Factory.StartNew(
                    Sub()
                        Dim queued As Boolean = Me.MaybeSendEmote(Me.DataContainer.SendEmoteOnConditionsPercent, emote)
                        If (queued) Then
                            Bot.Log(String.Format("[UltimateEmoter] Sending emote '{0}' due to condition: '{1}'.", emote.ToString(), NameOf(Me.DataContainer.EmoteOnLethal)))
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
                Dim emote As Bot.EmoteType = Me.DataContainer.EmoteOnConcedeType

                Dim t As Task = Task.Factory.StartNew(
                    Sub()
                        Dim queued As Boolean = Me.MaybeSendEmote(Me.DataContainer.SendEmoteOnConditionsPercent, emote)
                        If (queued) Then
                            Bot.Log(String.Format("[UltimateEmoter] Sending emote '{0}' due to condition: '{1}'.", emote.ToString(), NameOf(Me.DataContainer.EmoteOnConcede)))
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
                Dim emote As EmoteType = DirectCast(Me.Rng.Next(1, 5), EmoteType) ' Send a random emote.

                Dim t As Task = Task.Factory.StartNew(
                    Sub()
                        Dim queued As Boolean = Me.MaybeSendEmote(Me.DataContainer.SendEmoteOnConditionsPercent, emote)
                        If (queued) Then
                            Bot.Log(String.Format("[UltimateEmoter] Sending emote '{0}' as a reply to enemy emote: '{1}'.", emote.ToString(), emoteReceived.ToString()))
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
        Private Function MaybeSendEmote(ByVal percent As Integer, ByVal emote As Bot.EmoteType) As Boolean

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
                Bot.Log("[UltimateEmoter] Enemy squelched.")
            End If

            Return isWinner
        End Function

#End Region

    End Class

End Namespace

#End Region
