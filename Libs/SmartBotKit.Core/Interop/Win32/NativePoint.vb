
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Drawing
Imports System.Runtime.InteropServices

#End Region

#Region " NativePoint (Point) "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Defines the x- and y- coordinates of a point.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd162805%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <DebuggerStepThrough>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure NativePoint

#Region " Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The X-coordinate of the point.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public X As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The Y-coordinate of the point.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Y As Integer

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="NativePoint"/> struct.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="x">
        ''' The X-coordinate of the point.
        ''' </param>
        ''' 
        ''' <param name="y">
        ''' The Y-coordinate of the point.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New(ByVal x As Integer, ByVal y As Integer)

            Me.X = x
            Me.Y = y

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="NativePoint"/> struct.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pt">
        ''' A <see cref="Point"/> that contains the X-coordinate and the Y-coordinate.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New(ByVal pt As Point)

            Me.New(pt.X, pt.Y)

        End Sub

#End Region

#Region " Operator Conversions "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Performs an implicit conversion from <see cref="NativePoint"/> to <see cref="Point"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pt">
        ''' The <see cref="NativePoint"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Point"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Widening Operator CType(ByVal pt As NativePoint) As Point

            Return New Point(pt.X, pt.Y)

        End Operator

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Performs an implicit conversion from <see cref="Point"/> to <see cref="NativePoint"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pt">
        ''' The <see cref="Point"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="NativePoint"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Widening Operator CType(ByVal pt As Point) As NativePoint

            Return New NativePoint(pt.X, pt.Y)

        End Operator

#End Region

    End Structure

End Namespace

#End Region
