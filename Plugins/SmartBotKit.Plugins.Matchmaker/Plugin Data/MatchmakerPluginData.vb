
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

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Interop

#End Region

#Region " MatchmakerPluginData "

' ReSharper disable once CheckNamespace

Namespace Matchmaker

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="MatchmakerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class MatchmakerPluginData : Inherits PluginDataContainer

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
                Return "Helps you find your favorite opponent match."
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

#Region " Settings "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the bot mode for questing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Game mode")>
        <Browsable(True)>
        <ItemsSource(GetType(ModeSource))>
        Public Property Mode As Bot.Mode

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the deck for playing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Deck")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with DemonHunter class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("DemonHunter")>
        <Browsable(True)>
        Public Property AllowDemonHunter As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with Druid class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("Druid")>
        <Browsable(True)>
        Public Property AllowDruid As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with Hunter class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("Hunter")>
        <Browsable(True)>
        Public Property AllowHunter As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with Mage class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("Mage")>
        <Browsable(True)>
        Public Property AllowMage As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with Paladin class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("Paladin")>
        <Browsable(True)>
        Public Property AllowPaladin As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with Priest class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("Priest")>
        <Browsable(True)>
        Public Property AllowPriest As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with Rogue class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("Rogue")>
        <Browsable(True)>
        Public Property AllowRogue As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with Shaman class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("Shaman")>
        <Browsable(True)>
        Public Property AllowShaman As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with Warlock class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("Warlock")>
        <Browsable(True)>
        Public Property AllowWarlock As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to allow matchmaking with Warrior class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Allowed Matches")>
        <DisplayName("Warrior")>
        <Browsable(True)>
        Public Property AllowWarrior As Boolean

#End Region

#Region " Notification "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to play a wave sound file when a match is found.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Notification")>
        <DisplayName("Play sound file when a match is found")>
        Public Property PlaySoundFile As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to activate Hearthstone window when a match is found.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Notification")>
        <DisplayName("Activate Hearthstone window")>
        Public Property ActivateWindow As Boolean

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="MatchmakerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.Mode = Bot.Mode.UnrankedStandard
            Me.Deck = "None"

            Me.ActivateWindow = True
            Me.PlaySoundFile = True

            Me.AllowDemonHunter = True
            Me.AllowDruid = True
            Me.AllowHunter = True
            Me.AllowMage = True
            Me.AllowPaladin = True
            Me.AllowPriest = True
            Me.AllowRogue = True
            Me.AllowShaman = True
            Me.AllowWarlock = True
            Me.AllowWarrior = True
        End Sub

#End Region

    End Class

End Namespace

#End Region
