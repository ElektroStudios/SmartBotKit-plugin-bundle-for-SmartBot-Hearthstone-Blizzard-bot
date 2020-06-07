
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.Tasks
Imports SmartBotKit.Interop.Win32

#End Region

#Region " AudioUtil "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Audio


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains audio related utilities.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class AudioUtil

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A random sequence generator.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared RNG As New Random(Environment.TickCount)

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="AudioUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Mute the audio volume of the specified process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pr">
        ''' The process. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Sub SetMuteApplication(ByVal pr As Process, ByVal mute As Boolean)

            Dim volume As ISimpleAudioVolume = AudioUtil.GetVolumeObject(pr)
            If (volume IsNot Nothing) Then
                Dim guid As Guid = Guid.Empty
                volume.SetMute(mute, guid)
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the audio volume of the specified process is muted.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pr">
        ''' The process. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' Returns <see langword="True"/> if the application is muted, <see langword="False"/> otherwise.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function IsApplicationMuted(ByVal pr As Process) As Boolean
            Dim volume As ISimpleAudioVolume = AudioUtil.GetVolumeObject(pr)
            If (volume IsNot Nothing) Then
                Dim isMuted As Boolean
                volume.GetMute(isMuted)
                Return isMuted
            End If

            Return False
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the audio volume level of the specified process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pr">
        ''' The process. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The audio volume, expressed in the range between 0 and 100.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetPApplicationVolume(ByVal pr As Process) As Integer

            Dim volume As ISimpleAudioVolume = AudioUtil.GetVolumeObject(pr)
            If (volume IsNot Nothing) Then
                Dim levelNormalization As Single = Nothing
                volume.GetMasterVolume(levelNormalization)
                Return CInt(levelNormalization * 100)
            End If

            Return 100

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the audio volume level for the specified process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pr">
        ''' The process. 
        ''' </param>
        ''' 
        ''' <param name="volumeLevel">
        ''' The new volume level, expressed in the range between 0 and 100. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub SetApplicationVolume(ByVal pr As Process, ByVal volumeLevel As Integer)

            If (volumeLevel < 0) OrElse (volumeLevel > 100) Then
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(volumeLevel),
                                                      actualValue:=volumeLevel,
                                                      message:=String.Format(CultureInfo.CurrentCulture,
                                                               "A value of '{0}' is not valid for '{1}'. '{1}' must be between 0 and 100.",
                                                               volumeLevel, NameOf(volumeLevel)))
            End If

            Dim volume As ISimpleAudioVolume = AudioUtil.GetVolumeObject(pr)
            If (volume IsNot Nothing) Then
                Dim guid As Guid = Guid.Empty
                volume.SetMasterVolume((volumeLevel / 100.0F), guid)
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Plays a wav, mp3, mid or wma file.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="file">
        ''' The audio file. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub PlaySoundFile(file As FileInfo)
            AudioUtil.PlaySoundFile(file.FullName)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Plays a wav, mp3, mid or wma file.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="filepath">
        ''' The audio filepath. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub PlaySoundFile(filepath As String)

            Dim number As Integer = AudioUtil.RNG.Next(1000)
            Dim mciAlias As String = $"SmartBotKit_{number}"

            NativeMethods.MciSendString($"open ""{filepath}"" alias {mciAlias}", Nothing, 0, IntPtr.Zero)
            NativeMethods.MciSendString($"play ""{mciAlias}"" wait", Nothing, 0, IntPtr.Zero)
            ' NativeMethods.MciSendString($"stop ""{mciAlias}""", Nothing, 0, IntPtr.Zero)
            NativeMethods.MciSendString($"close ""{mciAlias}""", Nothing, 0, IntPtr.Zero)

        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Enumerate all the <see cref="IAudioSessionControl2"/> of the default (<see cref="IMMDevice"/>) audio device.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' Credits to @Simon Mourier: <see href="https://stackoverflow.com/a/14322736/1248295"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="IEnumerable(Of IAudioSessionControl2)"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepperBoundary>
        Private Shared Iterator Function EnumerateAudioSessionControls() As IEnumerable(Of IAudioSessionControl2)

            ' Get the (1st render + multimedia) aodio device.
            ' ReSharper disable once SuspiciousTypeConversion.Global
            Dim deviceEnumerator As IMMDeviceEnumerator = DirectCast(New MMDeviceEnumerator(), IMMDeviceEnumerator)
            Dim device As IMMDevice = Nothing
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.Render, ERole.Multimedia, device)

            ' Activate the session manager.
            Dim iidIAudioSessionManager2 As Guid = GetType(IAudioSessionManager2).GUID
            Dim obj As Object = Nothing
            device.Activate(iidIAudioSessionManager2, 0, IntPtr.Zero, obj)
            Dim manager As IAudioSessionManager2 = DirectCast(obj, IAudioSessionManager2)

            ' Enumerate sessions for on this device.
            Dim sessionEnumerator As IAudioSessionEnumerator = Nothing
            manager.GetSessionEnumerator(sessionEnumerator)
            Dim sessionCount As Integer
            sessionEnumerator.GetCount(sessionCount)

            For i As Integer = 0 To (sessionCount - 1)
                Dim ctl As IAudioSessionControl = Nothing
                Dim ctl2 As IAudioSessionControl2
                sessionEnumerator.GetSession(i, ctl)
                ' ReSharper disable once SuspiciousTypeConversion.Global
                ctl2 = DirectCast(ctl, IAudioSessionControl2)
                Yield ctl2
                Marshal.ReleaseComObject(ctl2)
                Marshal.ReleaseComObject(ctl)
            Next i

            Marshal.ReleaseComObject(sessionEnumerator)
            Marshal.ReleaseComObject(manager)
            Marshal.ReleaseComObject(device)
            Marshal.ReleaseComObject(deviceEnumerator)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Searchs and returns the corresponding <see cref="ISimpleAudioVolume"/> for the specified <see cref="Process"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' Credits to @Simon Mourier: <see href="https://stackoverflow.com/a/14322736/1248295"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pr">
        ''' The <see cref="Process"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="ISimpleAudioVolume"/>, 
        ''' or <see langword="Nothing"/> if a <see cref="ISimpleAudioVolume"/> is not found for the specified process.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepperBoundary>
        Private Shared Function GetVolumeObject(ByVal pr As Process) As ISimpleAudioVolume

            For Each ctl As IAudioSessionControl2 In AudioUtil.EnumerateAudioSessionControls()
                Dim pId As UInteger
                ctl.GetProcessId(pId)

                If (pId = pr.Id) Then
                    ' ReSharper disable once SuspiciousTypeConversion.Global
                    Return DirectCast(ctl, ISimpleAudioVolume)
                End If
            Next ctl

            Return Nothing

        End Function

#End Region

    End Class

End Namespace

#End Region
