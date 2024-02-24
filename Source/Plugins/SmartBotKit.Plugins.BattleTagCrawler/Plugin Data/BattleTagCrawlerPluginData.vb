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

#End Region

#Region " BattleTagCrawlerPluginData "

' ReSharper disable once CheckNamespace

Namespace BattleTagCrawler

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="BattleTagCrawlerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class BattleTagCrawlerPluginData : Inherits PluginDataContainer

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

#End Region

#Region " Settings "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to log duplicated BattleTag ids.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("General Settings")>
        <DisplayName("Add new entries at beginning of file instead of end of file")>
        <Browsable(True)>
        Public Property AddNewEntriesAtBeginningOfFile As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to log duplicated BattleTag ids.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("General Settings")>
        <DisplayName("Log duplicated BattleTags")>
        <Browsable(True)>
        Public Property LogDuplicates As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to write to a single log file instead of creating multiple logs.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("General Settings")>
        <DisplayName("Write to a single log file instead of creating multiple logs")>
        <Browsable(True)>
        Public Property UseSingleLogFile As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine the hour (in 24 hrs format) on which the plugin will start logging battle-tags.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("General Settings")>
        <DisplayName("The hour (in 24 hrs format) on which the plugin will start logging battle-tags")>
        <Browsable(True)>
        Public Property HourStart As Integer
            Get
                Return Me.hourStart_
            End Get
            Set(value As Integer)
                If value < 0 Then
                    Me.hourStart_ = 0
                ElseIf value > 24 Then
                    Me.hourStart_ = 24
                Else
                    Me.hourStart_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The hour (in 24 hrs format) on which the plugin will start logging battle-tags.
        ''' </summary>
        Private hourStart_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine the hour (in 24 hrs format) on which the plugin will stop from logging battle-tags.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("General Settings")>
        <DisplayName("The hour (in 24 hrs format) on which the plugin will stop from logging battle-tags")>
        <Browsable(True)>
        Public Property HourEnd As Integer
            Get
                Return Me.hourEnd_
            End Get
            Set(value As Integer)
                If value < 0 Then
                    Me.hourEnd_ = 0
                ElseIf value > 24 Then
                    Me.hourEnd_ = 24
                Else
                    Me.hourEnd_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The hour (in 24 hrs format) on which the plugin will stop from logging battle-tags.
        ''' </summary>
        Private hourEnd_ As Integer

#End Region

#Region " Modes "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to crawl BattleTag ids from players in standard games.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Modes")>
        <DisplayName("Crawl BattleTags from Standard game mode")>
        Public Property CrawlStandardGames As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to crawl BattleTag ids from players in wild games.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Modes")>
        <DisplayName("Crawl BattleTags from Wild game mode")>
        Public Property CrawlWildGames As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to crawl BattleTag ids from players in Twist games.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Modes")>
        <DisplayName("Crawl BattleTags from Twist game mode")>
        Public Property CrawlTwistGames As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to crawl BattleTag ids from players in duels games.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Modes")>
        <DisplayName("Crawl BattleTags from Duels game mode")>
        Public Property CrawlDuelsGames As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to crawl BattleTag ids from players in casual games.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Modes")>
        <DisplayName("Crawl BattleTags from Casual game mode")>
        Public Property CrawlCasualGames As Boolean

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="BattleTagCrawlerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.AddNewEntriesAtBeginningOfFile = True
            Me.LogDuplicates = False
            Me.UseSingleLogFile = True

            Me.CrawlStandardGames = True
            Me.CrawlWildGames = True
            Me.CrawlCasualGames = True
            Me.CrawlTwistGames = True
            Me.CrawlDuelsGames = True

            Me.HourStart = 0
            Me.HourEnd = 24
        End Sub

#End Region

    End Class

End Namespace

#End Region
