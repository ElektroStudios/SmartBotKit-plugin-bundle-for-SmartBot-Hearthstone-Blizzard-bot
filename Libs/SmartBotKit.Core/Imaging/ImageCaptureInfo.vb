
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Drawing
Imports System.Text

Imports SmartBotKit.Extensions.RectangleExtensions

#End Region

#Region " ImageCaptureInfo "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.[Imaging]


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents the information of a region to capture in Hearthstone window.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class ImageCaptureInfo

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the aspect ratio of Hearthstone window, represented as X:Y. (for example: 16:9)
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property AspectRatio As Point

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the resolution of Hearthstone window. (for example: 1920x1080)
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Resolution As Size

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the rectangle that contains the location and size to capture the image in Hearthstone window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property CaptureRectangle As Rectangle

#End Region

#Region " Cosntructors "

        ' ReSharper disable UnusedMember.Local
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="ImageCaptureInfo"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub New()
        End Sub
        ' ReSharper restore UnusedMember.Local

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="ImageCaptureInfo"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="aspectRatio">
        ''' The aspect ratio of Hearthstone window, represented as X:Y. (for example: 16:9)
        ''' </param>
        '''
        ''' <param name="resolution">
        ''' The resolution of Hearthstone window. (for example: 1920x1080)
        ''' </param>
        '''
        ''' <param name="captureRectangle">
        ''' The rectangle that contains the location and size to capture the image in Hearthstone window.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New(ByVal aspectRatio As Point, ByVal resolution As Size, ByVal captureRectangle As Rectangle)
            If Not ImageUtil.ResolutionIsOfAspectRatio(resolution, aspectRatio) Then
                Throw New ArgumentException("The specified resolution does not belong to the specified aspect ratio.",
                                                paramName:=NameOf(resolution))
            End If

            Me.AspectRatio = aspectRatio
            Me.Resolution = resolution
            Me.CaptureRectangle = captureRectangle
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Returns a <see cref="ImageCaptureInfo"/> object
        ''' whose <see cref="ImageCaptureInfo.CaptureRectangle"/> member is scaled to the specified size.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="newResolution">
        ''' The target resolution of Hearthstone window. (for example: 1920x1080)
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="ImageCaptureInfo"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Function Scale(ByVal newResolution As Size) As ImageCaptureInfo
            If Not ImageUtil.ResolutionIsOfAspectRatio(newResolution, Me.AspectRatio) Then
                Throw New ArgumentException("The specified resolution does not belong to the current aspect ratio.",
                                                paramName:=NameOf(newResolution))
            End If

            Dim cropRectangle As Rectangle = Me.CaptureRectangle.ScaleBySizeDifference(Me.Resolution, newResolution)
            Return New ImageCaptureInfo(Me.AspectRatio, newResolution, cropRectangle)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Returns a <see cref="String"/> that represents this <see cref="ImageCaptureInfo"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A <see cref="String"/> that represents this <see cref="ImageCaptureInfo"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder()
            sb.AppendLine($"{NameOf(Me.AspectRatio)}={Me.AspectRatio.ToString()}; ")
            sb.AppendLine($"{NameOf(Me.Resolution)}={Me.Resolution.ToString()}; ")
            sb.AppendLine($"{NameOf(Me.CaptureRectangle)}={Me.CaptureRectangle.ToString()}")

            Return $"{{{sb.ToString()}}}"
        End Function

#End Region

    End Class

End Namespace

#End Region
