
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Collections.ObjectModel
Imports System.Diagnostics

#End Region

#Region " MultiLauncherPlugin "

Namespace MultiLauncher

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' This plugin automatically launchs your favorite files or 3rd party programs for Hearthstone at SmartBot's startup.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class MultiLauncherPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As MultiLauncherPluginData
            Get
                Return DirectCast(MyBase.DataContainer, MultiLauncherPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="MultiLauncherPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A collection that holds the processes that were ran by this <see cref="MultiLauncherPlugin"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private processes As ICollection(Of Process)

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="MultiLauncherPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            SmartBotKit.ReservedUse.UpdateUtil.RunUpdaterExecutable()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="MultiLauncherPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Me.RunProcesses()
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="MultiLauncherPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[MultiLauncher] Plugin enabled.")
                Else
                    Bot.Log("[MultiLauncher] Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="MultiLauncherPlugin"/> instance.
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
        ''' Tries to run all the executable files that were specified in the <see cref="MultiLauncherPlugin.DataContainer"/> properties.
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
                    Bot.Log(String.Format("[MultiLauncher] executable file '{0}' not found.", fi.FullName))
                    Continue For
                End If

                Dim p As New Process()
                p.StartInfo.FileName = fi.FullName
                p.StartInfo.UseShellExecute = True

                Try
                    p.Start()
                    Me.processes.Add(p)
                    Bot.Log(String.Format("[MultiLauncher] executable file ran successfully: '{0}'.", fi.FullName))

                Catch ex As Exception
                    Bot.Log(String.Format("[MultiLauncher] exception thrown when trying to run executable file '{0}'...", fi.FullName))
                    Bot.Log(String.Format("[MultiLauncher] exception message: '{0}'", ex.Message))

                End Try

            Next fi

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Tries to kill all the processes that were ran as result of a call to <see cref="MultiLauncherPlugin.RunProcesses"/> method.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub KillProcesses()
            For Each p As Process In Me.processes
                If Not (p.HasExited) Then
                    Try
                        p.Kill()

                    Finally
                        p.Dispose()

                    End Try
                End If
            Next p

            Me.processes.Clear()
        End Sub

#End Region

    End Class

End Namespace

#End Region
