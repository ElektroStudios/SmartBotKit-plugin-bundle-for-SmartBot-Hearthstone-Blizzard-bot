
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Card
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Extensions.IEnumerableExtensions

#End Region

#Region " BountyHunterPluginData "

Namespace BountyHunter

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="BountyHunterPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class BountyHunterPluginData : Inherits PluginDataContainer

#Region " Properties "

#Region " Plugin "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="ApplicationServices.AssemblyInfo"/> for this assembly.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Friend ReadOnly Property AssemblyInfo As AssemblyInfo
            Get
                Return New AssemblyInfo(Assembly.GetExecutingAssembly())
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the assembly name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Assembly Name")>
        Public ReadOnly Property AssemblyName As String
            Get
                Return Path.ChangeExtension(Me.AssemblyInfo.AssemblyName, "dll")
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin description.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Description")>
        Public ReadOnly Property Description As String
            Get
                Return "Completes quests, schedules ranked mode" & ControlChars.NewLine &
                       "and level up heroes."
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete log files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Version")>
        Public ReadOnly Property Version As String
            Get
                Return Me.AssemblyInfo.Version.ToString()
            End Get
        End Property

        <Browsable(False)>
        Public Shadows Property Name As String

#End Region

#Region " Quests Completion "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should do quests.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Quests Completion")>
        <DisplayName("* Enable quests completion")>
        <Browsable(True)>
        Public Property EnableQuestCompletion As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the bot mode for questing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Quests Completion")>
        <DisplayName("Bot mode")>
        <Browsable(True)>
        <ItemsSource(GetType(QuestModeSource))>
        Public Property QuestMode As Bot.Mode

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether 50 gold quests should be reroll.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Quests Completion")>
        <DisplayName("Reroll 50 gold quests")>
        <Browsable(True)>
        Public Property Reroll50GoldQuests As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether unfulfillable quests should be reroll.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Quests Completion")>
        <DisplayName("Reroll unfulfillable quests")>
        <Browsable(True)>
        Public Property RerollUnfulfillableQuests As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether "Watch and Learn!" quest should be kept.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Quests Completion")>
        <DisplayName("Keep 'Watch and Learn!' quest")>
        <Browsable(True)>
        Public Property KeepQuestWatchAndLearn As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether "Play a Friend!" quest should be kept.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Quests Completion")>
        <DisplayName("Keep 'Play a Friend!' quest")>
        <Browsable(True)>
        Public Property KeepQuestPlayAFriend As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether "Catch a Big One!" quest should be kept.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Quests Completion")>
        <DisplayName("Keep 'Catch a Big One!' quest")>
        <Browsable(True)>
        Public Property KeepQuestCatchABigOne As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether "Spelunker!" quest should be kept.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Quests Completion")>
        <DisplayName("Keep 'Spelunker!' quest")>
        <Browsable(True)>
        Public Property KeepQuestSpelunker As Boolean

#End Region

