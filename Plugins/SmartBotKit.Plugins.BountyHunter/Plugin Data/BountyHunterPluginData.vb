
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Reflection
Imports System.Linq

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Card
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

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
        ''' Gets the author of this plugin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Author")>
        Public ReadOnly Property Author As String
            Get
                Return Me.AssemblyInfo.CompanyName
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Name")>
        Public ReadOnly Property ProductName As String
            Get
                Return Me.AssemblyInfo.Title
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
                Return Me.AssemblyInfo.Description
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

#Region " General Settings "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the bot mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("General Settings")>
        <DisplayName("Bot Mode")>
        <Browsable(True)>
        <ItemsSource(GetType(BotModeSource))>
        Public Property BotMode As Bot.Mode

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should do quests.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("General Settings")>
        <DisplayName("Enable Quests Completion")>
        <Browsable(True)>
        Public Property EnableQuestCompletion As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should do quests.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("General Settings")>
        <DisplayName("Enable Hero Levels Completion")>
        <Browsable(True)>
        Public Property EnableHeroLevelsCompletion As Boolean

#End Region

#Region " Quests Completion "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether 50 gold quests should be reroll.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Quests Completion")>
        <DisplayName("Reroll 50 Gold Quests")>
        <Browsable(True)>
        Public Property Reroll50GoldQuests() As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether unfulfillable quests should be reroll.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Quests Completion")>
        <DisplayName("Reroll Unfulfillable Quests")>
        <Browsable(True)>
        Public Property RerollUnfulfillableQuests() As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether "Watch and Learn!" quest should be kept.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Quests Completion")>
        <DisplayName("Keep 'Watch and Learn!' Quest")>
        <Browsable(True)>
        Public Property KeepWatchAndLearnQuest() As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether "Play a Friend!" quest should be kept.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Quests Completion")>
        <DisplayName("Keep 'Play a Friend!' Quest")>
        <Browsable(True)>
        Public Property KeepPlayAFriendQuest() As Boolean

#End Region

#Region " Hero Levels Completion "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Mage.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Hero Levels Completion")>
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
        <Category("Hero Levels Completion")>
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
        <Category("Hero Levels Completion")>
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
        <Category("Hero Levels Completion")>
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
        <Category("Hero Levels Completion")>
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
        <Category("Hero Levels Completion")>
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
        <Category("Hero Levels Completion")>
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
        <Category("Hero Levels Completion")>
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
        <Category("Hero Levels Completion")>
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

#Region " Prefered Decks "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the prefered deck for Druid.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Prefered Decks")>
        <DisplayName("Druid")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceDruid))>
        Public Property DeckDruid As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the prefered deck for Mage.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Prefered Decks")>
        <DisplayName("Mage")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceMage))>
        Public Property DeckMage As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the prefered deck for Hunter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Prefered Decks")>
        <DisplayName("Hunter")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceHunter))>
        Public Property DeckHunter As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the prefered deck for Paladin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Prefered Decks")>
        <DisplayName("Paladin")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourcePaladin))>
        Public Property DeckPaladin As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the prefered deck for Priest.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Prefered Decks")>
        <DisplayName("Priest")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourcePriest))>
        Public Property DeckPriest As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the prefered deck for Rogue.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Prefered Decks")>
        <DisplayName("Rogue")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceRogue))>
        Public Property DeckRogue As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the prefered deck for Shaman.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Prefered Decks")>
        <DisplayName("Shaman")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceShaman))>
        Public Property DeckShaman As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the prefered deck for Warrior.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Prefered Decks")>
        <DisplayName("Warrior")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceWarrior))>
        Public Property DeckWarrior As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the prefered deck for Warlock.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Prefered Decks")>
        <DisplayName("Warlock")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceWarlock))>
        Public Property DeckWarlock As String

#End Region

#Region " Reserved "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the prefered deck for the specified hero class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public ReadOnly Property PreferedDeck(ByVal heroClass As CClass) As Deck
            Get
                Return Me.GetPreferedDeck(heroClass)
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

            Me.BotMode = Bot.Mode.UnrankedStandard
            Me.EnableHeroLevelsCompletion = True
            Me.EnableQuestCompletion = True

            Me.Reroll50GoldQuests = False
            Me.RerollUnfulfillableQuests = False
            Me.KeepPlayAFriendQuest = True
            Me.KeepWatchAndLearnQuest = False

            Me.lvlDruidB = 10
            Me.lvlHunterB = 10
            Me.lvlMageB = 10
            Me.lvlPaladinB = 10
            Me.lvlPriestB = 10
            Me.lvlRogueB = 10
            Me.lvlShamanB = 10
            Me.lvlWarlockB = 10
            Me.lvlWarriorB = 10

            Me.DeckDruid = "None"
            Me.DeckHunter = "None"
            Me.DeckMage = "None"
            Me.DeckPaladin = "None"
            Me.DeckPriest = "None"
            Me.DeckRogue = "None"
            Me.DeckShaman = "None"
            Me.DeckWarlock = "None"
            Me.DeckWarrior = "None"
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the prefered deck for the specified hero class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetPreferedDeck(ByVal heroClass As CClass) As Deck

            Dim result As Deck

            Select Case heroClass

                Case CClass.DRUID
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckDruid)
                           ).FirstOrDefault()

                Case CClass.HUNTER
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckHunter)
                           ).FirstOrDefault()

                Case CClass.MAGE
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckMage)
                           ).FirstOrDefault()

                Case CClass.PALADIN
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckPaladin)
                           ).FirstOrDefault()

                Case CClass.PRIEST
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckPriest)
                           ).FirstOrDefault()

                Case CClass.ROGUE
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckRogue)
                           ).FirstOrDefault()

                Case CClass.SHAMAN
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckShaman)
                           ).FirstOrDefault()

                Case CClass.WARLOCK
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckWarlock)
                           ).FirstOrDefault()

                Case CClass.WARRIOR
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckWarrior)
                           ).FirstOrDefault()

                Case Else ' CClass.NONE
                    Return Nothing

            End Select

            If (result Is Nothing) Then
                result = (From deck As Deck In Bot.GetDecks()
                          Where (deck.Class = heroClass)
                         ).FirstOrDefault()
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
