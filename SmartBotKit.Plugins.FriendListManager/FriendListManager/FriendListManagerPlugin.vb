
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32

#End Region

#Region " FriendListManagerPlugin "

Namespace FriendListManager

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' This plugin will manages your friend list in Hearthstone.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class FriendListManagerPlugin : Inherits Plugin

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin's data container.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The plugin's data container.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shadows ReadOnly Property DataContainer As FriendListManagerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, FriendListManagerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="FriendListManagerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="FriendListManagerPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="FriendListManagerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log(String.Format("[FriendListManager] Plugin initialized. Current friends count: {0}", Bot.GetFriends.Count()))
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="FriendListManagerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log(String.Format("[FriendListManager] Plugin enabled. Current friends count: {0}", Bot.GetFriends.Count()))
                Else
                    Bot.Log("[FriendListManager] Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameEnd()
            If (Me.DataContainer.Enabled) Then

                If (Me.DataContainer.InviteOpponents) Then
                    Dim id As Long = Bot.GetPreviousOpponentId()
                    ' Bot.AddFriend()
                End If

                If (Me.DataContainer.RemoveInactiveFriends) Then
                    For Each dude As [Friend] In Bot.GetFriends()
                        'If dude.GetBattleTag...
                    Next
                End If

            End If

            MyBase.OnGameEnd()
        End Sub



#End Region

#Region " Private Methods "

#End Region

    End Class

End Namespace

#End Region
