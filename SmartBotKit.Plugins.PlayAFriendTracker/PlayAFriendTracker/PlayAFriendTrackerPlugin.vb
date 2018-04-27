
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32

#End Region

#Region " PlayAFriendTrackerPlugin "

Namespace PlayAFriendTracker

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' This plugin will manages your friend list in Hearthstone.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class PlayAFriendTrackerPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As PlayAFriendTrackerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, PlayAFriendTrackerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="PlayAFriendTrackerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PlayAFriendTrackerPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="PlayAFriendTrackerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[PlayAFriendTracker] Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="PlayAFriendTrackerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[PlayAFriendTracker] Plugin enabled.")
                Else
                    Bot.Log("[PlayAFriendTracker] Plugin disabled.")
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

            End If

            MyBase.OnGameEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot timer is ticked, every 300 milliseconds.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTick()
            'If SmartBot.Plugins.API.Bot.IsChallengedByAFriendActive Then
            '    ' ...
            'End If

            MyBase.OnTick()
        End Sub

#End Region

#Region " Private Methods "

#End Region

    End Class

End Namespace

#End Region
