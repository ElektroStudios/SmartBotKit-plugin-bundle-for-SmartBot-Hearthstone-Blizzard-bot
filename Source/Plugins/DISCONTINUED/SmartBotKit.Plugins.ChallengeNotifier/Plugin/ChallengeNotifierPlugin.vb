
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics
Imports System.IO
Imports System.Media

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.ReservedUse

Imports HearthWatcher
Imports HearthWatcher.EventArgs
Imports HearthWatcher.Providers

#End Region

#Region " ChallengeNotifierPlugin "

Namespace ChallengeNotifier

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that notifies when a friend challenge is received, like the 'Play a Friend' challenge.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class ChallengeNotifierPlugin : Inherits Plugin

#Region " Private Fields "

        Private WithEvents ChallengeWatcher As FriendlyChallengeWatcher
        Private ReadOnly challengeProvider As HearthMirrorFriendlyChallengeProvider

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="ChallengeNotifierPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

#End Region

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
        Public Shadows ReadOnly Property DataContainer As ChallengeNotifierPluginData
            Get
                Return DirectCast(MyBase.DataContainer, ChallengeNotifierPluginData)
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="ChallengeNotifierPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            UpdateUtil.RunUpdaterExecutable()

            Me.challengeProvider = New HearthMirrorFriendlyChallengeProvider
            Me.ChallengeWatcher = New FriendlyChallengeWatcher(Me.challengeProvider)
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="ChallengeNotifierPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Challenge Notifier] -> Plugin initialized.")
            End If
            Me.ChallengeWatcher.Run()
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="ChallengeNotifierPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Me.ChallengeWatcher.Stop()
                    Me.ChallengeWatcher.Run()
                    Bot.Log("[Challenge Notifier] -> Plugin enabled.")
                Else
                    Me.ChallengeWatcher.Stop()
                    Bot.Log("[Challenge Notifier] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        Private Sub ChallengeWatcher_OnFriendlyChallenge(ByVal sender As Object, ByVal e As FriendlyChallengeEventArgs) Handles ChallengeWatcher.OnFriendlyChallenge
            If Not (Me.DataContainer.Enabled) Then
                Exit Sub
            End If

            Dim p As Process = HearthstoneUtil.Process
            If (p Is Nothing) OrElse (p.HasExited) Then
                Exit Sub
            End If

            If (Me.DataContainer.SetHearthstoneWindowMaximized) Then
                NativeMethods.ShowWindow(p.MainWindowHandle, NativeWindowState.Maximize)
            End If

            If (Me.DataContainer.SetHearthstoneWindowToForeground) Then
                NativeMethods.SetForegroundWindow(p.MainWindowHandle)
                NativeMethods.BringWindowToTop(p.MainWindowHandle)
                Interaction.AppActivate(p.Id) ' Double ensure the window get visible and focused, because I experienced sometimes it don't get.
            End If

            If (e.DialogVisible) Then
                Me.PlaySound(Me.challengeProvider.FriendChallengeType)
            Else
                ' Me.PlaySound(FriendChallengeType.Undetermined)
            End If
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Notifies a challenge.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub PlaySound(ByVal friendChallengeType As FriendChallengeType)

            If (Me.DataContainer.PlaySoundFile) Then
                Dim filename As String = String.Empty
                Select Case friendChallengeType

                    Case FriendChallengeType.Normal
                        filename = "ChallengeNotifier-Normal.wav"

                    Case FriendChallengeType.GoldQuest
                        filename = "ChallengeNotifier-PlayAFriend.wav"

                    Case FriendChallengeType.Undetermined
                        filename = "ChallengeNotifier-Undetermined.wav"

                    Case Else
                        ' Do nothing.
                End Select

                Try
                    Using player As New SoundPlayer(Path.Combine(SmartBotUtil.PluginsDir.FullName, filename))
                        player.Play()
                    End Using

                Catch ex As Exception
                    Bot.Log("[Challenge Notifier] -> Failed to play the sound file. Error message: " & ex.Message)

                End Try
            End If

        End Sub

#End Region

    End Class

End Namespace

#End Region
