
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

#Region " Image Extensions "

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

#End Region

    End Module

End Namespace

#End Region
