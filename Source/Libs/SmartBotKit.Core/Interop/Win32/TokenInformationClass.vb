' ***********************************************************************
' Author   : ElektroStudios
' Modified : 19-April-2016
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Token Information Class "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies the type of information being assigned to or retrieved from an access token.
    ''' <para></para>
    ''' The <see cref="NativeMethods.GetTokenInformation"/> function uses these values to indicate the type of token information to retrieve.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/es-es/library/windows/desktop/aa379626%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum TokenInformationClass As Integer

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_USER</c> structure 
        ''' that contains the user account of the token.
        ''' </summary>
        TokenUser = 1

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_GROUPS</c> structure 
        ''' that contains the group accounts associated with the token.
        ''' </summary>
        TokenGroups

        ''' <summary>
        ''' The buffer receives a <see cref="TokenPrivileges"/> structure 
        ''' that contains the privileges of the token.
        ''' </summary>
        TokenPrivileges

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_OWNER</c> structure 
        ''' that contains the default owner security identifier (SID) for newly created objects.
        ''' </summary>
        TokenOwner

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_PRIMARY_GROUP</c> structure 
        ''' that contains the default primary group SID for newly created objects.
        ''' </summary>
        TokenPrimaryGroup

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_DEFAULT_DACL</c> structure 
        ''' that contains the default <c>DACL</c> for newly created objects.
        ''' </summary>
        TokenDefaultDacl

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_SOURCE</c> structure 
        ''' that contains the source of the token.
        ''' <para></para>
        ''' <see cref="TokenAccess.QuerySource"/> access is needed to retrieve this information.
        ''' </summary>
        TokenSource

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_TYPE</c> value 
        ''' that indicates whether the token is a primary or impersonation token.
        ''' </summary>
        TokenType

        ''' <summary>
        ''' The buffer receives a SecurityImpersonationLevel value 
        ''' that indicates the impersonation level of the token.
        ''' <para></para>
        ''' If the access token is not an impersonation token, the function fails.
        ''' </summary>
        TokenImpersonationLevel

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_STATISTICS</c> structure 
        ''' that contains various token statistics.
        ''' </summary>
        TokenStatistics

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_GROUPS</c> structure 
        ''' that contains the list of restricting SIDs in a restricted token.
        ''' </summary>
        TokenRestrictedSids

        ''' <summary>
        ''' The buffer receives a <c>DWORD</c> value 
        ''' that indicates the Terminal Services session identifier that is associated with the token.
        ''' </summary>
        TokenSessionId

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_GROUPS_AND_PRIVILEGES</c> structure 
        ''' that contains the user <c>SID</c>, the group accounts, the restricted <c>SID</c>s, 
        ''' and the authentication ID associated with the token.
        ''' </summary>
        TokenGroupsAndPrivileges

        ''' <summary>
        ''' Reserved.
        ''' </summary>
        TokenSessionReference

        ''' <summary>
        ''' The buffer receives a <c>DWORD</c> value 
        ''' that is nonzero if the token includes the <c>SANDBOX_INERT</c> flag.
        ''' </summary>
        TokenSandBoxInert

        ''' <summary>
        ''' Reserved.
        ''' </summary>
        TokenAuditPolicy

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_ORIGIN</c> value.
        ''' </summary>
        TokenOrigin

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_ELEVATION_TYP</c>E value 
        ''' that specifies the elevation level of the token.
        ''' </summary>
        TokenElevationType

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_LINKED_TOKEN</c> structure 
        ''' that contains a handle to another token that is linked to this token.
        ''' </summary>
        TokenLinkedToken

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_ELEVATION</c> structure 
        ''' that specifies whether the token is elevated.
        ''' </summary>
        TokenElevation

        ''' <summary>
        ''' The buffer receives a <c>DWORD</c> value 
        ''' that is nonzero if the token has ever been filtered.
        ''' </summary>
        TokenHasRestrictions

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_ACCESS_INFORMATION</c> structure 
        ''' that specifies security information contained in the token.
        ''' </summary>
        TokenAccessInformation

        ''' <summary>
        ''' The buffer receives a <c>DWORD</c> value 
        ''' that is nonzero if virtualization is allowed for the token.
        ''' </summary>
        TokenVirtualizationAllowed

        ''' <summary>
        ''' The buffer receives a <c>DWORD</c> value 
        ''' that is nonzero if virtualization is enabled for the token.
        ''' </summary>
        TokenVirtualizationEnabled

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_MANDATORY_LABEL</c> structure 
        ''' that specifies the token's integrity level.
        ''' </summary>
        TokenIntegrityLevel

        ''' <summary>
        ''' The buffer receives a <c>DWORD</c> value 
        ''' that is nonzero if the token has the UIAccess flag set.
        ''' </summary>
        TokenUIAccess

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_MANDATORY_POLICY</c> structure 
        ''' that specifies the token's mandatory integrity policy.
        ''' </summary>
        TokenMandatoryPolicy

        ''' <summary>
        ''' The buffer receives the token's logon security identifier (SID).
        ''' </summary>
        TokenLogonSid

        ''' <summary>
        ''' The buffer receives a <c>DWORD</c> value 
        ''' that is nonzero if the token is an app container token.
        ''' <para></para>
        ''' Any callers who check the <c>TokenIsAppContainer</c> and have it return <c>0</c> 
        ''' should also verify that the caller token is not an identify level impersonation token.
        ''' <para></para>
        ''' If the current token is not an app container but is an identity level token, you should return <c>AccessDenied</c>.
        ''' </summary>
        TokenIsAppContainer

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_GROUPS</c> structure 
        ''' that contains the capabilities associated with the token.
        ''' </summary>
        TokenCapabilities

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_APPCONTAINER_INFORMATION</c> structure 
        ''' that contains the <c>AppContainerSid</c> associated with the token.
        ''' <para></para>
        ''' If the token is not associated with an app container, 
        ''' the <c>TokenAppContainer</c> member of the <c>TOKEN_APPCONTAINER_INFORMATION</c> structure 
        ''' points to <see langword="Nothing"/>
        ''' </summary>
        TokenAppContainerSid

        ''' <summary>
        ''' The buffer receives a <c>DWORD</c> value that includes the app container number for the token.
        ''' <para></para>
        ''' For tokens that are not app container tokens, this value is <c>0</c>.
        ''' </summary>
        TokenAppContainerNumber

        ''' <summary>
        ''' The buffer receives a <c>CLAIM_SECURITY_ATTRIBUTES_INFORMATION</c> structure 
        ''' that contains the user claims associated with the token.
        ''' </summary>
        TokenUserClaimAttributes

        ''' <summary>
        ''' The buffer receives a <c>CLAIM_SECURITY_ATTRIBUTES_INFORMATION</c> structure 
        ''' that contains the device claims associated with the token.
        ''' </summary>
        TokenDeviceClaimAttributes

        ''' <summary>
        ''' This value is reserved.
        ''' </summary>
        TokenRestrictedUserClaimAttributes

        ''' <summary>
        ''' This value is reserved.
        ''' </summary>
        TokenRestrictedDeviceClaimAttributes

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_GROUPS</c> structure 
        ''' that contains the device groups that are associated with the token
        ''' </summary>
        TokenDeviceGroups

        ''' <summary>
        ''' The buffer receives a <c>TOKEN_GROUPS</c> structure 
        ''' that contains the restricted device groups that are associated with the token.
        ''' </summary>
        TokenRestrictedDeviceGroups

        ''' <summary>
        ''' This value is reserved.
        ''' </summary>
        TokenSecurityAttributes

        ''' <summary>
        ''' This value is reserved.
        ''' </summary>
        TokenIsRestricted

        ''' <summary>
        ''' The maximum value for this enumeration.
        ''' </summary>
        MaxTokenInfoClass

    End Enum

End Namespace

#End Region
