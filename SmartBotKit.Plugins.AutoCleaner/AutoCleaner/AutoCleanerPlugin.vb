
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
                If (Me.DataContainer.CleanerMode = CleanerMode.OnStartup) Then
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
        ''' Called when the bot is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStopped()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.CleanerMode = CleanerMode.OnBotStop) Then
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
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.CleanerMode = CleanerMode.OnExit) Then
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

            Dim pluginsPath As New DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            Dim sbDir As DirectoryInfo = pluginsPath.Parent
            Dim logsDir As New DirectoryInfo(Path.Combine(sbDir.FullName, "Logs"))
            Dim seedsDir As New DirectoryInfo(Path.Combine(sbDir.FullName, "Seeds"))

            If Not File.Exists(Path.Combine(sbDir.FullName, "SBAPI.dll")) Then
                Throw New DirectoryNotFoundException("SmartBot's root directory cannot be found.")
            End If

            Dim silentClean As Boolean = Me.DataContainer.SilentClean
            Dim minDaysDiff As Integer = Me.DataContainer.OlderThanDays

            Dim recycleOption As RecycleOption
            If Me.DataContainer.SendFilesToRecycleBin Then
                recycleOption = RecycleOption.SendToRecycleBin
            Else
                recycleOption = RecycleOption.DeletePermanently
            End If

            If (Me.DataContainer.DeleteSeeds) Then
                Dim seedDirs As IEnumerable(Of DirectoryInfo) =
                    seedsDir.EnumerateDirectories("*", System.IO.SearchOption.TopDirectoryOnly)

                For Each seedDir As DirectoryInfo In seedDirs
                    Dim daysDiff As Integer = CInt((Date.Now - seedDir.CreationTime).TotalDays)
                    If (daysDiff >= minDaysDiff) Then
                        Try
                            My.Computer.FileSystem.DeleteDirectory(seedDir.FullName, UIOption.OnlyErrorDialogs, recycleOption)
                            If Not (silentClean) Then
                                Bot.Log(String.Format("[AutoCleaner] Seed directory deleted: '{0}'. Older than {1} days.", seedDir.Name, daysDiff))
                            End If
                        Catch ex As Exception
                            ' Ignore all.
                        End Try
                    End If
                Next seedDir
            End If

            If (Me.DataContainer.DeleteLogs) Then
                Dim logFiles As IEnumerable(Of FileInfo) =
                    Enumerable.Concat(Of FileInfo)(logsDir.EnumerateFiles("*.log", System.IO.SearchOption.AllDirectories),
                                                   logsDir.EnumerateFiles("*.txt", System.IO.SearchOption.AllDirectories))

                For Each logFile As FileInfo In logFiles
                    Dim daysDiff As Integer = CInt((Date.Now - logFile.CreationTime).TotalDays)
                    If (daysDiff >= minDaysDiff) Then
                        Try
                            My.Computer.FileSystem.DeleteFile(logFile.FullName, UIOption.OnlyErrorDialogs, recycleOption)
                            If Not (silentClean) Then
                                Bot.Log(String.Format("[AutoCleaner] Log file deleted: '{0}'. Older than {1} days.", logFile.Name, daysDiff))
                            End If
                        Catch ex As Exception
                            ' Ignore all.
                        End Try
                    End If
                Next logFile

            End If

        End Sub

#End Region

    End Class

End Namespace

#End Region
