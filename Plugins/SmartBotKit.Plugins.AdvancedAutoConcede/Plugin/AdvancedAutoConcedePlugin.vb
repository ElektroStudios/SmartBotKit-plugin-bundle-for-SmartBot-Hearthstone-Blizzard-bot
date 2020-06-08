#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Linq

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Bot

Imports HearthMirror.Objects
Imports HearthMirror.Objects.MatchInfo

#End Region

#Region " AdvancedAutoConcedePlugin "

' ReSharper disable once CheckNamespace

Namespace AdvancedAutoConcede

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

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="AdvancedAutoConcedePluginData.AlwaysConcede"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastAlwaysConcede As Boolean

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
        Friend Shared rankedWinsCount As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the current wins in unranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxUnrankedWins"/>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Shared unrankedWinsCount As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the consecutive auto-concedes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxRankedConcedes"/>
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxUnrankedConcedes"/>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Shared concedesCount As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the current bot mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private currentMode As Mode = Bot.Mode.None

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A flag to help determine whether the current match is auto-conceded by the plugin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private isMatchAutoConceded As Boolean

        ' ReSharper restore InconsistentNaming

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
            Me.lastAlwaysConcede = Me.DataContainer.AlwaysConcede
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
                    AdvancedAutoConcedePlugin.rankedWinsCount = 0
                    AdvancedAutoConcedePlugin.unrankedWinsCount = 0
                    Bot.Log("[Advanced Auto Concede] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If

            If (Me.lastAlwaysConcede) AndAlso Not (Me.DataContainer.AlwaysConcede) Then
                Me.lastAlwaysConcede = False
                AdvancedAutoConcedePlugin.concedesCount = 0

            ElseIf Not (Me.lastAlwaysConcede) AndAlso (Me.DataContainer.AlwaysConcede) Then
                Me.lastAlwaysConcede = True

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
                AdvancedAutoConcedePlugin.rankedWinsCount = 0
                AdvancedAutoConcedePlugin.unrankedWinsCount = 0
                AdvancedAutoConcedePlugin.concedesCount = 0
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

                If Not (Me.DataContainer.AlwaysConcede) Then

                    Dim battletags As ReadOnlyCollection(Of String) = Me.DataContainer.GetBattletags()
                    If battletags?.Any() Then

                        Dim matchInfo As MatchInfo
                        Dim player As Player

                        Try
                            matchInfo = HearthMirror.Reflection.GetMatchInfo()
                            If matchInfo Is Nothing Then
                                Bot.Log($"[Advanced Auto Concede] -> Error retrieving opponent BattleTag, {NameOf(matchInfo)}' is null. (HDT is outdated?)")
                            End If

                            player = matchInfo.OpposingPlayer
                            If player Is Nothing Then
                                Bot.Log($"[Advanced Auto Concede] -> Error retrieving opponent BattleTag, {NameOf(matchInfo.OpposingPlayer)}' is null. (HDT is outdated?)")
                            End If

                            If battletags.Contains($"{player.BattleTag.Name}#{player.BattleTag.Number}".ToLower()) OrElse
                               battletags.Contains(player.BattleTag.Name.ToLower()) Then

                                Bot.Log($"[Advanced Auto Concede] -> Conceding the current match, reason: '{player.BattleTag.Name}#{player.BattleTag.Number}' Battletag name.")
                                Me.isMatchAutoConceded = True
                                Bot.Concede()
                            End If

                        Catch ex As Exception
                            Bot.Log($"[Advanced Auto Concede] -> Error retrieving opponent BattleTag: {ex.Message}")

                        End Try

                    End If

                    Me.currentMode = Bot.CurrentMode()

                    Select Case Me.currentMode

                        Case Mode.RankedStandard, Mode.RankedWild
                            If (Me.DataContainer.EnableRankedModeAutoConcede) Then
                                Dim isWildMode As Boolean = (Me.currentMode = Mode.RankedWild)
                                If Not isWildMode Then
                                    If (Bot.GetPlayerDatas.GetRank(wild:=False) > Me.DataContainer.MinRankStandard) Then
                                        AdvancedAutoConcedePlugin.rankedWinsCount = 0
                                        Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Wins count resets to zero, reason: don't concede below the current standard rank.")
                                        Exit Select
                                    End If
                                Else
                                    If (Bot.GetPlayerDatas.GetRank(wild:=True) > Me.DataContainer.MinRankWild) Then
                                        AdvancedAutoConcedePlugin.rankedWinsCount = 0
                                        Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Wins count resets to zero, reason: don't concede below the current wild rank.")
                                        Exit Select
                                    End If
                                End If

                                Bot.Log($"[Advanced Auto Concede] -> (Ranked Mode) Current wins count: {AdvancedAutoConcedePlugin.rankedWinsCount}")
                                If (AdvancedAutoConcedePlugin.rankedWinsCount >= Me.DataContainer.MaxRankedWins) Then
                                    Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Conceding the current match, reason: max. ranked wins")
                                    Me.isMatchAutoConceded = True
                                    Bot.Concede()
                                    Exit Select
                                End If

                                If (AdvancedAutoConcedePlugin.concedesCount <> 0) Then
                                    If (AdvancedAutoConcedePlugin.concedesCount < Me.DataContainer.MaxRankedConcedes) Then
                                        Bot.Log(
                                            $"[Advanced Auto Concede] -> (Ranked Mode) Conceding the current match, reason: repeat concede ({ _
                                                   (AdvancedAutoConcedePlugin.concedesCount + 1)} of {Me.DataContainer.MaxRankedConcedes})")
                                        Me.isMatchAutoConceded = True
                                        Bot.Concede()
                                    Else
                                        AdvancedAutoConcedePlugin.concedesCount = 0
                                    End If
                                End If
                            End If

                        Case Mode.UnrankedStandard, Mode.UnrankedWild
                            If (Me.DataContainer.EnableUnrankedModeAutoConcede) Then
                                Bot.Log(
                                    $"[Advanced Auto Concede] -> (Unranked Mode) Current wins count: {AdvancedAutoConcedePlugin.unrankedWinsCount}")
                                If (AdvancedAutoConcedePlugin.unrankedWinsCount >= Me.DataContainer.MaxUnrankedWins) Then
                                    Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Conceding the current match, reason: max. unranked wins")
                                    Me.isMatchAutoConceded = True
                                    Bot.Concede()
                                End If

                                If (AdvancedAutoConcedePlugin.concedesCount <> 0) Then
                                    If (AdvancedAutoConcedePlugin.concedesCount < Me.DataContainer.MaxUnrankedConcedes) Then
                                        Bot.Log(
                                            $"[Advanced Auto Concede] -> (Unranked Mode) Conceding the current match, reason: repeat concede ({ _
                                                   (AdvancedAutoConcedePlugin.concedesCount + 1)} of {Me.DataContainer.MaxUnrankedConcedes})")
                                        Me.isMatchAutoConceded = True
                                        Bot.Concede()
                                    Else
                                        AdvancedAutoConcedePlugin.concedesCount = 0
                                    End If
                                End If
                            End If

                        Case Else ' Mode.Arena, Mode.ArenaAuto, Mode.Practice
                            ' Do nothing.

                    End Select

                Else 'If Me.DataContainer.AlwaysConcede
                    Bot.Log("[Advanced Auto Concede] -> Conceding the current match, reason: 'Always concede' is enabled")
                    'Me.isMatchAutoConceded = True
                    AdvancedAutoConcedePlugin.concedesCount += 1
                    Bot.Concede()

                End If

            End If

            MyBase.OnGameBegin()

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when our hero is defeated by the opponent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDefeat()

            If (Me.DataContainer.Enabled) AndAlso Not (Me.DataContainer.AlwaysConcede) Then

                Select Case Me.currentMode

                    Case Mode.RankedStandard, Mode.RankedWild
                        If (Me.DataContainer.EnableRankedModeAutoConcede) Then
                            If (Me.DataContainer.ResetWinsCountAfterLose) Then
                                AdvancedAutoConcedePlugin.rankedWinsCount = 0
                                If Not (Me.isMatchAutoConceded) Then
                                    Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Wins count resets to zero, reason: defeat.")
                                End If
                            End If

                            Dim isWild As Boolean = (Me.currentMode = Mode.RankedWild)
                            If Not (isWild) AndAlso (Bot.GetPlayerDatas.GetRank(wild:=False) <= Me.DataContainer.MaxRankStandard) Then
                                Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Max. standard rank reached, switching to mode: unranked standard")
                                Bot.ChangeMode(Mode.UnrankedStandard)
                                Me.currentMode = Mode.UnrankedStandard
                            End If

                            If (isWild) AndAlso (Bot.GetPlayerDatas.GetRank(wild:=True) <= Me.DataContainer.MaxRankWild) Then
                                Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Max. wild rank reached, switching to mode: unranked wild")
                                Bot.ChangeMode(Mode.UnrankedWild)
                                Me.currentMode = Mode.UnrankedWild
                            End If

                        End If

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        If (Me.DataContainer.EnableUnrankedModeAutoConcede) Then
                            If (Me.DataContainer.ResetWinsCountAfterLose) Then
                                AdvancedAutoConcedePlugin.unrankedWinsCount = 0
                                If Not (Me.isMatchAutoConceded) Then
                                    Bot.Log("[Advanced Auto Concede] -> (Unranked Mode) Wins count resets to zero, reason: defeat.")
                                End If
                            End If
                        End If

                    Case Else ' Mode.Arena, Mode.ArenaAuto, Mode.Practice
                        ' Do nothing.

                End Select

            End If

            If (Me.isMatchAutoConceded) Then
                AdvancedAutoConcedePlugin.concedesCount += 1
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

            If (Me.DataContainer.Enabled) AndAlso Not (Me.DataContainer.AlwaysConcede) Then

                Select Case Me.currentMode

                    Case Mode.RankedStandard, Mode.RankedWild
                        If (Me.DataContainer.EnableRankedModeAutoConcede) Then
                            AdvancedAutoConcedePlugin.rankedWinsCount += 1
                            Bot.Log($"[Advanced Auto Concede] -> (Ranked Mode) Current wins count: {AdvancedAutoConcedePlugin.rankedWinsCount}")
                        End If

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        If (Me.DataContainer.EnableUnrankedModeAutoConcede) Then
                            AdvancedAutoConcedePlugin.unrankedWinsCount += 1
                            Bot.Log(
                                $"[Advanced Auto Concede] -> (Unranked Mode) Current wins count: {AdvancedAutoConcedePlugin.unrankedWinsCount}")
                        End If

                    Case Else ' Mode.Arena, Mode.ArenaAuto, Mode.Practice
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

                If (Me.DataContainer.AlwaysConcede) Then
                    Exit Sub
                End If

                Select Case Me.currentMode

                    Case Mode.RankedStandard, Mode.RankedWild
                        AdvancedAutoConcedePlugin.rankedWinsCount = 0
                        Bot.Log("[Advanced Auto Concede] -> (Ranked Mode) Wins count resets to zero, reason: auto-concede.")

                    Case Mode.UnrankedStandard, Mode.UnrankedWild
                        AdvancedAutoConcedePlugin.unrankedWinsCount = 0
                        Bot.Log("[Advanced Auto Concede] -> (Unranked Mode) Wins count resets to zero, reason: auto-concede.")

                    Case Else
                        ' Do nothing.

                End Select

            End If

            MyBase.OnConcede()

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="AdvancedAutoConcedePlugin"/> instance.
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
