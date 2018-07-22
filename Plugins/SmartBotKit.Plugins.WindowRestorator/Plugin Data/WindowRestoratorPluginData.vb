
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Reflection
Imports Microsoft.VisualBasic.ApplicationServices
Imports SmartBot.Plugins

Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32

#End Region

#Region " WindowRestoratorPluginData "

Namespace WindowRestorator

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="WindowRestoratorPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class WindowRestoratorPluginData : Inherits PluginDataContainer

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

#Region " Info "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current SmartBot's window position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Info")>
        <DisplayName("Current SmartBot's window position.")>
        <Browsable(True)>
        Public ReadOnly Property CurrentPosition As Point
            Get
                Return Me.GetWindowPosition()
            End Get
        End Property

#End Region

#Region " Reserved "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window position in its normal state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public Property NormalPosition As Point

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window size in its normal state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public Property NormalSize As Size

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window position in its maximized state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public Property MaximizedPosition As Point

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window position in its minimized state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public Property MinimizedPosition As Point

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public Property WindowState As WindowState

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot's window flags.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reserved")>
        <Browsable(False)>
        Public Property Flags As Integer

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="WindowRestoratorPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.AssemblyInfo.AssemblyName
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

                Case WindowState.Minimize, WindowState.ShowMinimized, WindowState.ForceMinimize
                    position = wpl.MinPosition
                    If (position.X = -1) AndAlso (position.Y = -1) Then
                        position = Point.Empty
                    End If

                Case WindowState.Maximize, WindowState.ShowMaximized
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
