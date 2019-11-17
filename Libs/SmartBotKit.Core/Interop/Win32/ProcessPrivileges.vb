
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Process Privileges "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies a process privilege.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb530716%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <Flags>
    Public Enum ProcessPrivileges As Long

        ''' <summary>
        ''' Any process privilege.
        ''' </summary>
        None = 0L

        ''' <summary>
        ''' Required to assign the primary token of a process.
        ''' <para></para>
        ''' User Right: Replace a process-level token.
        ''' </summary>
        AssignPrimaryTokenPrivilege = 1L

        ''' <summary>
        ''' Required to generate audit-log entries. Give this privilege to secure servers,
        ''' <para></para>
        ''' User Right: Generate security audits.
        ''' </summary>
        AuditPrivilege = 2L

        ''' <summary>
        ''' Required to perform backup operations.
        ''' <para></para>
        ''' This privilege causes the system to grant all read access control to any file, 
        ''' regardless of the access control list (ACL) specified for the file.
        ''' <para></para>
        ''' Any access request other than read is still evaluated with the ACL.
        ''' <para></para>
        ''' This privilege is required by the <c>RegSaveKey</c> and <c>RegSaveKeyExfunctions</c>. 
        ''' <para></para>
        ''' User Right: Back up files and directories.
        ''' </summary>
        BackupPrivilege = 4L

        ''' <summary>
        ''' Required to receive notifications of changes to files or directories.
        ''' <para></para>
        ''' This privilege also causes the system to skip all traversal access checks.
        ''' <para></para>
        ''' It is enabled by default for all users.
        ''' <para></para>
        ''' User Right: Bypass traverse checking.
        ''' </summary>
        ChangeNotifyPrivilege = 8L

        ''' <summary>
        ''' Required to create named file mapping objects in the global Namespace DevCase.Interop.during Terminal Services sessions.
        ''' <para></para>
        ''' This privilege is enabled by default for administrators, services, and the local system account
        ''' <para></para>
        ''' User Right: Create global objects.
        ''' </summary>
        CreateGlobalPrivilege = 16L

        ''' <summary>
        ''' Required to create a paging file.
        ''' <para></para>
        ''' User Right: Create a pagefile.
        ''' </summary>
        CreatePagefilePrivilege = 32L

        ''' <summary>
        ''' Required to create a permanent object.
        ''' <para></para>
        ''' User Right: Create permanent shared objects.
        ''' </summary>
        CreatePermanentPrivilege = 64L

        ''' <summary>
        ''' Required to create a symbolic link.
        ''' <para></para>
        ''' User Right: Create symbolic links.
        ''' </summary>
        CreateSymbolicLinkPrivilege = 128L

        ''' <summary>
        ''' Required to create a primary token.
        ''' <para></para>
        ''' User Right: Create a token object.
        ''' </summary>
        CreateTokenPrivilege = 256L

        ''' <summary>
        ''' Required to debug and adjust the memory of a process owned by another account.
        ''' <para></para>
        ''' User Right: Debug programs.
        ''' </summary>
        DebugPrivilege = 512L

        ''' <summary>
        ''' Required to mark user and computer accounts as trusted for delegation.
        ''' <para></para>
        ''' User Right: Enable computer and user accounts to be trusted for delegation.
        ''' </summary>
        EnableDelegationPrivilege = 1024L

        ''' <summary>
        ''' Required to impersonate.
        ''' <para></para>
        ''' User Right: Impersonate a client after authentication.
        ''' </summary>
        ImpersonatePrivilege = 2048L

        ''' <summary>
        ''' Required to increase the base priority of a process.
        ''' <para></para>
        ''' User Right: Increase scheduling priority.
        ''' </summary>
        IncreaseBasePriorityPrivilege = 4096L

        ''' <summary>
        ''' Required to increase the quota assigned to a process.
        ''' <para></para>
        ''' User Right: Adjust memory quotas for a process.
        ''' </summary>
        IncreaseQuotaPrivilege = 8192L

        ''' <summary>
        ''' Required to allocate more memory for applications that run in the context of users.
        ''' <para></para>
        ''' User Right: Increase a process working set.
        ''' </summary>
        IncreaseWorkingSetPrivilege = 16384L

        ''' <summary>
        ''' Required to load or unload a device driver. 
        ''' <para></para>
        ''' User Right: Load and unload device drivers.
        ''' </summary>
        LoadDriverPrivilege = 32768L

        ''' <summary>
        ''' Required to lock physical pages in memory. 
        ''' <para></para>
        ''' User Right: Lock pages in memory.
        ''' </summary>
        LockMemoryPrivilege = 65536L

        ''' <summary>
        ''' Required to create a computer account. 
        ''' <para></para>
        ''' User Right: Add workstations to domain.
        ''' </summary>
        MachineAccountPrivilege = 131072L

        ''' <summary>
        ''' Required to enable volume management privileges. 
        ''' <para></para>
        ''' User Right: Manage the files on a volume.
        ''' </summary>
        ManageVolumePrivilege = 262144L

        ''' <summary>
        ''' Required to gather profiling information for a single process. 
        ''' <para></para>
        ''' User Right: Profile single process.
        ''' </summary>
        ProfileSingleProcessPrivilege = 524288L

        ''' <summary>
        ''' Required to modify the mandatory integrity level of an object.
        ''' <para></para>
        ''' User Right: Modify an object label.
        ''' </summary>
        RelabelPrivilege = 1048576L

        ''' <summary>
        ''' Required to shut down a system using a network request. 
        ''' <para></para>
        ''' User Right: Force shutdown from a remote system.
        ''' </summary>
        RemoteShutdownPrivilege = 2097152L

        ''' <summary>
        ''' Required to perform restore operations.
        ''' <para></para>
        ''' This privilege causes the system to grant all write access control to any file, 
        ''' regardless of the ACL specified for the file.
        ''' <para></para>
        ''' Any access request other than write is still evaluated with the ACL.
        ''' <para></para>
        ''' Additionally, this privilege enables you to set any valid user or group SID as the owner of a file.
        ''' <para></para>
        ''' This privilege is required by the NativeMethods.RegLoadKey function.
        ''' <para></para>
        ''' User Right: Restore files and directories.
        ''' </summary>
        RestorePrivilege = 4194304L

        ''' <summary>
        ''' Required to perform a number of security-related functions, such as controlling and viewing audit messages.
        ''' <para></para>
        ''' This privilege identifies its holder as a security operator
        ''' <para></para>
        ''' User Right: Manage auditing and security log.
        ''' </summary>
        SecurityPrivilege = 8388608L

        ''' <summary>
        ''' Required to shut down a local system.
        ''' <para></para>
        ''' User Right: Shut down the system.
        ''' </summary>
        ShutdownPrivilege = 16777216L

        ''' <summary>
        ''' Required for a domain controller to use the Lightweight Directory Access Protocol directory synchronization services.
        ''' <para></para>
        ''' This privilege enables the holder to read all objects and properties in the directory, 
        ''' regardless of the protection on the objects and properties. 
        ''' <para></para>
        ''' By default, it is assigned to the Administrator and LocalSystem accounts on domain controllers. 
        ''' <para></para>
        ''' User Right: Synchronize directory service data.
        ''' </summary>
        SyncAgentPrivilege = 33554432L

        ''' <summary>
        ''' Required to modify the nonvolatile <c>RAM</c> memory of systems that use this type of memory to store configuration information.
        ''' <para></para>
        ''' User Right: Modify firmware environment values.
        ''' </summary>
        SystemEnvironmentPrivilege = 67108864L

        ''' <summary>
        ''' Required to gather profiling information for the entire system.
        ''' <para></para>
        ''' User Right: Profile system performance.
        ''' </summary>
        SystemProfilePrivilege = 134217728L

        ''' <summary>
        ''' Required to modify the system time. 
        ''' <para></para>
        ''' User Right: Change the system time.
        ''' </summary>
        SystemtimePrivilege = 268435456L

        ''' <summary>
        ''' Required to take ownership of an object without being granted discretionary access.
        ''' <para></para>
        ''' This privilege allows the owner value to be set only to 
        ''' those values that the holder may legitimately assign as the owner of an object. 
        ''' <para></para>
        ''' 
        ''' </summary>
        TakeOwnershipPrivilege = 536870912L

        ''' <summary>
        ''' This privilege identifies its holder as part of the trusted computer base.
        ''' <para></para>
        ''' Some trusted protected subsystems are granted this privilege.
        ''' <para></para>
        ''' User Right: Act as part of the operating system.
        ''' </summary>
        TcbPrivilege = 1073741824L

        ''' <summary>
        ''' Required to adjust the time zone associated with the computer's internal clock.
        ''' <para></para>
        ''' User Right: Change the time zone.
        ''' </summary>
        TimeZonePrivilege = 2147483648L

        ''' <summary>
        ''' Required to access Credential Manager as a trusted caller.
        ''' <para></para>
        ''' User Right: Access Credential Manager as a trusted caller.
        ''' </summary>
        TrustedCredManAccessPrivilege = 4294967296L

        ''' <summary>
        ''' Required to undock a laptop.
        ''' <para></para>
        ''' User Right: Remove computer from docking station.
        ''' </summary>
        UndockPrivilege = 8589934592L

        ''' <summary>
        ''' Required to read unsolicited input from a terminal device.
        ''' <para></para>
        ''' User Right: Not applicable.
        ''' </summary>
        UnsolicitedInputPrivilege = 17179869184L

    End Enum

End Namespace

#End Region
