
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Text
Imports System.Windows.Forms

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.Plugins.UIEnhancer

#End Region

#Region " SystemTrayIcon "

' ReSharper disable once CheckNamespace

Namespace UIEnhancer

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents a <see cref="Windows.Forms.NotifyIcon"/> for SmartBot program.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class SystemTrayIcon : Implements IDisposable

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The <see cref="Windows.Forms.NotifyIcon"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents NotifyIcon As NotifyIcon

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The <see cref="ContextMenuStrip"/> used by <see cref="SystemTrayIcon.NotifyIcon"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents Menu As ContextMenuStrip

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The menu item used to show/restore the Hearthstone window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents MenuItemShowHS As ToolStripMenuItem

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The menu item used to hide the Hearthstone window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents MenuItemHideHS As ToolStripMenuItem

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The menu item used to kill the Hearthstone process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents MenuItemCloseHS As ToolStripMenuItem

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The menu item used to show/restore the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents MenuItemShowSB As ToolStripMenuItem

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The menu item used to hide the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents MenuItemHideSB As ToolStripMenuItem

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The menu item used to kill the SmartBot process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents MenuItemCloseSB As ToolStripMenuItem

        Private ReadOnly ownerPlugin As UIEnhancerPlugin

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="SystemTrayIcon"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub New(ByVal ownerPlugin As UIEnhancerPlugin)
            Me.ownerPlugin = ownerPlugin

            Me.MenuItemShowHS = New ToolStripMenuItem("Show Hearthstone", My.Resources.ShowHS) With {.Visible = False}
            Me.MenuItemHideHS = New ToolStripMenuItem("Hide Hearthstone", My.Resources.HideHS) With {.Visible = False}
            Me.MenuItemCloseHS = New ToolStripMenuItem("Close Hearthstone", My.Resources.Close) With {.Visible = False}

            Me.MenuItemShowSB = New ToolStripMenuItem("Show SmartBot", My.Resources.ShowSB) With {.Visible = False}
            Me.MenuItemHideSB = New ToolStripMenuItem("Hide SmartBot", My.Resources.HideSB) With {.Visible = False}
            Me.MenuItemCloseSB = New ToolStripMenuItem("Close SmartBot", My.Resources.Close) With {.Visible = True}

            Me.Menu = New ContextMenuStrip()
            Me.Menu.Items.AddRange({Me.MenuItemShowHS, Me.MenuItemHideHS, Me.MenuItemCloseHS,
                                   Me.MenuItemShowSB, Me.MenuItemHideSB, Me.MenuItemCloseSB})

            Me.NotifyIcon = New NotifyIcon() With {
                .Text = "SmartBot",
                .Icon = My.Resources.SB,
                .ContextMenuStrip = Me.Menu,
                .Visible = True
            }

            Bot.Log("[UI Enhancer] -> System-Tray Icon initialized.")
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Toggles (hide or show) the visibility state of the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub ToggleSmartBotWindowVisibility()
            Me.Menu.Visible = False

            Dim isSbWindowVisible As Boolean = NativeMethods.IsWindowVisible(SmartBotUtil.Process.MainWindowHandle)

            If (isSbWindowVisible) Then
                Me.MenuItemHideSB.Visible = True
                Me.MenuItemHideSB.PerformClick()

            Else
                Me.MenuItemShowSB.Visible = True
                Me.MenuItemShowSB.PerformClick()

            End If
        End Sub

#End Region