#Region " Hero Levelling "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should level up heroes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("* Enable Hero Class Level Completion")>
        <Browsable(True)>
        Public Property EnableHeroLevelling As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the bot mode for levelling.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Bot Mode")>
        <Browsable(True)>
        <ItemsSource(GetType(LevelModeSource))>
        Public Property LevelMode As Bot.Mode

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Mage.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Mage")>
        <Browsable(True)>
        Public Property LvlMage As Integer
            Get
                Return Me.lvlMageB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlMageB = 1
                ElseIf (value > 60) Then
                    Me.lvlMageB = 60
                Else
                    Me.lvlMageB = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Mage.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlMageB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Priest.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Priest")>
        <Browsable(True)>
        Public Property LvlPriest As Integer
            Get
                Return Me.lvlPriestB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlPriestB = 1
                ElseIf (value > 60) Then
                    Me.lvlPriestB = 60
                Else
                    Me.lvlPriestB = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Priest.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlPriestB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Warrior.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Warrior")>
        <Browsable(True)>
        Public Property LvlWarrior As Integer
            Get
                Return Me.lvlWarriorB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlWarriorB = 1
                ElseIf (value > 60) Then
                    Me.lvlWarriorB = 60
                Else
                    Me.lvlWarriorB = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Warrior.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlWarriorB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Warlock.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Warlock")>
        <Browsable(True)>
        Public Property LvlWarlock As Integer
            Get
                Return Me.lvlWarlockB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlWarlockB = 1
                ElseIf (value > 60) Then
                    Me.lvlWarlockB = 60
                Else
                    Me.lvlWarlockB = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Warlock.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlWarlockB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Rogue.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Rogue")>
        <Browsable(True)>
        Public Property LvlRogue As Integer
            Get
                Return Me.lvlRogueB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlRogueB = 1
                ElseIf (value > 60) Then
                    Me.lvlRogueB = 60
                Else
                    Me.lvlRogueB = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Rogue.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlRogueB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Druid.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Druid")>
        <Browsable(True)>
        Public Property LvlDruid As Integer
            Get
                Return Me.lvlDruidB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlDruidB = 1
                ElseIf (value > 60) Then
                    Me.lvlDruidB = 60
                Else
                    Me.lvlDruidB = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Druid.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlDruidB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Hunter.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Hunter")>
        <Browsable(True)>
        Public Property LvlHunter As Integer
            Get
                Return Me.lvlHunterB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlHunterB = 1
                ElseIf (value > 60) Then
                    Me.lvlHunterB = 60
                Else
                    Me.lvlHunterB = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Hunter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlHunterB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Shaman.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Shaman")>
        <Browsable(True)>
        Public Property LvlShaman As Integer
            Get
                Return Me.lvlShamanB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlShamanB = 1
                ElseIf (value > 60) Then
                    Me.lvlShamanB = 60
                Else
                    Me.lvlShamanB = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Shaman.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlShamanB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Paladin.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Paladin")>
        <Browsable(True)>
        Public Property LvlPaladin As Integer
            Get
                Return Me.lvlPaladinB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlPaladinB = 1
                ElseIf (value > 60) Then
                    Me.lvlPaladinB = 60
                Else
                    Me.lvlPaladinB = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Paladin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlPaladinB As Integer

#End Region

