
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Text

#End Region

#Region " P/Invoking "

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Platform Invocation methods (P/Invoke), access unmanaged code.
    ''' <para></para>
    ''' This class does not suppress stack walks for unmanaged code permission.
    ''' <see cref="SuppressUnmanagedCodeSecurityAttribute"/> must not be applied to this class.
    ''' <para></para>
    ''' This class is for methods that can be used anywhere because a stack walk will be performed.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="http://msdn.microsoft.com/en-us/library/ms182161.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class NativeMethods

#Region " P/Invoke declarations "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Produces special effects when showing or hiding a window.
        ''' <para></para>
        ''' This doesn't show the window so make sure you call <see cref="Form.Show"/> 
        ''' or set <see cref="Form.Visible"/> property to <see langword="True"/> after calling <see cref="NativeMethods.AnimateWindow"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms632669%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A <see cref="IntPtr"/> handle to the window to animate.
        ''' <para></para>
        ''' The calling thread must own this window.
        ''' </param>
        ''' 
        ''' <param name="time">
        ''' The time it takes to play the animation, in milliseconds.
        ''' <para></para>
        ''' Typically, an animation takes 200 milliseconds to play.
        ''' </param>
        ''' 
        ''' <param name="animation">
        ''' The type of animation.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the function fails, the return value is <see langword="False"/>.
        ''' <para></para>
        ''' The function will fail in the following situations:
        ''' <para></para> If the window is already visible and you are trying to show the window.
        ''' <para></para> If the window is already hidden and you are trying to hide the window.
        ''' <para></para> When trying to animate a child window with <see cref="WindowAnimation.ShowFade"/> or <see cref="WindowAnimation.HideFade"/>.
        ''' <para></para> If the thread does not own the window. 
        ''' Note that, in this case, <see cref="NativeMethods.AnimateWindow"/> fails 
        ''' but <see cref="Marshal.GetLastWin32Error"/> returns Win32ErrorCode.ERROR_SUCCESS.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function AnimateWindow(ByVal hwnd As IntPtr,
                                             ByVal time As Integer,
                                             ByVal animation As WindowAnimation
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms633518%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window.
        ''' </param>
        ''' 
        ''' <param name="refWindowPlacement">
        ''' A pointer to the <see cref="WindowPlacement"/> structure that receives the 
        ''' show state and position information.
        ''' <para></para>
        ''' Before calling <see cref="NativeMethods.GetWindowPlacement"/>, set the length member to <c>Marshal.SizeOf(WindowPlacement)</c>.
        ''' <para></para>
        ''' <see cref="NativeMethods.GetWindowPlacement"/> fails if <paramref name="refWindowPlacement"/> length is not set correctly.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the function fails, the return value is <see langword="False"/>.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function GetWindowPlacement(ByVal hwnd As IntPtr,
                                                  ByRef refWindowPlacement As WindowPlacement
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Copies the text of the specified window's title bar (if it has one) into a buffer.
        ''' <para></para>
        ''' If the specified window is a control, the text of the control is copied.
        ''' <para></para>
        ''' <see cref="GetWindowText"/> cannot retrieve the text of a control in another application than the calling application.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms633520%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window or control containing the text.
        ''' </param>
        ''' 
        ''' <param name="lpString">
        ''' The buffer that will receive the text.
        ''' <para></para>
        ''' If the string is as long or longer than the buffer, the string is truncated and terminated with a null character.
        ''' </param>
        ''' 
        ''' <param name="cch">
        ''' The maximum number of characters to copy to the buffer, including the null character.
        ''' <para></para>
        ''' If the text exceeds this limit, it is truncated.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is the length, in characters, of the copied string, 
        ''' not including the terminating null character.
        ''' <para></para>
        ''' If the window has no title bar or text, if the title bar is empty, or if the window or control handle is invalid, 
        ''' the return value is zero.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>. 
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True, CharSet:=CharSet.Ansi, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Public Shared Function GetWindowText(ByVal hwnd As IntPtr,
                                             ByVal lpString As StringBuilder,
                                             ByVal cch As Integer
        ) As Integer
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the identifier of the thread that created the specified window 
        ''' and, optionally, the identifier of the process that created the window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms633522%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A <see cref="IntPtr"/> handle to the window.
        ''' </param>
        ''' 
        ''' <param name="refPid">
        ''' A pointer to a variable that receives the process identifier (PID). 
        ''' <para></para>
        ''' If this parameter is not <see langword="Nothing"/>, <see cref="NativeMethods.GetWindowThreadProcessId"/> copies the identifier of 
        ''' the process to the variable; otherwise, it does not.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The identifier of the thread that created the window.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function GetWindowThreadProcessId(ByVal hwnd As IntPtr,
                                                        ByRef refPid As Integer
        ) As Integer
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines the visibility state of the specified window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms633530%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window to be tested. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the specified window, its parent window, its parent's parent window, and so forth, 
        ''' have the <c>WS_VISIBLE</c> style, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' Otherwise, the return value is <see langword="False"/>.
        ''' <para></para>
        ''' Because the return value specifies whether the window has the WS_VISIBLE style, 
        ''' it may be nonzero even if the window is totally obscured by other windows.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function IsWindowVisible(ByVal hwnd As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Defines a system-wide hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/es-es/library/windows/desktop/ms646309%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window that will receive <see cref="WindowsMessages.WM_Hotkey"/> messages 
        ''' generated by the hot key.
        ''' <para></para>
        ''' If this parameter is <see cref="IntPtr.Zero"/>, 
        ''' <see cref="WindowsMessages.WM_Hotkey"/> messages are posted to the message queue of the calling thread 
        ''' and must be processed in the message loop.
        ''' </param>
        ''' 
        ''' <param name="id">
        ''' The identifier of the hotkey.
        ''' <para></para>
        ''' If the hwnd parameter is <c>0</c>, then the hotkey is associated with the current thread 
        ''' rather than with a particular window.
        ''' <para></para>
        ''' If a hotkey already exists with the same <paramref name="hwnd"/> and <paramref name="id"/> parameters, 
        ''' see Remarks for the action taken.
        ''' </param>
        ''' 
        ''' <param name="fsModifiers">
        ''' The keys that must be pressed in combination with the key specified by the <paramref name="vk"/> parameter
        ''' in order to generate the <see cref="WindowsMessages.WM_Hotkey"/> message.
        ''' </param>
        ''' 
        ''' <param name="vk">
        ''' The virtual-key code of the hotkey.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the function fails, the return value is <see langword="False"/>.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function RegisterHotKey(ByVal hwnd As IntPtr,
                                              ByVal id As Integer,
                                              ByVal fsModifiers As UInteger,
                                              ByVal vk As UInteger
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Brings the thread that created the specified window into the foreground and activates the window. 
        ''' <para></para>
        ''' Keyboard input is directed to the window, and various visual cues are changed for the user. 
        ''' <para></para>
        ''' The system assigns a slightly higher priority to the thread that created the foreground window than it does to other threads.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms633539%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A <see cref="IntPtr"/> handle to the window that should be activated and brought to the foreground.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the window was brought to the foreground, the return value is <see langword="True"/>.
        ''' If the window was not brought to the foreground, the return value is <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function SetForegroundWindow(ByVal hwnd As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the show state and the restored, minimized, and maximized positions of the specified window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms633544(v=vs.85).aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window.
        ''' </param>
        ''' 
        ''' <param name="refWindowPlacement">
        ''' A pointer to the <see cref="WindowPlacement"/> structure that 
        ''' specifies the new show state and window positions.
        ''' <para></para>
        ''' Before calling <see cref="NativeMethods.GetWindowPlacement"/>, set the length member to <c>Marshal.SizeOf(WindowPlacement)</c>.
        ''' <para></para>
        ''' <see cref="NativeMethods.SetWindowPlacement"/> fails if <paramref name="refWindowPlacement"/> length is not set correctly.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the function fails, the return value is <see langword="False"/>.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function SetWindowPlacement(ByVal hwnd As IntPtr,
                                                  ByRef refWindowPlacement As WindowPlacement
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Changes the text of the specified window's title bar (if it has one). 
        ''' <para></para>
        ''' If the specified window is a control, the text of the control is changed. 
        ''' <para></para>
        ''' However, <see cref="NativeMethods.SetWindowText"/> cannot change the text of a control in another application.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms633546(v=vs.85).aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window or control whose text is to be changed. 
        ''' </param>
        ''' 
        ''' <param name="text">
        ''' The new title or control text. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>
        ''' <para></para>
        ''' If the function fails, the return value is <see langword="False"/>. 
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Public Shared Function SetWindowText(ByVal hwnd As IntPtr,
                                             ByVal text As String
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the specified window's show state.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms633548%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A <see cref="IntPtr"/> handle to the window.
        ''' </param>
        ''' 
        ''' <param name="windowState">
        ''' Controls how the window is to be shown.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the window was previously visible, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the window was previously hidden, the return value is <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function ShowWindow(ByVal hwnd As IntPtr,
            <MarshalAs(UnmanagedType.I4)> ByVal windowState As WindowState
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Unregisters a hotkey previously registered with <see cref="NativeMethods.RegisterHotKey"/> function.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms646327%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window associated with the hot key to be freed.
        ''' <para></para>
        ''' This parameter should be <see cref="IntPtr.Zero"/> if the hot key is not associated with a window. 
        ''' </param>
        ''' 
        ''' <param name="id">
        ''' The identifier of the hotkey to be unregistered.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the function fails, the return value is <see langword="False"/>.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function UnregisterHotKey(ByVal hwnd As IntPtr,
                                                ByVal id As Integer
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the dimensions of the bounding rectangle of the specified window. 
        ''' <para></para>
        ''' The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/es-es/library/windows/desktop/ms633519%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A <see cref="IntPtr"/> handle to the window.
        ''' </param>
        ''' 
        ''' <param name="refRect">
        ''' A pointer to a <see cref="NativeRectangle"/> structure that receives the screen coordinates of the 
        ''' upper-left and lower-right corners of the window.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the function succeeds, <see langword="False"/> otherwise.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function GetWindowRect(ByVal hwnd As IntPtr,
                                       <Out> ByRef refRect As NativeRectangle
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Changes the size, position, and Z order of a child, pop-up, or top-level window.
        ''' <para></para>
        ''' These windows are ordered according to their appearance on the screen.
        ''' <para></para>
        ''' The topmost window receives the highest rank and is the first window in the Z order.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/es-es/library/windows/desktop/ms633545(v=vs.85).aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window.
        ''' </param>
        ''' 
        ''' <param name="hwndInsertAfter">
        ''' A handle to the window to precede the positioned window in the Z order.
        ''' </param>
        ''' 
        ''' <param name="x">
        ''' The new position of the left side of the window, in client coordinates.
        ''' </param>
        ''' 
        ''' <param name="y">
        ''' The new position of the top of the window, in client coordinates.
        ''' </param>
        ''' 
        ''' <param name="cx">
        ''' The new width of the window, in pixels.
        ''' </param>
        ''' 
        ''' <param name="cy">
        ''' The new height of the window, in pixels.
        ''' </param>
        ''' 
        ''' <param name="uFlags">
        ''' The window sizing and positioning flags.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the function fails, the return value is <see langword="False"/>.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>. 
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function SetWindowPos(ByVal hwnd As IntPtr,
                                            ByVal hwndInsertAfter As IntPtr,
                                            ByVal x As Integer,
                                            ByVal y As Integer,
                                            ByVal cx As Integer,
                                            ByVal cy As Integer,
                                            ByVal uFlags As SetWindowPosFlags
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

#End Region

    End Class

End Namespace

#End Region
