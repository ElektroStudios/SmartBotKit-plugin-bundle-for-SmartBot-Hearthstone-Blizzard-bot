
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Audio
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.ReservedUse

#End Region

#Region " PlayVIGAdvertsRemoverPlugin "

Namespace PlayVIGAdvertsRemover

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that removes and mute adverts from PlayVIG application.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class PlayVIGAdvertsRemoverPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As PlayVIGAdvertsRemoverPluginData
            Get
                Return DirectCast(MyBase.DataContainer, PlayVIGAdvertsRemoverPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="PlayVIGAdvertsRemoverPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A flag to determine whether the plugin should check for new adverts.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private needsCheck As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PlayVIGAdvertsRemoverPlugin"/> class.
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
        ''' Called when this <see cref="PlayVIGAdvertsRemoverPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[PlayVIG Adverts Remover] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="PlayVIGAdvertsRemoverPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[PlayVIG Adverts Remover] -> Plugin enabled.")
                Else
                    Me.needsCheck = False
                    Me.RestorePlayVIGVolume()
                    Bot.Log("[PlayVIG Adverts Remover] -> Plugin disabled.")
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
            Me.needsCheck = True
            MyBase.OnStarted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStopped()
            Me.needsCheck = False
            Me.RestorePlayVIGVolume()
            MyBase.OnStopped()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameBegin()
            Me.needsCheck = False
            MyBase.OnGameBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameEnd()
            Me.needsCheck = True
            MyBase.OnGameEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot timer is ticked, every 300 milliseconds.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTick()
            If (Me.needsCheck) Then

                Dim prcs As IEnumerable(Of Process) = Process.GetProcessesByName("PlayVIG")

                For Each pr As Process In prcs

                    ' Set audio mute.
                    If Not (AudioUtil.IsApplicationMuted(pr)) Then
                        'Bot.Log("[PlayVIG Adverts Remover] -> Muting audio volume.")
                        AudioUtil.SetApplicationVolume(pr, 0)
                        AudioUtil.SetMuteApplication(pr, mute:=True)
                    End If

                    ' Find the advert window.
                    Dim sb As New StringBuilder(32)
                    NativeMethods.GetClassName(pr.MainWindowHandle, sb, sb.Capacity)

                    If sb.ToString().Equals("Chrome_WidgetWin_0", StringComparison.OrdinalIgnoreCase) Then
                        Dim wsEx As WindowStylesEx
                        If (Environment.Is64BitProcess) Then
                            wsEx = CType(NativeMethods.GetWindowLongPtr(pr.MainWindowHandle, WindowLongFlags.WindowStyleEx).ToInt64(), WindowStylesEx)
                        Else
                            wsEx = CType(NativeMethods.GetWindowLong(pr.MainWindowHandle, WindowLongFlags.WindowStyleEx), WindowStylesEx)
                        End If

                        ' Remove advert.
                        If wsEx = (WindowStylesEx.RightScrollbar Or WindowStylesEx.Left Or WindowStylesEx.TopMost Or WindowStylesEx.WindowEdge) Then
                            Bot.Log("[PlayVIG Adverts Remover] -> Advert detected. Hiding it...")
                            NativeMethods.ShowWindow(pr.MainWindowHandle, NativeWindowState.Hide)
                            Bot.Log("[PlayVIG Adverts Remover] -> Advert hidden.")
                        End If
                    End If

                Next pr
            End If

            MyBase.OnTick()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="PlayVIGAdvertsRemoverPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            Me.RestorePlayVIGVolume()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Restores the audio volume of PlayVIG process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub RestorePlayVIGVolume()

            Dim prcs As IEnumerable(Of Process) = Process.GetProcessesByName("PlayVIG")
            If prcs.Any() Then
                Bot.Log("[PlayVIG Adverts Remover] -> Restoring audio volume...")
            End If

            For Each pr As Process In prcs
                AudioUtil.SetApplicationVolume(pr, 100)
                AudioUtil.SetMuteApplication(pr, mute:=False)
            Next pr

        End Sub

#End Region

    End Class

End Namespace

#End Region
