
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

Imports SmartBotKit.Interop
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

#End Region

#Region " DecklistKeeperPluginData "

' ReSharper disable once CheckNamespace

Namespace DecklistKeeper

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="DecklistKeeperPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class DecklistKeeperPluginData : Inherits PluginDataContainer

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
                Return "Prevents the bot from changing your deck-list."
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

#Region " Ladder Scheduler "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 1.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 01")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck1 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 2.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 02")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck2 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 3.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 03")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck3 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 4.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 04")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck4 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 5.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 05")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck5 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 6.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 06")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck6 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 7.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 07")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck7 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 8.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 08")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck8 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 9.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 09")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck9 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the preferred deck 10.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Decks")>
        <DisplayName("Deck 10")>
        <Browsable(True)>
        <ItemsSource(GetType(DeckSource))>
        Public Property Deck10 As String

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="DecklistKeeperPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.Deck1 = "None"
            Me.Deck2 = "None"
            Me.Deck3 = "None"
            Me.Deck4 = "None"
            Me.Deck5 = "None"
            Me.Deck6 = "None"
            Me.Deck7 = "None"
            Me.Deck8 = "None"
            Me.Deck9 = "None"
            Me.Deck10 = "None"
        End Sub

#End Region

    End Class

End Namespace

#End Region
