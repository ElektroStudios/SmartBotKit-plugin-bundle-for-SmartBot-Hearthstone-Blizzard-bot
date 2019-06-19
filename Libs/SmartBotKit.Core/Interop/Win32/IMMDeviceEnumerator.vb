#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.InteropServices

#End Region

#Region " IMMDeviceEnumerator "

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
    ''' <see href="https://docs.microsoft.com/en-us/windows/desktop/api/mmdeviceapi/nn-mmdeviceapi-immdeviceenumerator"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")>
    Public Interface IMMDeviceEnumerator

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented1() As HResult

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the default audio endpoint for the specified data-flow direction and role.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="dataFlow">
        ''' The <see cref="EDataFlow"/> direction for the endpoint device.
        ''' </param>
        ''' 
        ''' <param name="role">
        ''' The <see cref="ERole"/> of the endpoint device.
        ''' </param>
        ''' 
        ''' <param name="refDevice">
        ''' The <see cref="IMMDevice"/> interface of the default audio endpoint device.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns HResult.S_OK. 
        ''' If it fails, it returns an HResult value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function GetDefaultAudioEndpoint(<[In]> <MarshalAs(UnmanagedType.I4)> ByVal dataFlow As EDataFlow,
                                         <[In]> <MarshalAs(UnmanagedType.I4)> ByVal role As ERole,
                                         <Out()> <MarshalAs(UnmanagedType.Interface)> ByRef refDevice As IMMDevice) As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        Function NotImplemented2() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        Function NotImplemented3() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        Function NotImplemented4() As HResult

    End Interface

End Namespace

#End Region
