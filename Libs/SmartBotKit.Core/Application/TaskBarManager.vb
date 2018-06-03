
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports SmartBotKit.Interop.Win32

#End Region

#Region " Taskbar Manager "

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
        Private Shared lock As New Object()

#End Region

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an instance of the Windows Taskbar
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property Instance As TaskBarManager
            Get
                If (TaskBarManager.instanceB Is Nothing) Then
                    SyncLock lock
                        If (TaskBarManager.instanceB Is Nothing) Then
                            TaskBarManager.instanceB = New TaskBarManager()
                        End If
                    End SyncLock
                End If

                Return TaskBarManager.instanceB
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' A instance of the Windows Taskbar.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared instanceB As TaskBarManager

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the handle of the window whose taskbar button will be used to display progress.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Friend ReadOnly Property OwnerHandle As IntPtr
            Get
                If (Me.ownerHandleB = IntPtr.Zero) Then
                    Dim currentProcess As Process = Process.GetCurrentProcess()

                    If (currentProcess Is Nothing) OrElse (currentProcess.MainWindowHandle = IntPtr.Zero) Then
                        Throw New InvalidOperationException("A valid active Window is needed to update the Taskbar.")
                    End If

                    Me.ownerHandleB = currentProcess.MainWindowHandle
                End If

                Return Me.ownerHandleB
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' The handle of the window whose taskbar button will be used to display progress.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ownerHandleB As IntPtr

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="TaskbarManager"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="Global.System.PlatformNotSupportedException">
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
                Throw New PlatformNotSupportedException("Taskbar features are only supported on Windows 7 or newer.")
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

        '''' ----------------------------------------------------------------------------------------------------
        '''' <summary>
        '''' Displays or updates a progress bar hosted in a taskbar button of the given window  
        '''' to show the specific percentage completed of the full operation.
        '''' </summary>
        '''' ----------------------------------------------------------------------------------------------------
        '''' <param name="currentValue">
        '''' The proportion of the operation that has been completed at the time the method is called.
        '''' </param>
        '''' 
        '''' <param name="maximumValue">
        '''' The value <paramref name="currentValue"/> will have when the operation is complete.
        '''' </param>
        '''' 
        '''' <param name="window">
        '''' The window whose associated taskbar button is being used as a progress indicator. 
        '''' <para></para>
        '''' This window belong to a calling process associated with the button's application and must be already loaded.
        '''' </param>
        '''' ----------------------------------------------------------------------------------------------------
        '<DebuggerStepThrough>
        'Public Sub SetProgressValue(ByVal currentValue As Integer, maximumValue As Integer, ByVal window As System.Windows.Window)

        '    TaskbarList.Instance.SetProgressValue((New System.Windows.Interop.WindowInteropHelper(window)).Handle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue))

        'End Sub

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

        '''' ----------------------------------------------------------------------------------------------------
        '''' <summary>
        '''' Sets the type and state of the progress indicator displayed on a taskbar button of the given window. 
        '''' </summary>
        '''' ----------------------------------------------------------------------------------------------------
        '''' <param name="state">
        '''' Progress state of the progress button
        '''' </param>
        '''' 
        '''' <param name="window">
        '''' The window whose associated taskbar button is being used as a progress indicator. 
        '''' <para></para>
        '''' This window belong to a calling process associated with the button's application and must be already loaded.
        '''' </param>
        '''' ----------------------------------------------------------------------------------------------------
        '<DebuggerStepThrough>
        'Public Sub SetProgressState(ByVal state As TaskbarProgressBarState, ByVal window As System.Windows.Window)
        '    TaskbarList.Instance.SetProgressState((New System.Windows.Interop.WindowInteropHelper(window)).Handle, state)
        'End Sub

#End Region

    End Class

End Namespace

#End Region
