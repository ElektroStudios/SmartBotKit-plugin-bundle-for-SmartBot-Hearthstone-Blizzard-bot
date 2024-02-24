
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Card

Imports SmartBotKit.Extensions.IListExtensions
Imports SmartBotKit.Extensions.TimeSpanExtensions
Imports SmartBotKit.Interop
Imports SmartBotKit.ReservedUse

#End Region

#Region " BountyHunterPlugin "

' ReSharper disable once CheckNamespace

Namespace BountyHunter

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that completes quests, schedules ranked mode and level up heroes.
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

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="BountyHunterPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the decks selected before started to questing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastNormalSelectedDeckNames As IEnumerable(Of String) = Nothing

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
        ''' Keeps track of the current <see cref="BountyHunterMode"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private currentBountyHunterMode As BountyHunterMode = BountyHunterMode.None

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the unfulfillable quest types.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private unfulfillableQuestTypes As List(Of Quest.QuestType)

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the quest types to be kept/don't rerolled.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private questTypesToKeep As List(Of Quest.QuestType)

        ' ReSharper restore InconsistentNaming

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
                Bot.Log("[Bounty Hunter] -> Plugin initialized.")
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
                    Bot.Log("[Bounty Hunter] -> Plugin enabled.")
                Else
                    Me.RestoreNormalSettings()
                    Bot.Log("[Bounty Hunter] -> Plugin disabled.")
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
                Me.lastNormalBotMode = Bot.CurrentMode()
                Me.lastNormalSelectedDeckNames = (From deck As Deck In Bot.GetSelectedDecks() Select deck.Name)
                Me.lastNormalDeckName = Bot.CurrentDeck().Name
                Me.ChooseNextMode()
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
                Me.RestoreNormalSettings()
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
                Me.ChooseNextMode()
            End If
            MyBase.OnGameEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot timer is ticked, every 300 milliseconds.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTick()

            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.EnableLadderScheduler) AndAlso
               (Me.DataContainer.LadderModeAutoStart OrElse Me.DataContainer.LadderModeAutoStop) Then

                Dim now As TimeSpan = Date.Now.TimeOfDay
                Dim isLadderTime As Boolean = now.IsHourInRange(Me.DataContainer.LadderStartHour, Me.DataContainer.LadderEndHour)

                If (isLadderTime) AndAlso (Me.currentBountyHunterMode <> BountyHunterMode.Ladder) Then
                    Me.ActivateMode(BountyHunterMode.Ladder)

                ElseIf Not (isLadderTime) AndAlso (Me.currentBountyHunterMode = BountyHunterMode.Ladder) Then
                    Me.currentBountyHunterMode = BountyHunterMode.None
                    If (Me.DataContainer.LadderModeAutoStop) AndAlso (Bot.IsBotRunning()) Then
                        Bot.Log("[Bounty Hunter] -> Ladder Mode, auto-finishing bot...")
                        Bot.Finish()
                        Me.RestoreNormalSettings()
                    End If

                End If

            End If

            MyBase.OnTick()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="BountyHunterPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            If (Me.DataContainer.Enabled) Then
                Me.RestoreNormalSettings()
            End If
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Builds the <see cref="List(Of Quest.QuestType)"/> specified in <see cref="BountyHunterPlugin.questTypesToKeep"/> 
        ''' with the quest types to be kept/don't rerolled.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub BuildQuestTypesToKeep()

            ' *************************
            ' Full updated quests list:
            ' *************************
            ' https://hearthstone.gamepedia.com/Quest#List

            Me.questTypesToKeep = New List(Of Quest.QuestType)

            ' Dungeon quests.
            If (Me.DataContainer.KeepDungeonQuests) Then
                Me.questTypesToKeep.Add(Quest.QuestType.Crimewave)
                Me.questTypesToKeep.Add(Quest.QuestType.PlagueBusting)
                Me.questTypesToKeep.Add(Quest.QuestType.RisetoGlory)
            End If

            ' Play a friend, you both earn a reward!.
            If (Me.DataContainer.KeepQuestPlayAFriend) Then
                Me.questTypesToKeep.Add(Quest.QuestType.ChallengeaFriend)
            End If

            ' Watch a friend win in spectator mode.
            If (Me.DataContainer.KeepQuestWatchAndLearn) Then
                Me.questTypesToKeep.Add(Quest.QuestType.WatchandLearn)
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Builds the <see cref="List(Of Quest.QuestType)"/> specified in <see cref="BountyHunterPlugin.unfulfillableQuestTypes"/> 
        ''' with the quest types to reroll.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub BuildUnfulfillableQuestTypes()

            Me.unfulfillableQuestTypes = New List(Of Quest.QuestType) From {
                Quest.QuestType.EverybodyGetinHere, ' Win 3 Tavern Brawls.
                Quest.QuestType.BigBossBattle ' Play 2 games of Battlegrounds.
            }.Concat(From quest As Quest In Bot.GetQuests()
                     Where Not [Enum].IsDefined(GetType(Quest.QuestType), quest.Id) ' Workaround to include undefined and unsupported quests like The Witchwood Monster Hunt quests: https://hearthstone.gamepedia.com/Quest#The_Witchwood
                     Select quest.GetQuestType()
                    ).ToList()

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Chooses the next <see cref="BountyHunterMode"/> mode to begin playing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub ChooseNextMode()

            Dim isNextModeChoosed As Boolean

            If (Me.DataContainer.EnableLadderScheduler) Then
                Dim now As TimeSpan = Date.Now.TimeOfDay
                Dim isLadderTime As Boolean = now.IsHourInRange(Me.DataContainer.LadderStartHour, Me.DataContainer.LadderEndHour)

                If (isLadderTime) Then
                    isNextModeChoosed = Me.ActivateMode(BountyHunterMode.Ladder)

                ElseIf Not (isLadderTime) AndAlso (Me.currentBountyHunterMode = BountyHunterMode.Ladder) Then
                    Me.currentBountyHunterMode = BountyHunterMode.None
                    If (Me.DataContainer.LadderModeAutoStop) AndAlso (Bot.IsBotRunning()) Then
                        Bot.Log("[Bounty Hunter] -> Ladder Scheduler Mode, auto-finishing bot...")
                        Bot.Finish()
                        Me.RestoreNormalSettings()
                    End If

                End If

            End If

            If (Me.DataContainer.EnableQuestCompletion) AndAlso Not (isNextModeChoosed) Then
                isNextModeChoosed = Me.ActivateMode(BountyHunterMode.Questing)
            End If

            If (Me.DataContainer.EnableHeroLevelling) AndAlso Not (isNextModeChoosed) Then
                isNextModeChoosed = Me.ActivateMode(BountyHunterMode.HeroLevelling)
            End If

            If (Me.DataContainer.EnableRankedWinsCount) AndAlso Not (isNextModeChoosed) Then
#Disable Warning IDE0059 ' Unnecessary assignment of a value
                isNextModeChoosed = Me.ActivateMode(BountyHunterMode.RankedWinsCount)
#Enable Warning IDE0059 ' Unnecessary assignment of a value
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Activates the specified <see cref="BountyHunterMode"/> to begin playing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the specified mode is successfully activated, <see langword="False"/> otherwise.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function ActivateMode(ByVal mode As BountyHunterMode) As Boolean

            Select Case mode

                Case BountyHunterMode.Questing
                    Me.RerollQuests()

                    Dim quests As List(Of Quest) =
                        If(Me.DataContainer.Reroll50And60GoldQuests,
                        Bot.GetQuests().FindAll(Function(quest As Quest) (quest.GetGoldReward() > 60) AndAlso Not Me.questTypesToKeep.Contains(quest.GetQuestType())),
                        Bot.GetQuests().FindAll(Function(quest As Quest) Not Me.questTypesToKeep.Contains(quest.GetQuestType())))

                    Dim questToDo As Quest = quests.FirstOrDefault(Function(quest As Quest) Me.GetBestDeckForQuest(quest) IsNot Nothing)
                    If (questToDo IsNot Nothing) Then
                        Dim newDeck As Deck = Me.GetBestDeckForQuest(questToDo)
                        Dim botMode As Bot.Mode = Me.DataContainer.QuestMode
                        Bot.Log($"[Bounty Hunter] -> Daily Quests Mode | '{questToDo.Name}' | {botMode} | {newDeck.Name}")

                        SmartBotUtil.SafeChangeDeckOrMode(botMode, newDeck.Name)
                        ' Bot.ChangeMode(botMode)
                        ' Bot.ChangeDeck(newDeck.Name)
                        Me.currentBountyHunterMode = mode
                        Return True

                    Else
                        Me.RestoreNormalSettings()
                        Return False

                    End If

                Case BountyHunterMode.HeroLevelling
                    For Each heroClass As CClass In [Enum].GetValues(GetType(CClass))

                        Dim currentLevel As Integer = Bot.GetPlayerDatas.GetLevel(heroClass)
                        Dim targetLevel As Integer = Me.DataContainer.TargetHeroLevel(heroClass)

                        If (currentLevel < targetLevel) Then
                            Dim botMode As Bot.Mode = Me.DataContainer.LevelMode
                            Dim preferredDeck As Deck = Me.DataContainer.PreferredDeck(heroClass)

                            If (preferredDeck IsNot Nothing) Then
                                Bot.Log(
                                    $"[Bounty Hunter] -> Hero Levelling Mode | {heroClass} ({currentLevel} of { _
                                           targetLevel} levels) | {botMode} | {preferredDeck.Name}")
                                SmartBotUtil.SafeChangeDeckOrMode(botMode, preferredDeck.Name)
                                ' Bot.ChangeMode(botMode)
                                ' Bot.ChangeDeck(preferredDeck.Name)
                                Me.currentBountyHunterMode = mode
                                Return True

                            Else
                                Bot.Log(
                                    $"[Bounty Hunter] -> Hero Levelling Mode | No available deck for class: {heroClass _
                                           }. Switching to next class...")

                            End If
                        End If

                    Next
                    Me.RestoreNormalSettings()
                    Return False

                Case BountyHunterMode.RankedWinsCount

                    For Each heroClass As CClass In [Enum].GetValues(GetType(CClass))

                        Dim currentWins As Integer = HearthstoneUtil.GetHeroWins(heroClass)
                        Dim targetWins As Integer = Me.DataContainer.TargetRankedWinsCount(heroClass)

                        If (currentWins < targetWins) Then
                            Dim botMode As Bot.Mode = Me.DataContainer.RankedWinsCountMode
                            Dim preferredDeck As Deck = Me.DataContainer.PreferredDeck(heroClass)

                            If (preferredDeck IsNot Nothing) Then
                                Bot.Log(
                                    $"[Bounty Hunter] -> Ranked Wins Count Mode | {heroClass} ({currentWins} of { _
                                           targetWins} wins) | {botMode} | {preferredDeck.Name}")
                                SmartBotUtil.SafeChangeDeckOrMode(botMode, preferredDeck.Name)
                                ' Bot.ChangeMode(botMode)
                                ' Bot.ChangeDeck(preferredDeck.Name)
                                Me.currentBountyHunterMode = mode
                                Return True

                            Else
                                Bot.Log(
                                    $"[Bounty Hunter] -> Ranked Wins Count Mode | No available deck for class: {heroClass _
                                           }. Switching to next class...")

                            End If
                        End If

                    Next
                    Me.RestoreNormalSettings()
                    Return False

                Case BountyHunterMode.Ladder
                    Me.currentBountyHunterMode = mode
                    Dim botMode As Bot.Mode = Me.DataContainer.LadderMode
                    Dim deck As Deck = Bot.GetDecks().FirstOrDefault(Function(d As Deck) (d.Name = Me.DataContainer.LadderPreferredDeck) AndAlso (d.IsValid()))

                    If (deck Is Nothing) AndAlso (Me.DataContainer.LadderUseRandomDeckIfPreferredIsUnavailable) Then
                        deck = Me.GetRandomDeck(botMode)
                    End If

                    If (deck IsNot Nothing) Then
                        Bot.Log($"[Bounty Hunter] -> Ladder Scheduler Mode | {botMode} | {deck.Name}")
                        SmartBotUtil.SafeChangeDeckOrMode(botMode, deck.Name)
                        ' Bot.ChangeMode(botMode)
                        ' Bot.ChangeDeck(deck.Name)
                        If (Me.DataContainer.LadderModeAutoStart) AndAlso Not (Bot.IsBotRunning()) Then
                            Bot.Log("[Bounty Hunter] -> Ladder Scheduler Mode, auto-starting bot...")
                            Bot.StartBot()
                        End If
                        Return True

                    Else
                        Bot.Log("[Bounty Hunter] -> Ladder Scheduler Mode | No available deck.")
                        Me.RestoreNormalSettings()
                        Return False

                    End If

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(mode), mode, GetType(BountyHunterMode))

            End Select

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Rerolls unfulfillable quests.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub RerollQuests()

            Me.BuildQuestTypesToKeep()
            Me.BuildUnfulfillableQuestTypes()

            For Each quest As Quest In Bot.GetQuests()
                Dim goldReward As Integer = quest.GetGoldReward()

                If (Me.unfulfillableQuestTypes.Contains(quest.GetQuestType())) OrElse
                   ((goldReward = 50 OrElse goldReward = 60) AndAlso Me.DataContainer.Reroll50And60GoldQuests) Then

                    If (Bot.CanCancelQuest()) Then
                        Bot.Log($"[Bounty Hunter] -> {quest.GetGoldReward()} Gold Quest Reroll: {quest.Name}")
                        Bot.CancelQuest(quest)
                    End If

                End If

            Next quest

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Reverts bot mode and deck to normal settings.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub RestoreNormalSettings()

            If String.IsNullOrEmpty(Me.lastNormalDeckName) AndAlso (Me.lastNormalBotMode = Bot.Mode.None) Then
                Exit Sub
            End If

            If (Bot.CurrentMode <> Me.lastNormalBotMode) OrElse
               (Bot.CurrentDeck?.Name <> Me.lastNormalDeckName) OrElse
               (Bot.GetSelectedDecks?.Count <> Me.lastNormalSelectedDeckNames?.Count) Then

                SmartBotUtil.SafeChangeDeckOrMode(Me.lastNormalBotMode, Me.lastNormalDeckName)
                ' Bot.ChangeMode(Me.lastNormalBotMode)
                ' Bot.ChangeDeck(Me.lastNormalDeckName)
                Me.lastNormalBotMode = Bot.CurrentMode
                Me.lastNormalDeckName = Bot.CurrentDeck?.Name

                Bot.Log(
                    $"[Bounty Hunter] -> Reverted to normal settings: {Me.lastNormalBotMode} | {Me.lastNormalDeckName}")
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the count of available decks for the specified <see cref="Quest"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="quest">
        ''' The quest.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The count of available decks for the specified <see cref="Quest"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetDecksCountForQuest(ByVal quest As Quest) As Integer

            If (quest.GetClassRequired().Count = 0) Then
                Return Me.GetQualifiedDecksForQuestMode(Me.DataContainer.QuestMode).AsEnumerable().Count()

            Else
                Return Me.GetQualifiedDecksForQuestMode(Me.DataContainer.QuestMode).AsEnumerable().Count(Function(deck As Deck) quest.IsDeckQualified(deck))

            End If

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the best available deck for the specified <see cref="Quest"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="quest">
        ''' The quest.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The best available deck for the specified <see cref="Quest"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetBestDeckForQuest(ByVal quest As Quest) As Deck

            If Not (Me.unfulfillableQuestTypes.Contains(quest.GetQuestType())) AndAlso (quest.GetClassRequired().Count = 1) Then
                If Me.HasAvailableDeckForClass(quest.GetClassRequired()(0), Me.DataContainer.QuestMode) Then
                    Return Me.GetBestDeckForClass(quest.GetClassRequired()(0))
                End If

            ElseIf Not (Me.unfulfillableQuestTypes.Contains(quest.GetQuestType())) AndAlso (quest.GetClassRequired().Count > 1) Then
                Dim bestClass As CClass = quest.GetClassRequired()(0)
                Dim worstClass As CClass = quest.GetClassRequired().Find(Function(heroClass As CClass) heroClass <> bestClass)
                If Me.HasAvailableDeckForClass(bestClass, Me.DataContainer.QuestMode) Then
                    Return Me.GetBestDeckForClass(bestClass)
                End If
                If Me.HasAvailableDeckForClass(worstClass, Me.DataContainer.QuestMode) Then
                    Return Me.GetBestDeckForClass(worstClass)
                End If

            ElseIf Not (Me.unfulfillableQuestTypes.Contains(quest.GetQuestType())) AndAlso (Me.HasAvailableDeckForQuest(quest)) Then
                Dim decks As List(Of Deck) = Nothing
                If (quest.GetClassRequired().Count = 0) Then
                    decks = Me.GetQualifiedDecksForQuestMode(Me.DataContainer.QuestMode).FindAll(Function(deck As Deck) deck.IsValid())
                Else
                    decks = Me.GetQualifiedDecksForQuestMode(Me.DataContainer.QuestMode).FindAll(Function(deck As Deck) quest.IsDeckQualified(deck))
                End If

                If (decks.Count > 0) Then
                    Dim preferredDecks As List(Of Deck) = decks.FindAll(Function(deck As Deck) Me.IsPreferredDeck(deck.Name) AndAlso quest.GetMostQualifiedDeck(decks).Equals(deck))
                    If (preferredDecks.Count > 0) Then
                        Return preferredDecks.Randomize().First()
                    End If

                    Dim mostQualified As Deck = quest.GetMostQualifiedDeck(decks)
                    Dim highQualifiedDecks As List(Of Deck) = decks.FindAll(Function(deck As Deck) quest.GetQualifiedCardsCount(deck) >= quest.GetQualifiedCardsCount(mostQualified))
                    Return highQualifiedDecks.Randomize().FirstOrDefault()
                End If
            End If

            Bot.Log($"[Bounty Hunter] -> Questing Mode | No available deck for quest: '{quest.Name}', skipping it...")

            If (Me.DataContainer.RerollUnfulfillableQuests) Then
                If Bot.CanCancelQuest() Then
                    Bot.CancelQuest(quest)
                End If
            End If

            Return Nothing
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the best available deck for the specified <see cref="CClass"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="heroClass">
        ''' The hero class.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The best available deck for the specified <see cref="CClass"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetBestDeckForClass(ByVal heroClass As CClass) As Deck

            If Me.HasPreferredDeckForClass(heroClass) Then
                Return Me.GetPreferredDeckForClass(heroClass)
            End If

            If Me.HasAvailableDeckForClass(heroClass, Me.DataContainer.QuestMode) Then
                Return Me.GetQualifiedDecksForQuestMode(Me.DataContainer.QuestMode).Find(Function(deck As Deck) deck.Class = heroClass)
            End If

            Return Nothing
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determine whether the specified deck is a preferred deck.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="deckName">
        ''' The name of the deck to check.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the specified deck is a preferred deck; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function IsPreferredDeck(ByVal deckName As String) As Boolean

            For Each heroClass As CClass In [Enum].GetValues(GetType(CClass))
                If (deckName = Me.DataContainer.PreferredDeck(heroClass)?.Name) Then
                    Return True
                End If
            Next heroClass

            Return False

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the preferred deck for the specified <see cref="CClass"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="heroClass">
        ''' The hero class.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The preferred deck for the specified <see cref="CClass"/>, 
        ''' or <see langword="Nothing"/> if no preferred deck is available.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetPreferredDeckForClass(ByVal heroClass As CClass) As Deck

            Return Me.GetQualifiedDecksForQuestMode(Me.DataContainer.QuestMode).
                      Find(Function(deck As Deck) deck.Name = Me.DataContainer.PreferredDeck(heroClass)?.Name)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determine whether the specified <see cref="CClass"/> has any available deck for the specified <see cref="Bot.Mode"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="heroClass">
        ''' The hero class.
        ''' </param>
        ''' 
        ''' <param name="botMode">
        ''' The bot mode.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the specified <see cref="CClass"/> has 
        ''' any available deck for the specified <see cref="Bot.Mode"/>; 
        ''' otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function HasAvailableDeckForClass(ByVal heroClass As CClass, ByVal botMode As Bot.Mode) As Boolean

            Return Me.GetQualifiedDecksForQuestMode(botMode).Any(Function(deck As Deck) deck.Class = heroClass)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determine whether there is any available deck for the specified <see cref="Quest"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="quest">
        ''' The quest.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if there is any available deck for the specified <see cref="Quest"/>.; 
        ''' otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function HasAvailableDeckForQuest(ByVal quest As Quest) As Boolean

            Return (Me.GetDecksCountForQuest(quest) <> 0)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determine whether there is a preferred deck available for the specified <see cref="CClass"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="heroClass">
        ''' The hero class.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if there is a preferred deck available for the specified <see cref="CClass"/>; 
        ''' otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function HasPreferredDeckForClass(ByVal heroClass As CClass) As Boolean
            Return Me.GetPreferredDeckForClass(heroClass) IsNot Nothing
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets all the available qualified decks for the specified <see cref="Bot.Mode"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="botMode">
        ''' The bot mode.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The available qualified decks for the specified <see cref="Bot.Mode"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetQualifiedDecksForQuestMode(ByVal botMode As Bot.Mode) As List(Of Deck)

            Select Case botMode

                Case Bot.Mode.RankedStandard, Bot.Mode.UnrankedStandard
                    Return Bot.GetDecks().FindAll(Function(deck As Deck) (deck.Type = Deck.DeckType.Standard) AndAlso (deck.IsValid()))

                Case Else ' Wild, Practice, Arena
                    Return Bot.GetDecks().FindAll(Function(deck As Deck) deck.IsValid())

            End Select

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a random deck for the specified <see cref="Quest"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="botMode">
        ''' The bot mode.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Deck"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetRandomDeck(ByVal botMode As Bot.Mode) As Deck

            Select Case botMode

                Case Bot.Mode.RankedStandard, Bot.Mode.UnrankedStandard
                    Return Bot.GetDecks().FindAll(Function(deck As Deck) (deck.Type = Deck.DeckType.Standard) AndAlso (deck.IsValid()))?.Randomize().FirstOrDefault()

                Case Bot.Mode.RankedWild, Bot.Mode.UnrankedWild
                    Return Bot.GetDecks().FindAll(Function(deck As Deck) (deck.Type = Deck.DeckType.Wild) AndAlso (deck.IsValid()))?.Randomize().FirstOrDefault()

                Case Bot.Mode.Practice
                    Return Bot.GetDecks().FindAll(Function(deck As Deck) (deck.IsValid()))?.Randomize().FirstOrDefault()

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(botMode), botMode, GetType(Bot.Mode))

            End Select

        End Function

#End Region

    End Class

End Namespace

#End Region
