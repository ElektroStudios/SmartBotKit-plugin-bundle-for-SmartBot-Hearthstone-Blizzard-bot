
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Reflection

Imports SmartBot.Plugins
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Interop

#End Region

#Region " AutoCleanerPluginData "

Namespace AutoCleaner

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="AutoCleanerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class AutoCleanerPluginData : Inherits PluginDataContainer

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete log files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("What to delete?")>
        <DisplayName("Delete log files.")>
        Public Property DeleteLogs As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete seed files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("What to delete?")>
        <DisplayName("Delete seed files.")>
        Public Property DeleteSeeds As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete screenshots.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("What to delete?")>
        <DisplayName("Delete screenshots.")>
        Public Property DeleteScreenshots As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether deleted files will be sent to Recycle Bin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("How to delete?")>
        <DisplayName("If enabled, files will be sent to Recycle Bin." & ControlChars.NewLine &
                     "If disabled, files will be permanently deleted.")>
        Public Property SendFilesToRecycleBin As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that specifies when the garbage should be deleted.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("When to delete?")>
        <DisplayName("At which event should the garbage be cleaned?")>
        <ItemsSource(GetType(CleanerEventSource))>
        Public Property CleanerEvent As SmartBotEvent

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete log files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Global Settings")>
        <DisplayName("Verbose clean mode." & ControlChars.NewLine &
                     "If enabled, the cleaner will log the names of any deleted item.")>
        Public Property VerboseMode As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine to only clean garbage that is older than the number specified, in days.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The minimum number of days that a directory or file must have to be cleaned.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Global Settings")>
        <DisplayName("Only clean garbage that is older than the number specified, in days." & ControlChars.NewLine &
                     "Set the number to zero if you want to ignore this option.")>
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

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="AutoCleanerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name

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
