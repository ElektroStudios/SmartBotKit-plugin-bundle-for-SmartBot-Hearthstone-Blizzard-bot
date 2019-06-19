#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " ERole "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Defines constants that indicate the role that the system has assigned to an audio endpoint device.
    ''' <para></para>
    ''' The <see cref="IMMDeviceEnumerator.GetDefaultAudioEndpoint"/> and 
    ''' IMMNotificationClient.OnDefaultDeviceChanged methods use the constants defined in the <see cref="ERole"/> enumeration.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://docs.microsoft.com/en-us/windows/desktop/api/mmdeviceapi/ne-mmdeviceapi-__midl___midl_itf_mmdeviceapi_0000_0000_0002"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum ERole As Integer

        ' ReSharper disable InconsistentNaming

        ''' <summary>
        ''' Games, system notification sounds, and voice commands.
        ''' </summary>
        Console

        ''' <summary>
        ''' Music, movies, narration, and live music recording.
        ''' </summary>
        Multimedia

        ''' <summary>
        ''' Voice communications (talking to another person).
        ''' </summary>
        Communications

        ''' <summary>
        ''' The number of members in the <see cref="ERole"/> enumeration (not counting the <see cref="ERole.ERole_enum_count"/> member).
        ''' </summary>
        ERole_enum_count

    End Enum

End Namespace

#End Region
