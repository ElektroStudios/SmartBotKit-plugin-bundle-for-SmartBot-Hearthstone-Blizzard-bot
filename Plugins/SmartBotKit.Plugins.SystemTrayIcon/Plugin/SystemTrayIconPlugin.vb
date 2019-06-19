
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Threading
Imports System.Windows.Forms

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.ReservedUse

#End Region

#Region " SystemTrayIconPlugin "

' ReSharper disable once CheckNamespace

Namespace SystemTrayIcon


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that creates a <see cref="NotifyIcon"/> with menu commands to handle SmartBot and Hearthstone.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class SystemTrayIconPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As SystemTrayIconPluginData
            Get
                Return DirectCast(MyBase.DataContainer, SystemTrayIconPluginData)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="SystemTrayIcon"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Property Icon As SystemTrayIcon

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A <see cref="Thread"/> on which <see cref="SystemTrayIconPlugin.Icon"/> will be run within a message-loop.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private iconThread As Thread

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A delegate that represents the method to be invoked when <see cref="SystemTrayIconPlugin.iconThread"/> begins executed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly iconMethodDelegate As New ParameterizedThreadStart(
            Sub()
                Me.Icon = New SystemTrayIcon(Me)
                Windows.Forms.Application.Run()
            End Sub)

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="SystemTrayIconPlugin"/> class.
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
        ''' Called when this <see cref="SystemTrayIconPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.OnDataContainerUpdated()
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="SystemTrayIconPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            If (Me.DataContainer.Enabled) Then
                Me.InitializeThread()
            Else
                Me.DeinitializeThread()
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="SystemTrayIconPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            Me.DeinitializeThread()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes the <see cref="Thread"/> on which <see cref="SystemTrayIconPlugin.Icon"/> will run within a message-loop.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub InitializeThread()
            Me.DeinitializeThread()

            Me.iconThread = New Thread(Me.iconMethodDelegate) With {
                .IsBackground = True,
                .Priority = ThreadPriority.Lowest
            }

            Me.iconThread.SetApartmentState(ApartmentState.STA)
            Me.iconThread.Start()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Deinitializes the <see cref="Thread"/> on which <see cref="SystemTrayIconPlugin.Icon"/> is running.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub DeinitializeThread()
            If (HearthstoneUtil.Process IsNot Nothing) Then
                Try
                    Dim isHsWindowVisible As Boolean = NativeMethods.IsWindowVisible(HearthstoneUtil.Process.MainWindowHandle)

                    If Not (isHsWindowVisible) Then
                        NativeMethods.ShowWindow(HearthstoneUtil.Process.MainWindowHandle, NativeWindowState.ShowDefault)
                    End If
                    Bot.Log(
                        $"[System Tray Icon] -> Hearthstone window state changed to: { _
                               NameOf(NativeWindowState.ShowDefault)}")

                Catch ex As Exception
                    Bot.Log("[System Tray Icon] -> Failed to restore Hearthstone window.")
                    Bot.Log($"[System Tray Icon] -> Exception message. {ex.Message}")

                End Try

            End If

            If (Me.Icon IsNot Nothing) Then
                Me.Icon.Dispose()
            End If

            If (Me.iconThread IsNot Nothing) Then
                Me.iconThread.Abort()
            End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
