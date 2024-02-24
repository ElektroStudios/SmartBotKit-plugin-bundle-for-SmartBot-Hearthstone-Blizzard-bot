#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.InteropServices

#End Region

#Region " IAudioSessionControl "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Enables a client to configure the control parameters for an audio session and to monitor events in the session. 
    ''' <para></para>
    ''' The IAudioClient.Initialize method initializes a stream object and assigns the stream to an audio session.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://docs.microsoft.com/en-us/windows/desktop/api/audiopolicy/nn-audiopolicy-iaudiosessioncontrol"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("F4B1A599-7266-4319-A8CA-E70ACB11E8CD")>
    Public Interface IAudioSessionControl

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented1() As HResult

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the display name for the audio session.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="refDisplayName">
        ''' Receives a string that contains the display name.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns HResult.S_OK. 
        ''' If it fails, it returns an HResult value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function GetDisplayName(<Out()> <MarshalAs(UnmanagedType.LPWStr)> ByRef refDisplayName As String) As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented2() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented3() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented4() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented5() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented6() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented7() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented8() As HResult

    End Interface

End Namespace

#End Region
