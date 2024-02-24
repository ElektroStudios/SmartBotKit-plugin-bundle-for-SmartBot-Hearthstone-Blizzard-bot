

' ****************************************************************************
' This source-code belongs to the 'Challenge Notifier' plugin, which is discontinued.
' ****************************************************************************


#If DEBUG Then
Imports SmartBot.Plugins.API
#End If

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Imaging

Imports SmartBotKit.Imaging
Imports SmartBotKit.Interop.Win32

Imports HearthWatcher.Providers

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop


    Public NotInheritable Class HearthMirrorFriendlyChallengeProvider : Implements IFriendlyChallengeProvider

        ' ReSharper disable InconsistentNaming

        Private ReadOnly Colors As New List(Of Color) ' partial set of colors of the 80 gold image in Global.System.Resources.

        ' ReSharper restore InconsistentNaming

        Public ReadOnly Property DialogVisible() As Boolean Implements IFriendlyChallengeProvider.DialogVisible
            Get
                Return HearthMirror.Reflection.IsFriendlyChallengeDialogVisible()
            End Get
        End Property

        Public ReadOnly Property FriendChallengeType As FriendChallengeType
            Get
                If (Me.DialogVisible) Then
                    Return Me.GetFriendChallengeType()
                Else
                    Return FriendChallengeType.Undetermined
                End If
            End Get
        End Property

        Public Sub New()
            ' yellows
            Me.Colors.Add(Color.FromArgb(255, 252, 247, 140))
            Me.Colors.Add(Color.FromArgb(255, 254, 233, 113))
            Me.Colors.Add(Color.FromArgb(255, 255, 247, 140))
            Me.Colors.Add(Color.FromArgb(255, 251, 210, 79))
            Me.Colors.Add(Color.FromArgb(255, 255, 213, 83))

            ' oranges
            Me.Colors.Add(Color.FromArgb(255, 255, 170, 45))
            Me.Colors.Add(Color.FromArgb(255, 255, 166, 41))
            Me.Colors.Add(Color.FromArgb(255, 232, 140, 33))
            Me.Colors.Add(Color.FromArgb(255, 223, 121, 21))
            Me.Colors.Add(Color.FromArgb(255, 233, 146, 40))

            ' browns
            Me.Colors.Add(Color.FromArgb(255, 85, 68, 44))
            Me.Colors.Add(Color.FromArgb(255, 60, 47, 30))
            Me.Colors.Add(Color.FromArgb(255, 71, 58, 38))
            Me.Colors.Add(Color.FromArgb(255, 114, 92, 64))
            Me.Colors.Add(Color.FromArgb(255, 50, 40, 25))
        End Sub

        Private Function GetFriendChallengeType() As FriendChallengeType

            Dim p As Process = HearthstoneUtil.Process
            If (p Is Nothing) OrElse (p.HasExited) Then
                Return FriendChallengeType.Undetermined
            End If

            If Not NativeMethods.IsWindowVisible(p.MainWindowHandle) Then
                Return FriendChallengeType.Undetermined
            End If

            Dim pxInfoCollection As IEnumerable(Of PixelInfo) = Nothing
            Dim screenshot As Bitmap = ImageUtil.TakeScreenshotFromObject(p.MainWindowHandle, PixelFormat.Format32bppArgb)
            Dim resizedScreenshot As Bitmap = SmartBotKit.Extensions.BitmapExtensions.Resize(screenshot, New Size(640, 480))

            pxInfoCollection = SmartBotKit.Extensions.BitmapExtensions.GetPixelInfo(resizedScreenshot)

            Dim foundColors As New HashSet(Of Color)

            For Each pxInfo As PixelInfo In pxInfoCollection
                For Each c As Color In Me.Colors
                    If (pxInfo.Color = c) Then
                        foundColors.Add(c)
                    End If
                Next c
            Next pxInfo

            resizedScreenshot.Dispose()
            screenshot.Dispose()

#If DEBUG Then
            For Each c As Color In foundColors
                Bot.Log(String.Format("[Challenge Notifier] -> Found Color: {0}", c.ToString()))
            Next
#End If

            If (foundColors.Count >= 4) Then ' 4 found colors it seems enough accuracy in most cases.
                Return FriendChallengeType.GoldQuest
            Else
                Return FriendChallengeType.Normal
            End If

        End Function

    End Class

End Namespace