
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

#Region " PlayAFriendTrackerPluginData "

Namespace PlayAFriendTracker

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="PlayAFriendTrackerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class PlayAFriendTrackerPluginData : Inherits PluginDataContainer

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should...
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Test")>
        Public Property Test As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PlayAFriendTrackerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name
        End Sub

#End Region

    End Class

End Namespace

#End Region
