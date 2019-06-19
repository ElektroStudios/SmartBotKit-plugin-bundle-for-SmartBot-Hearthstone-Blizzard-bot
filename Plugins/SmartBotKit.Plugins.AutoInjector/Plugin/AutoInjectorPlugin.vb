#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop
Imports SmartBotKit.ReservedUse

#End Region

#Region " AutoInjectorPlugin "

' ReSharper disable once CheckNamespace

Namespace AutoInjector


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that automate SmartBot injection when Hearthstone process is detected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class AutoInjectorPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As AutoInjectorPluginData
            Get
                Return DirectCast(MyBase.DataContainer, AutoInjectorPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="AutoInjectorPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last discovered Hearthstone process identifier (pid).
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastHsPid As Integer?

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the elapsed time.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly stopWatch As Stopwatch

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="AutoInjectorPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True

            UpdateUtil.RunUpdaterExecutable()
            Me.stopWatch = New Stopwatch()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="AutoInjectorPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Auto-Injector] -> Plugin initialized.")
            End If
            Me.stopWatch.Start()
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="AutoInjectorPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Me.stopWatch.Restart()
                    Bot.Log("[Auto-Injector] -> Plugin enabled.")
                Else
                    Me.stopWatch.Reset()
                    Bot.Log("[Auto-Injector] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot injects Hearthstone process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnInjection()
            Me.lastHsPid = HearthstoneUtil.Process?.Id
            Bot.StopRelogger()
            If (Me.DataContainer.AutoStartBotAfterInjected) Then
                Thread.Sleep(TimeSpan.FromSeconds(5))
                If Not (Bot.IsBotRunning()) Then
                    Bot.Log("[Auto-Injector] -> Auto-resuming bot...")
                    Bot.StartBot()
                End If
            End If
            MyBase.OnInjection()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is started.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStarted()
            Me.lastHsPid = HearthstoneUtil.Process?.Id
            MyBase.OnStarted()
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

                If (Me.stopWatch.Elapsed.TotalSeconds >= Me.DataContainer.ProcessDiscoverInterval) Then
                    Me.stopWatch.Reset()

                    If Not Bot.IsBotRunning() Then
                        Dim hsProcess As Process = HearthstoneUtil.Process
                        If (hsProcess IsNot Nothing) AndAlso (hsProcess.Id <> Me.lastHsPid.GetValueOrDefault()) Then
                            Me.lastHsPid = hsProcess?.Id
                            Bot.Log("[Auto-Injector] -> Waiting to inject Hearthstone...")
                            Bot.StartRelogger()
                        End If
                    End If

                    Me.stopWatch.Start()
                End If

            Else
                Me.stopWatch.Reset()

            End If

            MyBase.OnTick()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="AutoInjectorPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

    End Class

End Namespace

#End Region
