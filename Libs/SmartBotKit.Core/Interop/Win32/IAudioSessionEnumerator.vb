#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " IAudioSessionEnumerator "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Enumerates audio sessions on an audio device.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://docs.microsoft.com/en-us/windows/desktop/api/audiopolicy/nn-audiopolicy-iaudiosessionenumerator"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8")>
    Public Interface IAudioSessionEnumerator

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the total number of audio sessions that are open on the audio device.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="refSessionCount">
        ''' Receives the total number of audio sessions.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns HResult.S_OK. 
        ''' If it fails, it returns an HResult value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function GetCount(ByRef refSessionCount As Integer) As HResult

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Notifies the client that the display name for the session has changed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sessionCount">
        ''' The session number. 
        ''' If there are n sessions, the sessions are numbered from 0 to n – 1. 
        ''' <para></para>
        ''' To get the number of sessions, call the <see cref="IAudioSessionEnumerator.GetCount"/> function.
        ''' </param>
        ''' 
        ''' <param name="refSession">
        ''' Receives a pointer to the <see cref="IAudioSessionControl"/> interface of the session object in the 
        ''' collection that is maintained by the session enumerator. 
        ''' <para></para>
        ''' The caller must release the interface pointer.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the method succeeds, it returns HResult.S_OK. 
        ''' If it fails, it returns an HResult value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <PreserveSig>
        Function GetSession(ByVal sessionCount As Integer, ByRef refSession As IAudioSessionControl) As HResult

    End Interface

End Namespace

#End Region
