
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Drawing
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.ReservedUse

#End Region

#Region " HearthstoneResizerPlugin "

' ReSharper disable once CheckNamespace

Namespace HearthstoneResizer


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that maintains a fixed size and location for Hearthstone window.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class HearthstoneResizerPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As HearthstoneResizerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, HearthstoneResizerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="HearthstoneResizerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the bot ticks count.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private curTickCount As Integer

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="HearthstoneResizerPlugin"/> class.
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
        ''' Called when this <see cref="HearthstoneResizerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Hearthstone Resizer] -> Plugin initialized.")
            End If
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.HearthstoneResizerEvent = SmartBotEvent.Startup) Then
                Me.SetHearthstoneWindowSizeAndPosition()
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="HearthstoneResizerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Hearthstone Resizer] -> Plugin enabled.")
                Else
                    Bot.Log("[Hearthstone Resizer] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is started.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStarted()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.HearthstoneResizerEvent = SmartBotEvent.BotStart) Then
                Me.SetHearthstoneWindowSizeAndPosition()
            End If
            MyBase.OnStarted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameBegin()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.HearthstoneResizerEvent = SmartBotEvent.GameBegin) Then
                Me.SetHearthstoneWindowSizeAndPosition()
            End If
            MyBase.OnGameBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a turn begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTurnBegin()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.HearthstoneResizerEvent = SmartBotEvent.TurnBegin) Then
                Me.SetHearthstoneWindowSizeAndPosition()
            End If
            MyBase.OnTurnBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot timer is ticked, every 300 milliseconds.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTick()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.HearthstoneResizerEvent = SmartBotEvent.TimerTick) Then

                If (Not Me.DataContainer.IgnoreTicksIfBotStopped) OrElse
                   (Me.DataContainer.IgnoreTicksIfBotStopped AndAlso Bot.IsBotRunning()) Then

                    If (Interlocked.Increment(Me.curTickCount) >= Me.DataContainer.TickCount) Then
                        Me.curTickCount = 0
                        Try
                            Me.SetHearthstoneWindowSizeAndPosition()
                        Catch ex As Exception
                        End Try
                    End If

                End If

            End If

            MyBase.OnTick()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the Hearthstone window size and position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub SetHearthstoneWindowSizeAndPosition()
            If (HearthstoneUtil.Process Is Nothing) Then
                Exit Sub
            End If

            Select Case HearthstoneUtil.WindowPlacement.WindowState
                Case NativeWindowState.ForceMinimize, NativeWindowState.Hide,
                     NativeWindowState.Maximize, NativeWindowState.Minimize,
                     NativeWindowState.ShowMaximized, NativeWindowState.ShowMinimized
                    ' Ignore all.

                Case Else
                    If Not (HearthstoneUtil.IsFullscreen) Then
                        Dim oldPos As Point = HearthstoneUtil.WindowPosition
                        Dim newPos As Point = Me.DataContainer.Position

                        Dim oldSize As Size = HearthstoneUtil.WindowSize
                        Dim newSize As Size = HearthstoneResizerPluginData.Resolutions(Me.DataContainer.Size)

                        Dim windowPlacementChanged As Boolean

                        If (Me.DataContainer.EnableFixedPos) AndAlso (oldPos <> newPos) Then
                            windowPlacementChanged = True
                            HearthstoneUtil.WindowPosition = newPos
                        End If

                        If (Me.DataContainer.EnableFixedSize) AndAlso (oldSize <> newSize) Then
                            windowPlacementChanged = True
                            HearthstoneUtil.WindowSize = newSize
                        End If

                        If (windowPlacementChanged = True) Then
                            Bot.Log(
                                $"[Hearthstone Resizer] -> Changed to: { _
                                       If(newPos <> Point.Empty, newPos.ToString(), oldPos.ToString())} | { _
                                       If(newSize <> Size.Empty, newSize.ToString(), oldSize.ToString())}")
                        End If

                    End If

            End Select
        End Sub

#End Region

    End Class

End Namespace

#End Region
