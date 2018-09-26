#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics

Imports SmartBotKit.ReservedUse

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Bot

#End Region

#Region " AutoConcedeSchedulerPlugin "

Namespace PluginTemplate

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin to schedule auto-concede after winning a match.
    ''' </summary>
    ''' <example></example>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class AutoConcedeSchedulerPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As AutoConcedeSchedulerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, AutoConcedeSchedulerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="AutoConcedeSchedulerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the current wins in ranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private currentRankedWins As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the current wins in ranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private currentUnrankedWins As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the current bot mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private currentMode As Bot.Mode = Bot.Mode.None

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A flag to help determine whether the current match is auto-conceded by the plugin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private isMatchAutoConceded As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="AutoConcedeSchedulerPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Public Sub New()
            Me.IsDll = True

            UpdateUtil.RunUpdaterExecutable()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="AutoConcedeSchedulerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Auto-Concede Scheduler] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="AutoConcedeSchedulerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Auto-Concede Scheduler] -> Plugin enabled.")
                Else
                    Me.currentRankedWins = 0
                    Me.currentUnrankedWins = 0
                    Bot.Log("[Auto-Concede Scheduler] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is started.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStarted()
            Me.currentMode = Bot.CurrentMode()
            MyBase.OnStarted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStopped()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.ResetWinsCountAfterBotStop) Then
                Me.currentRankedWins = 0
                Me.currentUnrankedWins = 0
                Bot.Log("[Auto-Concede Scheduler] -> Wins count resets to zero, reason: bot stop.")
            End If
            MyBase.OnStopped()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameBegin()
            If (Me.DataContainer.Enabled) Then
                Select Case Me.currentMode
                    Case Mode.RankedStandard, Mode.RankedWild
                        Dim isWildMode As Boolean = (Me.currentMode = Mode.RankedWild)
                        If (Bot.GetPlayerDatas.GetRank(isWildMode) < Me.DataContainer.MinRank) Then
                            Me.currentRankedWins = 0
                            Bot.Log("[Auto-Concede Scheduler] -> (Ranked Mode) Wins count resets to zero, reason: don't concede below the current rank.")
                            Exit Select
                        End If

                        Bot.Log(String.Format("[Auto-Concede Scheduler] -> (Ranked Mode) Current wins count: {0}", Me.currentRankedWins))
                        If (Me.currentRankedWins >= Me.DataContainer.MaxRankedWins) Then
                            Bot.Log("[Auto-Concede Scheduler] -> (Ranked Mode) Conceding the current match...")
                            Me.isMatchAutoConceded = True
                            Bot.Concede()
                        End If

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        Bot.Log(String.Format("[Auto-Concede Scheduler] -> (Unranked Mode) Current wins count: {0}", Me.currentUnrankedWins))
                        If (Me.currentUnrankedWins >= Me.DataContainer.MaxUnrankedWins) Then
                            Bot.Log("[Auto-Concede Scheduler] -> (Unranked Mode) Conceding the current match...")
                            Me.isMatchAutoConceded = True
                            Bot.Concede()
                        End If

                    Case Else
                        ' Do nothing.
                End Select
            End If

            MyBase.OnGameBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when our hero is defeated by the opponent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDefeat()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.ResetWinsCountAfterLose) Then
                Select Case Me.currentMode
                    Case Mode.RankedStandard, Mode.RankedWild
                        Me.currentRankedWins = 0
                        If Not (Me.isMatchAutoConceded) Then
                            Bot.Log("[Auto-Concede Scheduler] -> (Ranked Mode) Wins count resets to zero, reason: defeat.")
                        End If

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        Me.currentUnrankedWins = 0
                        If Not (Me.isMatchAutoConceded) Then
                            Bot.Log("[Auto-Concede Scheduler] -> (Unranked Mode) Wins count resets to zero, reason: defeat.")
                        End If

                    Case Else
                        ' Do nothing.
                End Select
            End If
            Me.isMatchAutoConceded = False

            MyBase.OnDefeat()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when our hero wins the opponent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnVictory()
            If (Me.DataContainer.Enabled) Then
                Select Case Me.currentMode
                    Case Mode.RankedStandard, Mode.RankedWild
                        Me.currentRankedWins += 1
                        Bot.Log(String.Format("[Auto-Concede Scheduler] -> (Ranked Mode) Current wins count: {0}", Me.currentRankedWins))

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        Me.currentUnrankedWins += 1
                        Bot.Log(String.Format("[Auto-Concede Scheduler] -> (Unranked Mode) Current wins count: {0}", Me.currentUnrankedWins))

                    Case Else
                        ' Do nothing.
                End Select
            End If
            MyBase.OnVictory()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game is conceded by our hero.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnConcede()
            If (Me.DataContainer.Enabled) Then
                Select Case Me.currentMode
                    Case Mode.RankedStandard, Mode.RankedWild
                        Me.currentRankedWins = 0
                        Bot.Log("[Auto-Concede Scheduler] -> (Ranked Mode) Wins count resets to zero, reason: concede.")

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        Me.currentUnrankedWins = 0
                        Bot.Log("[Auto-Concede Scheduler] -> (Unranked Mode) Wins count resets to zero, reason: concede.")

                    Case Else
                        ' Do nothing.
                End Select
            End If
            MyBase.OnConcede()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="AutoConcedeSchedulerPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

#End Region

    End Class

End Namespace

#End Region
