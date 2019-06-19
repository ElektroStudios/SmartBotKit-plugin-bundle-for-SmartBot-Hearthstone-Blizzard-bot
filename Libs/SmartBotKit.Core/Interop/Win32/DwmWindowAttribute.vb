
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " DwmWindowAttribute "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies the attribute to set when calling the <see cref="NativeMethods.DwmSetWindowAttribute"/> function.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb762108(v=vs.85).aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum DwmWindowAttribute As UInteger

        ''' <summary>
        ''' Is non-client rendering enabled/disabled.
        ''' </summary>
        NcRenderingEnabled = 1

        ''' <summary>
        ''' Non-client rendering policy.
        ''' </summary>
        NcRenderingPolicy

        ''' <summary>
        ''' Potentially enable/forcibly disable transitions.
        ''' </summary>
        TransitionsForceDisabled

        ''' <summary>
        ''' Allow contents rendered In the non-client area To be visible On the DWM-drawn frame.
        ''' </summary>
        AllowNcPaint

        ''' <summary>
        ''' Bounds Of the caption button area In window-relative space.
        ''' </summary>
        CaptionButtonBounds

        ''' <summary>
        ''' Set the non-client content RTL mirrored.
        ''' </summary>
        NonClientRtlLayout

        ''' <summary>
        ''' Force this window To display iconic thumbnails.
        ''' </summary>
        ForceIconicRepresentation

        ''' <summary>
        ''' Designates how Flip3D will treat the window.
        ''' </summary>
        Flip3DPolicy

        ''' <summary>
        ''' Gets the extended frame bounds rectangle In screen space.
        ''' </summary>
        ExtendedFrameBounds

        ''' <summary>
        ''' Indicates an available bitmap When there Is no better thumbnail representation.
        ''' </summary>
        HasIconicBitmap

        ''' <summary>
        ''' Don't invoke Peek on the window.
        ''' </summary>
        DisallowPeek

        ''' <summary>
        ''' Set LivePreview exclusion information.
        ''' </summary>
        ExcludedFromPeek

        ''' <summary>
        ''' Cloaks or uncloaks the window.
        ''' </summary>
        Cloak

        ''' <summary>
        ''' Gets the cloaked state Of the window.
        ''' </summary>
        Cloaked

        ''' <summary>
        ''' Force this window To freeze the thumbnail without live update.
        ''' </summary>
        FreezeRepresentation

        Last

    End Enum

End Namespace

#End Region
