#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.InteropServices

#End Region

#Region " IAudioSessionControl2 "

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
    <Guid("BFB7FF88-7239-4FC9-8FA2-07C950BE9C6D")>
    Public Interface IAudioSessionControl2

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

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented9() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented10() As HResult

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the process identifier of the audio session.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="refValue">
        ''' Receives the process identifier of the audio session.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns HResult.S_OK. 
        ''' If it fails, it returns an HResult value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function GetProcessId(<Out()> ByRef refValue As UInteger) As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented11() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented12() As HResult

    End Interface

End Namespace

#End Region
