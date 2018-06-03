
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

#End Region

#Region " MultiLauncherPluginData "

Namespace MultiLauncher

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="MultiLauncherPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class MultiLauncherPluginData : Inherits PluginDataContainer

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to terminate the programs specified in properties: 
        ''' <see cref="MultiLauncherPluginData.ExecutablePath1"/>, <see cref="MultiLauncherPluginData.ExecutablePath2"/>, 
        ''' <see cref="MultiLauncherPluginData.ExecutablePath3"/>, <see cref="MultiLauncherPluginData.ExecutablePath4"/>, 
        ''' <see cref="MultiLauncherPluginData.ExecutablePath5"/>, <see cref="MultiLauncherPluginData.ExecutablePath6"/>, 
        ''' <see cref="MultiLauncherPluginData.ExecutablePath7"/>, <see cref="MultiLauncherPluginData.ExecutablePath8"/> and 
        ''' <see cref="MultiLauncherPluginData.ExecutablePath9"/> when exiting from SmartBot application.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("Terminate programs when exiting from SmartBot.")>
        Public Property TerminateProgramsWhenClosingSmartBot As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (1) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Executable Paths")>
        <DisplayName("(1) Path to a file or program that you want run.")>
        Public Property ExecutablePath1 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (2) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Executable Paths")>
        <DisplayName("(2) Path to a file or program that you want run.")>
        Public Property ExecutablePath2 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (3) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Executable Paths")>
        <DisplayName("(3) Path to a file or program that you want run.")>
        Public Property ExecutablePath3 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (4) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Executable Paths")>
        <DisplayName("(4) Path to a file or program that you want run.")>
        Public Property ExecutablePath4 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (5) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Executable Paths")>
        <DisplayName("(5) Path to a file or program that you want run.")>
        Public Property ExecutablePath5 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (6) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Executable Paths")>
        <DisplayName("(6) Path to a file or program that you want run.")>
        Public Property ExecutablePath6 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (7) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Executable Paths")>
        <DisplayName("(7) Path to a file or program that you want run.")>
        Public Property ExecutablePath7 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (8) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Executable Paths")>
        <DisplayName("(8) Path to a file or program that you want run.")>
        Public Property ExecutablePath8 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (9) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Executable Paths")>
        <DisplayName("(9) Path to a file or program that you want run.")>
        Public Property ExecutablePath9 As String

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="MultiLauncherPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name
        End Sub

#End Region

    End Class

End Namespace

#End Region
