#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.InteropServices

#End Region

#Region " IAudioSessionManager2 "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Enables an application to manage submixes for the audio device. 
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://docs.microsoft.com/en-us/windows/desktop/api/audiopolicy/nn-audiopolicy-iaudiosessionmanager2"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F")>
    Public Interface IAudioSessionManager2

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented1() As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function NotImplemented2() As HResult

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a pointer to the audio session enumerator object.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="refSessionEnum">
        ''' Receives an <see cref="IAudioSessionEnumerator"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns <see cref="HResult.S_OK"/>. 
        ''' If it fails, it returns an <see cref="HResult"/> value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function GetSessionEnumerator(<Out()> <MarshalAs(UnmanagedType.Interface)> ByRef refSessionEnum As IAudioSessionEnumerator) As HResult

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

    End Interface

End Namespace

#End Region
