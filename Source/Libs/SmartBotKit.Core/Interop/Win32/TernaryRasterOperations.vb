
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " TernaryRasterOperations "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies a raster-operation code.
    ''' <para></para>
    ''' These codes define how the color data for the source rectangle is to be combined with 
    ''' the color data for the destination rectangle to achieve the final color.
    ''' </summary>
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/es-es/library/windows/desktop/dd183370%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum TernaryRasterOperations As Integer

        ''' <summary>
        ''' [ Destination = Source ]
        ''' <para></para>
        ''' Copies the source rectangle directly to the destination rectangle.
        ''' </summary>
        SrcCopy = &HCC0020

        ''' <summary>
        ''' [ Destination = Source OR Destination ]
        ''' <para></para>
        ''' Combines the colors of the source and destination rectangles by using the Boolean OR operator.
        ''' </summary>
        SrcPaint = &HEE0086

        ''' <summary>
        ''' [ Destination = Source AND Destination ]
        ''' <para></para>
        ''' Combines the colors of the source and destination rectangles by using the Boolean AND operator.
        ''' </summary>
        SrcAnd = &H8800C6

        ''' <summary>
        ''' [ Destination = Source XOR Destination ]
        ''' <para></para>
        ''' Combines the colors of the source and destination rectangles by using the Boolean XOR operator.
        ''' </summary>
        SrcInvert = &H660046

        ''' <summary>
        ''' [ Destination = Source AND NOT Destination ]
        ''' <para></para>
        ''' Combines the colors of the source and destination rectangles by using the Boolean OR operator and 
        ''' then inverts the resultant color.
        ''' </summary>
        SrcErase = &H440328

        ''' <summary>
        ''' [ Destination = NOT Source ]
        ''' <para></para>
        ''' Copies the inverted source rectangle to the destination.
        ''' </summary>
        NotSrcCopy = &H330008

        ''' <summary>
        ''' [ Destination = NOT Source AND NOT Destinatio) ]
        ''' <para></para>
        ''' Combines the colors of the source and destination rectangles 
        ''' by using the Boolean OR operator and then inverts the resultant color.
        ''' </summary>
        NotSrcErase = &H1100A6

        ''' <summary>
        ''' [ Destination = Source AND Pattern) ]
        ''' <para></para>
        ''' Merges the colors of the source rectangle with the brush currently selected in hdcDest, 
        ''' by using the Boolean AND operator.
        ''' </summary>
        MergeCopy = &HC000CA

        ''' <summary>
        ''' [ Destination = NOT Source OR Destination ]
        ''' <para></para>
        ''' Merges the colors of the inverted source rectangle with the colors of the destination rectangle 
        ''' by using the Boolean OR operator.
        ''' </summary>
        MergePaint = &HBB0226

        ''' <summary>
        ''' [ Destination = Pattern ]
        ''' <para></para>
        ''' Copies the brush currently selected in hdcDest, into the destination bitmap.
        ''' </summary>
        PatCopy = &HF00021

        ''' <summary>
        ''' [ Destination = DPSnoo ]
        ''' <para></para>
        ''' Combines the colors of the brush currently selected in hdcDest, 
        ''' with the colors of the inverted source rectangle by using the Boolean OR operator. 
        ''' <para></para>
        ''' The result of this operation is combined with the colors of the destination rectangle 
        ''' by using the Boolean OR operator.
        ''' </summary>
        PatPaint = &HFB0A09

        ''' <summary>
        ''' [ Destination = Pattern XOR Destination ]
        ''' <para></para>
        ''' Combines the colors of the brush currently selected in hdcDest, 
        ''' with the colors of the destination rectangle by using the Boolean XOR operator.
        ''' </summary>
        PatInvert = &H5A0049

        ''' <summary>
        ''' [ Destination = NOT Destination ]
        ''' <para></para>
        ''' Inverts the destination rectangle.
        ''' </summary>
        DstInvert = &H550009

        ''' <summary>
        ''' [ Destination = BLACK ]
        ''' <para></para>
        ''' Fills the destination rectangle using the color associated with index 0 in the physical palette. 
        ''' <para></para>
        ''' (This color is black for the default physical palette).
        ''' </summary>
        Blackness = &H42

        ''' <summary>
        ''' [ Destination = WHITE ]
        ''' <para></para>
        ''' Fills the destination rectangle using the color associated with index 1 in the physical palette.
        ''' <para></para>
        ''' (This color is white for the default physical palette).
        ''' </summary>
        Whiteness = &HFF0062

        ''' <summary>
        ''' Capture window as seen on screen.
        ''' <para></para>
        ''' This includes layered windows such as WPF windows with AllowsTransparency="True"
        ''' </summary>
        CaptureBlt = &H40000000

        ''' <summary>
        ''' Prevents the bitmap from being mirrored.
        ''' </summary>
        NoMirrorBitmap = &H80000000

    End Enum

End Namespace

#End Region
