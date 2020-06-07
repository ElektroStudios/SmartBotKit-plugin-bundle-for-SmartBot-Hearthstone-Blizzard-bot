
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
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Interop

#End Region

#Region " GarbageCleanerPluginData "

' ReSharper disable once CheckNamespace

Namespace GarbageCleaner

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="GarbageCleanerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class GarbageCleanerPluginData : Inherits PluginDataContainer

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
                Return "Cleans temporary files generated by SmartBot."
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

#Region " Deletion Behavior "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether deleted files will be sent to Recycle Bin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Deletion Behavior")>
        <DisplayName("Send files to Recycle Bin." & ControlChars.NewLine &
                     "( if unchecked, files will be permanently deleted. )")>
        Public Property SendFilesToRecycleBin As Boolean

#End Region

#Region " Files to clean "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete SmartBot logs and temp files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Files to clean")>
        <DisplayName("SmartBot logs and temp files")>
        Public Property DeleteSmartBotLogs As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete Soviet Mulligan Kit (SMK) logs and temp files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Files to clean")>
        <DisplayName("Soviet Mulligan Kit (SMK) logs and temp files")>
        Public Property DeleteSovietLogs As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete BattleTag Crawler log files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Files to clean")>
        <DisplayName("BattleTag Crawler log files")>
        Public Property DeleteBattleTagCrawlertLogs As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete seed files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Files to clean")>
        <DisplayName("Seed files")>
        Public Property DeleteSeeds As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete screenshots.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Files to clean")>
        <DisplayName("Screenshot files")>
        Public Property DeleteScreenshots As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete SmartBot updates (*.zip)").
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Files to clean")>
        <DisplayName("SmartBot updates (*.zip)")>
        Public Property DeleteUpdates As Boolean

#End Region

#Region " Date Filtering "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine to only clean garbage that is older than the number specified, in days.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The minimum number of days that a directory or file must have to be cleaned.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Date Filtering")>
        <DisplayName("Only cleans garbage that is older than the number of days specified." & ControlChars.NewLine &
                     "( set the number to zero if you want to disable date filtering. )")>
        <Browsable(True)>
        Public Property OlderThanDays() As Integer
            Get
                Return Me.olderThanDays_
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.olderThanDays_ = 0
                ElseIf (value > 90) Then
                    Me.olderThanDays_ = 90
                Else
                    Me.olderThanDays_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The minimum number of days that a directory or file must have to be cleaned.
        ''' </summary>
        Private olderThanDays_ As Integer

#End Region

#Region " Logging "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete log files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Logging")>
        <DisplayName("Enable verbose cleaning mode." & ControlChars.NewLine &
                     "( If enabled, full paths of deleted files will be sent to log. )")>
        Public Property VerboseMode As Boolean

#End Region

#Region " Scheduling "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that specifies when the garbage should be deleted.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Scheduler")>
        <DisplayName("Run Garbage Cleaner at the specified SmartBot event: ")>
        <ItemsSource(GetType(CleanerEventSource))>
        Public Property CleanerEvent As SmartBotEvent

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="GarbageCleanerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.CleanerEvent = SmartBotEvent.Startup
            Me.SendFilesToRecycleBin = True
            Me.OlderThanDays = 0
            Me.VerboseMode = True

            Me.DeleteSmartBotLogs = False
            Me.DeleteSovietLogs = False
            Me.DeleteBattleTagCrawlertLogs = False
            Me.DeleteSeeds = False
            Me.DeleteScreenshots = False
            Me.DeleteUpdates = False
        End Sub

#End Region

    End Class

End Namespace

#End Region
