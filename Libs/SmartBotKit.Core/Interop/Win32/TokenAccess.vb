' ***********************************************************************
' Author   : ElektroStudios
' Modified : 18-April-2016
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Token Access "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Flags combination for <c>desiredAccess</c> parameter of 
    ''' <see cref="NativeMethods.OpenProcessToken"/> function.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa374905%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <Flags>
    Public Enum TokenAccess As UInteger

        ''' <summary>
        ''' 
        ''' </summary>
        StandardRightsRequired = &HF0000UI

        ''' <summary>
        ''' 
        ''' </summary>
        StandradRightsRead = &H20000UI

        ''' <summary>
        ''' Required to attach a primary token to a process.
        ''' <para></para>
        ''' The <see cref="ProcessPrivileges.AssignPrimaryTokenPrivilege"/> privilege is also required to accomplish this task.
        ''' </summary>
        AssignPrimary = &H1UI

        ''' <summary>
        ''' Required to duplicate an access token.
        ''' </summary>
        Duplicate = &H2UI

        ''' <summary>
        ''' Required to attach an impersonation access token to a process.
        ''' </summary>
        Impersonate = &H4UI

        ''' <summary>
        ''' Required to query an access token.
        ''' </summary>
        Query = &H8UI

        ''' <summary>
        ''' Required to query the source of an access token.
        ''' </summary>
        QuerySource = &H10UI

        ''' <summary>
        ''' Required to enable or disable the privileges in an access token.
        ''' </summary>
        AdjustPrivileges = &H20UI

        ''' <summary>
        ''' Required to adjust the attributes of the groups in an access token.
        ''' </summary>
        AdjustGroups = &H40UI

        ''' <summary>
        ''' Required to change the default owner, primary group, or <c>DACL</c> of an access token.
        ''' </summary>
        AdjustDefault = &H80UI

        ''' <summary>
        ''' Required to adjust the session ID of an access token.
        ''' <para></para>
        ''' The <see cref="ProcessPrivileges.TcbPrivilege"/> privilege is required.
        ''' </summary>
        AdjustSessionId = &H100UI

        ''' <summary>
        ''' Combines <see cref="TokenAccess.StandardRightsRequired"/> and <see cref="TokenAccess.Query"/>.
        ''' </summary>
        Read = (TokenAccess.StandardRightsRequired Or TokenAccess.Query)

        ''' <summary>
        ''' Combines all possible access rights for a token.
        ''' </summary>
        AllAccess = (TokenAccess.StandardRightsRequired Or
                     TokenAccess.AssignPrimary Or
                     TokenAccess.Duplicate Or
                     TokenAccess.Impersonate Or
                     TokenAccess.Query Or
                     TokenAccess.QuerySource Or
                     TokenAccess.AdjustPrivileges Or
                     TokenAccess.AdjustGroups Or
                     TokenAccess.AdjustDefault Or
                     TokenAccess.AdjustSessionId)

    End Enum

End Namespace

#End Region
