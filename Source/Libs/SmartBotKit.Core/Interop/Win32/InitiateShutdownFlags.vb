
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " InitiateShutdown Flags "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Flags for <c>dwShutdownFlags</c> parameter of <see cref="NativeMethods.InitiateShutdown"/> function.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa376872%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <Flags>
    Public Enum InitiateShutdownFlags As UInteger

        ''' <summary>
        ''' Overrides the grace period so that the computer is shut down immediately.
        ''' </summary>
        GraceOverride = &H20UI

        ''' <summary>
        ''' The computer installs any updates before starting the shutdown.
        ''' </summary>
        InstallUpdates = &H40UI

        ''' <summary>
        ''' The computer is shut down but is not powered down or restarted.
        ''' </summary>
        Shutdown = &H10UI

        ''' <summary>
        ''' ( Beginning with Windows 8 )
        ''' <para></para>
        ''' Prepares the system for a faster startup by combining 
        ''' the <see cref="InitiateShutdownFlags.HybridShutdown"/> flag with the 
        ''' <see cref="InitiateShutdownFlags.Shutdown"/> flag.
        ''' <para></para>
        ''' <see cref="NativeMethods.InitiateShutdown"/> always initiate a full system shutdown if the 
        ''' <see cref="InitiateShutdownFlags.HybridShutdown"/> flag is not set. 
        ''' </summary>
        HybridShutdown = &H200UI

        ''' <summary>
        ''' The computer is shut down and powered down.
        ''' </summary>
        PowerOff = &H8UI

        ''' <summary>
        ''' The computer is shut down and restarted.
        ''' </summary>
        Restart = &H4UI

        ''' <summary>
        ''' The system is restarted using the NativeMethods.ExitWindowsEx function with the 
        ''' <see cref="InitiateShutdownFlags.RestartApps"/> flag.
        ''' <para></para>
        ''' This restarts any applications that have been registered for restart 
        ''' using the <c>RegisterApplicationRestart</c> function.
        ''' </summary>
        RestartApps = &H80UI

        ''' <summary>
        ''' Don't force the system to close the applications.
        ''' <para></para>
        ''' This is the default parameter.
        ''' </summary>
        Wait = &H0UI

        ''' <summary>
        ''' All sessions are forcefully logged off. 
        ''' <para></para>
        ''' If this flag is not set and users other than the current user are logged on to the computer 
        ''' specified by the <c>lpMachineName</c> parameter.
        ''' </summary>
        ForceOthers = &H1UI

        ''' <summary>
        ''' Specifies that the originating session is logged off forcefully.
        ''' <para></para>
        ''' If this flag is not set, the originating session is shut down interactively, 
        ''' so a shutdown is not guaranteed even if the function returns successfully.
        ''' </summary>
        ForceSelf = &H2UI

    End Enum

End Namespace

#End Region
