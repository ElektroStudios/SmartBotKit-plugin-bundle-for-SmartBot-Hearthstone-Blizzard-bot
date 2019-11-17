' ***********************************************************************
' Author   : ElektroStudios
' Modified : 09-November-2015
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports SmartBotKit.Interop.Win32

#End Region

#Region " Shutdown Mode "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Computer

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies the mode to shutdown/restart the system.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum ShutdownMode As UInteger

        ''' <summary>
        ''' Don't force the system to close the applications.
        ''' </summary>
        Wait = InitiateShutdownFlags.Wait

        ''' <summary>
        ''' All sessions are forcefully logged off.
        ''' </summary>
        ForceOthers = InitiateShutdownFlags.ForceOthers

        ''' <summary>
        ''' Specifies that the originating session is logged off forcefully.
        ''' <para></para>
        ''' If this flag is not set, the originating session is shut down interactively, 
        ''' so a shutdown is not guaranteed.
        ''' </summary>
        ForceSelf = InitiateShutdownFlags.ForceSelf

    End Enum

End Namespace

#End Region
