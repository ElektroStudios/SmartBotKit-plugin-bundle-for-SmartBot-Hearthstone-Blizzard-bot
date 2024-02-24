
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

#Region " AppLauncherPluginData "

' ReSharper disable once CheckNamespace

Namespace AppLauncher

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="AppLauncherPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class AppLauncherPluginData : Inherits PluginDataContainer

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
                Return "Automates external files and programs execution" & ControlChars.NewLine &
                       "at SmartBot's startup."
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

#Region " Behavior "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to terminate the programs specified in properties: 
        ''' <see cref="AppLauncherPluginData.ExecutablePath1"/>, <see cref="AppLauncherPluginData.ExecutablePath2"/>, 
        ''' <see cref="AppLauncherPluginData.ExecutablePath3"/>, <see cref="AppLauncherPluginData.ExecutablePath4"/>, 
        ''' <see cref="AppLauncherPluginData.ExecutablePath5"/>, <see cref="AppLauncherPluginData.ExecutablePath6"/>, 
        ''' <see cref="AppLauncherPluginData.ExecutablePath7"/>, <see cref="AppLauncherPluginData.ExecutablePath8"/> and 
        ''' <see cref="AppLauncherPluginData.ExecutablePath9"/> when exiting from SmartBot application.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("Terminate all launched programs when exiting from SmartBot")>
        Public Property TerminateProgramsWhenClosingSmartBot As Boolean

#End Region

#Region " Application Paths "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (1) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Application Paths")>
        <DisplayName("(1) Full path to a file or program...")>
        Public Property ExecutablePath1 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (2) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Application Paths")>
        <DisplayName("(2) Full path to a file or program...")>
        Public Property ExecutablePath2 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (3) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Application Paths")>
        <DisplayName("(3) Full path to a file or program...")>
        Public Property ExecutablePath3 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (4) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Application Paths")>
        <DisplayName("(4) Full path to a file or program...")>
        Public Property ExecutablePath4 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (5) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Application Paths")>
        <DisplayName("(5) Full path to a file or program...")>
        Public Property ExecutablePath5 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (6) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Application Paths")>
        <DisplayName("(6) Full path to a file or program...")>
        Public Property ExecutablePath6 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (7) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Application Paths")>
        <DisplayName("(7) Full path to a file or program...")>
        Public Property ExecutablePath7 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (8) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Application Paths")>
        <DisplayName("(8) Full path to a file or program...")>
        Public Property ExecutablePath8 As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' (9) Gets or sets the full path to a file or program that you want run.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Application Paths")>
        <DisplayName("(9) Full path to a file or program...")>
        Public Property ExecutablePath9 As String

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="AppLauncherPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name
        End Sub

#End Region

    End Class

End Namespace

#End Region
