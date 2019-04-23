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
Imports System.Reflection

Imports SmartBot.Plugins

#End Region

#Region " BattleTagCrawlerPluginData "

Namespace PluginTemplate

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

#Region " Settings "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to log duplicated BattleTag ids.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Add duplicated BattleTag ids to log file")>
        <Browsable(True)>
        Public Property LogDuplicates As Boolean

#End Region

#Region " Standard Mode "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to crawl BattleTag ids from players in ranked standard games.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard Mode")>
        <DisplayName("Crawl BattleTags from Ranked Standard games")>
        Public Property CrawlRankedStandardGames As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to crawl BattleTag ids from players in unranked standard games.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard Mode")>
        <DisplayName("Crawl BattleTags from Unranked Standard games")>
        Public Property CrawlUnrankedStandardGames As Boolean

#End Region

#Region " Wild Mode "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to crawl BattleTag ids from players in ranked wild games.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Wild Mode")>
        <DisplayName("Crawl BattleTags from Ranked Wild games")>
        Public Property CrawlRankedWildGames As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether or not to crawl BattleTag ids from players in unranked wild games.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Wild Mode")>
        <DisplayName("Crawl BattleTags from Unranked Wild games")>
        Public Property CrawlUnrankedWildGames As Boolean

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="BattleTagCrawlerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.AssemblyInfo.AssemblyName

            Me.CrawlRankedStandardGames = True
            Me.CrawlUnrankedStandardGames = True

            Me.CrawlRankedWildGames = True
            Me.CrawlUnrankedWildGames = True
        End Sub

#End Region

    End Class

End Namespace

#End Region
