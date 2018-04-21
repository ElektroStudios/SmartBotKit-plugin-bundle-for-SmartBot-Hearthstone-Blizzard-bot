
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

#Region " FriendListManagerPluginData "

Namespace FriendListManager

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="FriendListManagerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class FriendListManagerPluginData : Inherits PluginDataContainer

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should automatically send a friend invitation to opponents.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Automation")>
        <DisplayName("Send friend invitation to the opponent.")>
        Public Property InviteOpponents As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin should automatically remove inactive friends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Automation")>
        <DisplayName("Remove inactive friends.")>
        Public Property RemoveInactiveFriends As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the maximum number of inactive days that a friend can have 
        ''' before being removed from the friend list.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The maximum number of inactive days that a friend can have before being removed from the friend list.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Global Settings")>
        <DisplayName("The maximum number of inactive days that a friend can have before being removed from the friend list.")>
        <Browsable(True)>
        Public Property MaxInactiveDays() As Integer
            Get
                Return Me.maxInactiveDaysB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.maxInactiveDaysB = 1
                ElseIf (value > 300) Then
                    Me.maxInactiveDaysB = 300
                Else
                    Me.maxInactiveDaysB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The maximum number of inactive days that a friend can have before being removed from the friend list.
        ''' </summary>
        Private maxInactiveDaysB As Integer

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="FriendListManagerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name
        End Sub

#End Region

    End Class

End Namespace

#End Region
