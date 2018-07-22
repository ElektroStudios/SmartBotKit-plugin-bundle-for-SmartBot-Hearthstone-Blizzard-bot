
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Card

Imports SmartBotKit.ReservedUse

#End Region

#Region " BountyHunterPlugin "

Namespace BountyHunter

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that complete quests and level up hero classes.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class BountyHunterPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As BountyHunterPluginData
            Get
                Return DirectCast(MyBase.DataContainer, BountyHunterPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="BountyHunterPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last deck name used before started to questing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastNormalDeckName As String = String.Empty

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last bot mode used before started to questing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastNormalBotMode As Bot.Mode = Bot.Mode.None

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private hadQuest As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="BountyHunterPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            UpdateUtil.RunUpdaterExecutable()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="BountyHunterPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Ultimate Questing] Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="BountyHunterPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Ultimate Questing] Plugin enabled.")
                Else
                    Bot.Log("[Ultimate Questing] Plugin disabled.")
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
            If Me.DataContainer.Enabled Then
                Bot.StopBot()

                Me.lastNormalBotMode = Bot.CurrentMode()
                Me.lastNormalDeckName = Bot.CurrentDeck().Name
                Me.hadQuest = False
                Me.RerollUnfulfillableQuests()
                Me.ChooseNewDeck()

                Bot.StartBot()
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
                If (Me.hadQuest) Then
                    Me.RevertToNormalMode()
                End If
            End If
            MyBase.OnStopped()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameEnd()
            If (Me.DataContainer.Enabled) Then
                If (Me.DataContainer.Reroll50GoldQuests) Then
                    Me.RerollUnfulfillableQuests()
                End If

                Me.ChooseNewDeck()
            End If
            MyBase.OnGameEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="BountyHunterPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Reverts bot mode and deck to normal.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub RevertToNormalMode()
            Bot.ChangeMode(Me.lastNormalBotMode)
            Bot.ChangeDeck(Me.lastNormalDeckName)
            Bot.Log(String.Format("[Ultimate Questing] Questing finished, reverted to normal settings. Mode: {0}, Deck: {1}",
                                  Me.lastNormalBotMode, Me.lastNormalDeckName))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Rerolls unfulfillable quests.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub RerollUnfulfillableQuests()

            Dim unfulfillableQuestTypes As New List(Of Quest.QuestType) From {
                Quest.QuestType.CatchaBigOne, ' Defeat 3 Monster Hunt Bosses. 
                Quest.QuestType.EverybodyGetinhere, ' Win 3 Tavern Brawls. 
                Quest.QuestType.Spelunker ' Defeat 3 Dungeon Run Bosses. 
            }

            If Not (Me.DataContainer.KeepPlayAFriendQuest) Then
                unfulfillableQuestTypes.Add(Quest.QuestType.ChallengeaFriend) ' Play a friend, you both earn a reward!
            End If

            If Not (Me.DataContainer.KeepWatchAndLearnQuest) Then
                unfulfillableQuestTypes.Add(Quest.QuestType.WatchandLearn) ' Watch a friend win in spectator mode.
            End If

            For Each quest As Quest In Bot.GetQuests()

                If (unfulfillableQuestTypes.Contains(quest.GetQuestType())) OrElse
                   (quest.GetGoldReward() = 50 AndAlso Me.DataContainer.Reroll50GoldQuests) Then

                    If (Bot.CanCancelQuest()) Then
                        Bot.Log(String.Format("[Ultimate Questing] {0} Gold Quest Reroll: {1}", quest.GetGoldReward(), quest.Name))
                        Bot.CancelQuest(quest)
                    End If
                End If

            Next quest

        End Sub

        'Private Sub ffffffffffff()

        '    Thread.Sleep(TimeSpan.FromSeconds(2))

        '    Do Until (Me.queue.Count = 0)

        '        Dim currentClass As CClass = Me.queue.Dequeue
        '        If (currentClass = CClass.NONE) Then
        '            Continue Do
        '        End If

        '        Dim targetLevel As Integer = Me.DataContainer.TargetHeroLevel(currentClass)
        '        Dim currentLevel As Integer = Bot.GetPlayerDatas.GetLevel(currentClass)
        '        Bot.Log(String.Format("[Ultimate Questing] Class: {0} Level: {1}", currentClass.ToString(), currentLevel))
        '        If (currentLevel >= targetLevel) Then
        '            Bot.Log(String.Format("[Ultimate Questing] Level reached for class: {0}. Changing to next class...", currentClass.ToString()))
        '            Continue Do
        '        End If

        '        Dim preferedDeck As Deck = Me.DataContainer.PreferedDeck(currentClass)
        '        If (preferedDeck IsNot Nothing) Then
        '            API.Bot.ChangeDeck(preferedDeck.Name)
        '        Else
        '            Bot.Log(String.Format("[Ultimate Questing] No available decks for class: {0}. Changing to next class...", currentClass.ToString()))
        '            Continue Do
        '        End If

        '        Bot.ChangeMode(Me.DataContainer.BotMode)
        '        Bot.ResumeBot()

        '        Do Until Bot.GetPlayerDatas.GetLevel(currentClass) >= targetLevel
        '            Thread.Sleep(TimeSpan.FromSeconds(60))
        '        Loop
        '        Bot.Log(String.Format("[Ultimate Questing] Level reached for class: {0}. Changing to next class...", currentClass.ToString()))
        '        Bot.Concede()
        '        Bot.SuspendBot()
        '        Thread.Sleep(TimeSpan.FromSeconds(2))


        '    Loop



        'End Sub

#End Region

    End Class

End Namespace

#End Region
