
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

#End Region

#Region " Image Extensions "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Extensions.ImageExtensions


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with a <see cref="Image"/> type.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    <HideModuleName>
    Public Module ImageExtensions

#Region " Public Extension Methods "


        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Converts the piel format of the source <see cref="Image"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Image"/>.
        ''' </param>
        ''' 
        ''' <param name="format">
        ''' The new pixel format.
        ''' </param>
        ''' 
        ''' <param name="disposeSourceImage">
        ''' If <see langword="True"/>, disposes the source <see cref="Image"/> object.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Image"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function ConvertToPixelFormat(ByVal sender As Image, ByVal format As PixelFormat, Optional ByVal disposeSourceImage As Boolean = False) As Image

            Return Extensions.BitmapExtensions.ConvertToPixelFormat(DirectCast(sender, Bitmap), format, disposeSourceImage)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Crops an <see cref="Image"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Image"/>.
        ''' </param>
        ''' 
        ''' <param name="x">
        ''' The x coordinate.
        ''' </param>
        ''' 
        ''' <param name="y">
        ''' The y coordinate.
        ''' </param>
        ''' 
        ''' <param name="width">
        ''' The width.
        ''' </param>
        ''' 
        ''' <param name="height">
        ''' The height.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Image"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Crop(ByVal sender As Image,
                             ByVal x As Integer, ByVal y As Integer,
                             ByVal width As Integer, ByVal height As Integer) As Image

            Return Extensions.ImageExtensions.Crop(sender, New Rectangle(x, y, width, height))

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Crops an <see cref="Image"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Image"/>.
        ''' </param>
        ''' 
        ''' <param name="location">
        ''' The position of the area to crop.
        ''' </param>
        ''' 
        ''' <param name="size">
        ''' The size of the area to crop.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Image"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Crop(ByVal sender As Image,
                             ByVal location As Point,
                             ByVal size As Size) As Image

            Return Extensions.ImageExtensions.Crop(sender, New Rectangle(location, size))

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Crops an <see cref="Image"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Image"/>.
        ''' </param>
        ''' 
        ''' <param name="rect">
        ''' A <see cref="Rectangle"/> that specifies the size and position of the area to crop.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Image"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Crop(ByVal sender As Image,
                             ByVal rect As Rectangle) As Image

            Dim bmp As New Bitmap(rect.Width, rect.Height, sender.PixelFormat)
            Dim dstRectange As New Rectangle(0, 0, rect.Width, rect.Height)

            Using g As Graphics = Graphics.FromImage(bmp)
                g.DrawImage(sender, dstRectange, rect, GraphicsUnit.Pixel)
            End Using

            Return bmp

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' For each pixel in the source image, gets the <see cref="Color"/>, pixel position, 
        ''' and coordinates location respectivelly to the image.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim color As Color = color.FromArgb(117, 228, 26)
        ''' Dim bmp As Bitmap = CreateSolidcolorBitmap(New Size(2, 2), color, PixelFormat.Format32bppArgb)
        ''' Dim pxInfoCol As IEnumerable(Of PixelInfo) = bmp.GetPixelInfo()
        ''' 
        ''' For Each pxInfo As PixelInfo In pxInfoCol
        '''     Console.WriteLine(String.Format("Position: {0}, Location: {1}, Color: {2}",
        '''                                     pxInfo.Position, pxInfo.Location.ToString(), pxInfo.Color.ToString()))
        ''' Next
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A <see cref="IEnumerable(Of SmartBotKit.Imaging.PixelInfo)"/> containing the <see cref="Color"/>, pixel position, 
        ''' and coordinates location respectivelly to the image, of each pixel in the image.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Iterator Function GetPixelInfo(ByVal sender As Image) As IEnumerable(Of Imaging.PixelInfo)

            Dim bmp As Bitmap = DirectCast(sender, Bitmap)

            ' Lock the bitmap bits.
            Dim pixelFormat As PixelFormat = PixelFormat.Format32bppArgb
            Dim bytesPerPixel As Integer = 4 ' PixelFormat.Format32bppArgb
            Dim rect As New Rectangle(Drawing.Point.Empty, bmp.Size)
            Dim bmpData As BitmapData = bmp.LockBits(rect, ImageLockMode.ReadOnly, pixelFormat)

            ' Get the address of the first row.
            Dim address As IntPtr = bmpData.Scan0

            ' Declare an array to hold the bytes of the bitmap. 
            Dim numBytes As Integer = (Math.Abs(bmpData.Stride) * rect.Height)
            Dim rawImageData As Byte() = New Byte(numBytes - 1) {}

            ' Copy the RGB values into the array.
            Marshal.Copy(address, rawImageData, 0, numBytes)

            ' Unlock the bitmap bits and free th cloned bitmap.
            bmp.UnlockBits(bmpData)

            ' Iterate the pixels.
            For i As Integer = 0 To (rawImageData.Length - bytesPerPixel) Step bytesPerPixel

                Dim color As Color =
                            Drawing.Color.FromArgb(alpha:=rawImageData(i + 3),
                                                                 red:=rawImageData(i + 2),
                                                                 green:=rawImageData(i + 1),
                                                                 blue:=rawImageData(i))

                Dim position As Integer = (i \ bytesPerPixel)

                Dim location As New Point(x:=(position Mod rect.Width),
                                                                y:=(position - (position Mod rect.Width)) \ rect.Width)

                Yield New Imaging.PixelInfo(color, position, location)

            Next i

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Resizes an <see cref="Image"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Image"/>.
        ''' </param>
        ''' 
        ''' <param name="size">
        ''' The new size.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resized <see cref="Image"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' Value greater than 0 is required.;width
        ''' or
        ''' Value greater than 0 is required.;height
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Resize(ByVal sender As Image, ByVal size As Size) As Image

            If (size.Width <= 0) Then
                Throw New ArgumentException(message:="Width bigger than 0 is reqired.", paramName:=NameOf(size.Width))

            ElseIf (size.Height <= 0) Then
                Throw New ArgumentException(message:="Height bigger than 0 is reqired.", paramName:=NameOf(size.Height))

            Else
                Dim bmp As New Bitmap(size.Width, size.Height, sender.PixelFormat)

                Using g As Graphics = Graphics.FromImage(bmp)
                    g.DrawImage(sender, 0, 0, bmp.Width, bmp.Height)
                End Using

                Return bmp
            End If

        End Function

#End Region

    End Module

End Namespace

#End Region
