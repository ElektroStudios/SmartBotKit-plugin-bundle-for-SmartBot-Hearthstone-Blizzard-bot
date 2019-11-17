
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " TokenPrivileges "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains information about a set of privileges for an access token.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa379630%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <StructLayout(LayoutKind.Sequential)>
    Public Structure TokenPrivileges

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' This must be set to the number of entries in the Privileges array
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public PrivilegeCount As UInteger

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Specifies an array of <see cref="LuIdAndAttributes"/> structures.
        ''' <para></para>
        ''' Each structure contains the LUID and attributes of a privilege. 
        ''' <para></para>
        ''' To get the name of the privilege associated with a LUID, call the <c>LookupPrivilegeName</c> function, 
        ''' passing the address of the LUID as the value of the <c>lpLuid</c> parameter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)>
        Public Privileges As LuidAndAttributes()

    End Structure

End Namespace

#End Region
