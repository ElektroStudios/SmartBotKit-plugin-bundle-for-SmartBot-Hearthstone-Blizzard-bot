
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Application
Imports SmartBotKit.Extensions.StringExtensions
Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.ReservedUse
Imports SmartBotKit.Text

#End Region

#Region " UIEnhancerPlugin "

' ReSharper disable once CheckNamespace

Namespace UIEnhancer

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that adds visual enhancements for the SmartBot user-interface.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class UIEnhancerPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As UIEnhancerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, UIEnhancerPluginData)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="SystemTrayIcon"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Property Icon As SystemTrayIcon

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="UIEnhancerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="UIEnhancerPluginData.EnableSystrayIcon"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabledSysTrayIcon As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A <see cref="Thread"/> on which <see cref="UIEnhancerPlugin.Icon"/> will be run within a message-loop.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private iconThread As Thread

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A delegate that represents the method to be invoked when <see cref="UIEnhancerPlugin.iconThread"/> begins executed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly iconMethodDelegate As New ParameterizedThreadStart(
            Sub()
                Me.Icon = New SystemTrayIcon(Me)
                Windows.Forms.Application.Run()
            End Sub)

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="UIEnhancerPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            UpdateUtil.RunUpdaterExecutable()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="UIEnhancerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            Me.lastEnabledSysTrayIcon = Me.DataContainer.EnableSystrayIcon
            If (Me.lastEnabled) Then
                Bot.Log("[UI Enhancer] -> Plugin initialized.")

                If (Me.DataContainer.EnableWindowPositionRestorator) Then
                    Me.RestoreWindowPlacement()
                    Select Case Me.DataContainer.WindowState
                        Case NativeWindowState.Maximize, NativeWindowState.ShowMaximized
                            Bot.Log($"[UI Enhancer] -> SmartBot window state restored to: {Me.DataContainer.WindowState.ToString()}")

                        Case Else
                            Bot.Log(
                            $"[UI Enhancer] -> SmartBot window state restored to: {Me.DataContainer.WindowState.ToString()} | { _
                                   Me.DataContainer.CurrentPosition.ToString()} | { _
                                   Me.DataContainer.NormalSize.ToString()}")

                    End Select
                End If

                If (Me.DataContainer.EnableSystrayIcon) Then
                    Me.InitializeSysTrayThread()
                End If

                Me.SetWindowInfo(Bot.Mode.None, Card.CClass.NONE, Card.CClass.NONE)
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="UIEnhancerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            Dim enabledSysTrayIcon As Boolean = Me.DataContainer.EnableSystrayIcon

            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[UI Enhancer] -> Plugin enabled.")
                    If (Me.DataContainer.EnableSystrayIcon) Then
                        Me.InitializeSysTrayThread()
                    End If
                Else
                    If (Me.DataContainer.EnableSystrayIcon) Then
                        Me.DeinitializeSysTrayThread()
                    End If
                    Bot.Log("[UI Enhancer] -> Plugin disabled.")
                End If

                Me.SetWindowInfo(Bot.Mode.None, Card.CClass.NONE, Card.CClass.NONE)
                Me.lastEnabled = enabled
            End If

            If (enabledSysTrayIcon <> Me.lastEnabledSysTrayIcon) Then
                If (Me.DataContainer.EnableSystrayIcon) Then
                    Me.InitializeSysTrayThread()
                Else
                    Me.DeinitializeSysTrayThread()
                End If
                Me.lastEnabledSysTrayIcon = enabledSysTrayIcon
            End If

            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is about to handle mulligan (to decide which card to replace) before a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="choices">
        ''' The mulligan choices.
        ''' </param>
        ''' 
        ''' <param name="opponentClass">
        ''' The opponent class.
        ''' </param>
        ''' 
        ''' <param name="ownClass">
        ''' Our hero class.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnHandleMulligan(ByVal choices As List(Of Card.Cards), ByVal opponentClass As Card.CClass, ByVal ownClass As Card.CClass)
            If (Me.DataContainer.Enabled) Then
                Me.SetWindowInfo(Bot.CurrentMode, ownClass, opponentClass)
            End If
            MyBase.OnHandleMulligan(choices, opponentClass, ownClass)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a turn begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTurnBegin()
            If (Me.DataContainer.Enabled) Then
                Me.SetWindowInfo(Bot.CurrentMode, Bot.CurrentDeck.Class, Bot.CurrentBoard.EnemyClass)
            End If
            MyBase.OnTurnBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameEnd()
            If (Me.DataContainer.Enabled) Then
                Me.SetWindowInfo(Bot.Mode.None, Card.CClass.NONE, Card.CClass.NONE)
            End If
            MyBase.OnGameEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStopped()
            If (Me.DataContainer.Enabled) Then
                Me.SetWindowInfo(Bot.Mode.None, Card.CClass.NONE, Card.CClass.NONE)
            End If
            MyBase.OnStopped()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="UIEnhancerPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            If (Me.DataContainer.Enabled) Then
                If (Me.DataContainer.EnableWindowPositionRestorator) Then
                    Me.SaveWindowPlacement()
                End If

                If (Me.DataContainer.EnableSystrayIcon) Then
                    Me.DeinitializeSysTrayThread()
                End If
            End If
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Updates the SmartBot's main window title and its taskbar icon progressbar.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="mode">
        ''' The current bot mode.
        ''' </param>
        ''' 
        ''' <param name="heroClass">
        ''' The hero class.
        ''' </param>
        ''' 
        ''' <param name="enemyClass">
        ''' The enemy class.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub SetWindowInfo(ByVal mode As Bot.Mode, ByVal heroClass As Card.CClass, ByVal enemyClass As Card.CClass)

            Dim title As String
            Dim state As TaskbarProgressBarState
            Dim minValue As Integer
            Dim maxValue As Integer
            Dim pr As Process = SmartBotUtil.Process

            Select Case mode

                Case Bot.Mode.None
                    state = TaskbarProgressBarState.NoProgress
                    minValue = 0
                    maxValue = 0
                    title = $"SmartBot {{ {pr.MainModule.ModuleName} }}"

                Case Bot.Mode.Arena, Bot.Mode.ArenaAuto
                    state = TaskbarProgressBarState.Normal
                    minValue = Statistics.ArenaWins
                    maxValue = 12
                    title = $"Arena|{minValue}/12W|{Statistics.ArenaLosses}/3L"

                Case Else
                    state = TaskbarProgressBarState.NoProgress
                    minValue = 0
                    maxValue = 0
                    title =
                        $"{heroClass.ToString().Rename(StringCase.WordCase)} vs. { _
                            enemyClass.ToString().Rename(StringCase.WordCase)}"

            End Select

            Do While (SmartBotUtil.IsInSplashScreen) OrElse (pr.MainWindowHandle = IntPtr.Zero)
                Thread.Sleep(TimeSpan.FromSeconds(1))
                pr.Refresh()
            Loop

            NativeMethods.SetWindowText(pr.MainWindowHandle, title)

            If (Me.DataContainer.EnableTaskBarEnhancements) Then
                TaskBarManager.Instance.SetProgressState(state)
                TaskBarManager.Instance.SetProgressValue(minValue, maxValue)
                TaskBarManager.Instance.SetThumbnailClip(New Rectangle(10, 130, 300, 145), pr.MainWindowHandle)
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes the <see cref="Thread"/> on which <see cref="UIEnhancerPlugin.Icon"/> will run within a message-loop.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub InitializeSysTrayThread()
            Me.DeinitializeSysTrayThread()

            Me.iconThread = New Thread(Me.iconMethodDelegate) With {
                .IsBackground = True,
                .Priority = ThreadPriority.Lowest
            }

            Me.iconThread.SetApartmentState(ApartmentState.STA)
            Me.iconThread.Start()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Deinitializes the <see cref="Thread"/> on which <see cref="UIEnhancerPlugin.Icon"/> is running.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub DeinitializeSysTrayThread()
            If (HearthstoneUtil.Process IsNot Nothing) Then
                Try
                    Dim isHsWindowVisible As Boolean = NativeMethods.IsWindowVisible(HearthstoneUtil.Process.MainWindowHandle)
                    If Not (isHsWindowVisible) Then
                        NativeMethods.ShowWindow(HearthstoneUtil.Process.MainWindowHandle, NativeWindowState.ShowDefault)
                    End If
                    Bot.Log($"[UI Enhancer] -> Hearthstone window state changed to: {NameOf(NativeWindowState.ShowDefault)}")

                Catch ex As Exception
                    Bot.Log("[UI Enhancer] -> Failed to restore Hearthstone window state...")
                    Bot.Log($"[UI Enhancer] -> Exception message: {ex.Message}")

                End Try
            End If

            If (Me.Icon IsNot Nothing) Then
                Me.Icon.Dispose()
            End If

            If (Me.iconThread IsNot Nothing) Then
                Me.iconThread.Abort()
            End If
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Saves the current window size and position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepperBoundary>
        Private Sub SaveWindowPlacement()
            Dim wpl As WindowPlacement = SmartBotUtil.WindowPlacement

            Dim normalRect As Rectangle = wpl.NormalPosition

            Me.DataContainer.NormalPosition = normalRect.Location
            Me.DataContainer.NormalSize = normalRect.Size
            Me.DataContainer.MinimizedPosition = wpl.MinPosition
            Me.DataContainer.MaximizedPosition = wpl.MaxPosition
            Me.DataContainer.WindowState = wpl.WindowState
            Me.DataContainer.Flags = wpl.Flags
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Restores the last saved window size and position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepperBoundary>
        Private Sub RestoreWindowPlacement()
            Dim p As Process = SmartBotUtil.Process
            Do While (SmartBotUtil.IsInSplashScreen) OrElse (p.MainWindowHandle = IntPtr.Zero)
                Thread.Sleep(100)
                p.Refresh()
            Loop

