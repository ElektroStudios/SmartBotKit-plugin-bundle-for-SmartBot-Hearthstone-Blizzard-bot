
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel

Imports SmartBotKit.IPC
Imports SmartBotKit.Interop.Win32

#End Region

#Region " Power Util "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Computer

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains system powering related utilities.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class PowerUtil

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="PowerUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The maximum number of seconds to wait before shutting down the computer. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/es-es/library/windows/desktop/aa376872%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        Public Const MaxShutdownTimeout As Integer = 315360000I

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Shutdowns the specified computer.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The name of the computer to be shut down.
        ''' <para></para>
        ''' If the value of this parameter is an empty string, the local computer is shut down.
        ''' <para></para>
        ''' This parameter can be an addres, for example: <c>127.0.0.1</c>
        ''' </param>
        ''' 
        ''' <param name="message">
        ''' The message to be displayed in the interactive shutdown dialog box.
        ''' </param>
        ''' 
        ''' <param name="timeout">
        ''' The number of seconds to wait before shutting down the computer.
        ''' <para></para>
        ''' If the value of this parameter is zero, the computer is shut down immediately.
        ''' <para></para>
        ''' This value is limited to <see cref="PowerUtil.MaxShutdownTimeout"/>.
        ''' </param>
        ''' 
        ''' <param name="mode">
        ''' Indicates whether to force the shutdown.
        ''' </param>
        ''' 
        ''' <param name="reason">
        ''' The reason for initiating the shutdown.
        ''' By default, it is also an 'unplanned' shutdown.
        ''' <para></para>
        ''' If this parameter is zero,
        ''' the default is an undefined shutdown that is logged as "No title for this reason could be found".
        ''' </param>
        ''' 
        ''' <param name="planning">
        ''' Indicates whether it's a planned or unplanned shutdoen operation.
        ''' </param>
        ''' 
        ''' <param name="ignoreErrors">
        ''' If <see langword="True"/>, a <see cref="Win32Exception"/> exception will be thrown if error found.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' Timeout should be zero or greater than zero.;timeout
        ''' </exception>
        ''' 
        ''' <exception cref="Win32Exception">
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the shutdown operation is initiated correctlly, <see langword="False"/> otherwise.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function Shutdown(Optional computer As String = "",
                                        Optional timeout As Integer = 0,
                                        Optional message As String = "",
                                        Optional mode As ShutdownMode = ShutdownMode.Wait,
                                        Optional reason As ShutdownReason = ShutdownReason.Other,
                                        Optional planning As ShutdownPlanning = ShutdownPlanning.Unplanned,
                                        Optional ignoreErrors As Boolean = True) As Boolean

            PowerUtil.GetShutdownPrivileges(computer)

            Dim exitCode As UInteger

            Select Case timeout

                Case Is < 0
                    Throw New ArgumentException(message:="Timeout should be zero or greater than zero.", paramName:=NameOf(timeout))

                Case Is = 0
                    exitCode = NativeMethods.InitiateShutdown(computer, message, timeout,
                                                              InitiateShutdownFlags.Shutdown Or
                                                              InitiateShutdownFlags.GraceOverride Or mode,
                                                              reason Or planning)

                Case Else
                    exitCode = NativeMethods.InitiateShutdown(computer, message, timeout,
                                                              InitiateShutdownFlags.Shutdown Or mode,
                                                              reason Or planning)

            End Select

            If (exitCode <> 0) AndAlso (Not ignoreErrors) Then
                Throw New Win32Exception([error]:=Convert.ToInt32(exitCode))

            Else
                Return True

            End If

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the system in Hibernate state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' If an application does not respond to a Hibernate request within 20 seconds, 
        ''' the operating system determines that it is in a non-responsive state, 
        ''' and that the application can either be put to sleep or terminated. 
        ''' <para></para>
        ''' Once an application responds to a Hibernate request, 
        ''' it can take whatever time it needs to clean up resources and shut down active processes.
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="force">
        ''' <see langword="True"/> to force the Hibernate mode immediately; 
        ''' <see langword="False"/> to cause the operating system to send a Hibernate request to every application.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub Hibernate(Optional force As Boolean = False)

            Global.System.Windows.Forms.Application.SetSuspendState(PowerState.Hibernate, force:=force, disableWakeEvent:=False)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the system in Suspend state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' If an application does not respond to a Suspend request within 20 seconds, 
        ''' the operating system determines that it is in a non-responsive state, 
        ''' and that the application can either be put to sleep or terminated. 
        ''' <para></para>
        ''' Once an application responds to a suspend request, 
        ''' it can take whatever time it needs to clean up resources and shut down active processes.
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="force">
        ''' <see langword="True"/> to force the suspended mode immediately; 
        ''' <see langword="False"/> to cause the operating system to send a suspend request to every application.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub Suspend(Optional force As Boolean = False)

            Global.System.Windows.Forms.Application.SetSuspendState(PowerState.Suspend, force:=force, disableWakeEvent:=False)

        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the necessary shutdown privileges to perform a local or remote shutdown operation.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The computer where to set the privileges for the process to shutdown the system.
        ''' <para></para>
        ''' The computer name can be an empty string.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Shared Sub GetShutdownPrivileges(computer As String)

            Const privileges As ProcessPrivileges = (ProcessPrivileges.ShutdownPrivilege Or ProcessPrivileges.RemoteShutdownPrivilege)

            ProcessUtil.SetCurrentProcessPrivileges(privileges, PrivilegeStates.PrivilegeEnabled)

        End Sub

#End Region

    End Class

End Namespace

#End Region
