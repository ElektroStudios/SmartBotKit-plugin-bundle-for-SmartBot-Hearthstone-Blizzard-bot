
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.Win32

Imports System.Linq
Imports System.Management

#End Region

#Region " SystemInfo ( Windows ) "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.SystemInfo


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains operating system info.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class SystemInfoUtil

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="SystemInfo"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the current application is running in a Virtual Machine OS.
        ''' <para></para>
        ''' The detection algorithm is compatible with:
        ''' <para></para>
        ''' Virtual-Box, VMWare, and QEmu.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' <see langword="True"/> if the current application is running in a Virtual Machine OS, <see langword="False"/> otherwise.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsVirtualMachine As Boolean
            <DebuggerStepThrough>
            Get
                Return SystemInfoUtil.InternalIsVirtualMachine()
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows XP</c>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If IsWinXP Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under Windows XP.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows XP</c>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWinXp() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.Major = 5)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows XP</c>, or greater.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If Not IsWinXPOrGreater Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under the current Windows version.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows XP</c>, or greater.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWinXpOrGreater() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.Major >= 5)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows VISTA</c>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If IsWinVista Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under Windows VISTA.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows VISTA</c>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWinVista() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.CompareTo(New Version(6, 0)) = 0)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows VISTA</c>, or greater.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If Not IsWinVistaOrGreater Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under the current Windows version.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows VISTA</c>, or greater.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWinVistaOrGreater() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.CompareTo(New Version(6, 0)) >= 0)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows 7</c>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If IsWin7 Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under Windows 7.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows 7</c>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWin7() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.CompareTo(New Version(6, 1)) = 0)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows 7</c>, or greater.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If Not IsWin7OrGreater Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under the current Windows version.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows 7</c>, or greater.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWin7OrGreater() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.CompareTo(New Version(6, 1)) >= 0)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows 8</c>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If IsWin8 Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under Windows 8.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows 8</c>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWin8() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.CompareTo(New Version(6, 2)) = 0)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows 8</c>, or greater.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If Not IsWin8OrGreater Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under the current Windows version.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows 8</c>, or greater.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWin8OrGreater() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.CompareTo(New Version(6, 2)) >= 0)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows 8.1</c>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If IsWin81 Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under Windows 8.1.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows 8.1</c>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWin81() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.CompareTo(New Version(6, 3)) = 0) AndAlso
                   Not (SystemInfoUtil.InternalIsWin10())
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows 8.1</c>, or greater.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If Not IsWin81OrGreater Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under the current Windows version.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows 8.1</c>, or greater.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWin81OrGreater() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (Environment.OSVersion.Version.CompareTo(New Version(6, 2)) >= 0)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determines whether the current operating system is <c>Windows 10</c>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' If IsWin10 Then
        '''     Throw New PlatformNotSupportedException("This application cannot run under Windows 10.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determines whether the current operating system is <c>Windows 10</c>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsWin10() As Boolean
            <DebuggerStepThrough>
            Get
                Return (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                       (SystemInfoUtil.InternalIsWin10())
            End Get
        End Property

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the current application is running in a Virtual Machine OS.
        ''' <para></para>
        ''' The detection algorithm is compatible with:
        ''' <para></para>
        ''' Virtual-Box, VMWare, and QEmu.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the current application is running in a Virtual Machine OS, <see langword="False"/> otherwise.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Shared Function InternalIsVirtualMachine() As Boolean

            Dim modelName As String
            Dim scope As New ManagementScope("root\CIMV2")
            Dim query As New SelectQuery("SELECT * FROM Win32_DiskDrive WHERE BytesPerSector > 0")
            Dim options As New EnumerationOptions With {.ReturnImmediately = True, .Rewindable = False}

            Using mos As New ManagementObjectSearcher(scope, query, options),
                  moc As ManagementObjectCollection = mos.Get()

                For Each mo As ManagementObject In moc

                    modelName = mo("Model").ToString().Split(" "c).FirstOrDefault()

                    If Not (String.IsNullOrEmpty(modelName)) AndAlso (
                           (modelName.Equals("virtual", StringComparison.OrdinalIgnoreCase)) OrElse
                           (modelName.Equals("vmware", StringComparison.OrdinalIgnoreCase)) OrElse
                           (modelName.Equals("vbox", StringComparison.OrdinalIgnoreCase)) OrElse
                           (modelName.Equals("qemu", StringComparison.OrdinalIgnoreCase))
                    ) Then
                        Return True ' Virtual machine HDD Model Name found.

                    End If

                Next mo

            End Using

            Return False

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the current operating system is <c>Windows 10</c>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/es-es/library/windows/desktop/dn424972%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the current operating system is <c>Windows 10</c>; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Shared Function InternalIsWin10() As Boolean

            Using reg As RegistryKey = Microsoft.Win32.Registry.LocalMachine.
                                       OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", writable:=False)

                Dim productName As String = DirectCast(reg.GetValue("ProductName", "Empty", RegistryValueOptions.None), String)
                Return productName.StartsWith("Windows 10", StringComparison.OrdinalIgnoreCase)

            End Using

        End Function

#End Region

    End Class

End Namespace

#End Region
