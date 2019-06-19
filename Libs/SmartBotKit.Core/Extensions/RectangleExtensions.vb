
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

#Region " Rectangle Extensions "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Extensions.RectangleExtensions


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with a <see cref="Rectangle"/> type.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    <HideModuleName>
    Public Module RectangleExtensions

#Region " Public Extension Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Scale the size and position of the source <see cref="Rectangle"/> 
        ''' by the difference of the specified sizes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim oldSize As New Size(640, 480)
        ''' Dim oldRect As New Rectangle(New Point(100, 100), New Size(639, 479))
        ''' 
        ''' Dim newSize As New Size(800, 600)
        ''' Dim newRect As Rectangle = ScaleBySizeDifference(oldRect, oldSize, newSize)
        ''' 
        ''' Console.WriteLine(String.Format("oldRect: {0}", oldRect.ToString())) ' {X=100,Y=100,Width=639,Height=479}
        ''' Console.WriteLine(String.Format("newRect: {0}", newRect.ToString())) ' {X=125,Y=125,Width=798,Height=598}
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Rectangle"/>.
        ''' </param>
        ''' 
        ''' <param name="fromSize">
        ''' The source <see cref="Size"/>.
        ''' </param>
        ''' 
        ''' <param name="toSize">
        ''' The target <see cref="Size"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="Rectangle"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function ScaleBySizeDifference(ByVal sender As Rectangle,
                                              ByVal fromSize As Size,
                                              ByVal toSize As Size) As Rectangle

            Dim percentChangeX As Double = (toSize.Width / fromSize.Width)
            Dim percentChangeY As Double = (toSize.Height / fromSize.Height)

            Return New Rectangle With {
                    .X = CInt(sender.X * percentChangeX),
                    .Y = CInt(sender.Y * percentChangeY),
                    .Width = CInt(sender.Width * percentChangeX),
                    .Height = CInt(sender.Height * percentChangeY)
                }

        End Function

#End Region

    End Module

End Namespace

#End Region
