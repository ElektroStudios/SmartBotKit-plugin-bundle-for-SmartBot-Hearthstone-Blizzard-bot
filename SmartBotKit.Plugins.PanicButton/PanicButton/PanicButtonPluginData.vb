
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Reflection
Imports System.Windows.Forms

Imports SmartBot.Plugins
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.IO

#End Region

#Region " PanicButtonPluginData "

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

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the bot should be stopped when the hotkey is pressed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("The hotkey should stop the bot.")>
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
        <Category("Behavior")>
        <DisplayName("The hotkey should terminate SmartBot process.")>
        Public Property KillProcess As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the primary keyboard modifier key.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Modifier Keys")>
        <DisplayName("The first keyboard modifier key.")>
        <ItemsSource(GetType(HotkeyModifiersSource))>
        Public Property ModifierA As HotkeyModifiers

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the secondary keyboard modifier key.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Modifier Keys")>
        <DisplayName("The second keyboard modifier key.")>
        <ItemsSource(GetType(HotkeyModifiersSource))>
        Public Property ModifierB As HotkeyModifiers

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the keyboard key.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Key")>
        <DisplayName("The keyboard key.")>
        <ItemsSource(GetType(KeysSource))>
        Public Property Key As Keys

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PanicButtonPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name
        End Sub

#End Region

    End Class

End Namespace

#End Region
