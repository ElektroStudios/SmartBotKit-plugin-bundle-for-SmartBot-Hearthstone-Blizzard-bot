
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq
Imports System.Media
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Imaging
Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32

Imports AForge.Imaging
Imports Image = System.Drawing.Image

#End Region

#Region " PlayAFriendChallengeNotifierPlugin "

Namespace PlayAFriendChallengeNotifier

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' This plugin will notify when a friend challenges you for the 'Play a Friend' quest.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class PlayAFriendChallengeNotifierPlugin : Inherits Plugin

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly AspectRatioDict As New Dictionary(Of Point, ImageCaptureInfo) From {
            {New Point(4, 3), New ImageCaptureInfo(New Point(4, 3), New Size(640, 480), New Rectangle(448, 216, 76, 56))}
        }

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The directory where to save the generated screenshots.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly stopWatch As Stopwatch

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="PlayAFriendChallengeNotifierPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

#If DEBUG Then
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The directory where to save the generated screenshots.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly screenshotsDir As New DirectoryInfo(Path.Combine(SmartBotUtil.PluginsDir.FullName, "Screenshot Samples"))

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the generated screenshot files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private screenshotCount As Integer

#End If

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
        Public Shadows ReadOnly Property DataContainer As PlayAFriendChallengeNotifierPluginData
            Get
                Return DirectCast(MyBase.DataContainer, PlayAFriendChallengeNotifierPluginData)
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PlayAFriendChallengeNotifierPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            'SmartBotKit.ReservedUse.UpdateUtil.CheckForSmartBotKitUpdates()

            Me.stopWatch = New Stopwatch()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="PlayAFriendChallengeNotifierPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[PlayAFriendChallengeNotifier] Plugin initialized.")
            End If
            Me.stopWatch.Start()
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="PlayAFriendChallengeNotifierPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Me.stopWatch.Restart()
                    Bot.Log("[PlayAFriendChallengeNotifier] Plugin enabled.")
                Else
                    Me.stopWatch.Reset()
                    Bot.Log("[PlayAFriendChallengeNotifier] Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot timer is ticked, every 300 milliseconds.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTick()
            If (Me.DataContainer.Enabled) AndAlso Not (Bot.IsBotRunning) Then

                If Not (Me.stopWatch.IsRunning) Then
                    Me.stopWatch.Start()
                    Exit Sub
                End If

                If (Me.stopWatch.Elapsed.TotalSeconds >= Me.DataContainer.Interval) Then
                    Me.stopWatch.Reset()
                    If Me.IsChallengeDetected() Then
                        Me.NotifyNewChallenge()
                        Thread.Sleep(TimeSpan.FromSeconds(60)) ' A break of 60 seconds to avoid detecting the same challenge invitation.
                    End If
                    Me.stopWatch.Start()
                End If

            Else
                Me.stopWatch.Reset()

            End If

            MyBase.OnTick()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether a 'Play a Friend' challenge invitation is detected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if a 'Play a Friend' challenge invitation is detected; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Function IsChallengeDetected() As Boolean

            Dim p As Process = HearthstoneUtil.Process
            If (p Is Nothing) OrElse (p.HasExited) Then
                Return False
            End If

            Dim currentWindowSize As Size = HearthstoneUtil.WindowSize

            If PlayAFriendChallengeNotifierPluginData.SupportedResolutions.ContainsKey(currentWindowSize) Then

                Dim aspectRatio As Point = PlayAFriendChallengeNotifierPluginData.SupportedResolutions(currentWindowSize)
                Dim captureInfo As ImageCaptureInfo = Me.AspectRatioDict(aspectRatio).Scale(currentWindowSize)

                Dim sizeFormat As String = String.Format("{0}x{1}", captureInfo.Resolution.Width, captureInfo.Resolution.Height)
                Dim resourceNameFormat As String = String.Format("_{0}", sizeFormat)
                Dim pixelFormat As PixelFormat = PixelFormat.Format24bppRgb
                Dim similarity As Double = 85.0R

                Using findImage As Image = DirectCast(My.Resources.ResourceManager.GetObject(resourceNameFormat), Image),
                      screenshot As Image = ImageUtil.TakeScreenshotFromObject(p.MainWindowHandle, pixelFormat),
                      croppedRegion As Image = Extensions.ImageExtensions.Crop(screenshot, captureInfo.CaptureRectangle)
#If DEBUG Then
                    If (Me.DataContainer.SaveScreenshots) Then
                        If Not Me.screenshotsDir.Exists Then
                            Me.screenshotsDir.Create()
                        End If

                        Dim numericSuffix As String = Interlocked.Increment(Me.screenshotCount).ToString().PadLeft(5, "0"c)
                        Dim screenshotFilenameformat As String = String.Format("Screenshot_{0}_{1}.bmp", sizeFormat, numericSuffix)
                        Dim croppedRegionFilenameformat As String = String.Format("CroppedRegion_{0}_{1}.bmp", sizeFormat, numericSuffix)

                        Bot.Log("[PlayAFriendChallengeNotifier] Screenshot Saved. ")
                        screenshot.Save(Path.Combine(Me.screenshotsDir.FullName, screenshotFilenameformat), ImageFormat.Bmp)
                        croppedRegion.Save(Path.Combine(Me.screenshotsDir.FullName, croppedRegionFilenameformat), ImageFormat.Bmp)
                    End If
#End If
                    Dim matches As TemplateMatch() = AForgeUtil.MatchImage(croppedRegion, findImage, similarity)
#If DEBUG Then
                    For Each match As TemplateMatch In matches
                        Bot.Log("[PlayAFriendChallengeNotifier] *DEBUG* AForge Rectangle: " & match.Rectangle.ToString())
                    Next match
#End If
                    Return matches.Any()

                End Using

            Else
                Return False

            End If

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Notifies a challenge.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub NotifyNewChallenge()

            Dim p As Process = HearthstoneUtil.Process
            If (p Is Nothing) OrElse (p.HasExited) Then
                Exit Sub
            End If

            If (Me.DataContainer.PlaySoundFile) Then
                Try
                    Using player As New SoundPlayer(Path.Combine(SmartBotUtil.PluginsDir.FullName, "PlayAFriendChallengeNotifier.wav"))
                        player.Play()
                    End Using

                Catch ex As Exception
                    Bot.Log("[PlayAFriendChallengeNotifier] Failed to play the sound file. Error message: " & ex.Message)

                End Try
            End If

            If (Me.DataContainer.SetHearthstoneWindowMaximized) Then
                NativeMethods.ShowWindow(p.MainWindowHandle, WindowState.Maximize)
            End If

            If (Me.DataContainer.SetHearthstoneWindowToForeground) Then
                NativeMethods.SetForegroundWindow(p.MainWindowHandle)
                NativeMethods.BringWindowToTop(p.MainWindowHandle)
                Interaction.AppActivate(p.Id) ' Double ensure the window get visible and focused, because I experienced sometimes it don't get.
            End If

        End Sub

#End Region

    End Class

End Namespace

#End Region
