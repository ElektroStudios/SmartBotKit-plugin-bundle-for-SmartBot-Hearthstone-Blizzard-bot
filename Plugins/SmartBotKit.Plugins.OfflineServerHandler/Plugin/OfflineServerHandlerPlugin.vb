
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Audio
Imports SmartBotKit.Computer
Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.ReservedUse

#End Region

#Region " OfflineServerHandlerPlugin "

' ReSharper disable once CheckNamespace

Namespace OfflineServerHandler


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that handles the bot behavior when the server gets down. 
    ''' <para></para>
    ''' Note that it ﻿does not ﻿handle ﻿lag, ﻿local network inactivity﻿﻿ neither ﻿authentication connection problems.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class OfflineServerHandlerPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As OfflineServerHandlerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, OfflineServerHandlerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="OfflineServerHandlerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the creation datetime of this plugin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastDateActive As Date

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="OfflineServerHandlerPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            UpdateUtil.RunUpdaterExecutable()
            Me.lastDateActive = Date.Now()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="OfflineServerHandlerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Offline Server Handler] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="OfflineServerHandlerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Offline Server Handler] -> Plugin enabled.")
                Else
                    Bot.Log("[Offline Server Handler] -> Plugin disabled.")
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
            Me.lastDateActive = Date.Now
            MyBase.OnStarted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStopped()
            Me.lastDateActive = Date.Now
            MyBase.OnStopped()
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
            Me.lastDateActive = Date.Now
            MyBase.OnHandleMulligan(choices, opponentClass, ownClass)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot timer is ticked, every 300 milliseconds.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTick()
            If (Me.DataContainer.Enabled) AndAlso (Bot.IsBotRunning()) Then

                Dim lastServerDownRecord As TimeSpan = SmartBotUtil.LastServerDownRecord
                If (lastServerDownRecord = Nothing) Then
                    Exit Sub
                End If

                If (lastServerDownRecord < Me.lastDateActive.TimeOfDay) Then
                    Exit Sub
                End If

                Dim diffTime As TimeSpan = lastServerDownRecord.Subtract(Date.Now.TimeOfDay)
                If (diffTime.TotalSeconds < 60) Then ' If lastServerDownRecord occured more than 1 minute ago then...

                    Dim isBotStopped As Boolean
                    Select Case Bot.CurrentMode

                        Case Bot.Mode.ArenaAuto
                            If (Me.DataContainer.StopTheBotIfArena) Then
                                Bot.StopBot()
                                isBotStopped = True
                            End If

                        Case Bot.Mode.RankedStandard, Bot.Mode.RankedWild
                            If (Me.DataContainer.StopTheBotIfRanked) Then
                                Bot.StopBot()
                                isBotStopped = True
                            End If

                        Case Bot.Mode.UnrankedStandard, Bot.Mode.UnrankedWild
                            If (Me.DataContainer.StopTheBotIfUnranked) Then
                                Bot.StopBot()
                                isBotStopped = True
                            End If

                        Case Else
                            ' Do Nothing.

                    End Select

                    If (isBotStopped) Then
                        Bot.Log("[Offline Server Handler] -> Server down detected. Bot has been stopped.")

                        Select Case Me.DataContainer.SetComputerState

                            Case ComputerState.Hibernate
                                Bot.Log("[Offline Server Handler] -> Hibernating the computer...")
                                PowerUtil.Hibernate(force:=True)

                            Case ComputerState.Suspend
                                Bot.Log("[Offline Server Handler] -> Suspending the computer...")
                                PowerUtil.Suspend(force:=True)

                            Case ComputerState.Shutdown
                                Bot.Log("[Offline Server Handler] -> Powering off the computer...")
                                PowerUtil.Shutdown("", 0, "", ShutdownMode.ForceOthers, ShutdownReason.FagUserPlanned, ShutdownPlanning.Planned, True)

                            Case Else ' ComputerState.NoChange
                                If (Me.DataContainer.ResumeEnabled()) Then
                                    Dim minutes As Integer = Me.DataContainer.ResumeInterval
                                    Me.ScheduleResume(minutes)
                                    Bot.Log($"[Offline Server Handler] -> Bot resumption scheduled to {minutes} minutes.")
                                End If

                                If (Me.DataContainer.PlaySoundFile) Then
                                    Dim file As New FileInfo(Path.Combine(SmartBotUtil.PluginsDir.FullName, "OfflineServerHandler.mp3"))
                                    If Not file.Exists() Then
                                        Bot.Log($"[Offline Server Handler] -> Audio file not found: '{file.FullName}'")
                                    Else
                                        AudioUtil.PlaySoundFile(file)
                                    End If
                                End If

                        End Select

                    End If

                End If

            End If

            MyBase.OnTick()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="OfflineServerHandlerPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Schedule a bot resume to try a reconnection to the server.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub ScheduleResume(ByVal minutes As Integer)

            Dim resumeMethod As New Action(
                Sub()
                    If Not (Me.DataContainer.ResumeEnabled) Then
                        Exit Sub
                    End If

                    Dim lastDateActive As Date = Me.lastDateActive
                    Thread.Sleep(TimeSpan.FromMinutes(minutes))

                    If (Me.DataContainer.Enabled) AndAlso
                       (Me.DataContainer.ResumeEnabled) AndAlso
                       (Me.lastDateActive = lastDateActive) Then

                        If Not (Bot.IsBotRunning()) Then
                            Bot.StartBot()
                            Bot.Log("[Offline Server Handler] -> Bot resumed.")
                        End If
                    End If
                End Sub)

            Dim resumeTask As New Task(resumeMethod)
            resumeTask.Start()

        End Sub

#End Region

    End Class

End Namespace

#End Region
