
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports SmartBotKit.Interop.Win32

#End Region

#Region " Shutdown Reason "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Computer

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Flags for <c>reason</c> parameter of NativeMethods.ExitWindowsEx function.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa376885%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <Flags>
    Public Enum ShutdownReason As UInteger

        ''' <summary>
        ''' Other issue.
        ''' </summary>
        Other = &H0

        ''' <summary>
        ''' Application issue.
        ''' </summary>
        MajorApplication = &H40000

        ''' <summary>
        ''' Hardware issue.
        ''' </summary>
        MajorHardware = &H10000

        ''' <summary>
        ''' The <see cref="NativeMethods.InitiateShutdown"/> function was used instead of <c>InitiateSystemShutdownEx</c> function.
        ''' </summary>
        MajorLegacyApi = &H70000

        ''' <summary>
        ''' Operating system issue.
        ''' </summary>
        MajorOperatingSystem = &H20000

        ''' <summary>
        ''' Power failure.
        ''' </summary>
        MajorPower = &H60000

        ''' <summary>
        ''' Software issue.
        ''' </summary>
        MajorSoftware = &H30000

        ''' <summary>
        ''' System failure..
        ''' </summary>
        MajorSystem = &H50000

        ''' <summary>
        ''' Blue screen crash event.
        ''' </summary>
        MinorBlueScreen = &HF

        ''' <summary>
        ''' Unplugged.
        ''' </summary>
        MinorCordUnplugged = &HB

        ''' <summary>
        ''' Disk.
        ''' </summary>
        MinorDisk = &H7

        ''' <summary>
        ''' Environment.
        ''' </summary>
        MinorEnvironment = &HC

        ''' <summary>
        ''' Driver.
        ''' </summary>
        MinorHardwareDriver = &HD

        ''' <summary>
        ''' Hot fix.
        ''' </summary>
        MinorHotfix = &H11

        ''' <summary>
        ''' Hot fix uninstallation.
        ''' </summary>
        MinorHotfixUninstall = &H17

        ''' <summary>
        ''' Unresponsive.
        ''' </summary>
        MinorHung = &H5

        ''' <summary>
        ''' Installation.
        ''' </summary>
        MinorInstallation = &H2

        ''' <summary>
        ''' Maintenance.
        ''' </summary>
        MinorMaintenance = &H1

        ''' <summary>
        ''' MMC issue.
        ''' </summary>
        MinorMmc = &H19

        ''' <summary>
        ''' Network connectivity.
        ''' </summary>
        MinorNetworkConnectivity = &H14

        ''' <summary>
        ''' Network card.
        ''' </summary>
        MinorNetworkCard = &H9

        ''' <summary>
        ''' Other driver event.
        ''' </summary>
        MinorOtherDriver = &HE

        ''' <summary>
        ''' Power supply.
        ''' </summary>
        MinorPowerSupply = &HA

        ''' <summary>
        ''' Processor.
        ''' </summary>
        MinorProcessor = &H8

        ''' <summary>
        ''' Reconfigure.
        ''' </summary>
        MinorReconfig = &H4

        ''' <summary>
        ''' Security issue.
        ''' </summary>
        MinorSecurity = &H13

        ''' <summary>
        ''' Security patch.
        ''' </summary>
        MinorSecurityFix = &H12

        ''' <summary>
        ''' Security patch uninstallation.
        ''' </summary>
        MinorSecurityFixUninstall = &H18

        ''' <summary>
        ''' Service pack.
        ''' </summary>
        MinorServicePack = &H10

        ''' <summary>
        ''' Service pack uninstallation.
        ''' </summary>
        MinorServicePackUninstall = &H16

        ''' <summary>
        ''' Terminal Services.
        ''' </summary>
        MinorTermSrv = &H20

        ''' <summary>
        ''' Unstable.
        ''' </summary>
        MinorUnstable = &H6

        ''' <summary>
        ''' Upgrade.
        ''' </summary>
        MinorUpgrade = &H3

        ''' <summary>
        ''' WMI issue.
        ''' </summary>
        MinorWmi = &H15

        ''' <summary>
        ''' The reason code is defined by the user.
        ''' <para></para>
        ''' If this flag is not present, the reason code is defined by the system.
        ''' </summary>
        FagUserDefined = &H40000000UI

        ''' <summary>
        ''' The shutdown was planned. 
        ''' <para></para>
        ''' The system generates a System State Data (SSD) file. 
        ''' This file contains system state information such as the processes, threads, memory usage, and configuration.
        ''' <para></para>
        ''' If this flag is not present, the shutdown was unplanned. 
        ''' <para></para>
        ''' Notification and reporting options are controlled by a set of policies. 
        ''' For example, after logging in, the system displays a dialog box reporting the unplanned shutdown 
        ''' if the policy has been enabled. 
        ''' <para></para>
        ''' An SSD file is created only if the SSD policy is enabled on the system. 
        ''' </summary>
        FagUserPlanned = &H80000000UI

    End Enum

End Namespace

#End Region
