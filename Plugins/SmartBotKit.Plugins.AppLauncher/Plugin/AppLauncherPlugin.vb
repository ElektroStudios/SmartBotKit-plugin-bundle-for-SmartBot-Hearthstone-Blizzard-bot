
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.ReservedUse

#End Region

#Region " AppLauncherPlugin "

' ReSharper disable once CheckNamespace

Namespace AppLauncher


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that automate external files and programs execution at SmartBot's startup.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class AppLauncherPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As AppLauncherPluginData
            Get
                Return DirectCast(MyBase.DataContainer, AppLauncherPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="AppLauncherPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A collection that holds the processes that were ran by this <see cref="AppLauncherPlugin"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private processes As ICollection(Of Process)

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="AppLauncherPlugin"/> class.
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
        ''' Called when this <see cref="AppLauncherPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[App Launcher] -> Plugin initialized.")
                Me.RunProcesses()
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="AppLauncherPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[App Launcher] -> Plugin enabled.")
                Else
                    Bot.Log("[App Launcher] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="AppLauncherPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            If (Me.DataContainer.Enabled) AndAlso (Me.DataContainer.TerminateProgramsWhenClosingSmartBot) Then
                Me.KillProcesses()
            End If
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Tries to run all the executable files that were specified in the <see cref="AppLauncherPlugin.DataContainer"/> properties.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub RunProcesses()

            If (Me.processes Is Nothing) Then
                Me.processes = New Collection(Of Process)
            End If

            Dim files As IEnumerable(Of FileInfo) =
                From filepath As String In {
                    Me.DataContainer.ExecutablePath1,
                    Me.DataContainer.ExecutablePath2,
                    Me.DataContainer.ExecutablePath3,
                    Me.DataContainer.ExecutablePath4,
                    Me.DataContainer.ExecutablePath5,
                    Me.DataContainer.ExecutablePath6,
                    Me.DataContainer.ExecutablePath7,
                    Me.DataContainer.ExecutablePath8,
                    Me.DataContainer.ExecutablePath9
                } Where Not String.IsNullOrWhiteSpace(filepath)
                Select New FileInfo(filepath)

            For Each fi As FileInfo In files

                If Not (fi.Exists) Then
                    Bot.Log($"[App Launcher] -> File not found: '{fi.FullName}'")
                    Continue For
                End If

#Disable Warning IDE0068 ' Use recommended dispose pattern
                Dim pr As New Process()
#Enable Warning IDE0068 ' Use recommended dispose pattern
                pr.StartInfo.FileName = fi.FullName
                pr.StartInfo.UseShellExecute = True

                Try
                    pr.Start()
                    Me.processes.Add(pr)
                    Bot.Log($"[App Launcher] -> Execution success : '{fi.FullName}'")

                Catch ex As Exception
                    Bot.Log($"[App Launcher] -> Execution failed: '{fi.FullName}'")
                    Bot.Log($"[App Launcher] -> Exception message: '{ex.Message}'")

                End Try

            Next fi

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Tries to kill all the processes that were ran as result of a call to <see cref="AppLauncherPlugin.RunProcesses"/> method.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub KillProcesses()
            For Each p As Process In Me.processes
                If Not (p?.HasExited) Then
                    Try
                        p?.Kill()

                    Finally
                        p?.Dispose()

                    End Try
                End If
            Next p

            Me.processes.Clear()
        End Sub

#End Region

    End Class

End Namespace

#End Region
