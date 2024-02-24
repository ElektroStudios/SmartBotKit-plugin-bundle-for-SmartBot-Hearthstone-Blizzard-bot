
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Card
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Extensions.IEnumerableExtensions
Imports SmartBotKit.Interop

#End Region

#Region " BountyHunterPluginData "

' ReSharper disable once CheckNamespace

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
        ''' Gets the assembly path.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <Browsable(False)>
        <DisplayName("Assembly Name")>
        Public ReadOnly Property AssemblyName As String
            Get
                Return System.IO.Path.ChangeExtension(Me.AssemblyInfo.AssemblyName, "dll")
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the assembly path.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Path")>
        Public ReadOnly Property Path As String
            Get
                Return $".\{SmartBotUtil.PluginsDir.Name}\{Me.AssemblyName}"
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin title.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("[Title]")>
        Public ReadOnly Property Title As String
            Get
                Return Me.AssemblyInfo.Title
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Name")>
        <Browsable(False)>
        <[ReadOnly](True)>
        Public Shadows ReadOnly Property Name As String
            Get
                Return Me.AssemblyInfo.AssemblyName
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin author.
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

#End Region

#Region " Daily Quests "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should do quests.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Daily Quests")>
        <DisplayName("* Enable Daily Quests Mode")>
        <Browsable(True)>
        Public Property EnableQuestCompletion As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the bot mode for questing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Daily Quests")>
        <DisplayName("Bot mode")>
        <Browsable(True)>
        <ItemsSource(GetType(QuestModeSource))>
        Public Property QuestMode As Bot.Mode

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether 50 and 60 gold quests should be reroll.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Daily Quests")>
        <DisplayName("Reroll 50 and 60 gold quests")>
        <Browsable(True)>
        Public Property Reroll50And60GoldQuests As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether unfulfillable quests should be reroll.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Daily Quests")>
        <DisplayName("Reroll unfulfillable quests")>
        <Browsable(True)>
        Public Property RerollUnfulfillableQuests As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether dungeon quests should be kept.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Daily Quests")>
        <DisplayName("Keep dungeon quest")>
        <Browsable(True)>
        Public Property KeepDungeonQuests As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether "Play a Friend!" quest should be kept.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Daily Quests")>
        <DisplayName("Keep 'Play a Friend!' quest")>
        <Browsable(True)>
        Public Property KeepQuestPlayAFriend As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether "Watch and Learn!" quest should be kept.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Daily Quests")>
        <DisplayName("Keep 'Watch and Learn!' quest")>
        <Browsable(True)>
        Public Property KeepQuestWatchAndLearn As Boolean

#End Region

#Region " Hero Levelling "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should level up heroes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("* Enable Hero Levelling mode")>
        <Browsable(True)>
        Public Property EnableHeroLevelling As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the bot mode for levelling.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("** Bot Mode")>
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
                Return Me.lvlMage_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlMage_ = 1
                ElseIf (value > 60) Then
                    Me.lvlMage_ = 60
                Else
                    Me.lvlMage_ = value
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
        Private lvlMage_ As Integer

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
                Return Me.lvlPriest_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlPriest_ = 1
                ElseIf (value > 60) Then
                    Me.lvlPriest_ = 60
                Else
                    Me.lvlPriest_ = value
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
        Private lvlPriest_ As Integer

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
                Return Me.lvlWarrior_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlWarrior_ = 1
                ElseIf (value > 60) Then
                    Me.lvlWarrior_ = 60
                Else
                    Me.lvlWarrior_ = value
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
        Private lvlWarrior_ As Integer

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
                Return Me.lvlWarlock_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlWarlock_ = 1
                ElseIf (value > 60) Then
                    Me.lvlWarlock_ = 60
                Else
                    Me.lvlWarlock_ = value
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
        Private lvlWarlock_ As Integer

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
                Return Me.lvlRogue_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlRogue_ = 1
                ElseIf (value > 60) Then
                    Me.lvlRogue_ = 60
                Else
                    Me.lvlRogue_ = value
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
        Private lvlRogue_ As Integer

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
                Return Me.lvlDruid_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlDruid_ = 1
                ElseIf (value > 60) Then
                    Me.lvlDruid_ = 60
                Else
                    Me.lvlDruid_ = value
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
        Private lvlDruid_ As Integer

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
                Return Me.lvlHunter_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlHunter_ = 1
                ElseIf (value > 60) Then
                    Me.lvlHunter_ = 60
                Else
                    Me.lvlHunter_ = value
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
        Private lvlHunter_ As Integer

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
                Return Me.lvlShaman_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlShaman_ = 1
                ElseIf (value > 60) Then
                    Me.lvlShaman_ = 60
                Else
                    Me.lvlShaman_ = value
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
        Private lvlShaman_ As Integer

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
                Return Me.lvlPaladin_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlPaladin_ = 1
                ElseIf (value > 60) Then
                    Me.lvlPaladin_ = 60
                Else
                    Me.lvlPaladin_ = value
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
        Private lvlPaladin_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target level for Demon Hunter.
        ''' <para></para>
        ''' Valid range is between 1 and 60.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Hero Levelling")>
        <DisplayName("Demon Hunter")>
        <Browsable(True)>
        Public Property LvlDemonHunter As Integer
            Get
                Return Me.lvlDemonHunter_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.lvlDemonHunter_ = 1
                ElseIf (value > 60) Then
                    Me.lvlDemonHunter_ = 60
                Else
                    Me.lvlDemonHunter_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target level for Demon Hunter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lvlDemonHunter_ As Integer

#End Region

#Region " Ranked Wins Count "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should try to play for reach a specific amount of ranked wins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("* Enable Ranked Wins Count Mode")>
        <Browsable(True)>
        Public Property EnableRankedWinsCount As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the bot mode for levelling.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("** Bot Mode")>
        <Browsable(True)>
        <ItemsSource(GetType(LadderModeSource))>
        Public Property RankedWinsCountMode As Bot.Mode

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the bot should try to use a random available deck if 
        ''' the specified preferred deck in <see cref="BountyHunterPluginData.PreferredDeck"/> is unavailable.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("** Try a random available deck if preferred deck is unavailable")>
        <Browsable(True)>
        Public Property RankedWinsCountUseRandomDeckIfPreferredIsUnavailable As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Mage.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Mage")>
        <Browsable(True)>
        Public Property RankedWinsMage As Integer
            Get
                Return Me.rankedWinsMage_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsMage_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsMage_ = 9999
                Else
                    Me.rankedWinsMage_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Mage.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsMage_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Priest.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Priest")>
        <Browsable(True)>
        Public Property RankedWinsPriest As Integer
            Get
                Return Me.rankedWinsPriest_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsPriest_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsPriest_ = 9999
                Else
                    Me.rankedWinsPriest_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Priest.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsPriest_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Warrior.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Warrior")>
        <Browsable(True)>
        Public Property RankedWinsWarrior As Integer
            Get
                Return Me.rankedWinsWarrior_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsWarrior_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsWarrior_ = 9999
                Else
                    Me.rankedWinsWarrior_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Warrior.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsWarrior_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Warlock.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Warlock")>
        <Browsable(True)>
        Public Property RankedWinsWarlock As Integer
            Get
                Return Me.rankedWinsWarlock_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsWarlock_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsWarlock_ = 9999
                Else
                    Me.rankedWinsWarlock_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Warlock.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsWarlock_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Rogue.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Rogue")>
        <Browsable(True)>
        Public Property RankedWinsRogue As Integer
            Get
                Return Me.rankedWinsRogue_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsRogue_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsRogue_ = 9999
                Else
                    Me.rankedWinsRogue_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Rogue.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsRogue_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Druid.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Druid")>
        <Browsable(True)>
        Public Property RankedWinsDruid As Integer
            Get
                Return Me.rankedWinsDruid_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsDruid_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsDruid_ = 9999
                Else
                    Me.rankedWinsDruid_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Druid.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsDruid_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Hunter.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Hunter")>
        <Browsable(True)>
        Public Property RankedWinsHunter As Integer
            Get
                Return Me.rankedWinsHunter_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsHunter_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsHunter_ = 9999
                Else
                    Me.rankedWinsHunter_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Hunter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsHunter_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Shaman.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Shaman")>
        <Browsable(True)>
        Public Property RankedWinsShaman As Integer
            Get
                Return Me.rankedWinsShaman_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsShaman_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsShaman_ = 9999
                Else
                    Me.rankedWinsShaman_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Shaman.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsShaman_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Paladin.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Paladin")>
        <Browsable(True)>
        Public Property RankedWinsPaladin As Integer
            Get
                Return Me.rankedWinsPaladin_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsPaladin_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsPaladin_ = 9999
                Else
                    Me.rankedWinsPaladin_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Paladin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsPaladin_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the target amount of ranked wins for Demon Hunter.
        ''' <para></para>
        ''' Valid range is between 1 and 9999.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ranked Wins Count")>
        <DisplayName("Demon Hunter")>
        <Browsable(True)>
        Public Property RankedWinsDemonHunter As Integer
            Get
                Return Me.rankedWinsDemonHunter_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.rankedWinsDemonHunter_ = 0
                ElseIf (value > 9999) Then
                    Me.rankedWinsDemonHunter_ = 9999
                Else
                    Me.rankedWinsDemonHunter_ = value
                End If
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The target amount of ranked wins for Demon Hunter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private rankedWinsDemonHunter_ As Integer

