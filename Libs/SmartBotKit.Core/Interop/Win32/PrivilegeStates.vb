' ***********************************************************************
' Author   : ElektroStudios
' Modified : 27-April-2016
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Privilege State "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies a privilege state.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa379630%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <Flags>
    Public Enum PrivilegeStates As UInteger

        ''' <summary>
        ''' The privilege is disabled.
        ''' </summary>        
        ''' <remarks>
        ''' <see href="http://referencesource.microsoft.com/system.servicemodel/System/ServiceModel/ComIntegration/SafeNativeMethods.cs.html#"/>
        ''' </remarks>
        PrivilegeDisabled = &H0UI

        ''' <summary>
        ''' The privilege is enabled by default.
        ''' </summary>
        PrivilegeEnabledByDefault = &H1UI

        ''' <summary>
        ''' The privilege is enabled.
        ''' </summary>
        PrivilegeEnabled = &H2UI

        ''' <summary>
        ''' Used to remove a privilege.
        ''' </summary>
        PrivilegeRemoved = &H4UI

        ''' <summary>
        ''' The privilege was used to gain access to an object or service.
        ''' <para></para>
        ''' This flag is used to identify the relevant privileges 
        ''' in a set passed by a client application that may contain unnecessary privileges
        ''' </summary>
        PrivilegeUsedForAccess = &H80000000UI

    End Enum

End Namespace

#End Region
