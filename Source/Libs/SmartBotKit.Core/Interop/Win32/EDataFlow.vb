#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " EDataFlow "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Defines constants that indicate the direction in which audio data flows between an audio endpoint device and an application.
    ''' <para></para>
    ''' The <see cref="IMMDeviceEnumerator.GetDefaultAudioEndpoint"/>, 
    ''' IMMDeviceEnumerator.EnumAudioEndpoints, 
    ''' IMMEndpoint.GetDataFlow, and IMMNotificationClient.OnDefaultDeviceChanged methods 
    ''' use the constants defined in the <see cref="EDataFlow"/> enumeration.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://docs.microsoft.com/en-us/windows/desktop/api/mmdeviceapi/ne-mmdeviceapi-__midl___midl_itf_mmdeviceapi_0000_0000_0001"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum EDataFlow As Integer

        ' ReSharper disable InconsistentNaming

        ''' <summary>
        ''' Audio rendering stream. 
        ''' <para></para>
        ''' Audio data flows from the application to the audio endpoint device, which renders the stream.
        ''' </summary>
        Render

        ''' <summary>
        ''' Audio capture stream. 
        ''' <para></para>
        ''' Audio data flows from the audio endpoint device that captures the stream, to the application.
        ''' </summary>
        Capture

        ''' <summary>
        ''' Audio rendering or capture stream. 
        ''' <para></para>
        ''' Audio data can flow either from the application to the audio endpoint device, 
        ''' or from the audio endpoint device to the application.
        ''' </summary>
        All

        ''' <summary>
        ''' The number of members in the <see cref="EDataFlow"/> enumeration (not counting the <see cref="EDataFlow.EDataFlow_enum_count"/> member).
        ''' </summary>
        EDataFlow_enum_count

    End Enum

End Namespace

#End Region
