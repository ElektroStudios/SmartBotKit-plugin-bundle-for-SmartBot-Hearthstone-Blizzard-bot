
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
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Interop

#End Region

#Region " GarbageCleanerPluginData "

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

#Region " File types "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete log files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("File types")>
        <DisplayName("Delete log files.")>
        Public Property DeleteLogs As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete seed files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("File types")>
        <DisplayName("Delete seed files.")>
        Public Property DeleteSeeds As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete screenshots.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("File types")>
        <DisplayName("Delete screenshot files.")>
        Public Property DeleteScreenshots As Boolean

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
                Return Me.olderThanDaysB
            End Get
            Set(ByVal value As Integer)
                If (value < 0) Then
                    Me.olderThanDaysB = 0
                ElseIf (value > 90) Then
                    Me.olderThanDaysB = 90
                Else
                    Me.olderThanDaysB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The minimum number of days that a directory or file must have to be cleaned.
        ''' </summary>
        Private olderThanDaysB As Integer

#End Region

#Region " Logging "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete log files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Logging")>
        <DisplayName("Enable verbose cleaning mode.")>
        Public Property VerboseMode As Boolean

#End Region

#Region " Scheduling "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that specifies when the garbage should be deleted.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Scheduling")>
        <DisplayName("Run Garbage Cleaner at SmartBot event: ")>
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
            MyBase.Name = Me.AssemblyInfo.AssemblyName

            Me.CleanerEvent = SmartBotEvent.Startup
            Me.SendFilesToRecycleBin = True
            Me.DeleteLogs = False
            Me.DeleteSeeds = False
            Me.DeleteScreenshots = False
            Me.OlderThanDays = 0
            Me.VerboseMode = True
        End Sub

#End Region

    End Class

End Namespace

#End Region
