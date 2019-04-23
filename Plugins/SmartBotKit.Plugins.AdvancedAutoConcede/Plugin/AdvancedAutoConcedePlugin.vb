#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Bot

#End Region

#Region " AdvancedAutoConcedePlugin "

Namespace PluginTemplate

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin to build auto-concede rules. 
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class AdvancedAutoConcedePlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As AdvancedAutoConcedePluginData
            Get
                Return DirectCast(MyBase.DataContainer, AdvancedAutoConcedePluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="AdvancedAutoConcedePluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the current wins in ranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxRankedWins"/>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsCount As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the current wins in unranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxUnrankedWins"/>
        ''' ----------------------------------------------------------------------------------------------------
        Private unrankedWinsCount As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the consecutive auto-concedes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxRankedConcedes"/>
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxUnrankedConcedes"/>
        ''' ----------------------------------------------------------------------------------------------------
        Private concedesCount As Integer

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
        ''' Initializes a new instance of the <see cref="AdvancedAutoConcedePlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Public Sub New()
            Me.IsDll = True
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="AdvancedAutoConcedePlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Advanced Auto Concede] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="AdvancedAutoConcedePluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Advanced Auto Concede] -> Plugin enabled.")
                Else
                    Me.rankedWinsCount = 0
                    Me.unrankedWinsCount = 0
                    Bot.Log("[Advanced Auto Concede] -> Plugin disabled.")
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
                Me.rankedWinsCount = 0
                Me.unrankedWinsCount = 0
                Me.concedesCount = 0
                Bot.Log("[Advanced Auto Concede] -> Wins count resets to zero, reason: bot stop.")
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
                Me.currentMode = Bot.CurrentMode()

                Select Case Me.currentMode
                    Case Mode.RankedStandard, Mode.RankedWild
                        Dim isWildMode As Boolean = (Me.currentMode = Mode.RankedWild)
                        If Not isWildMode Then
                            If (Bot.GetPlayerDatas.GetRank(wild:=False) > Me.DataContainer.MinRankStandard) Then
                                Me.rankedWinsCount = 0
                                Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Wins count resets to zero, reason: don't concede below the current standard rank.")
                                Exit Select
                            End If
                        Else
                            If (Bot.GetPlayerDatas.GetRank(wild:=True) > Me.DataContainer.MinRankWild) Then
                                Me.rankedWinsCount = 0
                                Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Wins count resets to zero, reason: don't concede below the current wild rank.")
                                Exit Select
                            End If
                        End If

                        Bot.Log(String.Format("[Advanced Auto Concede] -> (Ranked Mode) Current wins count: {0}", Me.rankedWinsCount))
                        If (Me.rankedWinsCount >= Me.DataContainer.MaxRankedWins) Then
                            Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Conceding the current match, reason: max. ranked wins")
                            Me.isMatchAutoConceded = True
                            Bot.Concede()
                            Exit Select
                        End If

                        If (Me.concedesCount <> 0) Then
                            If (Me.concedesCount < Me.DataContainer.MaxRankedConcedes) Then
                                Bot.Log(String.Format("[Advanced Auto Concede] -> (Ranked Mode) Conceding the current match, reason: repeat concede ({0} of {1})", (Me.concedesCount + 1), Me.DataContainer.MaxRankedConcedes))
                                Me.isMatchAutoConceded = True
                                Bot.Concede()
                            Else
                                Me.concedesCount = 0
                            End If
                        End If

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        Bot.Log(String.Format("[Advanced Auto Concede] -> (Unranked Mode) Current wins count: {0}", Me.unrankedWinsCount))
                        If (Me.unrankedWinsCount >= Me.DataContainer.MaxUnrankedWins) Then
                            Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Conceding the current match, reason: max. unranked wins")
                            Me.isMatchAutoConceded = True
                            Bot.Concede()
                        End If

                        If (Me.concedesCount <> 0) Then
                            If (Me.concedesCount < Me.DataContainer.MaxUnrankedConcedes) Then
                                Bot.Log(String.Format("[Advanced Auto Concede] -> (Unranked Mode) Conceding the current match, reason: repeat concede ({0} of {1})", (Me.concedesCount + 1), Me.DataContainer.MaxRankedConcedes))
                                Me.isMatchAutoConceded = True
                                Bot.Concede()
                            Else
                                Me.concedesCount = 0
                            End If
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
            If (Me.DataContainer.Enabled) Then
                Select Case Me.currentMode
                    Case Mode.RankedStandard, Mode.RankedWild
                        If (Me.DataContainer.ResetWinsCountAfterLose) Then
                            Me.rankedWinsCount = 0
                            If Not (Me.isMatchAutoConceded) Then
                                Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Wins count resets to zero, reason: defeat.")
                            End If
                        End If

                        Dim isWild As Boolean = (Me.currentMode = Mode.RankedWild)
                        If Not (isWild) AndAlso (Bot.GetPlayerDatas.GetRank(wild:=False) >= Me.DataContainer.MaxRankStandard) Then
                            Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Max. standard rank reached, switching to mode: unranked standard")
                            Bot.ChangeMode(Mode.UnrankedStandard)
                            Me.currentMode = Mode.UnrankedStandard
                        End If

                        If (isWild) AndAlso (Bot.GetPlayerDatas.GetRank(wild:=True) >= Me.DataContainer.MaxRankWild) Then
                            Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Max. wild rank reached, switching to mode: unranked wild")
                            Bot.ChangeMode(Mode.UnrankedWild)
                            Me.currentMode = Mode.UnrankedWild
                        End If

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        If (Me.DataContainer.ResetWinsCountAfterLose) Then
                            Me.unrankedWinsCount = 0
                            If Not (Me.isMatchAutoConceded) Then
                                Bot.Log("[Advanced Auto Concede] -> (Unranked Mode) Wins count resets to zero, reason: defeat.")
                            End If
                        End If

                    Case Else
                        ' Do nothing.
                End Select
            End If
            If (Me.isMatchAutoConceded) Then
                Me.concedesCount += 1
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
                        Me.rankedWinsCount += 1
                        Bot.Log(String.Format("[Advanced Auto Concede] -> (Ranked Mode) Current wins count: {0}", Me.rankedWinsCount))

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        Me.unrankedWinsCount += 1
                        Bot.Log(String.Format("[Advanced Auto Concede] -> (Unranked Mode) Current wins count: {0}", Me.unrankedWinsCount))

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
                        Me.rankedWinsCount = 0
                        Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Wins count resets to zero, reason: auto-concede.")

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        Me.unrankedWinsCount = 0
                        Bot.Log("[Advanced Auto Concede] -> (Unranked Mode) Wins count resets to zero, reason: auto-concede.")

                    Case Else
                        ' Do nothing.
                End Select
            End If
            MyBase.OnConcede()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="AdvancedAutoConcedePlugin"/> instance.
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
