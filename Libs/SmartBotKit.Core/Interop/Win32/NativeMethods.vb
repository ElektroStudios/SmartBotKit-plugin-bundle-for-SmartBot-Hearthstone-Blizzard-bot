
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

' ReSharper disable once CheckNamespace

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
        ''' Performs a bit-block transfer of the color data corresponding to a rectangle of pixels from 
        ''' the specified source device context into a destination device context.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/es-es/library/windows/desktop/dd183370%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hdc">
        ''' A handle to the destination device context.
        ''' </param>
        ''' 
        ''' <param name="nXDest">
        ''' The x-coordinate, in logical units, of the upper-left corner of the destination rectangle.
        ''' </param>
        ''' 
        ''' <param name="nYDest">
        ''' The y-coordinate, in logical units, of the upper-left corner of the destination rectangle.
        ''' </param>
        ''' 
        ''' <param name="nWidth">
        ''' The width, in logical units, of the source and destination rectangles.
        ''' </param>
        ''' 
        ''' <param name="nHeight">
        ''' The height, in logical units, of the source and the destination rectangles.
        ''' </param>
        ''' 
        ''' <param name="hdcSrc">
        ''' A handle to the source device context.
        ''' </param>
        ''' 
        ''' <param name="nXSrc">
        ''' The x-coordinate, in logical units, of the upper-left corner of the source rectangle.
        ''' </param>
        ''' 
        ''' <param name="nYSrc">
        ''' The y-coordinate, in logical units, of the upper-left corner of the source rectangle.
        ''' </param>
        ''' 
        ''' <param name="dwRop">
        ''' A raster-operation code.
        ''' These codes define how the color data for the source rectangle is to be combined with 
        ''' <para></para>
        ''' the color data for the destination rectangle to achieve the final color.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the function fails, the return value is <see langword="False"/>
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("GDI32.dll")>
        Public Shared Function BitBlt(ByVal hdc As IntPtr,
                                      ByVal nXDest As Integer,
                                      ByVal nYDest As Integer,
                                      ByVal nWidth As Integer,
                                      ByVal nHeight As Integer,
                                      ByVal hdcSrc As IntPtr,
                                      ByVal nXSrc As Integer,
                                      ByVal nYSrc As Integer,
                                      ByVal dwRop As TernaryRasterOperations
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Brings the specified window to the top of the Z order. If the window is a top-level window, it is activated.
        ''' <para></para>
        ''' If the window is a child window, the top-level parent window associated with the child window is activated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632673%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window to bring to the top of the Z order.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the the window is zoomed, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the window is not zoomed, the return value is <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function BringWindowToTop(ByVal hwnd As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Creates a bitmap compatible with the device that is associated with the specified device context. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd183488(v=vs.85).aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hdc">
        ''' A handle to the Device Context (DC).
        ''' </param>
        ''' 
        ''' <param name="width">
        ''' The bitmap width, in pixels.
        ''' </param>
        ''' 
        ''' <param name="height">
        ''' The bitmap height, in pixels.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is a handle to the compatible bitmap (DDB).
        ''' <para></para>
        ''' If the function fails, the return value is <see cref="IntPtr.Zero"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("GDI32.dll")>
        Public Shared Function CreateCompatibleBitmap(ByVal hdc As IntPtr,
                                                      ByVal width As Integer,
                                                      ByVal height As Integer
        ) As IntPtr
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Creates a memory device context (DC) compatible with the specified device.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd183489%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hdc">
        ''' A handle to an existing device context (DC).
        ''' <para></para>
        ''' If this handle is <see cref="IntPtr.Zero"/>, 
        ''' the function creates a memory device context (DC) compatible with the application's current screen.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is the handle to a memory device context (DC).
        ''' <para></para>
        ''' If the function fails, the return value is <see cref="IntPtr.Zero"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("GDI32.dll", SetLastError:=True)>
        Public Shared Function CreateCompatibleDC(ByVal hdc As IntPtr
        ) As IntPtr
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Deletes the specified device context (DC).
        ''' <para></para>
        ''' An application must not delete a DC whose handle was obtained by calling the "GetDC" function. 
        ''' instead, it must call the <see cref="ReleaseDC"/> function to free the DC.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd183533%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hdc">
        ''' A handle to the device context.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the function fails, the return value is <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("GDI32.dll")>
        Public Shared Function DeleteDC(ByVal hdc As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Deletes a logical pen, brush, font, bitmap, region, or palette,
        ''' freeing all system Global.System.Resources.associated with the object.
        ''' <para></para>
        ''' After the object is deleted, the specified handle is no longer valid.
        ''' <para></para>
        ''' Do not delete a drawing object (pen or brush) while it is still selected into a DC.
        ''' <para></para>
        ''' When a pattern brush is deleted, the bitmap associated with the brush is not deleted. 
        ''' The bitmap must be deleted independently.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms633540%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hObject">
        ''' A handle to a logical pen, brush, font, bitmap, region, or palette.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the specified handle is not valid or is currently selected into a DC, the return value is <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("GDI32.dll", CharSet:=CharSet.Auto, ExactSpelling:=False, SetLastError:=True)>
        Public Shared Function DeleteObject(ByVal hObject As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the current value of a specified attribute applied to a window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa969515%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' The handle to the window from which the attribute data is retrieved.
        ''' </param>
        ''' 
        ''' <param name="attribute">
        ''' The attribute to retrieve
        ''' </param>
        ''' 
        ''' <param name="refAttribute">
        ''' A pointer to a value that, when this function returns successfully, receives the current value of the attribute.
        ''' <para></para>
        ''' The type of the retrieved value depends on the value of the <paramref name="attribute"/> parameter.
        ''' </param>
        ''' 
        ''' <param name="sizeAttribute">
        ''' The size of the <see cref="DwmWindowAttribute"/> value being retrieved.
        ''' <para></para>
        ''' The size is dependent on the type of the <paramref name="refAttribute"/> parameter.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If this function succeeds, it returns <see cref="HResult.S_OK"/>. 
        ''' Otherwise, it returns an HRESULT error code.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("DwmApi.dll", SetLastError:=False)>
        Public Shared Function DwmGetWindowAttribute(ByVal hwnd As IntPtr,
                                                     ByVal attribute As DwmWindowAttribute,
                                                     ByRef refAttribute As NativeRectangle,
                                                     ByVal sizeAttribute As Integer
        ) As Integer
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the value of non-client rendering attributes for a window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa969524%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' The handle to the window that will receive the attributes.
        ''' </param>
        ''' 
        ''' <param name="attributeToSet">
        ''' A single <see cref="DwmWindowAttribute"/> flag to apply to the window.
        ''' <para></para>
        ''' This parameter specifies the attribute and the <paramref name="attributeValue"/> parameter points to the value of that attribute.
        ''' </param>
        ''' 
        ''' <param name="attributeValue">
        ''' A pointer to the value of the attribute specified in the <paramref name="attributeToSet"/> parameter.
        ''' <para></para>
        ''' Note that different <see cref="DwmWindowAttribute"/> flags require different value types.
        ''' </param>
        ''' 
        ''' <param name="attributeSize">
        ''' The size, in bytes, of the value type pointed To by the <paramref name="attributeValue"/> parameter.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If this function succeeds, it returns <see cref="HResult.S_OK"/>. 
        ''' Otherwise, it returns an <c>HRESULT</c> error code.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("DwmApi.dll", PreserveSig:=True)>
        Public Shared Function DwmSetWindowAttribute(ByVal hwnd As IntPtr,
                                                     ByVal attributeToSet As DwmWindowAttribute,
                                                     ByVal attributeValue As IntPtr,
                                                     ByVal attributeSize As UInteger
        ) As Integer
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves the device context (DC) for the entire window, including title bar, menus, and scroll bars.
        ''' <para></para>
        ''' A window device context permits painting anywhere in a window, 
        ''' because the origin of the device context is the upper-left corner of the window instead of the client area.
        ''' <para></para>
        ''' <see cref="NativeMethods.GetWindowDC"/> assigns default attributes to the window device context 
        ''' each time it retrieves the device context. Previous attributes are lost.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd144947%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window with a device context that is to be retrieved.
        ''' If this value is <see cref="IntPtr.Zero"/>, 
        ''' <see cref="NativeMethods.GetWindowDC"/> retrieves the device context for the entire screen of the primary display monitor.
        ''' <para></para>
        ''' To get the device context for other display monitors, 
        ''' use the "EnumDisplayMonitors" and "CreateDC" functions.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is a handle to a device context for the specified window.
        ''' <para></para>
        ''' If the function fails, the return value is <see cref="IntPtr.Zero"/>, 
        ''' indicating an error or an invalid <paramref name="hwnd"/> parameter.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function GetWindowDC(ByVal hwnd As IntPtr
        ) As IntPtr
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
        ''' Determines whether the specified window is minimized (iconic). 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms633527%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window to be tested.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the window is iconic, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the window is not iconic, the return value is <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function IsIconic(ByVal hwnd As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the specified window handle identifies an existing window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms633528%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window to be tested. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the window handle identifies an existing window, the return value is <see langword="True"/>.
        ''' <para></para>
        ''' If the window handle does not identify an existing window, the return value is <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=True)>
        Public Shared Function IsWindow(ByVal hwnd As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
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
        ''' Releases a device context (DC), freeing it for use by other applications.
        ''' <para></para>
        ''' The effect of the <see cref="ReleaseDC"/> function depends on the type of DC. 
        ''' It frees only common and window DCs. 
        ''' It has no effect on class or private DCs.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd162920%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A <see cref="IntPtr"/> handle to the window whose DC is to be released.
        ''' </param>
        ''' 
        ''' <param name="hdc">
        ''' A <see cref="IntPtr"/> handle to the DC to be released.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the DC was released, <see langword="False"/> if the DC was not released.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", SetLastError:=False)>
        Public Shared Function ReleaseDC(ByVal hwnd As IntPtr,
                                         ByVal hdc As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Selects an object into a specified device context.
        ''' <para></para>
        ''' The new object replaces the previous object of the same type. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd162957%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hdc">
        ''' A handle to the Device Context (DC).
        ''' </param>
        ''' 
        ''' <param name="hObject">
        ''' A handle to the object to be selected.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the selected object is not a region and the function succeeds, 
        ''' the return value is a handle to the object being replaced.
        ''' <para></para>
        ''' If the selected object is a region and the function succeeds, 
        ''' the return value is one of the following values.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("GDI32.dll", CharSet:=CharSet.Auto, ExactSpelling:=False)>
        Public Shared Function SelectObject(ByVal hdc As IntPtr,
                                            ByVal hObject As IntPtr
        ) As IntPtr
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
            <MarshalAs(UnmanagedType.I4)> ByVal windowState As NativeWindowState
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
        ''' Retrieves the name of the class to which the specified window belongs.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms633582(v=vs.85).aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window and, indirectly, the class to which the window belongs.
        ''' </param>
        ''' 
        ''' <param name="className">
        ''' The class name string. 
        ''' </param>
        ''' 
        ''' <param name="maxCount">
        ''' The length of the <paramref name="className"/> buffer, in characters. 
        ''' <para></para>
        ''' The buffer must be large enough to include the terminating null character; 
        ''' otherwise, the class name string is truncated to <paramref name="maxCount"/>-1 characters. 
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is the number of characters copied to the buffer, 
        ''' not including the terminating null character.
        ''' <para></para>
        ''' If the function fails, the return value is <c>0</c>. 
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Public Shared Function GetClassName(ByVal hwnd As IntPtr,
                                            ByVal className As StringBuilder,
                                            ByVal maxCount As Integer
        ) As Integer
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves information about the specified window.
        ''' <para></para>
        ''' The function also retrieves the 32-bit (<c>DWORD</c>) value at the specified offset into the extra window memory.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms633584%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window and, indirectly, the class to which the window belongs.
        ''' </param>
        ''' 
        ''' <param name="flags">
        ''' The zero-based offset to the value to be retrieved.
        ''' <para></para>
        ''' Valid values are in the range zero through the number of bytes of extra window memory, 
        ''' minus the size of an integer.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is the requested value.
        ''' <para></para>
        ''' If the function fails, the return value is zero.
        ''' <para></para>
        ''' If SetWindowLong has not been called previously, 
        ''' <see cref="GetWindowLong"/> returns zero for values in the extra window or class memory.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>. 
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", EntryPoint:="GetWindowLong", SetLastError:=True)>
        Public Shared Function GetWindowLong(ByVal hwnd As IntPtr,
               <MarshalAs(UnmanagedType.I4)> ByVal flags As WindowLongFlags
        ) As UInteger
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves information about the specified window.
        ''' <para></para>
        ''' The function also retrieves the value at a specified offset into the extra window memory.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms633585%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window and, indirectly, the class to which the window belongs.
        ''' </param>
        ''' 
        ''' <param name="flags">
        ''' The zero-based offset to the value to be retrieved.
        ''' <para></para>
        ''' Valid values are in the range zero through the number of bytes of extra window memory, 
        ''' minus the size of an integer
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is the requested value.
        ''' <para></para>
        ''' If the function fails, the return value is zero.
        ''' <para></para>
        ''' If SetWindowLongPtr has not been called previously, 
        ''' <see cref="GetWindowLongPtr"/> returns zero for values in the extra window or class memory.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>. 
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist",
          Justification:="Code-Analysis is 32-Bit so it only checks for the entrypoint in the user32.dll of the Win32 API.")>
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", EntryPoint:="GetWindowLongPtr", SetLastError:=True)>
        Public Shared Function GetWindowLongPtr(ByVal hwnd As IntPtr,
                  <MarshalAs(UnmanagedType.I4)> ByVal flags As WindowLongFlags
        ) As IntPtr
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Changes an attribute of the specified window.
        ''' <para></para>
        ''' The function also sets the 32-bit (<c>LONG</c>) value at the specified offset into the extra window memory.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window and, indirectly, the class to which the window belongs.
        ''' </param>
        ''' 
        ''' <param name="flags">
        ''' The zero-based offset to the value to be set. 
        ''' <para></para>
        ''' Valid values are in the range zero through the number of bytes of extra window memory, 
        ''' minus the size of an integer.
        ''' </param>
        ''' 
        ''' <param name="newLong">
        ''' The replacement value.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is the previous value of the specified 32-bit integer.
        ''' <para></para>
        ''' If the function fails, the return value is zero.
        ''' <para></para>
        ''' If the previous value of the specified 32-bit integer is zero, 
        ''' and the function succeeds, the return value is zero.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>. 
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", EntryPoint:="SetWindowLong", SetLastError:=True)>
        Public Shared Function SetWindowLong(ByVal hwnd As IntPtr,
               <MarshalAs(UnmanagedType.I4)> ByVal flags As WindowLongFlags,
                                             ByVal newLong As UInteger
        ) As UInteger
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Changes an attribute of the specified window.
        ''' <para></para>
        ''' The function also sets a value at the specified offset in the extra window memory.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' <see href="https://msdn.microsoft.com/es-es/library/windows/desktop/ms644898%28v=vs.85%29.aspx"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window and, indirectly, the class to which the window belongs.
        ''' <para></para>
        ''' The SetWindowLongPtr function fails if the process that owns the window specified by the 
        ''' <paramref name="hwnd"/> parameter is at a higher process privilege in the 
        ''' <c>UIPI</c> hierarchy than the process the calling thread resides in.
        ''' </param>
        ''' 
        ''' <param name="flags">
        ''' The zero-based offset to the value to be set. 
        ''' <para></para>
        ''' Valid values are in the range zero through the number of bytes of extra window memory, 
        ''' minus the size of an integer.
        ''' </param>
        ''' 
        ''' <param name="newLong">
        ''' The replacement value.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' If the function succeeds, the return value is the previous value of the specified offset.
        ''' <para></para>
        ''' If the function fails, the return value is zero.
        ''' <para></para>
        ''' If the previous value is zero and the function succeeds, the return value is zero.
        ''' <para></para>
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>. 
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist",
          Justification:="Code-Analysis is 32-Bit so it only checks for the entrypoint in the user32.dll of the Win32 API.")>
        <SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification:="Visible for API users")>
        <SuppressUnmanagedCodeSecurity>
        <DllImport("User32.dll", EntryPoint:="SetWindowLongPtr", SetLastError:=True)>
        Public Shared Function SetWindowLongPtr(ByVal hwnd As IntPtr,
                  <MarshalAs(UnmanagedType.I4)> ByVal flags As WindowLongFlags,
                                                ByVal newLong As IntPtr
        ) As IntPtr
        End Function

#End Region

    End Class

End Namespace

#End Region