#Region " Event Handlers "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ContextMenuStrip.Opening"/> event of the <see cref="SystemTrayIcon.Menu"/> component.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="e">
        ''' The <see cref="CancelEventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub Menu_Opening(sender As Object, e As CancelEventArgs) Handles Menu.Opening

            ' Hearthstone Commands
            If (HearthstoneUtil.Process Is Nothing) OrElse Not (Me.ownerPlugin.DataContainer.ShowHearthstoneMenuCommands) Then
                Me.MenuItemHideHS.Visible = False
                Me.MenuItemShowHS.Visible = False
                Me.MenuItemCloseHS.Visible = False

            Else
                Dim isHsWindowVisible As Boolean = NativeMethods.IsWindowVisible(HearthstoneUtil.Process.MainWindowHandle)
                Me.MenuItemHideHS.Visible = isHsWindowVisible
                Me.MenuItemShowHS.Visible = Not isHsWindowVisible
                Me.MenuItemCloseHS.Visible = True

            End If

            ' SmartBot Commands
            If Not (Me.ownerPlugin.DataContainer.ShowSmartBotMenuCommands) Then
                Me.MenuItemHideSB.Visible = False
                Me.MenuItemShowSB.Visible = False
                Me.MenuItemCloseSB.Visible = False

            Else
                Dim isSbWindowVisible As Boolean = NativeMethods.IsWindowVisible(SmartBotUtil.Process.MainWindowHandle)
                Me.MenuItemHideSB.Visible = isSbWindowVisible
                Me.MenuItemShowSB.Visible = Not isSbWindowVisible
                Me.MenuItemCloseSB.Visible = True

            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="System.Windows.Forms.NotifyIcon.MouseMove"/> event of the <see cref="SystemTrayIcon.NotifyIcon"/> component.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        '''
        ''' <param name="e">
        ''' The <see cref="MouseEventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub NotifyIcon_MouseMove(sender As Object, e As MouseEventArgs) Handles NotifyIcon.MouseMove

            Dim wins As Integer = API.Statistics.Wins
            Dim losses As Integer = API.Statistics.Losses
            Dim concedes As Integer = API.Statistics.Conceded
            Dim winsRatio As Double = ((wins / (wins + losses)) * 100)

            If Double.IsNaN(winsRatio) Then
                winsRatio = 0R
            End If

            Dim str As String
            Select Case winsRatio
                Case = 0
                    str = $"{wins}W {losses}L {concedes}C - WR: 0%"
                Case = 100.0R
                    str = $"{wins}W {losses}L {concedes}C - WR: 100%"
                Case Else
                    str = $"{wins}W {losses}L {concedes}C - WR: {winsRatio:F2}%"
            End Select

            Dim sb As New StringBuilder(64, 64) ' 64 character length is the max. capacity allowed for a NotifyIcon.
            With sb
                .AppendLine("SmartBot")
                .AppendLine()
                .AppendLine(str)
            End With

            DirectCast(sender, NotifyIcon).Text = sb.ToString()

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="System.Windows.Forms.NotifyIcon.MouseDoubleClick"/> event of the <see cref="SystemTrayIcon.NotifyIcon"/> component.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        '''
        ''' <param name="e">
        ''' The <see cref="MouseEventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub NotifyIcon_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles NotifyIcon.MouseDoubleClick

            Me.ToggleSmartBotWindowVisibility()

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ToolStripMenuItem.Click"/> event of the <see cref="SystemTrayIcon.MenuItemShowSB"/> component.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub MenuItemShowHS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItemShowHS.Click

            If (HearthstoneUtil.Process Is Nothing) Then
                Exit Sub
            End If

            Dim hwnd As IntPtr = HearthstoneUtil.Process.MainWindowHandle

            Dim placement As WindowPlacement
            NativeMethods.GetWindowPlacement(hwnd, placement)

            Dim newWindowState As NativeWindowState
            Select Case placement.WindowState
                Case NativeWindowState.Minimize, NativeWindowState.ForceMinimize, NativeWindowState.ShowMinimized
                    newWindowState = NativeWindowState.Restore
                Case NativeWindowState.Maximize, NativeWindowState.ShowMaximized
                    newWindowState = NativeWindowState.ShowMaximized
                Case Else
                    newWindowState = NativeWindowState.Normal
            End Select

            NativeMethods.ShowWindow(hwnd, newWindowState)
            NativeMethods.SetForegroundWindow(hwnd) ' Bring window to top and avtivate input.
            Bot.Log($"[UI Enhancer] -> Hearthstone window state changed to: {newWindowState.ToString()}")

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ToolStripMenuItem.Click"/> event of the <see cref="SystemTrayIcon.MenuItemHideHS"/> component.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub MenuItemHideHS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItemHideHS.Click

            Try
                NativeMethods.ShowWindow(HearthstoneUtil.Process.MainWindowHandle, NativeWindowState.Hide)
                Bot.Log($"[UI Enhancer] -> Hearthstone window state changed to: {NameOf(NativeWindowState.Hide)}")

            Catch ex As Exception
                Bot.Log("[UI Enhancer] -> Failed to hide Hearthstone window.")
                Bot.Log($"[UI Enhancer] -> Exception message. {ex.Message}")

            End Try

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ToolStripMenuItem.Click"/> event of the <see cref="SystemTrayIcon.MenuItemCloseHS"/> component.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub MenuItemCloseHS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItemCloseHS.Click

            Bot.Log("[UI Enhancer] -> Closing Hearthstone...")

            Me.MenuItemShowHS.PerformClick() ' Restore Hearthstone window visibility.

            Try
                ' HearthstoneUtil.Process.Kill()
                Bot.CloseHs()

            Catch ex As Exception
                Bot.Log($"[UI Enhancer] -> {ex.Message}")

            End Try

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ToolStripMenuItem.Click"/> event of the <see cref="SystemTrayIcon.MenuItemShowSB"/> component.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub MenuItemShowS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItemShowSB.Click

            Dim placement As WindowPlacement
            NativeMethods.GetWindowPlacement(SmartBotUtil.Process.MainWindowHandle, placement)

            Select Case placement.WindowState
                Case NativeWindowState.Minimize, NativeWindowState.ForceMinimize, NativeWindowState.ShowMinimized
                    NativeMethods.ShowWindow(SmartBotUtil.Process.MainWindowHandle, NativeWindowState.Restore)
                    Bot.Log(
                        $"[UI Enhancer] -> SmartBot window state changed to: {NameOf(NativeWindowState.Restore)}")

                Case NativeWindowState.Maximize, NativeWindowState.ShowMaximized
                    NativeMethods.ShowWindow(SmartBotUtil.Process.MainWindowHandle, NativeWindowState.ShowMaximized)
                    Bot.Log(
                        $"[UI Enhancer] -> SmartBot window state changed to: { _
                               NameOf(NativeWindowState.ShowMaximized)}")

                Case Else
                    NativeMethods.ShowWindow(SmartBotUtil.Process.MainWindowHandle, NativeWindowState.Normal)
                    Bot.Log(
                        $"[UI Enhancer] -> SmartBot window state changed to: {NameOf(NativeWindowState.Normal)}")

            End Select
            NativeMethods.SetForegroundWindow(SmartBotUtil.Process.MainWindowHandle) ' Bring window to top and avtivate input.

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ToolStripMenuItem.Click"/> event of the <see cref="SystemTrayIcon.MenuItemHideSB"/> component.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub MenuItemHideS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItemHideSB.Click

            NativeMethods.ShowWindow(SmartBotUtil.Process.MainWindowHandle, NativeWindowState.Hide)

            Bot.Log($"[UI Enhancer] -> SmartBot window state changed to: {NameOf(NativeWindowState.Hide)}")

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ToolStripMenuItem.Click"/> event of the <see cref="SystemTrayIcon.MenuItemCloseSB"/> component.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub MenuItemCloseS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItemCloseSB.Click

            Bot.Log("[UI Enhancer] -> Closing SmartBot...")

            Me.MenuItemShowHS.PerformClick() ' Restore Hearthstone window visibility.
            Me.NotifyIcon.Visible = False

            Bot.StopBot()
            Bot.StopRelogger()
            Bot.CloseBot()

        End Sub

#End Region

#Region " IDisposable Implementation "

        ' ReSharper disable InconsistentNaming
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Flag to detect redundant calls when disposing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private isDisposed As Boolean = False
        ' ReSharper restore InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="SystemTrayIcon"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(isDisposing:=True)
            ' GC.SuppressFinalize(obj:=Me)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged Global.System.Resources.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="isDisposing">
        ''' <see langword="True"/>  to release both managed and unmanaged Global.System.Resources.
        ''' <see langword="False"/> to release only unmanaged Global.System.Resources.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub Dispose(ByVal isDisposing As Boolean)
            If (Not Me.isDisposed) AndAlso (isDisposing) Then
                Me.MenuItemShowHS.Dispose()
                Me.MenuItemHideHS.Dispose()
                Me.MenuItemCloseHS.Dispose()

                Me.MenuItemShowSB.Dispose()
                Me.MenuItemHideSB.Dispose()
                Me.MenuItemCloseSB.Dispose()

                Me.Menu.Dispose()
                Me.NotifyIcon.Visible = False
                Me.NotifyIcon.Dispose()
                Bot.Log("[UI Enhancer] -> System-Tray Icon deinitialized.")
            End If

            Me.isDisposed = True
        End Sub

#End Region

    End Class

End Namespace

#End Region
