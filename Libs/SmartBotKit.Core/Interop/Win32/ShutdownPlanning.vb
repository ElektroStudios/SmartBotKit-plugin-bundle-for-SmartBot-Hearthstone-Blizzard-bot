' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-November-2015
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Shutdown Planning "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Flags combination for <c>dwReason</c> parameter of NativeMethods.ExitWindowsEx 
    ''' and <see cref="NativeMethods.InitiateShutdown"/> functions.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa376885%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum ShutdownPlanning As UInteger

        ''' <summary>
        ''' The shutdown was unplanned.
        ''' <para></para>
        ''' This is the default parameter.
        ''' </summary>
        Unplanned = &H0UI

        ''' <summary>
        ''' The reason code is defined by the user.
        ''' <para></para>
        ''' If this flag is not present, the reason code is defined by the system.
        ''' For more information, see Defining a Custom Reason Code.
        ''' </summary>
        UserDefined = &H40000000UI

        ''' <summary>
        ''' The shutdown was planned. 
        ''' <para></para>
        ''' If this flag is not present, the shutdown was unplanned.
        ''' <para></para>
        ''' The system generates a System State Data (SSD) file. 
        ''' This file contains system state information such as the processes, threads, memory usage, and configuration.
        ''' </summary>
        Planned = &H80000000UI

    End Enum

End Namespace

#End Region
