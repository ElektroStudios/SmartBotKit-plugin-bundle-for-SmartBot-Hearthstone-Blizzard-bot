
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq

Imports HearthMirror.Objects
Imports HearthMirror.Objects.MatchInfo

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Bot

Imports SmartBotKit.Audio
Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
' Imports SmartBotKit.ReservedUse

#End Region

#Region " NotifyAFKPlugin "

' ReSharper disable once CheckNamespace

Namespace NotifyAFK

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that helps you find your favorite opponent match.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class NotifyAFKPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As NotifyAFKPluginData
            Get
                Return DirectCast(MyBase.DataContainer, NotifyAFKPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="NotifyAFKPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        Private turnsAFK As Integer

        Private turnsNotAFK As Integer

        Private BoardAfterMulligan As Board = Nothing
        Private ReadOnly mulliganStopWatch As New Stopwatch

        Private isBusy As Boolean = False
        Private mulliganCalculated As Boolean = True

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="NotifyAFKPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            ' UpdateUtil.RunUpdaterExecutable()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="NotifyAFKPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[NotifyAFK] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="NotifyAFKPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[NotifyAFK] -> Plugin enabled.")
                    'Me.RunPluginComatibilityCheck()
                Else
                    Bot.Log("[NotifyAFK] -> Plugin disabled.")
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
            If (Me.DataContainer.Enabled) Then
                Me.turnsAFK = 0
                Me.turnsNotAFK = 0
                Me.isBusy = False
            End If
            MyBase.OnStarted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStopped()
            If (Me.DataContainer.Enabled) Then
                Me.mulliganStopWatch.Reset()
                Me.turnsAFK = 0
                Me.turnsNotAFK = 0
                Me.isBusy = False
            End If
            MyBase.OnStopped()
        End Sub

        ''' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameBegin()
            If (Me.DataContainer.Enabled) AndAlso Bot.IsBotRunning Then

                Me.mulliganStopWatch.Reset()
                Me.turnsAFK = 0
                Me.turnsNotAFK = 0
                Me.BoardAfterMulligan = Nothing
                Me.mulliganCalculated = False
                Me.isBusy = False

                Dim battletags As ReadOnlyCollection(Of String) = Me.DataContainer.GetBattletags()
                If battletags?.Any() Then

                    Dim matchInfo As MatchInfo
                    Dim player As Player

                    Try
                        matchInfo = HearthMirror.Reflection.GetMatchInfo()
                        If matchInfo Is Nothing Then
                            Bot.Log($"[NotifyAFK] -> Error retrieving opponent BattleTag, {NameOf(matchInfo)}' is null. (HDT is outdated?)")
                        End If

                        player = matchInfo.OpposingPlayer
                        If player Is Nothing Then
                            Bot.Log($"[NotifyAFK] -> Error retrieving opponent BattleTag, {NameOf(matchInfo.OpposingPlayer)}' is null. (HDT is outdated?)")
                        End If

                        If battletags.Contains($"{player.BattleTag.Name}#{player.BattleTag.Number}".ToLower()) OrElse
                           battletags.Contains(player.BattleTag.Name.ToLower()) Then

                            If Not Me.isBusy Then
                                Me.isBusy = True
                                Bot.Log($"[NotifyAFK] -> Found AFK botter with Battletag name: '{player.BattleTag.Name}#{player.BattleTag.Number}'")
                                Me.Notify()
                            End If
                        End If

                    Catch ex As Exception
                        Bot.Log($"[NotifyAFK] -> Error retrieving opponent BattleTag: {ex.Message}")

                    End Try

                End If
            End If

            MyBase.OnGameBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameEnd()
            If (Me.DataContainer.Enabled) AndAlso Bot.IsBotRunning Then
                Me.mulliganStopWatch.Reset()
                Me.turnsAFK = 0
                Me.turnsNotAFK = 0
                Me.isBusy = False
            End If

            MyBase.OnGameEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game is conceded by our hero.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnConcede()
            If (Me.DataContainer.Enabled) AndAlso Bot.IsBotRunning Then
                Me.mulliganStopWatch.Reset()
                Me.turnsAFK = 0
                Me.turnsNotAFK = 0
            End If

            MyBase.OnConcede()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a turn begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTurnBegin()
            If (Me.DataContainer.Enabled) AndAlso Bot.IsBotRunning Then

                If Me.isBusy Then
                    Exit Sub
                End If

                Try
                    ' Bot.Log($"[NotifyAFK] -> Bot.CurrentBoard.TurnCount {Bot.CurrentBoard.TurnCount}")
                    If (Bot.CurrentBoard.TurnCount > 1) Then

                        Dim baseMinionDiedThisTurnFriend As Boolean = (Bot.CurrentBoard.BaseMinionDiedThisTurnFriend <> 0)
                        Dim baseMinionDiedThisTurnEnemy As Boolean = (Bot.CurrentBoard.BaseMinionDiedThisTurnEnemy <> 0)
                        Dim weaponEnemyIsUsed As Boolean = (Bot.CurrentBoard.WeaponEnemy IsNot Nothing) AndAlso (Bot.CurrentBoard.WeaponEnemy.IsUsed)
                        Dim enemyAbilityIsUsed As Boolean = (Bot.CurrentBoard.EnemyAbility IsNot Nothing) AndAlso (Bot.CurrentBoard.EnemyAbility.IsUsed)
                        Dim enemyHasMinions As Boolean = (Bot.CurrentBoard.MinionEnemy.Count <> 0)
                        Dim enemyHasSecrets As Boolean = Bot.CurrentBoard.SecretEnemy
                        Dim enemyCardDraw As Boolean = (Bot.CurrentBoard.EnemyCardDraw <> 0)
                        Dim enemyCardCount As Integer = Bot.CurrentBoard.EnemyCardCount

                        If Not baseMinionDiedThisTurnFriend AndAlso
                           Not baseMinionDiedThisTurnEnemy AndAlso
                           Not enemyAbilityIsUsed AndAlso
                           Not weaponEnemyIsUsed AndAlso
                           Not enemyHasMinions AndAlso
                           Not enemyHasMinions AndAlso
                           Not enemyCardDraw AndAlso
                           enemyCardCount > 4 Then

                            Me.turnsAFK += 1
                            Bot.Log($"[NotifyAFK] -> Opponent is AFK for {Me.turnsAFK} turns.")
                        Else

                            Me.turnsAFK = 0
                            Me.turnsNotAFK += 1
                        End If

                        If (Me.turnsNotAFK >= Me.DataContainer.TurnsNotAFK) Then
                            If Not Me.isBusy Then
                                Me.isBusy = True
                                Bot.Log($"[NotifyAFK] -> Conceding game because opponent was not AFK for {Me.turnsNotAFK} turns.")
                                Bot.Concede()
                            End If
                        End If

                        If (Me.turnsAFK >= Me.DataContainer.TurnsAFK) Then
                            If Not Me.isBusy Then
                                Me.isBusy = True
                                Me.Notify()
                            End If
                        End If

                    End If

                Catch ex As Exception

                End Try
            End If

            MyBase.OnTurnBegin()
        End Sub

        Public Overrides Sub OnTick()

            If (Me.DataContainer.Enabled) Then

                If Me.isBusy OrElse
                   Me.mulliganCalculated OrElse
                   Not Bot.IsBotRunning() OrElse
                   Bot.CurrentScene <> Scene.GAMEPLAY Then
                    Exit Sub
                End If

                If Me.BoardAfterMulligan Is Nothing Then
                    Try
                        Me.BoardAfterMulligan = Bot.CurrentBoard
                    Catch ex As Exception
                    End Try

                    If Me.BoardAfterMulligan IsNot Nothing AndAlso Me.mulliganStopWatch.IsRunning Then
                        Me.mulliganStopWatch.Stop()
                        ' Bot.Log($"[NotifyAFK] -> Mulligan took {CInt(Me.mulliganStopWatch.Elapsed.TotalSeconds)} seconds.")
                    End If

                Else
                    If (Me.BoardAfterMulligan.TurnCount > 0) Then
                        Exit Sub
                    End If

                    Dim mulliganSeconds As Integer = CInt(Me.mulliganStopWatch.Elapsed.TotalSeconds)
                    Me.mulliganCalculated = True

                    If mulliganSeconds < 60 AndAlso Me.DataContainer.ConcedeIfMulliganTakesLessThan60Seconds Then
                        If Not Me.isBusy Then
                            Me.isBusy = True
                            Bot.Log($"[NotifyAFK] -> Conceding game because mulligan took less than 60 seconds ({mulliganSeconds} seconds).")
                            Bot.Concede()
                        End If

                    ElseIf mulliganSeconds > 60 AndAlso Me.DataContainer.NotifyAFKIfMulliganTakesMoreThan60Seconds Then
                        If Not Me.isBusy Then
                            Me.isBusy = True
                            Bot.Log($"[NotifyAFK] -> Found possible AFK botter! (mulligan took {mulliganSeconds} seconds)'")
                            Me.Notify()
                        End If
                    End If

                End If

            End If

            MyBase.OnTick()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a turn ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTurnEnd()
            MyBase.OnTurnEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the opponent send us a emote.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="emoteType">
        ''' The emote received.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnReceivedEmote(emoteType As EmoteType)
            If (Me.DataContainer.Enabled) AndAlso Bot.IsBotRunning Then
                Me.turnsAFK = 0

                If Me.mulliganCalculated AndAlso Me.DataContainer.ConcedeIfEmoteReceived Then
                    If Not Me.isBusy Then
                        Me.isBusy = True
                        Bot.Log($"[NotifyAFK] -> Conceding game because an emote was received.")
                        Bot.Concede()
                    End If
                End If
            End If

            MyBase.OnReceivedEmote(emoteType)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is about to handle mulligan (to decide which card to replace) before a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="choices">
        ''' The mulligan choices.
        ''' </param>
        ''' 
        ''' <param name="opponentClass">
        ''' The opponent class.
        ''' </param>
        ''' 
        ''' <param name="ownClass">
        ''' Our hero class.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnHandleMulligan(choices As List(Of API.Card.Cards), opponentClass As API.Card.CClass, ownClass As API.Card.CClass)
            If (Me.DataContainer.Enabled) AndAlso Bot.IsBotRunning Then
                Me.mulliganStopWatch.Restart()
                Me.turnsAFK = 0
                Me.turnsNotAFK = 0
                Me.BoardAfterMulligan = Nothing
                Me.mulliganCalculated = False
                Me.isBusy = False
            End If
            MyBase.OnHandleMulligan(choices, opponentClass, ownClass)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="NotifyAFKPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Notify an AFK game.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub Notify()

            Me.turnsAFK = 0
            Me.turnsNotAFK = 0

            Bot.Log($"[NotifyAFK] -> Opponent is AFK for {Me.turnsAFK} turns!")
            Bot.StopBot()
            Bot.Log("[NotifyAFK] -> Bot stopped. You can play the game now.")

            If (Me.DataContainer.PlaySoundFile) Then
                Dim file As New FileInfo(Path.Combine(SmartBotUtil.PluginsDir.FullName, "NotifyAFK.mp3"))
                If Not file.Exists() Then
                    Bot.Log($"[NotifyAFK] -> Sound file not found: '{file.FullName}'")
                Else
                    AudioUtil.PlaySoundFile(file)
                End If
            End If

            If Me.DataContainer.ActivateWindow Then
                Dim p As Process = HearthstoneUtil.Process
                If (p Is Nothing) OrElse (p.HasExited) Then
                    Exit Sub
                End If

                NativeMethods.SetForegroundWindow(p.MainWindowHandle)
                NativeMethods.BringWindowToTop(p.MainWindowHandle)
                Interaction.AppActivate(p.Id) ' Double ensure the window get visible and focused, because I experienced sometimes it don't get.
            End If

        End Sub

#End Region

    End Class

End Namespace

#End Region