#Disable Warning IDE0009 ' Member access should be qualified.
            Dim wpl As New WindowPlacement With {
                .NormalPosition = New Rectangle(Me.DataContainer.NormalPosition, Me.DataContainer.NormalSize),
                .MinPosition = Me.DataContainer.MinimizedPosition,
                .MaxPosition = Me.DataContainer.MaximizedPosition,
                .WindowState = Me.DataContainer.WindowState,
                .Flags = Me.DataContainer.Flags
            }
#Enable Warning IDE0009 ' Member access should be qualified.

            Select Case wpl.WindowState
                Case NativeWindowState.ForceMinimize, NativeWindowState.Minimize,
                     NativeWindowState.ShowMinimized, NativeWindowState.ShowMinNoActive,
                     NativeWindowState.Hide

                    ' This is because we don't want to set the window minimized or hidden when restoring it.
                    wpl.WindowState = NativeWindowState.Normal
                    Me.DataContainer.WindowState = NativeWindowState.Normal

                Case Else
                    ' Do nothing.

            End Select
            wpl.Length = Marshal.SizeOf(wpl)

            Do Until NativeMethods.IsWindowVisible(p.MainWindowHandle)
                Thread.Sleep(100)
            Loop

            SmartBotUtil.WindowPlacement = wpl

            ' Dim success As Boolean = NativeMethods.SetWindowPlacement(p.MainWindowHandle, wpl)

        End Sub

#End Region

    End Class

End Namespace

#End Region
