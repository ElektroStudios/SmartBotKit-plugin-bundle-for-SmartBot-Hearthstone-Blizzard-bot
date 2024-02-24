
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
Imports System.Windows.Forms

Imports SmartBot.Plugins
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.IO
Imports SmartBotKit.Interop

#End Region

#Region " PanicButtonPluginData "

' ReSharper disable once CheckNamespace

Namespace PanicButton

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="PanicButtonPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class PanicButtonPluginData : Inherits PluginDataContainer

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
                Return "Stops or terminates SmartBot process" & ControlChars.NewLine &
                       "when a specified hotkey combination is pressed."
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
        ''' Gets or sets a value that determine whether the bot should be stopped when the hotkey is pressed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior (when the hotkey is pressed)")>
        <DisplayName("Stop bot")>
        Public ReadOnly Property StopBot As Boolean
            Get
                Return True
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether SmartBot process should be terminated when the hotkey is pressed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior (when the hotkey is pressed)")>
        <DisplayName("Terminate SmartBot process")>
        Public Property KillProcess As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the new computer state when the hotkey is pressed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior (when the hotkey is pressed)")>
        <DisplayName("Set the computer state")>
        <ItemsSource(GetType(ComputerStateSource))>
        Public Property SetComputerState As ComputerState

#End Region

#Region " Hotkey "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the keyboard key.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Hotkey")>
        <DisplayName("Main key")>
        <ItemsSource(GetType(KeysSource))>
        Public Property Key As Keys

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the primary keyboard modifier key.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Hotkey")>
        <DisplayName("1st modifier key")>
        <ItemsSource(GetType(HotkeyModifiersSource))>
        Public Property ModifierA As HotkeyModifiers

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the secondary keyboard modifier key.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Hotkey")>
        <DisplayName("2nd modifier key")>
        <ItemsSource(GetType(HotkeyModifiersSource))>
        Public Property ModifierB As HotkeyModifiers

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PanicButtonPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.KillProcess = False
            Me.SetComputerState = ComputerState.NoChange

            Me.ModifierA = HotkeyModifiers.Control
            Me.ModifierB = HotkeyModifiers.Alt
            Me.Key = Keys.F1
        End Sub

#End Region

    End Class

End Namespace

#End Region
