
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics

Imports SmartBot.Plugins.API

Imports SmartBotKit.Computer
Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.IO

#End Region

#Region " PanicButton "

' ReSharper disable once CheckNamespace

Namespace PanicButton


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents a system-wide hotkey that will kill SmartBot program when is pressed.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class PanicButton : Inherits Hotkey : Implements IDisposable

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The plugin's data container.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly dataContainer As PanicButtonPluginData

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PanicButton"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub New(ByVal dataContainer As PanicButtonPluginData)
            MyBase.New(dataContainer.ModifierA Or dataContainer.ModifierB, dataContainer.Key)
            Me.dataContainer = dataContainer

            Me.Register()

            Bot.Log(
                $"[Panic Button] Plugin initialized. Active hotkey is: {{ {dataContainer.ModifierA.ToString()} + { _
                       dataContainer.ModifierB.ToString()} + {dataContainer.Key.ToString()} }} ")
        End Sub

#End Region

#Region " Event Handlers "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="HotKey.Press"/> event of this <see cref="PanicButton"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        '''
        ''' <param name="e">
        ''' The <see cref="HotkeyPressEventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub PanicButton_Press(ByVal sender As Object, ByVal e As HotkeyPressEventArgs) Handles Me.Press

            Select Case Me.dataContainer.SetComputerState

                Case ComputerState.Hibernate
                    Bot.Log("[Panic Button] -> Hibernating the computer...")
                    PowerUtil.Hibernate(force:=True)

                Case ComputerState.Suspend
                    Bot.Log("[Panic Button] -> Suspending the computer...")
                    PowerUtil.Suspend(force:=True)

                Case ComputerState.Shutdown
                    Bot.Log("[Panic Button] -> Powering off the computer...")
                    PowerUtil.Shutdown("", 0, "", ShutdownMode.ForceOthers, ShutdownReason.FagUserPlanned, ShutdownPlanning.Planned, True)

                Case Else ' ComputerState.NoChange
                    If (Me.dataContainer.KillProcess) Then
                        Bot.Log("[Panic Button] Terminating SmartBot process by user demand...")
                        ' Me.Dispose()
                        Bot.CloseBot()
                    Else
                        If (Bot.IsBotRunning) Then
                            Bot.StopBot()
                            Bot.Log("[Panic Button] Bot stopped by user demand.")
                        End If
                    End If

            End Select

        End Sub

#End Region

#Region " IDisposable Implementation "

        '''' ----------------------------------------------------------------------------------------------------
        '''' <summary>
        '''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged Global.System.Resources.
        '''' </summary>
        '''' ----------------------------------------------------------------------------------------------------
        '''' <param name="isDisposing">
        '''' <see langword="True"/>  to release both managed and unmanaged Global.System.Resources.
        '''' <see langword="False"/> to release only unmanaged Global.System.Resources.
        '''' </param>
        '''' ----------------------------------------------------------------------------------------------------
        Protected Overrides Sub Dispose(isDisposing As Boolean)
            If (Not Me.isDisposed) AndAlso (isDisposing) Then
                Bot.Log("[Panic Button] Plugin deinitialized.")
            End If
            MyBase.Dispose(isDisposing)
        End Sub

#End Region

    End Class

End Namespace

#End Region
