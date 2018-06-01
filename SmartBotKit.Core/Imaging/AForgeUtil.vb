
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Drawing
Imports System.Globalization
Imports AForge.Imaging

#End Region

#Region " AForgeUtil "

Namespace SmartBotKit.[Imaging]

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains AForge related utilities.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class AForgeUtil

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="AForgeUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Matches a part of an image inside of the specified source image,
        ''' <para></para>
        ''' then returns the relative top-left corner coordinates of any matched image and their similarity percent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' ' A desktop screenshot, in 1920x1080 px. resolution.
        ''' Dim desktopScreenshoot As String = "C:\Desktop.png"
        ''' 
        ''' ' A cutted piece of the screenshot, in 50x50 px. resolution.
        ''' Dim partOfDesktopToFind As String = "C:\PieceOfDesktop.png"
        ''' 
        ''' ' Match part of the image in the desktop, with the specified similarity threeshold.
        ''' Dim matches As TemplateMatch() = MatchImage(srcImagePath:=desktopScreenshoot,
        '''                                             findImagePath:=partOfDesktopToFind,
        '''                                             similarity:=80.5R) ' 80,5% similarity threeshold.
        ''' 
        ''' For Each match As TemplateMatch In matches
        ''' 
        '''     Dim sb As New System.Text.StringBuilder
        '''     sb.AppendFormat("Top-Left Corner Coordinates: {0}", match.Rectangle.Location.ToString())
        '''     sb.AppendLine()
        '''     sb.AppendFormat("Similarity Image Percentage: {0}%", (match.Similarity * 100.0F).ToString("00.00"))
        ''' 
        '''     MessageBox.Show(sb.ToString())
        ''' 
        ''' Next match
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="srcImagePath">
        ''' The filepath of the source image.
        ''' </param>
        ''' 
        ''' <param name="findImagePath">
        ''' The filepath of the image to find inside <paramref name="srcImagePath"/> image.
        ''' </param>
        ''' 
        ''' <param name="similarity">
        ''' The similarity percentage threshold to compare the images.
        ''' <para></para>
        ''' A value of <c>100</c> means find a 100% identical image. 
        ''' <para></para>
        ''' Note: High percentage values with images of big resolutions could take several minutes to accomplish.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' An Array of <see cref="TemplateMatch"/> that contains the 
        ''' relative top-left corner coordinates of any matched image and their similarity percent.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' similarity
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function MatchImage(ByVal srcImagePath As String,
                                          ByVal findImagePath As String,
                                          ByVal similarity As Double) As TemplateMatch()

            Dim srcImageBmp As New Bitmap(srcImagePath)
            Dim findBmp As New Bitmap(findImagePath)

            Dim result As TemplateMatch() = MatchImage(srcImageBmp, findBmp, similarity)

            srcImageBmp.Dispose()
            findBmp.Dispose()

            Return result

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Matches a part of an image inside of the specified source image,
        ''' <para></para>
        ''' then returns the relative top-left corner coordinates of any matched image and their similarity percent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' ' A desktop screenshot, in 1920x1080 px. resolution.
        ''' Dim desktopScreenshoot As New Bitmap("C:\Desktop.png")
        ''' 
        ''' ' A cutted piece of the screenshot, in 50x50 px. resolution.
        ''' Dim partOfDesktopToFind As New Bitmap("C:\PieceOfDesktop.png")
        ''' 
        ''' ' Match part of the image in the desktop, with the specified similarity threeshold.
        ''' Dim matches As TemplateMatch() = MatchImage(srcImage:=desktopScreenshoot,
        '''                                             findImage:=partOfDesktopToFind,
        '''                                             similarity:=80.5R) ' 80,5% similarity threeshold.
        ''' 
        ''' For Each match As TemplateMatch In matches
        ''' 
        '''     Dim sb As New System.Text.StringBuilder
        '''     sb.AppendFormat("Top-Left Corner Coordinates: {0}", match.Rectangle.Location.ToString())
        '''     sb.AppendLine()
        '''     sb.AppendFormat("Similarity Image Percentage: {0}%", (match.Similarity * 100.0F).ToString("00.00"))
        ''' 
        '''     MessageBox.Show(sb.ToString())
        ''' 
        ''' Next match
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="srcImage">
        ''' The source image.
        ''' </param>
        ''' 
        ''' <param name="findImage">
        ''' The image to find inside <paramref name="srcImage"/> image.
        ''' </param>
        ''' 
        ''' <param name="similarity">
        ''' The similarity percentage threshold to compare the images.
        ''' <para></para>
        ''' A value of <c>100</c> means find a 100% identical image. 
        ''' <para></para>
        ''' Note: High percentage values with images of big resolutions could take several minutes to accomplish.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' An Array of <see cref="TemplateMatch"/> that contains the 
        ''' relative top-left corner coordinates of any matched image and their similarity percent.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' similarity
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function MatchImage(ByVal srcImage As Global.System.Drawing.Image,
                                          ByVal findImage As Global.System.Drawing.Image,
                                          ByVal similarity As Double) As TemplateMatch()

            Return MatchImage(DirectCast(srcImage, Bitmap), DirectCast(findImage, Bitmap), similarity)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Matches a part of an image inside of the specified source image,
        ''' <para></para>
        ''' then returns the relative top-left corner coordinates of any matched image and their similarity percent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' ' A desktop screenshot, in 1920x1080 px. resolution.
        ''' Dim desktopScreenshoot As New Bitmap("C:\Desktop.png")
        ''' 
        ''' ' A cutted piece of the screenshot, in 50x50 px. resolution.
        ''' Dim partOfDesktopToFind As New Bitmap("C:\PieceOfDesktop.png")
        ''' 
        ''' ' Match part of the image in the desktop, with the specified similarity threeshold.
        ''' Dim matches As TemplateMatch() = MatchImage(srcImage:=desktopScreenshoot,
        '''                                             findImage:=partOfDesktopToFind,
        '''                                             similarity:=80.5R) ' 80,5% similarity threeshold.
        ''' 
        ''' For Each match As TemplateMatch In matches
        ''' 
        '''     Dim sb As New System.Text.StringBuilder
        '''     sb.AppendFormat("Top-Left Corner Coordinates: {0}", match.Rectangle.Location.ToString())
        '''     sb.AppendLine()
        '''     sb.AppendFormat("Similarity Image Percentage: {0}%", (match.Similarity * 100.0F).ToString("00.00"))
        ''' 
        '''     MessageBox.Show(sb.ToString())
        ''' 
        ''' Next match
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="srcImage">
        ''' The source image.
        ''' </param>
        ''' 
        ''' <param name="findImage">
        ''' The image to find inside <paramref name="srcImage"/> image.
        ''' </param>
        ''' 
        ''' <param name="similarity">
        ''' The similarity percentage threshold to compare the images.
        ''' <para></para>
        ''' A value of <c>100</c> means find a 100% identical image. 
        ''' <para></para>
        ''' Note: High percentage values with images of big resolutions could take several minutes to accomplish.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' An Array of <see cref="TemplateMatch"/> that contains the 
        ''' relative top-left corner coordinates of any matched image and their similarity percent.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' similarity
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function MatchImage(ByVal srcImage As Bitmap,
                                          ByVal findImage As Bitmap,
                                          ByVal similarity As Double) As TemplateMatch()

            Dim currentSimilarity As Single

            ' Translate the friendly Double similarity value to a Single value.
            Select Case similarity

                Case Is < 0.1R, Is > 100.0R ' Value is out of range.
                    Throw New ArgumentOutOfRangeException(paramName:="similarity",
                                                          actualValue:=similarity,
                                                          message:=String.Format(CultureInfo.CurrentCulture,
                                                                   "A value of '{0}' is not valid for '{1}'. '{1}' must be between 0.1 and 100.0.",
                                                                   similarity, "similarity"))


                Case Is = 100.0R ' Identical image comparission.
                    currentSimilarity = 1.0F

                Case Else ' Image comparission with specific similarity.
                    currentSimilarity = (Convert.ToSingle(similarity) / 100.0F)

            End Select

            ' Set the similarity threshold to find all matching images with specified similarity.
            Dim tm As New ExhaustiveTemplateMatching(currentSimilarity)

            ' Return all the found matching images.
            ' Matchings are sortered by it's similarity percent.
            ' It also contains the relative top-left corner coordinates of each matching.
            Return tm.ProcessImage(srcImage, findImage)

        End Function

#End Region

    End Class

End Namespace

#End Region
