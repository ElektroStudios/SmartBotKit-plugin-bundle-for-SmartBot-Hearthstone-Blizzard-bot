
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Window Animation "

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies a window animation.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms632669%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum WindowAnimation As Integer

        ''' <summary>
        ''' No animation.
        ''' </summary>
        None = 0

        ''' <summary>
        ''' Show and animates the window from top to bottom.
        ''' </summary>
        ShowTopToBottom = &H4

        ''' <summary>
        ''' Show and animates the window from bottom to top.
        ''' </summary>
        ShowBottomToTop = &H8

        ''' <summary>
        ''' Show and animates the window from left to right.
        ''' </summary>
        ShowLeftToRight = &H1

        ''' <summary>
        ''' Show and animates the window from right to left.
        ''' </summary>
        ShowRightToLeft = &H2

        ''' <summary>
        ''' Show and animates the window from upper left corner to bottom right corner.
        ''' </summary>
        ShowCornerLeftUp = &H5

        ''' <summary>
        ''' Show and animates the window from bottom left corner to upper right corner.
        ''' </summary>
        ShowCornerLeftDown = &H9

        ''' <summary>
        ''' Show and animates the window from upper right corner bottom left corner.
        ''' </summary>
        ShowCornerRightUp = &H6

        ''' <summary>
        ''' Show and animates the window from bottom right corner to upper left corner.
        ''' </summary>
        ShowCornerRightDown = &HA

        ''' <summary>
        ''' Makes the window appear to expand outward. 
        ''' </summary>
        ShowCenter = &H10

        ''' <summary>
        ''' Show the window using a fade effect. 
        ''' </summary>
        ShowFade = &H80000

        ''' <summary>
        ''' Hide and animates the window from left to right.
        ''' </summary>
        HideLeftToRight = WindowAnimation.ShowLeftToRight Or &H10000

        ''' <summary>
        ''' Hide and animates the window from right to left.
        ''' </summary>
        HideRightToLeft = WindowAnimation.ShowRightToLeft Or &H10000

        ''' <summary>
        ''' Hide and animates the window from top to bottom.
        ''' </summary>
        HideTopToBottom = WindowAnimation.ShowTopToBottom Or &H10000

        ''' <summary>
        ''' Hide and animates the window from bottom to top.
        ''' </summary>
        HideBottomToTop = WindowAnimation.ShowBottomToTop Or &H10000

        ''' <summary>
        ''' Hide and animates the window from upper left corner to bottom right corner.
        ''' </summary>
        HideCornerLeftUp = WindowAnimation.ShowCornerLeftUp Or &H10000

        ''' <summary>
        ''' Hide and animates the window from bottom left corner to upper right corner.
        ''' </summary>
        HideCornerLeftDown = WindowAnimation.ShowCornerLeftDown Or &H10000

        ''' <summary>
        ''' Hide and animates the window from upper right corner bottom left corner.
        ''' </summary>
        HideCornerRightUp = WindowAnimation.ShowCornerRightUp Or &H10000

        ''' <summary>
        ''' Hide and animates the window from bottom right corner to upper left corner.
        ''' </summary>
        HideCornerRightDown = WindowAnimation.ShowCornerRightDown Or &H10000

        ''' <summary>
        ''' Makes the window disappear collapsing inward.
        ''' </summary>
        HideCenter = WindowAnimation.ShowCenter Or &H10000

        ''' <summary>
        ''' Hides the window using a fade effect. 
        ''' </summary>
        HideFade = WindowAnimation.ShowFade Or &H10000

    End Enum

End Namespace

#End Region
