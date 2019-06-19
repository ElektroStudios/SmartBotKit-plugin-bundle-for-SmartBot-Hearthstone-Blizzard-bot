#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " ISimpleAudioVolume "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Enables a client to control the master volume level of an audio session. 
    ''' <para></para>
    ''' The IAudioClient.Initialize method initializes a stream object and assigns the stream to an audio session. 
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://docs.microsoft.com/en-us/windows/desktop/api/audioclient/nn-audioclient-isimpleaudiovolume"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8")>
    Public Interface ISimpleAudioVolume

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the master volume level for the audio session.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="levelNormalization">
        ''' The new volume level expressed as a normalized value between 0.0 and 1.0.
        ''' </param>
        ''' 
        ''' <param name="eventContext">
        ''' A user context value that is passed to the notification callback.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns <see cref="HResult.S_OK"/>. 
        ''' If it fails, it returns an <see cref="HResult"/> value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function SetMasterVolume(<[In]> <MarshalAs(UnmanagedType.R4)> ByVal levelNormalization As Single,
                                 <[In]> <MarshalAs(UnmanagedType.LPStruct)> ByVal eventContext As Guid) As HResult

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the client volume level for the audio session.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="refLevelNormalization">
        ''' Receives the volume level expressed as a normalized value between 0.0 and 1.0.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns <see cref="HResult.S_OK"/>. 
        ''' If it fails, it returns an <see cref="HResult"/> value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function GetMasterVolume(<Out()> <MarshalAs(UnmanagedType.R4)> ByRef refLevelNormalization As Single) As HResult

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the muting state for the audio session.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="isMuted">
        ''' The new muting state.
        ''' </param>
        ''' 
        ''' <param name="eventContext">
        ''' A user context value that is passed to the notification callback.
        ''' </param>
        ''' 
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns <see cref="HResult.S_OK"/>. 
        ''' If it fails, it returns an <see cref="HResult"/> value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function SetMute(<[In]> <MarshalAs(UnmanagedType.Bool)> ByVal isMuted As Boolean,
                         <[In]> <MarshalAs(UnmanagedType.LPStruct)> ByVal eventContext As Guid) As HResult

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the current muting state for the audio session.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="refIsMuted">
        ''' Receives the muting state.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns <see cref="HResult.S_OK"/>. 
        ''' If it fails, it returns an <see cref="HResult"/> value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function GetMute(<Out()> <MarshalAs(UnmanagedType.Bool)> ByRef refIsMuted As Boolean) As HResult

    End Interface

End Namespace

#End Region