#End Region

#Region " Ladder Scheduler "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the hour to start playing ladder.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Mode: Ladder Scheduler")>
        <DisplayName("* Enable Ladder Scheduler Mode" & ControlChars.NewLine & ControlChars.NewLine &
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
                Return Me.ladderStartHour_
            End Get
            Set(value As TimeSpan)
                If (value < TimeSpan.Zero) Then
                    value = TimeSpan.Zero
                End If
                If (value >= TimeSpan.FromHours(24)) Then
                    value = TimeSpan.FromHours(24).Add(TimeSpan.FromSeconds(-1))
                End If
                Me.ladderStartHour_ = value
            End Set
        End Property
        Private ladderStartHour_ As TimeSpan

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
                Return Me.ladderEndHour_
            End Get
            Set(value As TimeSpan)
                If (value < TimeSpan.Zero) Then
                    value = TimeSpan.Zero
                End If
                If (value >= TimeSpan.FromHours(24)) Then
                    value = TimeSpan.FromHours(24).Add(TimeSpan.FromSeconds(-1))
                End If
                Me.ladderEndHour_ = value
            End Set
        End Property
        Private ladderEndHour_ As TimeSpan

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

#Region " Preferred Decks (for all modes)"

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Druid.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Druid")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceDruid))>
        Public Property DeckDruid As String
            Get
                If String.IsNullOrEmpty(Me.deckDruid_) Then
                    Me.deckDruid_ = "None"
                End If
                Return Me.deckDruid_
            End Get
            Set(value As String)
                Me.deckDruid_ = value
            End Set
        End Property
        Private deckDruid_ As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Mage.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Mage")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceMage))>
        Public Property DeckMage As String
            Get
                If String.IsNullOrEmpty(Me.deckMage_) Then
                    Me.deckMage_ = "None"
                End If
                Return Me.deckMage_
            End Get
            Set(value As String)
                Me.deckMage_ = value
            End Set
        End Property
        Private deckMage_ As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Hunter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Hunter")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceHunter))>
        Public Property DeckHunter As String
            Get
                If String.IsNullOrEmpty(Me.deckHunter_) Then
                    Me.deckHunter_ = "None"
                End If
                Return Me.deckHunter_
            End Get
            Set(value As String)
                Me.deckHunter_ = value
            End Set
        End Property
        Private deckHunter_ As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Paladin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Paladin")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourcePaladin))>
        Public Property DeckPaladin As String
            Get
                If String.IsNullOrEmpty(Me.deckPaladin_) Then
                    Me.deckPaladin_ = "None"
                End If
                Return Me.deckPaladin_
            End Get
            Set(value As String)
                Me.deckPaladin_ = value
            End Set
        End Property
        Private deckPaladin_ As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Priest.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Priest")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourcePriest))>
        Public Property DeckPriest As String
            Get
                If String.IsNullOrEmpty(Me.deckPriest_) Then
                    Me.deckPriest_ = "None"
                End If
                Return Me.deckPriest_
            End Get
            Set(value As String)
                Me.deckPriest_ = value
            End Set
        End Property
        Private deckPriest_ As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Rogue.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Rogue")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceRogue))>
        Public Property DeckRogue As String
            Get
                If String.IsNullOrEmpty(Me.deckRogue_) Then
                    Me.deckRogue_ = "None"
                End If
                Return Me.deckRogue_
            End Get
            Set(value As String)
                Me.deckRogue_ = value
            End Set
        End Property
        Private deckRogue_ As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Shaman.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Shaman")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceShaman))>
        Public Property DeckShaman As String
            Get
                If String.IsNullOrEmpty(Me.deckShaman_) Then
                    Me.deckShaman_ = "None"
                End If
                Return Me.deckShaman_
            End Get
            Set(value As String)
                Me.deckShaman_ = value
            End Set
        End Property
        Private deckShaman_ As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Warrior.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Warrior")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceWarrior))>
        Public Property DeckWarrior As String
            Get
                If String.IsNullOrEmpty(Me.deckWarrior_) Then
                    Me.deckWarrior_ = "None"
                End If
                Return Me.deckWarrior_
            End Get
            Set(value As String)
                Me.deckWarrior_ = value
            End Set
        End Property
        Private deckWarrior_ As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Warlock.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Warlock")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceWarlock))>
        Public Property DeckWarlock As String
            Get
                If String.IsNullOrEmpty(Me.deckWarlock_) Then
                    Me.deckWarlock_ = "None"
                End If
                Return Me.deckWarlock_
            End Get
            Set(value As String)
                Me.deckWarlock_ = value
            End Set
        End Property
        Private deckWarlock_ As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck for Demon Hunter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Preferred Decks (for all modes)")>
        <DisplayName("Demon Hunter")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSourceDemonHunter))>
        Public Property DeckDemonHunter As String
            Get
                If String.IsNullOrEmpty(Me.deckDemonHunter_) Then
                    Me.deckDemonHunter_ = "None"
                End If
                Return Me.deckDemonHunter_
            End Get
            Set(value As String)
                Me.deckDemonHunter_ = value
            End Set
        End Property
        Private deckDemonHunter_ As String

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

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the target ranked wins count to reach for the specified hero class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public ReadOnly Property TargetRankedWinsCount(ByVal heroClass As CClass) As Integer
            Get
                Return Me.GetTargetRankedWinsCount(heroClass)
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
            MyBase.Name = Me.Name

            Me.EnableQuestCompletion = False
            Me.QuestMode = Bot.Mode.UnrankedStandard
            Me.Reroll50And60GoldQuests = False
            Me.RerollUnfulfillableQuests = False
            Me.KeepDungeonQuests = False
            Me.KeepQuestPlayAFriend = False
            Me.KeepQuestWatchAndLearn = False

            Me.EnableLadderScheduler = False
            Me.LadderMode = Bot.Mode.RankedStandard
            Me.LadderPreferredDeck = "None"
            Me.LadderUseRandomDeckIfPreferredIsUnavailable = False
            Me.ladderStartHour_ = TimeSpan.Zero
            Me.ladderEndHour_ = Me.ladderStartHour_.Add(TimeSpan.FromHours(10))
            Me.LadderModeAutoStart = False
            Me.LadderModeAutoStop = False

            Me.EnableRankedWinsCount = False
            Me.RankedWinsCountMode = Bot.Mode.RankedStandard
            Me.RankedWinsCountUseRandomDeckIfPreferredIsUnavailable = False
            Me.rankedWinsDruid_ = 1000
            Me.rankedWinsHunter_ = 1000
            Me.rankedWinsMage_ = 1000
            Me.rankedWinsPaladin_ = 1000
            Me.rankedWinsPriest_ = 1000
            Me.rankedWinsRogue_ = 1000
            Me.rankedWinsShaman_ = 1000
            Me.rankedWinsWarlock_ = 1000
            Me.rankedWinsWarrior_ = 1000
            Me.rankedWinsDemonHunter_ = 1000

            Me.EnableHeroLevelling = False
            Me.LevelMode = Bot.Mode.UnrankedStandard
            Me.lvlDruid_ = 60
            Me.lvlHunter_ = 60
            Me.lvlMage_ = 60
            Me.lvlPaladin_ = 60
            Me.lvlPriest_ = 60
            Me.lvlRogue_ = 60
            Me.lvlShaman_ = 60
            Me.lvlWarlock_ = 60
            Me.lvlWarrior_ = 60
            Me.lvlDemonHunter_ = 60

            Me.deckDruid_ = "None"
            Me.deckHunter_ = "None"
            Me.deckMage_ = "None"
            Me.deckPaladin_ = "None"
            Me.deckPriest_ = "None"
            Me.deckRogue_ = "None"
            Me.deckShaman_ = "None"
            Me.deckWarlock_ = "None"
            Me.deckWarrior_ = "None"
            Me.deckDemonHunter_ = "None"

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

                Case CClass.DEMONHUNTER
                    result = (From deck As Deck In Bot.GetDecks()
                              Where (deck.Class = heroClass) AndAlso (deck.Name = Me.DeckDemonHunter) AndAlso (deck.IsValid())
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

                Case CClass.DEMONHUNTER
                    Return Me.LvlDemonHunter

                Case Else ' CClass.NONE
                    Return -1

            End Select

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the target ranked wins count to reach for the specified hero class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetTargetRankedWinsCount(ByVal heroClass As CClass) As Integer

            Select Case heroClass

                Case CClass.DRUID
                    Return Me.RankedWinsDruid

                Case CClass.HUNTER
                    Return Me.RankedWinsHunter

                Case CClass.MAGE
                    Return Me.RankedWinsMage

                Case CClass.PALADIN
                    Return Me.RankedWinsPaladin

                Case CClass.PRIEST
                    Return Me.RankedWinsPriest

                Case CClass.ROGUE
                    Return Me.RankedWinsRogue

                Case CClass.SHAMAN
                    Return Me.RankedWinsShaman

                Case CClass.WARLOCK
                    Return Me.RankedWinsWarlock

                Case CClass.WARRIOR
                    Return Me.RankedWinsWarrior

                Case CClass.DEMONHUNTER
                    Return Me.RankedWinsDemonHunter

                Case Else ' CClass.NONE
                    Return -1

            End Select

        End Function

#End Region

    End Class

End Namespace

#End Region
