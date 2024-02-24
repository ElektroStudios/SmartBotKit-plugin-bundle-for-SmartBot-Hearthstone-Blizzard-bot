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
        ''' Keeps track of the last <see cref="AdvancedAutoConcedePluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="AdvancedAutoConcedePluginData.AlwaysConcede"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastAlwaysConcede As Boolean

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
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxCasualWins"/>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Shared casualWinsCount As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the consecutive auto-concedes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxRankedConcedes"/>
        ''' <seealso cref="AdvancedAutoConcedePluginData.MaxCasualConcedes"/>
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
            Me.lastAlwaysConcede = (Me.DataContainer.AlwaysConcedeToDeathKnight AndAlso
                                    Me.DataContainer.AlwaysConcedeToDemonHunter AndAlso
                                    Me.DataContainer.AlwaysConcedeToDruid AndAlso
                                    Me.DataContainer.AlwaysConcedeToHunter AndAlso
                                    Me.DataContainer.AlwaysConcedeToMage AndAlso
                                    Me.DataContainer.AlwaysConcedeToPaladin AndAlso
                                    Me.DataContainer.AlwaysConcedeToPriest AndAlso
                                    Me.DataContainer.AlwaysConcedeToRogue AndAlso
                                    Me.DataContainer.AlwaysConcedeToShaman AndAlso
                                    Me.DataContainer.AlwaysConcedeToWarlock AndAlso
                                    Me.DataContainer.AlwaysConcedeToWarrior)
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
                    AdvancedAutoConcedePlugin.casualWinsCount = 0
                    Bot.Log("[Advanced Auto Concede] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If

            If (Me.lastAlwaysConcede) AndAlso Not (Me.DataContainer.AlwaysConcedeToDeathKnight AndAlso
                                                   Me.DataContainer.AlwaysConcedeToDemonHunter AndAlso
                                                   Me.DataContainer.AlwaysConcedeToDruid AndAlso
                                                   Me.DataContainer.AlwaysConcedeToHunter AndAlso
                                                   Me.DataContainer.AlwaysConcedeToMage AndAlso
                                                   Me.DataContainer.AlwaysConcedeToPaladin AndAlso
                                                   Me.DataContainer.AlwaysConcedeToPriest AndAlso
                                                   Me.DataContainer.AlwaysConcedeToRogue AndAlso
                                                   Me.DataContainer.AlwaysConcedeToShaman AndAlso
                                                   Me.DataContainer.AlwaysConcedeToWarlock AndAlso
                                                   Me.DataContainer.AlwaysConcedeToWarrior) Then
                Me.lastAlwaysConcede = False
                AdvancedAutoConcedePlugin.concedesCount = 0

            ElseIf Not (Me.lastAlwaysConcede) AndAlso (Me.DataContainer.AlwaysConcedeToDeathKnight AndAlso
                                                       Me.DataContainer.AlwaysConcedeToDemonHunter AndAlso
                                                       Me.DataContainer.AlwaysConcedeToDruid AndAlso
                                                       Me.DataContainer.AlwaysConcedeToHunter AndAlso
                                                       Me.DataContainer.AlwaysConcedeToMage AndAlso
                                                       Me.DataContainer.AlwaysConcedeToPaladin AndAlso
                                                       Me.DataContainer.AlwaysConcedeToPriest AndAlso
                                                       Me.DataContainer.AlwaysConcedeToRogue AndAlso
                                                       Me.DataContainer.AlwaysConcedeToShaman AndAlso
                                                       Me.DataContainer.AlwaysConcedeToWarlock AndAlso
                                                       Me.DataContainer.AlwaysConcedeToWarrior) Then
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
                AdvancedAutoConcedePlugin.casualWinsCount = 0
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

                    Case Mode.Standard, Mode.Wild, Mode.Twist
                        If (Me.DataContainer.EnableRankedModeAutoConcede) Then

                            Bot.Log($"[Advanced Auto Concede] -> (Mode: {Bot.CurrentMode}) Current wins count: {AdvancedAutoConcedePlugin.rankedWinsCount}")
                            If (AdvancedAutoConcedePlugin.rankedWinsCount >= Me.DataContainer.MaxRankedWins) Then
                                Bot.Log($"[Advanced Auto Concede] -> (Mode: {Bot.CurrentMode}) Conceding the current match, reason: max. ranked wins")
                                Me.isMatchAutoConceded = True
                                Bot.Concede()
                                Exit Select
                            End If

                            If (AdvancedAutoConcedePlugin.concedesCount <> 0) Then
                                If (AdvancedAutoConcedePlugin.concedesCount < Me.DataContainer.MaxRankedConcedes) Then
                                    Bot.Log(
                                            $"[Advanced Auto Concede] -> (Mode: {Bot.CurrentMode}) Conceding the current match, reason: repeat concede ({ _
                                                   (AdvancedAutoConcedePlugin.concedesCount + 1)} of {Me.DataContainer.MaxRankedConcedes})")
                                    Me.isMatchAutoConceded = True
                                    Bot.Concede()
                                Else
                                    AdvancedAutoConcedePlugin.concedesCount = 0
                                End If
                            End If
                        End If

                    Case Mode.Casual
                        If (Me.DataContainer.EnableCasualModeAutoConcede) Then
                            Bot.Log(
                                    $"[Advanced Auto Concede] -> (Mode: Casual) Current wins count: {AdvancedAutoConcedePlugin.casualWinsCount}")
                            If (AdvancedAutoConcedePlugin.casualWinsCount >= Me.DataContainer.MaxCasualWins) Then
                                Bot.Log("[Advanced Auto Concede] -> (Mode: Casual) Conceding the current match, reason: max. casual wins")
                                Me.isMatchAutoConceded = True
                                Bot.Concede()
                            End If

                            If (AdvancedAutoConcedePlugin.concedesCount <> 0) Then
                                If (AdvancedAutoConcedePlugin.concedesCount < Me.DataContainer.MaxCasualConcedes) Then
                                    Bot.Log(
                                            $"[Advanced Auto Concede] -> (Mode: Casual) Conceding the current match, reason: repeat concede ({ _
                                                   (AdvancedAutoConcedePlugin.concedesCount + 1)} of {Me.DataContainer.MaxCasualConcedes})")
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

            End If

            MyBase.OnGameBegin()

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a turn begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTurnBegin()

            If (Me.DataContainer.Enabled) Then

                Me.currentMode = Bot.CurrentMode()

                Select Case Me.currentMode

                    Case Mode.Standard, Mode.Wild, Mode.Twist
                        If (Me.DataContainer.EnableRankedModeAutoConcede) Then
                            If (Me.DataContainer.AlwaysConcedeToDeathKnight AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.DEATHKNIGHT) OrElse
                               (Me.DataContainer.AlwaysConcedeToDemonHunter AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.DEMONHUNTER) OrElse
                               (Me.DataContainer.AlwaysConcedeToDruid AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.DRUID) OrElse
                               (Me.DataContainer.AlwaysConcedeToHunter AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.HUNTER) OrElse
                               (Me.DataContainer.AlwaysConcedeToMage AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.MAGE) OrElse
                               (Me.DataContainer.AlwaysConcedeToPaladin AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.PALADIN) OrElse
                               (Me.DataContainer.AlwaysConcedeToPriest AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.PRIEST) OrElse
                               (Me.DataContainer.AlwaysConcedeToRogue AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.ROGUE) OrElse
                               (Me.DataContainer.AlwaysConcedeToShaman AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.SHAMAN) OrElse
                               (Me.DataContainer.AlwaysConcedeToWarlock AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.WARLOCK) OrElse
                               (Me.DataContainer.AlwaysConcedeToWarrior AndAlso Bot.CurrentBoard.EnemyClass = API.Card.CClass.WARRIOR) Then

                                Bot.Log($"[Advanced Auto Concede] -> Conceding the current match, reason: 'Always concede to {Bot.CurrentBoard.EnemyClass}' setting enabled.")
                                Me.isMatchAutoConceded = True
                                AdvancedAutoConcedePlugin.concedesCount += 1
                                Bot.Concede()
                            End If
                        End If

                    Case Else
                        ' Do Nothing.

                End Select


            End If

            MyBase.OnTurnBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when our hero is defeated by the opponent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDefeat()

            If (Me.DataContainer.Enabled) AndAlso Not (Me.DataContainer.AlwaysConcedeToDeathKnight AndAlso
                                                       Me.DataContainer.AlwaysConcedeToDemonHunter AndAlso
                                                       Me.DataContainer.AlwaysConcedeToDruid AndAlso
                                                       Me.DataContainer.AlwaysConcedeToHunter AndAlso
                                                       Me.DataContainer.AlwaysConcedeToMage AndAlso
                                                       Me.DataContainer.AlwaysConcedeToPaladin AndAlso
                                                       Me.DataContainer.AlwaysConcedeToPriest AndAlso
                                                       Me.DataContainer.AlwaysConcedeToRogue AndAlso
                                                       Me.DataContainer.AlwaysConcedeToShaman AndAlso
                                                       Me.DataContainer.AlwaysConcedeToWarlock AndAlso
                                                       Me.DataContainer.AlwaysConcedeToWarrior) Then

                Select Case Me.currentMode

                    Case Mode.Standard, Mode.Wild, Mode.Twist
                        If (Me.DataContainer.EnableRankedModeAutoConcede) Then
                            If (Me.DataContainer.ResetWinsCountAfterLose) Then
                                AdvancedAutoConcedePlugin.rankedWinsCount = 0
                                If Not (Me.isMatchAutoConceded) Then
                                    Bot.Log($"[Advanced Auto Concede] -> (Mode: {Bot.CurrentMode}) Wins count resets to zero, reason: defeat.")
                                End If
                            End If

                            If (Bot.GetPlayerDatas.GetRank(Bot.CurrentMode) <= Me.DataContainer.MaxRank) Then
                                Bot.Log($"[Advanced Auto Concede] -> (Mode: {Bot.CurrentMode}) Max. rank reached, switching to mode: Casual")
                                Bot.ChangeMode(Mode.Casual)
                                Me.currentMode = Mode.Casual
                            End If

                        End If

                    Case Mode.Casual
                        If (Me.DataContainer.EnableCasualModeAutoConcede) Then
                            If (Me.DataContainer.ResetWinsCountAfterLose) Then
                                AdvancedAutoConcedePlugin.casualWinsCount = 0
                                If Not (Me.isMatchAutoConceded) Then
                                    Bot.Log("[Advanced Auto Concede] -> (Mode: Casual) Wins count resets to zero, reason: defeat.")
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

            If (Me.DataContainer.Enabled) AndAlso Not (Me.DataContainer.AlwaysConcedeToDeathKnight AndAlso
                                                       Me.DataContainer.AlwaysConcedeToDemonHunter AndAlso
                                                       Me.DataContainer.AlwaysConcedeToDruid AndAlso
                                                       Me.DataContainer.AlwaysConcedeToHunter AndAlso
                                                       Me.DataContainer.AlwaysConcedeToMage AndAlso
                                                       Me.DataContainer.AlwaysConcedeToPaladin AndAlso
                                                       Me.DataContainer.AlwaysConcedeToPriest AndAlso
                                                       Me.DataContainer.AlwaysConcedeToRogue AndAlso
                                                       Me.DataContainer.AlwaysConcedeToShaman AndAlso
                                                       Me.DataContainer.AlwaysConcedeToWarlock AndAlso
                                                       Me.DataContainer.AlwaysConcedeToWarrior) Then

                Select Case Me.currentMode

                    Case Mode.Standard, Mode.Wild, Mode.Twist
                        If (Me.DataContainer.EnableRankedModeAutoConcede) Then
                            AdvancedAutoConcedePlugin.rankedWinsCount += 1
                            Bot.Log($"[Advanced Auto Concede] -> (Mode: {Bot.CurrentMode}) Current wins count: {AdvancedAutoConcedePlugin.rankedWinsCount}")
                        End If

                    Case Mode.Casual
                        If (Me.DataContainer.EnableCasualModeAutoConcede) Then
                            AdvancedAutoConcedePlugin.casualWinsCount += 1
                            Bot.Log(
                                $"[Advanced Auto Concede] -> (Mode: Casual) Current wins count: {AdvancedAutoConcedePlugin.casualWinsCount}")
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

                If Me.DataContainer.AlwaysConcedeToDeathKnight AndAlso
                   Me.DataContainer.AlwaysConcedeToDemonHunter AndAlso
                   Me.DataContainer.AlwaysConcedeToDruid AndAlso
                   Me.DataContainer.AlwaysConcedeToHunter AndAlso
                   Me.DataContainer.AlwaysConcedeToMage AndAlso
                   Me.DataContainer.AlwaysConcedeToPaladin AndAlso
                   Me.DataContainer.AlwaysConcedeToPriest AndAlso
                   Me.DataContainer.AlwaysConcedeToRogue AndAlso
                   Me.DataContainer.AlwaysConcedeToShaman AndAlso
                   Me.DataContainer.AlwaysConcedeToWarlock AndAlso
                   Me.DataContainer.AlwaysConcedeToWarrior Then
                    Exit Sub
                End If

                Select Case Me.currentMode

                    Case Mode.Standard, Mode.Wild, Mode.Twist
                        AdvancedAutoConcedePlugin.rankedWinsCount = 0
                        Bot.Log($"[Advanced Auto Concede] -> (Mode: {Bot.CurrentMode}) Wins count resets to zero, reason: auto-concede.")

                    Case Mode.Casual
                        AdvancedAutoConcedePlugin.casualWinsCount = 0
                        Bot.Log("[Advanced Auto Concede] -> (Mode: Casual) Wins count resets to zero, reason: auto-concede.")

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
