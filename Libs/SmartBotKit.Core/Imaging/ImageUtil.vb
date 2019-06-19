
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.InteropServices

Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.SystemInfo

#End Region

#Region " Image Util "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.[Imaging]


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains image related utilities.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------   
    Public NotInheritable Class ImageUtil

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="ImageUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Takes a screenshot of an object in the screen.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim jpgCodec As ImageCodecInfo =
        '''     (From codec As ImageCodecInfo In ImageCodecInfo.GetImageDecoders
        '''      Where codec.FormatID = ImageFormat.Jpeg.Guid).SingleOrDefault
        ''' 
        ''' Dim encoderParams As New EncoderParameters(1)
        ''' Dim qualityEncoder As Imaging.Encoder = Imaging.Encoder.Quality
        ''' Dim qualityParameter As New EncoderParameter(qualityEncoder, 80)
        ''' encoderParams.Param(0) = qualityParameter
        ''' 
        ''' Dim hwnd As IntPtr = Process.GetProcessesByName("notepad").FirstOrDefault.MainWindowHandle
        ''' 
        ''' Dim screenshot As Image = TakeScreenshotFromObject(hwnd)
        ''' screenShot.Save("C:\Screenshot.jpg", jpgCodec, encoderParams)
        ''' Process.Start("C:\Screenshot.jpg")
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' The <see cref="IntPtr"/> window handle to the object.
        ''' </param>
        ''' 
        ''' <param name="pixelFormat">
        ''' The image pixel format.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting image.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' The specified handle is not a valid window handle (hWnd).
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' The specified window is hidden.
        ''' </exception>
        ''' 
        ''' <exception cref="Win32Exception">
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function TakeScreenshotFromObject(ByVal hwnd As IntPtr,
                                                        Optional ByVal pixelFormat As Drawing.Imaging.PixelFormat =
                                                                                      Drawing.Imaging.PixelFormat.Format32bppArgb) As Image

            If Not NativeMethods.IsWindow(hwnd) Then
                Throw New ArgumentException(paramName:="hwnd", message:="The specified handle is not a valid window handle (hWnd).")
            End If

            ' Throw an exception instead of restoring a minimized window, because the user didn't demanded it.
            If NativeMethods.IsIconic(hwnd) Then
                Throw New ArgumentException(paramName:="hwnd", message:="The specified window is minimized.")
                ' NativeMethods.OpenIconicWindow(hwnd)
            End If

            ' Throw an exception instead of restoring a hidden window, because the user didn't demanded it.
            If Not NativeMethods.IsWindowVisible(hwnd) Then
                Throw New ArgumentException(paramName:="hwnd", message:="The specified window is hidden.")
                ' NativeMethods.ShowWindow(hwnd, Win32.Enums.WindowState.Restore)
            End If

            ' Get a handle to the device context (DC) of the target window.
            Dim hdcSrc As IntPtr = NativeMethods.GetWindowDC(hwnd)

            ' Get the window size.
            Dim nativeRect As NativeRectangle = ImageUtil.GetRealWindowRect(hwnd)
            Dim rect As Rectangle = nativeRect

            ' Create a device context we can copy to.
            Dim hdcDest As IntPtr = NativeMethods.CreateCompatibleDC(hdcSrc)

            ' Create a bitmap we can copy it to, using 'GetDeviceCaps' to get the width/height.
            Dim hBitmap As IntPtr = NativeMethods.CreateCompatibleBitmap(hdcSrc, rect.Width, rect.Height)

            ' Select the bitmap object
            Dim hOld As IntPtr = NativeMethods.SelectObject(hdcDest, hBitmap)

            ' Bitblt over.
            NativeMethods.BitBlt(hdcDest, 0, 0, rect.Width, rect.Height, hdcSrc, 0, 0, TernaryRasterOperations.SrcCopy)

            ' Restore selection.
            NativeMethods.SelectObject(hdcDest, hOld)

            ' Clean up.
            NativeMethods.DeleteDC(hdcDest)
            NativeMethods.ReleaseDC(hwnd, hdcSrc)

            ' Get a .NET image object for it.
            Dim img As Image = Image.FromHbitmap(hBitmap)

            ' Free up the Bitmap object.
            NativeMethods.DeleteObject(hBitmap)

            ' Convert image to pixel format.
            img = Extensions.ImageExtensions.ConvertToPixelFormat(img, pixelFormat, disposeSourceImage:=True)

            Return img

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the (non-client) <see cref="Rectangle"/> of a window.
        ''' <para></para>
        ''' This method supports a borderless <c>Windows 10</c> window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="hwnd">
        ''' A handle to the window.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting (non-client) <see cref="Rectangle"/> of the window.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetRealWindowRect(ByVal hwnd As IntPtr) As Rectangle

            Dim rc As NativeRectangle = Rectangle.Empty

            If (SystemInfoUtil.IsWin10) Then
                Dim hResult As Integer
                hResult = NativeMethods.DwmGetWindowAttribute(hwnd, DwmWindowAttribute.ExtendedFrameBounds, rc, Marshal.SizeOf(rc))
                If (DirectCast(hResult, HResult) <> Interop.Win32.HResult.S_OK) Then
                    Marshal.ThrowExceptionForHR(hResult)
                End If

            Else
                Dim success As Boolean
                Dim win32Err As Integer
                success = NativeMethods.GetWindowRect(hwnd, rc)
                win32Err = Marshal.GetLastWin32Error()
                If Not (success) Then
                    Throw New Win32Exception(win32Err)
                End If

            End If

            Return rc

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the (non-client) <see cref="Rectangle"/> of a window.
        ''' <para></para>
        ''' This method supports a borderless <c>Windows 10</c> window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="window">
        ''' The window.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting (non-client) <see cref="Rectangle"/> of the window.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetRealWindowRect(ByVal window As IWin32Window) As Rectangle

            Return ImageUtil.GetRealWindowRect(window.Handle)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determine whether the source resolution belongs to the specified aspect ratio.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim resolution As New Size(width:=1920, height:=1080)
        ''' Dim aspectRatio As New Point(x:=16, y:=9)
        ''' 
        ''' Dim result As Boolean = ResolutionIsOfAspectRatio(resolution, aspectRatio)
        ''' 
        ''' Console.WriteLine(result)
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="resolution">
        ''' The source resolution.
        ''' </param>
        ''' 
        ''' <param name="aspectRatio">
        ''' The aspect ratio.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the source resolution belongs to the specified aspect ratio; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function ResolutionIsOfAspectRatio(ByVal resolution As Size, ByVal aspectRatio As Point) As Boolean

            Return ((resolution.Width / aspectRatio.X) * aspectRatio.Y = resolution.Height)

        End Function

#End Region

    End Class

End Namespace

#End Region
