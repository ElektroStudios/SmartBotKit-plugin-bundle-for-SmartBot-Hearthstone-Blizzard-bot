
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.CompilerServices

#End Region

#Region " Bitmap Extensions "

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

            Return DirectCast(Extensions.ImageExtensions.Crop(sender, New Rectangle(location, size)), Drawing.Bitmap)

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

#End Region

    End Module

End Namespace

#End Region
