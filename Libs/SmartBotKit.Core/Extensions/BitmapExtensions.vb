
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

#End Region

#Region " Bitmap Extensions "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Extensions.BitmapExtensions


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with a <see cref="Bitmap"/> type.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    <HideModuleName>
    Public Module BitmapExtensions

#Region " Public Extension Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Converts the piel format of the source <see cref="Bitmap"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Bitmap"/>.
        ''' </param>
        ''' 
        ''' <param name="format">
        ''' The new pixel format.
        ''' </param>
        ''' 
        ''' <param name="disposeSourceImage">
        ''' If <see langword="True"/>, disposes the source <see cref="Bitmap"/> object.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Bitmap"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function ConvertToPixelFormat(ByVal sender As Bitmap, ByVal format As PixelFormat, Optional ByVal disposeSourceImage As Boolean = False) As Bitmap

            Dim copy As New Bitmap(sender.Width, sender.Height, format)

            Using gr As Graphics = Graphics.FromImage(copy)
                gr.DrawImage(sender, New Rectangle(Point.Empty, copy.Size))
            End Using

            If (disposeSourceImage) Then
                sender.Dispose()
            End If

            Return copy

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Crops a <see cref="Bitmap"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Bitmap"/>.
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
        Public Function Crop(ByVal sender As Bitmap,
                             ByVal x As Integer, ByVal y As Integer,
                             ByVal width As Integer, ByVal height As Integer) As Bitmap

            Return DirectCast(Extensions.ImageExtensions.Crop(sender, New Rectangle(x, y, width, height)), Bitmap)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Crops an <see cref="Bitmap"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Bitmap"/>.
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
        ''' The resulting <see cref="Bitmap"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Crop(ByVal sender As Bitmap,
                             ByVal location As Point,
                             ByVal size As Size) As Bitmap

            Return DirectCast(Extensions.ImageExtensions.Crop(sender, New Rectangle(location, size)), Bitmap)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Crops an <see cref="Bitmap"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Bitmap"/>.
        ''' </param>
        ''' 
        ''' <param name="rect">
        ''' A <see cref="Rectangle"/> that specifies the size and position of the area to crop.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Bitmap"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Crop(ByVal sender As Bitmap,
                             ByVal rect As Rectangle) As Image

            Return DirectCast(Extensions.ImageExtensions.Crop(sender, rect), Bitmap)

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
        ''' A <see cref="IEnumerable(Of SmartBotKit.Core.Imaging.PixelInfo)"/> containing the <see cref="Color"/>, pixel position, 
        ''' and coordinates location respectivelly to the image, of each pixel in the image.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function GetPixelInfo(ByVal sender As Bitmap) As IEnumerable(Of Imaging.PixelInfo)

            Return Extensions.ImageExtensions.GetPixelInfo(sender)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Resizes an <see cref="Bitmap"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Bitmap"/>.
        ''' </param>
        ''' 
        ''' <param name="size">
        ''' The new size.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resized <see cref="Bitmap"/>.
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
        Public Function Resize(ByVal sender As Bitmap, ByVal size As Size) As Bitmap

            Return DirectCast(ImageExtensions.Resize(sender, size), Bitmap)

        End Function

#End Region

    End Module

End Namespace

#End Region
