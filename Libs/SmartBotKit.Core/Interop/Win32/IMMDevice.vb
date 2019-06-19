#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.InteropServices

#End Region

#Region " IMMDevice "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides methods for enumerating multimedia device Global.System.Resources. 
    ''' <para></para>
    ''' In the current implementation of the MMDevice API, 
    ''' the only device Global.System.Resources.that this interface can enumerate are audio endpoint devices. 
    ''' <para></para>
    ''' A client obtains a reference to an <see cref="IMMDeviceEnumerator"/> interface by calling the CoCreateInstance.
    ''' <para></para>
    ''' The device Global.System.Resources.enumerated by the methods in the IMMDeviceEnumerator interface are represented as 
    ''' collections of objects with <see cref="IMMDevice"/> interfaces. 
    ''' <para></para>
    ''' A collection has an IMMDeviceCollection interface. 
    ''' The IMMDeviceEnumerator.EnumAudioEndpoints method creates a device collection.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://docs.microsoft.com/en-us/windows/desktop/api/mmdeviceapi/nn-mmdeviceapi-immdevice"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("D666063F-1587-4E43-81F1-B948E807363F")>
    Public Interface IMMDevice

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Creates a COM object with the specified interface
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="refId">
        ''' The interface identifier. 
        ''' <para></para>
        ''' This parameter is a reference to a GUID that identifies the interface that the caller requests be activated. 
        ''' <para></para>
        ''' The caller will use this interface to communicate with the COM object.
        ''' </param>
        ''' 
        ''' <param name="clsCtx">
        ''' The execution context in which the code that manages the newly created object will run. 
        ''' <para></para>
        ''' The caller can restrict the context by setting this parameter to the bitwise OR of one or more CLSCTX enumeration values. 
        ''' <para></para>
        ''' Alternatively, the client can avoid imposing any context restrictions by specifying CLSCTX_ALL. 
        ''' </param>
        ''' 
        ''' <param name="activationParams">
        ''' Set to <see cref="IntPtr.Zero"/> to activate an IAudioClient, IAudioEndpointVolume, 
        ''' IAudioMeterInformation, IAudioSessionManager, or IDeviceTopology interface on an audio endpoint device. 
        ''' <para></para>
        ''' When activating an IBaseFilter, IDirectSound, IDirectSound8, 
        ''' IDirectSoundCapture, or IDirectSoundCapture8 interface on the device, 
        ''' the caller can specify a pointer to a PROPVARIANT structure that contains stream-initialization information.
        ''' </param>
        ''' 
        ''' <param name="refInterface">
        ''' Pointer to a pointer variable into which the method writes the address of the interface specified by 
        ''' parameter <paramref name="refId"/>. 
        ''' <para></para>
        ''' Through this method, the caller obtains a counted reference to the interface. 
        ''' <para></para>
        ''' The caller is responsible for releasing the interface, when it is no longer needed, 
        ''' by calling the interface's Release method. 
        ''' <para></para>
        ''' If the <see cref="IMMDevice.Activate"/> call fails, <paramref name="refInterface"/> is <see langword="Nothing"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns HResult.S_OK. 
        ''' If it fails, it returns an HResult value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function Activate(ByRef refId As Guid, ByVal clsCtx As Integer, ByVal activationParams As IntPtr,
                          <MarshalAs(UnmanagedType.IUnknown)> ByRef refInterface As Object) As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented1() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented2() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented3() As HResult

    End Interface

End Namespace

#End Region