#Region " Ladder Scheduler "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the hour to start playing ladder.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ladder Scheduler")>
        <DisplayName("* Enable Ladder Scheduler" & ControlChars.NewLine & ControlChars.NewLine &
                     "If enabled, between the specified hours " & ControlChars.NewLine &
                     "the plugin will ignore questing and levelling " & ControlChars.NewLine &
                     "and it will only play ranked mode.")>
        <Browsable(True)>
        Public Property EnableLadderScheduler As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the ranked mode to play ladder.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ladder Scheduler")>
        <DisplayName("Bot Mode")>
        <Browsable(True)>
        <ItemsSource(GetType(LadderModeSource))>
        Public Property LadderMode As Bot.Mode

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for playing ladder.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ladder Scheduler")>
        <DisplayName("Preferred Deck")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property LadderPreferredDeck As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the bot should try to use a random available deck if 
        ''' the specified preferred deck in <see cref="BountyHunterPluginData.LadderPreferredDeck"/> is unavailable.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ladder Scheduler")>
        <DisplayName("Try a random available deck if preferred deck is unavailable")>
        <Browsable(True)>
        Public Property LadderUseRandomDeckIfPreferredIsUnavailable As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the hour to start playing ladder.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ladder Scheduler")>
        <DisplayName("Hour Begin")>
        <Browsable(True)>
        Public Property LadderStartHour As TimeSpan
            Get
                Return Me.ladderStartHourB
            End Get
            Set(value As TimeSpan)
                If (value < TimeSpan.Zero) Then
                    value = TimeSpan.Zero
                End If
                If (value >= TimeSpan.FromHours(24)) Then
                    value = TimeSpan.FromHours(24).Add(TimeSpan.FromSeconds(-1))
                End If
                Me.ladderStartHourB = value
            End Set
        End Property
        Private ladderStartHourB As TimeSpan

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the hour to end playing ladder.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ladder Scheduler")>
        <DisplayName("Hour Finish")>
        <Browsable(True)>
        Public Property LadderEndHour As TimeSpan
            Get
                Return Me.ladderEndHourB
            End Get
            Set(value As TimeSpan)
                If (value < TimeSpan.Zero) Then
                    value = TimeSpan.Zero
                End If
                If (value >= TimeSpan.FromHours(24)) Then
                    value = TimeSpan.FromHours(24).Add(TimeSpan.FromSeconds(-1))
                End If
                Me.ladderEndHourB = value
            End Set
        End Property
        Private ladderEndHourB As TimeSpan

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether SmartBot should be started at the initial ladder hour if it is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ladder Scheduler")>
        <DisplayName("Auto-start SmartBot if it is stopped")>
        <Browsable(True)>
        Public Property LadderModeAutoStart As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether SmartBot should be stopped when ladder scheduler ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ladder Scheduler")>
        <DisplayName("Auto-stop SmartBot at the finish hour")>
        <Browsable(True)>
        Public Property LadderModeAutoStop As Boolean

#End Region

#Region " Preferred Decks (for questing and levelling)"

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Druid.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for questing and levelling)")>
        <DisplayName("Druid")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceDruid))>
        Public Property DeckDruid As String
            Get
                If String.IsNullOrEmpty(Me.deckDruidB) Then
                    Me.deckDruidB = "None"
                End If
                Return Me.deckDruidB
            End Get
            Set(value As String)
                Me.deckDruidB = value
            End Set
        End Property
        Private deckDruidB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Mage.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for questing and levelling)")>
        <DisplayName("Mage")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceMage))>
        Public Property DeckMage As String
            Get
                If String.IsNullOrEmpty(Me.deckMageB) Then
                    Me.deckMageB = "None"
                End If
                Return Me.deckMageB
            End Get
            Set(value As String)
                Me.deckMageB = value
            End Set
        End Property
        Private deckMageB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Hunter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for questing and levelling)")>
        <DisplayName("Hunter")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceHunter))>
        Public Property DeckHunter As String
            Get
                If String.IsNullOrEmpty(Me.deckHunterB) Then
                    Me.deckHunterB = "None"
                End If
                Return Me.deckHunterB
            End Get
            Set(value As String)
                Me.deckHunterB = value
            End Set
        End Property
        Private deckHunterB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Paladin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for questing and levelling)")>
        <DisplayName("Paladin")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourcePaladin))>
        Public Property DeckPaladin As String
            Get
                If String.IsNullOrEmpty(Me.deckPaladinB) Then
                    Me.deckPaladinB = "None"
                End If
                Return Me.deckPaladinB
            End Get
            Set(value As String)
                Me.deckPaladinB = value
            End Set
        End Property
        Private deckPaladinB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Priest.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for questing and levelling)")>
        <DisplayName("Priest")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourcePriest))>
        Public Property DeckPriest As String
            Get
                If String.IsNullOrEmpty(Me.deckPriestB) Then
                    Me.deckPriestB = "None"
                End If
                Return Me.deckPriestB
            End Get
            Set(value As String)
                Me.deckPriestB = value
            End Set
        End Property
        Private deckPriestB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Rogue.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for questing and levelling)")>
        <DisplayName("Rogue")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceRogue))>
        Public Property DeckRogue As String
            Get
                If String.IsNullOrEmpty(Me.deckRogueB) Then
                    Me.deckRogueB = "None"
                End If
                Return Me.deckRogueB
            End Get
            Set(value As String)
                Me.deckRogueB = value
            End Set
        End Property
        Private deckRogueB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Shaman.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for questing and levelling)")>
        <DisplayName("Shaman")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceShaman))>
        Public Property DeckShaman As String
            Get
                If String.IsNullOrEmpty(Me.deckShamanB) Then
                    Me.deckShamanB = "None"
                End If
                Return Me.deckShamanB
            End Get
            Set(value As String)
                Me.deckShamanB = value
            End Set
        End Property
        Private deckShamanB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Warrior.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for questing and levelling)")>
        <DisplayName("Warrior")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceWarrior))>
        Public Property DeckWarrior As String
            Get
                If String.IsNullOrEmpty(Me.deckWarriorB) Then
                    Me.deckWarriorB = "None"
                End If
                Return Me.deckWarriorB
            End Get
            Set(value As String)
                Me.deckWarriorB = value
            End Set
        End Property
        Private deckWarriorB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Warlock.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for questing and levelling)")>
        <DisplayName("Warlock")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceWarlock))>
        Public Property DeckWarlock As String
            Get
                If String.IsNullOrEmpty(Me.deckWarlockB) Then
                    Me.deckWarlockB = "None"
                End If
                Return Me.deckWarlockB
            End Get
            Set(value As String)
                Me.deckWarlockB = value
            End Set
        End Property
        Private deckWarlockB As String

#End Region

#Region " Reserved "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the preferred deck for the specified hero class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public ReadOnly Property PreferredDeck(ByVal heroClass As CClass) As Deck
            Get
                Return Me.GetPreferredDeck(heroClass)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the target level to reach for the specified hero class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public ReadOnly Property TargetHeroLevel(ByVal heroClass As CClass) As Integer
            Get
                Return Me.GetTargetHeroLevel(heroClass)
            End Get
        End Property

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="BountyHunterPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.AssemblyInfo.AssemblyName

            Me.EnableQuestCompletion = False
            Me.QuestMode = Bot.Mode.UnrankedStandard
            Me.Reroll50GoldQuests = False
            Me.RerollUnfulfillableQuests = False
            Me.KeepQuestCatchABigOne = False
            Me.KeepQuestPlayAFriend = False
            Me.KeepQuestSpelunker = False
            Me.KeepQuestWatchAndLearn = False

            Me.EnableLadderScheduler = False
            Me.LadderMode = Bot.Mode.RankedStandard
            Me.LadderPreferredDeck = "None"
            Me.LadderUseRandomDeckIfPreferredIsUnavailable = False
            Me.ladderStartHourB = TimeSpan.Zero
            Me.ladderEndHourB = Me.ladderStartHourB.Add(TimeSpan.FromHours(10))
            Me.LadderModeAutoStart = False
            Me.LadderModeAutoStop = False

            Me.EnableHeroLevelling = False
            Me.LevelMode = Bot.Mode.UnrankedStandard
            Me.lvlDruidB = 1
            Me.lvlHunterB = 1
            Me.lvlMageB = 1
            Me.lvlPaladinB = 1
            Me.lvlPriestB = 1
            Me.lvlRogueB = 1
            Me.lvlShamanB = 1
            Me.lvlWarlockB = 1
            Me.lvlWarriorB = 1

            Me.deckDruidB = "None"
            Me.deckHunterB = "None"
            Me.deckMageB = "None"
            Me.deckPaladinB = "None"
            Me.deckPriestB = "None"
            Me.deckRogueB = "None"
            Me.deckShamanB = "None"
            Me.deckWarlockB = "None"
            Me.deckWarriorB = "None"
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the preferred deck for the specified hero class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetPreferredDeck(ByVal heroClass As CClass) As Deck

            Dim result As Deck

            Select Case heroClass

                Case CClass.DRUID
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckDruid) AndAlso (deck.IsValid())
                           ).FirstOrDefault()

                Case CClass.HUNTER
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckHunter) AndAlso (deck.IsValid())
                           ).FirstOrDefault()

                Case CClass.MAGE
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckMage) AndAlso (deck.IsValid())
                           ).FirstOrDefault()

                Case CClass.PALADIN
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckPaladin) AndAlso (deck.IsValid())
                           ).FirstOrDefault()

                Case CClass.PRIEST
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckPriest) AndAlso (deck.IsValid())
                           ).FirstOrDefault()

                Case CClass.ROGUE
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckRogue) AndAlso (deck.IsValid())
                           ).FirstOrDefault()

                Case CClass.SHAMAN
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckShaman) AndAlso (deck.IsValid())
                           ).FirstOrDefault()

                Case CClass.WARLOCK
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckWarlock) AndAlso (deck.IsValid())
                           ).FirstOrDefault()

                Case CClass.WARRIOR
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckWarrior) AndAlso (deck.IsValid())
                           ).FirstOrDefault()

                Case Else ' CClass.NONE
                    Return Nothing

            End Select

            If (result Is Nothing) Then
                result = (From deck As Deck In Bot.GetDecks()
                          Where (deck.Class = heroClass) AndAlso (deck.IsValid())
                         ).Randomize().FirstOrDefault()
            End If

            If (result Is Nothing) Then
                result = New Deck() With {.Name = "None"}
            End If

            Return result

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the target level to reach for the specified hero class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetTargetHeroLevel(ByVal heroClass As CClass) As Integer

            Select Case heroClass

                Case CClass.DRUID
                    Return Me.LvlDruid

                Case CClass.HUNTER
                    Return Me.LvlHunter

                Case CClass.MAGE
                    Return Me.LvlMage

                Case CClass.PALADIN
                    Return Me.LvlPaladin

                Case CClass.PRIEST
                    Return Me.LvlPriest

                Case CClass.ROGUE
                    Return Me.LvlRogue

                Case CClass.SHAMAN
                    Return Me.LvlShaman

                Case CClass.WARLOCK
                    Return Me.LvlWarlock

                Case CClass.WARRIOR
                    Return Me.LvlWarrior

                Case Else ' CClass.NONE
                    Return -1

            End Select

        End Function

#End Region

    End Class

End Namespace

#End Region
