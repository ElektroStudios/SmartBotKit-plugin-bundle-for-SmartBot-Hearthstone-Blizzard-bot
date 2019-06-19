
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
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

#Region " TaskbarInformerPlugin "

' ReSharper disable once CheckNamespace

Namespace TaskbarInformer


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that prints statistical info. on the taskbar icon and also displays a progress bar for Arena mode.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class TaskbarInformerPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As TaskbarInformerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, TaskbarInformerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="TaskbarInformerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="TaskbarInformerPlugin"/> class.
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
        ''' Called when this <see cref="TaskbarInformerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Taskbar Informer] -> Plugin initialized.")
                Me.SetInfo(Bot.Mode.None, Card.CClass.NONE, Card.CClass.NONE)
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="TaskbarInformerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Taskbar Informer] -> Plugin enabled.")
                Else
                    Bot.Log("[Taskbar Informer] -> Plugin disabled.")
                End If
                Me.SetInfo(Bot.Mode.None, Card.CClass.NONE, Card.CClass.NONE)
                Me.lastEnabled = enabled
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
                Me.SetInfo(Bot.CurrentMode, ownClass, opponentClass)
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
                Me.SetInfo(Bot.CurrentMode, Bot.CurrentDeck.Class, Bot.CurrentBoard.EnemyClass)
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
                Me.SetInfo(Bot.Mode.None, Card.CClass.NONE, Card.CClass.NONE)
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
                Me.SetInfo(Bot.Mode.None, Card.CClass.NONE, Card.CClass.NONE)
            End If
            MyBase.OnStopped()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="TaskbarInformerPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
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
        Private Sub SetInfo(ByVal mode As Bot.Mode, ByVal heroClass As Card.CClass, ByVal enemyClass As Card.CClass)

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

            TaskBarManager.Instance.SetProgressState(state)
            TaskBarManager.Instance.SetProgressValue(minValue, maxValue)
            TaskBarManager.Instance.SetThumbnailClip(New Rectangle(10, 130, 300, 145), pr.MainWindowHandle)

        End Sub

#End Region

    End Class

End Namespace

#End Region
