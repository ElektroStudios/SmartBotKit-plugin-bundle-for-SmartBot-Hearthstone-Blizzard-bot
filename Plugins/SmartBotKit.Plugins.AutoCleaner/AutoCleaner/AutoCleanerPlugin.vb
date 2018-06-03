
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.FileIO

Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Reflection

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop

#End Region

#Region " AutoCleanerPlugin "

Namespace AutoCleaner

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' This plugin will automatically clean temporary/garbage files when exiting from SmartBot.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class AutoCleanerPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As AutoCleanerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, AutoCleanerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="AutoCleanerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="AutoCleanerPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            SmartBotKit.ReservedUse.UpdateUtil.RunUpdaterExecutable()
            Me.IsDll = True
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="AutoCleanerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[AutoCleaner] Plugin initialized.")
                If (Me.DataContainer.CleanerEvent = SmartBotEvent.Startup) Then
                    Me.CleanGarbage()
                End If
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="AutoCleanerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[AutoCleaner] Plugin enabled.")
                Else
                    Bot.Log("[AutoCleaner] Plugin disabled.")
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
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.CleanerEvent = SmartBotEvent.BotStart) Then
                Me.CleanGarbage()
            End If
            MyBase.OnStarted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStopped()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.CleanerEvent = SmartBotEvent.BotStop) Then
                Me.CleanGarbage()
            End If
            MyBase.OnStopped()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="AutoCleanerPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.CleanerEvent = SmartBotEvent.Exit) Then
                Me.CleanGarbage()
            End If
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Cleans the temporary files and folders.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub CleanGarbage()

            If Not File.Exists(Path.Combine(SmartBotUtil.SmartBotDir.FullName, "SBAPI.dll")) Then
                Throw New DirectoryNotFoundException("SmartBot's root directory cannot be found.")
            End If

            Dim verboseMode As Boolean = Me.DataContainer.VerboseMode
            Dim minDaysDiff As Integer = Me.DataContainer.OlderThanDays

            Dim recycleOption As RecycleOption
            If Me.DataContainer.SendFilesToRecycleBin Then
                recycleOption = RecycleOption.SendToRecycleBin
            Else
                recycleOption = RecycleOption.DeletePermanently
            End If

            ' Delete seeds
            If (Me.DataContainer.DeleteSeeds) Then
                Dim seeds As New List(Of DirectoryInfo)
                If (SmartBotUtil.SeedsDir.Exists) Then
                    seeds.AddRange(SmartBotUtil.SeedsDir.EnumerateDirectories("*", System.IO.SearchOption.TopDirectoryOnly))
                End If

                For Each seed As DirectoryInfo In seeds
                    Dim daysDiff As Integer = CInt((Date.Now - seed.CreationTime).TotalDays)
                    If (daysDiff >= minDaysDiff) Then
                        Try
                            My.Computer.FileSystem.DeleteDirectory(seed.FullName, UIOption.OnlyErrorDialogs, recycleOption)
                            If (verboseMode) Then
                                Bot.Log(String.Format("[AutoCleaner] Seed directory deleted: '{0}'. Older than {1} days.", seed.Name, daysDiff))
                            End If
                        Catch ex As Exception
                            ' Ignore all.
                        End Try
                    End If
                Next seed
            End If

            ' Delete logs
            If (Me.DataContainer.DeleteLogs) Then
                Dim logs As New List(Of FileInfo)
                If (SmartBotUtil.CrashesDir.Exists) Then
                    logs.AddRange(SmartBotUtil.CrashesDir.EnumerateFiles("*.txt", System.IO.SearchOption.TopDirectoryOnly))
                End If
                If (SmartBotUtil.LogsDir.Exists) Then
                    logs.AddRange(SmartBotUtil.LogsDir.EnumerateFiles("*.log", System.IO.SearchOption.AllDirectories))
                    logs.AddRange(SmartBotUtil.LogsDir.EnumerateFiles("*.txt", System.IO.SearchOption.AllDirectories))
                End If
                logs.Add(New FileInfo(Path.Combine(SmartBotUtil.SmartBotDir.FullName, "UpdaterLog.txt")))
                logs.Add(New FileInfo(Path.Combine(SmartBotUtil.SmartBotDir.FullName, "CompileErrorsDiscoverCC.txt")))

                For Each log As FileInfo In logs
                    Dim daysDiff As Integer = CInt((Date.Now - log.CreationTime).TotalDays)
                    If (daysDiff >= minDaysDiff) Then
                        Try
                            My.Computer.FileSystem.DeleteFile(log.FullName, UIOption.OnlyErrorDialogs, recycleOption)
                            If (verboseMode) Then
                                Bot.Log(String.Format("[AutoCleaner] Log file deleted: '{0}'. Older than {1} days.", log.Name, daysDiff))
                            End If
                        Catch ex As Exception
                            ' Ignore all.
                        End Try
                    End If
                Next log
            End If

            ' Delete screenshots
            If (Me.DataContainer.DeleteScreenshots) Then
                Dim screenshots As New List(Of FileInfo)
                If (SmartBotUtil.ScreenshotsDir.Exists) Then
                    screenshots.AddRange(SmartBotUtil.ScreenshotsDir.EnumerateFiles("*.png", System.IO.SearchOption.TopDirectoryOnly))
                End If

                For Each screenshot As FileInfo In screenshots
                    Dim daysDiff As Integer = CInt((Date.Now - screenshot.CreationTime).TotalDays)
                    If (daysDiff >= minDaysDiff) Then
                        Try
                            My.Computer.FileSystem.DeleteFile(screenshot.FullName, UIOption.OnlyErrorDialogs, recycleOption)
                            If (verboseMode) Then
                                Bot.Log(String.Format("[AutoCleaner] Screenshot deleted: '{0}'. Older than {1} days.", screenshot.Name, daysDiff))
                            End If
                        Catch ex As Exception
                            ' Ignore all.
                        End Try
                    End If
                Next screenshot
            End If

        End Sub

#End Region

    End Class

End Namespace

#End Region
