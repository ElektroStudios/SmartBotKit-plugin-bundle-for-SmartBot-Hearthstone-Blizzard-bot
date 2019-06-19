
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Drawing
Imports SmartBotKit.Interop.Win32

#End Region

#Region " Taskbar Manager "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Application


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents an instance of the Windows taskbar
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class TaskBarManager

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Object to lock on instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared ReadOnly Lock As New Object()

#End Region

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an instance of the Windows Taskbar
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property Instance As TaskBarManager
            Get
                If (TaskBarManager.instance_ Is Nothing) Then
                    SyncLock Lock
                        If (TaskBarManager.instance_ Is Nothing) Then
                            TaskBarManager.instance_ = New TaskBarManager()
                        End If
                    End SyncLock
                End If

                Return TaskBarManager.instance_
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' A instance of the Windows Taskbar.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared instance_ As TaskBarManager

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the handle of the window whose taskbar button will be used to display progress.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property OwnerHandle As IntPtr
            Get
                If (Me.ownerHandle_ = IntPtr.Zero) Then
                    Dim currentProcess As Process = Process.GetCurrentProcess()

                    If (currentProcess Is Nothing) OrElse (currentProcess.MainWindowHandle = IntPtr.Zero) Then
                        Throw New InvalidOperationException("A valid active Window is needed to update the Taskbar.")
                    End If

                    Me.ownerHandle_ = currentProcess.MainWindowHandle
                End If

                Return Me.ownerHandle_
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' The handle of the window whose taskbar button will be used to display progress.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ownerHandle_ As IntPtr

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="TaskbarManager"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="PlatformNotSupportedException">
        ''' Taskbar features are only supported on Windows 7 or newer.
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub New()

            ' Verifies that OS version is 6.1 or greater, and the Platform is WinNT.
            Dim isRunningOnWin7 As Boolean =
                (Environment.OSVersion.Platform = PlatformID.Win32NT) AndAlso
                (Environment.OSVersion.Version.CompareTo(New Version(6, 1)) >= 0)

            If Not (isRunningOnWin7) Then
                Throw New PlatformNotSupportedException("Taskbar features are only supported on Windows 7 and newer.")
            End If

        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Displays or updates a progress bar hosted in a taskbar button of the main application window 
        ''' to show the specific percentage completed of the full operation.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="currentValue">
        ''' The proportion of the operation that has been completed at the time the method is called.
        ''' </param>
        ''' 
        ''' <param name="maximumValue">
        ''' The value <paramref name="currentValue"/> will have when the operation is complete.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub SetProgressValue(ByVal currentValue As Integer, ByVal maximumValue As Integer)

            TaskbarList.Instance.SetProgressValue(Me.OwnerHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue))

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Displays or updates a progress bar hosted in a taskbar button of the given window handle 
        ''' to show the specific percentage completed of the full operation.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="currentValue">
        ''' The proportion of the operation that has been completed at the time the method is called.
        ''' </param>
        ''' 
        ''' <param name="maximumValue">
        ''' The value <paramref name="currentValue"/> will have when the operation is complete.
        ''' </param>
        ''' 
        ''' <param name="windowHandle">
        ''' The handle of the window whose associated taskbar button is being used as a progress indicator.
        ''' <para></para>
        ''' This window belong to a calling process associated with the button's application and must be already loaded.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub SetProgressValue(ByVal currentValue As Integer, maximumValue As Integer, ByVal windowHandle As IntPtr)

            TaskbarList.Instance.SetProgressValue(windowHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue))

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the type and state of the progress indicator displayed on a taskbar button of the main application.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="state">
        ''' Progress state of the progress button
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub SetProgressState(ByVal state As TaskbarProgressBarState)

            TaskbarList.Instance.SetProgressState(Me.OwnerHandle, state)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the type and state of the progress indicator displayed on a taskbar button of the given window handle.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="state">
        ''' Progress state of the progress button
        ''' </param>
        ''' 
        ''' <param name="windowHandle">
        ''' The handle of the window whose associated taskbar button is being used as a progress indicator.
        ''' <para></para>
        ''' This window belong to a calling process associated with the button's application and must be already loaded.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub SetProgressState(ByVal state As TaskbarProgressBarState, ByVal windowHandle As IntPtr)

            TaskbarList.Instance.SetProgressState(windowHandle, state)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Selects a portion of a window's client area to display as that window's thumbnail in the taskbar.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="clipRect">
        ''' A <see cref="Rectangle"/> structure that specifies a selection within the window's client area, 
        ''' relative to the upper-left corner of that client area. 
        ''' 
        ''' To clear a clip that is already in place and return to the default display of the thumbnail, 
        ''' set this parameter to <see langword="Nothing"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub SetThumbnailClip(ByVal clipRect As Rectangle)

            ' ReSharper disable once UnusedVariable
            Dim result As HResult = TaskbarList.Instance.SetThumbnailClip(Me.OwnerHandle, clipRect)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Selects a portion of a window's client area to display as that window's thumbnail in the taskbar.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="clipRect">
        ''' A <see cref="Rectangle"/> structure that specifies a selection within the window's client area, 
        ''' relative to the upper-left corner of that client area. 
        ''' 
        ''' To clear a clip that is already in place and return to the default display of the thumbnail, 
        ''' set this parameter to <see langword="Nothing"/>.
        ''' </param>
        ''' 
        ''' <param name="windowHandle">
        ''' The handle to the window represented in the taskbar.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub SetThumbnailClip(ByVal clipRect As Rectangle, ByVal windowHandle As IntPtr)

            ' ReSharper disable once UnusedVariable
            Dim result As HResult = TaskbarList.Instance.SetThumbnailClip(windowHandle, clipRect)

        End Sub

#End Region

    End Class

End Namespace

#End Region
