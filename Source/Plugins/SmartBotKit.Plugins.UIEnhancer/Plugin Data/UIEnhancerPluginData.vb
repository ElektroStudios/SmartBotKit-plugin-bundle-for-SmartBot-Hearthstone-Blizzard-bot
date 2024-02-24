#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Reflection

Imports SmartBot.Plugins

Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32

#End Region

#Region " UIEnhancerPluginData "

' ReSharper disable once CheckNamespace

Namespace UIEnhancer

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="UIEnhancerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class UIEnhancerPluginData : Inherits PluginDataContainer

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
                Return "Adds visual enhancements for the SmartBot user-interface."
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

#Region " TaskBar "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether .
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("TaskBar")>
        <DisplayName("Enable TaskBar Enhancements")>
        Public Property EnableTaskBarEnhancements As Boolean

#End Region

#Region " System-Tray Icon "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the system-tray icon for SmartBot is enabled.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("System-Tray Icon")>
        <DisplayName("Enable System-Tray Icon")>
        Public Property EnableSystrayIcon As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the systray icon should display Hearthstone menu commands.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("System-Tray Icon")>
        <DisplayName("Show menu commands for Hearthstone")>
        Public Property ShowHearthstoneMenuCommands As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the systray icon should display SmartBot menu commands.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("System-Tray Icon")>
        <DisplayName("Show menu commands for SmartBot")>
        Public Property ShowSmartBotMenuCommands As Boolean

#End Region

#Region " Window Position Restorator "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether .
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Window Position Restorator")>
        <DisplayName("Save and restore SmartBot window position")>
        Public Property EnableWindowPositionRestorator As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current SmartBot's window position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Window Position Restorator")>
        <DisplayName("Current SmartBot's window position")>
        <Browsable(True)>
        Public ReadOnly Property CurrentPosition As Point
            Get
                Return Me.GetWindowPosition()
            End Get
        End Property

#Region " Reserved "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window position in its normal state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Window Position Restorator - Reserved")>
        <Browsable(False)>
        Public Property NormalPosition As Point

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window size in its normal state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Window Position Restorator - Reserved")>
        <Browsable(False)>
        Public Property NormalSize As Size

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window position in its maximized state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Window Position Restorator - Reserved")>
        <Browsable(False)>
        Public Property MaximizedPosition As Point

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window position in its minimized state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Window Position Restorator - Reserved")>
        <Browsable(False)>
        Public Property MinimizedPosition As Point

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Window Position Restorator - Reserved")>
        <Browsable(False)>
        Public Property WindowState As NativeWindowState

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window flags.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Window Position Restorator - Reserved")>
        <Browsable(False)>
        Public Property Flags As Integer

#End Region

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="UIEnhancerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.EnableTaskBarEnhancements = True
            Me.EnableSystrayIcon = True
            Me.EnableWindowPositionRestorator = True

            Me.ShowHearthstoneMenuCommands = True
            Me.ShowSmartBotMenuCommands = True
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current SmartBot's window position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Point"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function GetWindowPosition() As Point

            Dim p As Process = SmartBotUtil.Process

            Dim wpl As New WindowPlacement
            Dim success As Boolean = NativeMethods.GetWindowPlacement(p.MainWindowHandle, wpl)

            Dim position As Point
            Select Case wpl.WindowState

                Case NativeWindowState.Minimize, NativeWindowState.ShowMinimized, NativeWindowState.ForceMinimize
                    position = wpl.MinPosition
                    If (position.X = -1) AndAlso (position.Y = -1) Then
                        position = Point.Empty
                    End If

                Case NativeWindowState.Maximize, NativeWindowState.ShowMaximized
                    position = wpl.MaxPosition
                    If (position.X = -1) AndAlso (position.Y = -1) Then
                        position = Point.Empty
                    End If

                Case Else
                    position = CType(wpl.NormalPosition, Rectangle).Location

            End Select

            Return position

        End Function

#End Region

    End Class

End Namespace

#End Region
