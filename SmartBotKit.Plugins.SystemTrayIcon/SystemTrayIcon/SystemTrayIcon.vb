
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

Imports SmartBotKit.Interop.Win32

#End Region

#Region " SystemTrayIcon "

Namespace SystemTrayIcon

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents a <see cref="Windows.Forms.NotifyIcon"/> for SmartBot program.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class SystemTrayIcon : Implements IDisposable

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the animation effect to fade in the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property FxFadeIn As WindowAnimation

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the animation effect to fade out the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property FxFadeOut As WindowAnimation

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The <see cref="Windows.Forms.NotifyIcon"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents NotifyIcon As NotifyIcon

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The <see cref="Windows.Forms.ContextMenuStrip"/> used by <see cref="SystemTrayIcon.NotifyIcon"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents Menu As ContextMenuStrip

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The menu item used to show/restore the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents MenuItemShow As ToolStripMenuItem

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The menu item used to hide the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents MenuItemHide As ToolStripMenuItem

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The menu item used to kill the SmartBot process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private WithEvents MenuItemExit As ToolStripMenuItem

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The plugin's data container.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private dataContainer As SystemTrayIconPluginData

        Private p As Process

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="SystemTrayIcon"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub New(ByVal dataContainer As SystemTrayIconPluginData)
            Me.p = Process.GetCurrentProcess()
            Me.dataContainer = dataContainer
            Me.FxFadeIn = dataContainer.FxFadeIn
            Me.FxFadeOut = dataContainer.FxFadeOut

            Me.MenuItemShow = New ToolStripMenuItem("Show", My.Resources.MenuItemShow)
            Me.MenuItemHide = New ToolStripMenuItem("Hide", My.Resources.MenuItemHide)
            Me.MenuItemExit = New ToolStripMenuItem("Exit", My.Resources.MenuItemExit)

            Me.Menu = New ContextMenuStrip()
            Me.Menu.Items.AddRange({Me.MenuItemShow, Me.MenuItemHide, Me.MenuItemExit})

            Me.NotifyIcon = New NotifyIcon() With {
                .Text = "SmartBot",
                .Icon = My.Resources.SB,
                .ContextMenuStrip = Me.Menu,
                .Visible = True
            }

            Bot.Log("[SystemTrayIcon] Plugin initialized.")
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Toggles (hide or show) the visibility state of the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub ToggleWindowVisibility()
            Me.Menu.Visible = False

            Dim isWindowVisible As Boolean = NativeMethods.IsWindowVisible(Me.p.MainWindowHandle)

            If (isWindowVisible) Then
                Me.MenuItemHide.Enabled = True
                Me.MenuItemHide.PerformClick()

            Else
                Me.MenuItemShow.Enabled = True
                Me.MenuItemShow.PerformClick()

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

            Me.MenuItemHide.Enabled = NativeMethods.IsWindowVisible(Me.p.MainWindowHandle)
            Me.MenuItemShow.Enabled = Not Me.MenuItemHide.Enabled

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="Windows.Forms.NotifyIcon.MouseMove"/> event of the <see cref="SystemTrayIcon.NotifyIcon"/> component.
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
            Dim concedes As Integer = API.Statistics.ConcededTotal
            Dim winsRatio As Double = ((wins / (wins + losses + concedes)) * 100)

            If Double.IsNaN(winsRatio) Then
                winsRatio = 0R
            End If

            Dim str As String
            Select Case winsRatio
                Case = 0
                    str = String.Format("{0}W {1}L {2}C - WR: 0%", wins, losses, concedes)
                Case = 100.0R
                    str = String.Format("{0}W {1}L {2}C - WR: 100%", wins, losses, concedes)
                Case Else
                    str = String.Format("{0}W {1}L {2}C - WR: {3:F2}%", wins, losses, concedes, winsRatio)
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
        ''' Handles the <see cref="Windows.Forms.NotifyIcon.MouseDoubleClick"/> event of the <see cref="SystemTrayIcon.NotifyIcon"/> component.
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

            Me.ToggleWindowVisibility()

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ToolStripMenuItem.Click"/> event of the <see cref="SystemTrayIcon.MenuItemHide"/> component.
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
        Private Sub MenuItemHide_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItemHide.Click

            NativeMethods.ShowWindow(Me.p.MainWindowHandle, WindowState.Hide)

            Bot.Log("[SystemTrayIcon] Window visibility changed to hidden.")

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ToolStripMenuItem.Click"/> event of the <see cref="SystemTrayIcon.MenuItemShow"/> component.
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
        Private Sub MenuItemShow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItemShow.Click

            Dim placement As WindowPlacement
            NativeMethods.GetWindowPlacement(Me.p.MainWindowHandle, placement)

            Select Case placement.WindowState
                Case WindowState.Minimize, WindowState.ForceMinimize, WindowState.ShowMinimized
                    NativeMethods.ShowWindow(Me.p.MainWindowHandle, WindowState.Restore)

                Case WindowState.Maximize, WindowState.ShowMaximized
                    NativeMethods.ShowWindow(Me.p.MainWindowHandle, WindowState.ShowMaximized)

                Case Else
                    NativeMethods.ShowWindow(Me.p.MainWindowHandle, WindowState.Normal)
            End Select
            NativeMethods.SetForegroundWindow(Me.p.MainWindowHandle) ' Bring window to top and avtivate input.

            Bot.Log("[SystemTrayIcon] Window visibility changed to visible.")

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="ToolStripMenuItem.Click"/> event of the <see cref="SystemTrayIcon.MenuItemExit"/> component.
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
        Private Sub MenuItemExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItemExit.Click

            Bot.Log("[SystemTrayIcon] Terminating SmartBot process by user demand.")

            Me.NotifyIcon.Visible = False
            Bot.CloseBot()

        End Sub

#End Region

#Region " IDisposable Implementation "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Flag to detect redundant calls when disposing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private isDisposed As Boolean = False

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="SystemTrayIcon"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(isDisposing:=True)
            GC.SuppressFinalize(obj:=Me)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="isDisposing">
        ''' <see langword="True"/>  to release both managed and unmanaged resources; 
        ''' <see langword="False"/> to release only unmanaged resources.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub Dispose(ByVal isDisposing As Boolean)
            If (Not Me.isDisposed) AndAlso (isDisposing) Then
                Me.MenuItemShow.Dispose()
                Me.MenuItemHide.Dispose()
                Me.MenuItemExit.Dispose()
                Me.Menu.Dispose()
                Me.NotifyIcon.Visible = False
                Me.NotifyIcon.Dispose()
                Bot.Log("[SystemTrayIcon] Plugin deinitialized.")
            End If

            Me.isDisposed = True
        End Sub

#End Region

    End Class

End Namespace

#End Region
