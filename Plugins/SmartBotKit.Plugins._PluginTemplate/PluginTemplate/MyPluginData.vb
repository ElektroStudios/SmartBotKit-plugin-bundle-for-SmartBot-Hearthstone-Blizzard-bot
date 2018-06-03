
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

#Region " MyPluginData "

Namespace PluginTemplate

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="MyPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class MyPluginData : Inherits PluginDataContainer

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether... { add description here }
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("A test property.")>
        <Browsable(True)>
        <ItemsSource(GetType(SmartBotEvent))>
        Public Property TestProperty As SmartBotEvent

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="MyPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name
        End Sub

#End Region

    End Class

End Namespace

#End Region
