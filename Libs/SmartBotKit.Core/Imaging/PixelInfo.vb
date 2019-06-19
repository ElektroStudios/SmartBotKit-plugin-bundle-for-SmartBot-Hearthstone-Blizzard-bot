' ***********************************************************************
' Author   : Elektro
' Modified : 15-December-2016
' ***********************************************************************

#Region " Public Members Summary "

#Region " Constructors "

' New(Color, Integer, Point)
' New(Pen, Integer, Point)
' New(SolidBrush, Integer, Point)

#End Region

#Region " Properties "

' Color As Color
' Position As Integer
' Location As Point

#End Region

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.Serialization
Imports System.Security.Permissions
Imports System.Xml.Serialization

#End Region

#Region " Pixel Info "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.[Imaging]


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents pixel information relative to an image.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <XmlRoot(NameOf(PixelInfo))>
    Public NotInheritable Class PixelInfo : Implements ISerializable

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="Color"/> of the pixel.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The <see cref="Color"/> of the pixel.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Color As Color

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the position index of the pixel in the image.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The position of the pixel in the image.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Position As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the location coordinates of the pixel relative to the image.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The pixel coordinates.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Location As Point

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="PixelInfo"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub New()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PixelInfo"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="color">
        ''' The <see cref="Color"/> of the pixel.
        ''' </param>
        ''' 
        ''' <param name="position">
        ''' The index position of the pixel in the image.
        ''' </param>
        ''' 
        ''' <param name="location">
        ''' The location coordinates of the pixel relative to the image.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub New(ByVal color As Color,
                       ByVal position As Integer,
                       ByVal location As Point)

            Me.Color = color
            Me.Position = position
            Me.Location = location

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PixelInfo"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pen">
        ''' The <see cref="Pen"/> that contains the <see cref="Color"/> of the pixel.
        ''' </param>
        ''' 
        ''' <param name="position">
        ''' The index position of the pixel in the image.
        ''' </param>
        ''' 
        ''' <param name="location">
        ''' The location coordinates of the pixel relative to the image.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub New(ByVal pen As Pen,
                       ByVal position As Integer,
                       ByVal location As Point)

            Me.New(pen.Color, position, location)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PixelInfo"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="brush">
        ''' The <see cref="SolidBrush"/> that contains the <see cref="Color"/> of the pixel.
        ''' </param>
        ''' 
        ''' <param name="position">
        ''' The position index of the pixel in the image.
        ''' </param>
        ''' 
        ''' <param name="location">
        ''' The location coordinates of the pixel relative to the image.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub New(ByVal brush As SolidBrush,
                       ByVal position As Integer,
                       ByVal location As Point)

            Me.New(brush.Color, position, location)

        End Sub

#End Region

#Region " ISerializable implementation " ' For Binary serialization.

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="info">
        ''' The <see cref="SerializationInfo"/> to populate with data.
        ''' </param>
        ''' 
        ''' <param name="context">
        ''' The destination (see <see cref="StreamingContext"/>) for this serialization.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentNullException">
        ''' info
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <SecurityPermission(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        <DebuggerStepThrough>
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) _
        Implements ISerializable.GetObjectData

            If (info Is Nothing) Then
                Throw New ArgumentNullException(paramName:=NameOf(info))
            End If

            With info
                .AddValue("COLOR", Me.Color.ToArgb, GetType(Integer))
                .AddValue("POSITION", Me.Position, GetType(Integer))
                .AddValue("LOCATION_X", Me.Location.X, GetType(Integer))
                .AddValue("LOCATION_Y", Me.Location.X, GetType(Integer))
            End With

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PixelInfo"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' This constructor is used to deserialize values.
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="info">
        ''' The <see cref="SerializationInfo"/> to populate with data.
        ''' </param>
        ''' 
        ''' <param name="context">
        ''' The destination (see <see cref="StreamingContext"/>) for this deserialization.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentNullException">
        ''' info
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)

            If (info Is Nothing) Then
                Throw New ArgumentNullException("info")
            End If

            Me.Color = Color.FromArgb(info.GetInt32("COLOR"))
            Me.Position = info.GetInt32("POSITION")
            Me.Location = New Point(info.GetInt32("LOCATION_X"), info.GetInt32("LOCATION_Y"))

        End Sub

#End Region

    End Class

End Namespace

#End Region
