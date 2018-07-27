
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.ReservedUse

#End Region

#Region " WindowRestoratorPlugin "

Namespace WindowRestorator

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that reminds the last SmartBot's window size and position and restores it at the next program startup.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class WindowRestoratorPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As WindowRestoratorPluginData
            Get
                Return DirectCast(MyBase.DataContainer, WindowRestoratorPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="WindowRestoratorPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="WindowRestoratorPlugin"/> class.
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
        ''' Called when this <see cref="WindowRestoratorPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Window Restorator] -> Plugin initialized.")
                Me.RestoreWindowPlacement()
                '#If DEBUG Then
                Select Case Me.DataContainer.WindowState
                    Case WindowState.Maximize, WindowState.ShowMaximized
                        Bot.Log(String.Format("[Window Restorator] -> {0}",
                                              Me.DataContainer.WindowState.ToString()))

                    Case Else
                        Bot.Log(String.Format("[Window Restorator] -> Changed to: {0} | {1} | {2}",
                                          Me.DataContainer.WindowState.ToString(),
                                          Me.DataContainer.CurrentPosition.ToString(),
                                          Me.DataContainer.NormalSize.ToString()))

                End Select
                '#End If
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="WindowRestoratorPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Window Restorator] -> Plugin enabled.")
                Else
                    Bot.Log("[Window Restorator] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="WindowRestoratorPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            Me.SaveWindowPlacement()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

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
                Case WindowState.ForceMinimize, WindowState.Minimize,
                     WindowState.ShowMinimized, WindowState.ShowMinNoActive,
                     WindowState.Hide

                    ' This is because we don't want to set the window minimized or hidden when restoring it.
                    wpl.WindowState = WindowState.Normal
                    Me.DataContainer.WindowState = WindowState.Normal

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
