
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics.CodeAnalysis
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.ReservedUse

#End Region

#Region " PanicButtonPlugin "

' ReSharper disable once CheckNamespace

Namespace PanicButton


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that stops or terminates SmartBot process when a specified hotkey combination is pressed.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    <SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
                     Justification:="No really need to implement it since it disposes via the inherited Plugin.Dispose() method. 
                                     The real problem is with the design of the 'SmartBot.Plugins.Plugin' class 
                                     which does not implement a IDIsposable iterface but a Dispose() method.")>
    Public NotInheritable Class PanicButtonPlugin : Inherits Plugin

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="Plugins.PanicButton.PanicButton"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private panicButton As PanicButton

        ' ReSharper restore InconsistentNaming

#End Region

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
        Public Shadows ReadOnly Property DataContainer As PanicButtonPluginData
            Get
                Return DirectCast(MyBase.DataContainer, PanicButtonPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A <see cref="Thread"/> on which <see cref="PanicButtonPlugin.PanicButton"/> will be run within a message-loop.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private hotkeyThread As Thread

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A delegate that represents the method to be invoked when <see cref="PanicButtonPlugin.hotkeyThread"/> begins executed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly hotkeyMethodDelegate As New ParameterizedThreadStart(
            Sub()
                Me.panicButton = New PanicButton(Me.DataContainer)
                System.Windows.Forms.Application.Run()
            End Sub)

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A flag to determine whether a warning log message is sent when the user modifies plugin settings.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private warningIsSent As Boolean

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PanicButtonPlugin"/> class.
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
        ''' Called when this <see cref="PanicButtonPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            If (Me.DataContainer.Enabled) Then
                Me.InitializeThread()
            End If

            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="PanicButtonPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            If Not (Me.DataContainer.Enabled) AndAlso (Me.hotkeyThread IsNot Nothing) Then
                Me.DeinitializeThread()
            ElseIf (Me.DataContainer.Enabled) AndAlso (Me.hotkeyThread Is Nothing) Then
                Me.InitializeThread()
            Else
                If Not (Me.warningIsSent) Then
                    Me.warningIsSent = True
                    Bot.Log("[Panic Button] -> Any modified settings (including the plugin reactivation) will be applied the next time you start SmartBot.")
                End If
            End If

            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="PanicButtonPlugin"/> instance.
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
        ''' Initializes the <see cref="Thread"/> on which <see cref="PanicButtonPlugin.PanicButton"/> will run within a message-loop.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <STAThread>
        Private Sub InitializeThread()
            Me.hotkeyThread = New Thread(Me.hotkeyMethodDelegate) With {
                .IsBackground = True,
                .Priority = ThreadPriority.Lowest
            }

            Me.hotkeyThread.SetApartmentState(ApartmentState.STA)
            Me.hotkeyThread.Start()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Deinitializes the <see cref="Thread"/> on which <see cref="PanicButtonPlugin.PanicButton"/> is running.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <STAThread>
        Private Sub DeinitializeThread()
            If (Me.hotkeyThread IsNot Nothing) Then
                Me.hotkeyThread.Abort()
            End If
            If (Me.panicButton IsNot Nothing) Then
                Me.panicButton.Dispose()
                Me.panicButton = Nothing
            End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
