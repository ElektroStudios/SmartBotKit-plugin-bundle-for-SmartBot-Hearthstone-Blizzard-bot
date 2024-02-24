#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Window Styles Ex "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Extended window styles.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/es-es/library/windows/desktop/ff700543%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <Flags>
    Public Enum WindowStylesEx As UInteger

        ''' <summary>
        ''' Specifies a window that accepts drag-drop files.
        ''' </summary>
        AcceptFiles = &H10UI

        ''' <summary>
        ''' Forces a top-level window onto the taskbar when the window is visible.
        ''' </summary>
        AppWindow = &H40000UI

        ''' <summary>
        ''' Specifies a window that has a border with a sunken edge.
        ''' </summary>
        ClientEdge = &H200UI

        ''' <summary>
        ''' Specifies a window that paints all descendants in bottom-to-top painting order using double-buffering.
        ''' <para></para>
        ''' This cannot be used if the window has a class style of either <c>CS_OWNDC</c> or <c>CS_CLASSDC</c>.
        ''' <para></para>
        ''' With <see cref="WindowStylesEx.Composited"/> set, 
        ''' all descendants of a window get bottom-to-top painting order using double-buffering.
        ''' <para></para>
        ''' Bottom-to-top painting order allows a descendent window to have translucency (alpha) and transparency (color-key) effects,
        ''' but only if the descendent window also has the<see cref="WindowStylesEx.Transparent"/> bit set.
        ''' <para></para>
        ''' Double-buffering allows the window and its descendents to be painted without flicker.
        ''' </summary>
        Composited = &H2000000UI

        ''' <summary>
        ''' Specifies a window that includes a question mark in the title bar.
        ''' <para></para>
        ''' When the user clicks the question mark, the cursor changes to a question mark with a pointer.
        ''' <para></para>
        ''' If the user then clicks a child window, the child receives a <c>WM_HELP</c> message.
        ''' <para></para>
        ''' The child window should pass the message to the parent window procedure, 
        ''' which should call the <c>WinHelp</c> function using the <c>HELP_WM_HELP</c> command.
        ''' <para></para>
        ''' The Help application displays a pop-up window that typically contains help for the child window.
        ''' <para></para>
        ''' <see cref="WindowStylesEx.ContextHelp"/> cannot be used with the <c>WS_MAXIMIZEBOX</c> or <c>WS_MINIMIZEBOX</c> styles.
        ''' </summary>
        ContextHelp = &H400UI

        ''' <summary>
        ''' Specifies a window which contains child windows that should take part in dialog box navigation.
        ''' <para></para>
        ''' If this style is specified, the dialog manager recurses into children of 
        ''' this window when performing navigation operations
        ''' such as handling the <c>TAB</c> key, an arrow key, or a keyboard mnemonic.
        ''' </summary>
        ControlParent = &H10000UI

        ''' <summary>
        ''' Specifies a window that has a double border.
        ''' </summary>
        DlgModalFrame = &H1UI

        ''' <summary>
        ''' Specifies a window that is a layered window.
        ''' <para></para>
        ''' This cannot be used for child windows or if the window has a class style of either <c>CS_OWNDC</c> or <c>CS_CLASSDC</c>.
        ''' </summary>
        Layered = &H80000UI

        ''' <summary>
        ''' Specifies a window with the horizontal origin on the right edge.
        ''' <para></para>
        ''' Increasing horizontal values advance to the left.
        ''' <para></para>
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' </summary>
        LayoutRtl = &H400000UI

        ''' <summary>
        ''' Specifies a window that has generic left-aligned properties.
        ''' <para></para>
        ''' This is the default.
        ''' </summary>
        Left = &H0UI

        ''' <summary>
        ''' Specifies a window with the vertical scroll bar (if present) to the left of the client area.
        ''' <para></para>
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' </summary>
        LeftScrollbar = &H4000UI

        ''' <summary>
        ''' Specifies a window that displays text using left-to-right reading-order properties.
        ''' <para></para>
        ''' This is the default.
        ''' </summary>
        LtrReading = &H0UI

        ''' <summary>
        ''' Specifies a multiple-document interface (MDI) child window.
        ''' </summary>
        MdiChild = &H40UI

        ''' <summary>
        ''' Specifies a top-level window created with this style does not become the 
        ''' foreground window when the user clicks it.
        ''' <para></para>
        ''' The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
        ''' <para></para>
        ''' The window does not appear on the taskbar by default.
        ''' <para></para>
        ''' To force the window to appear on the taskbar, use the <see cref="WindowStylesEx.AppWindow"/> style.
        ''' <para></para>
        ''' To activate the window, use the NativeMethods.SetActiveWindow or NativeMethods.SetForegroundWindow function.
        ''' </summary>
        NoActivate = &H8000000UI

        ''' <summary>
        ''' Specifies a window which does not pass its window layout to its child windows.
        ''' </summary>
        NoInheritLayout = &H100000UI

        ''' <summary>
        ''' Specifies that a child window created with this style does not send the <c>WM_PARENTNOTIFY</c> message 
        ''' to its parent window when it is created or destroyed.
        ''' </summary>
        NoParentNotify = &H4UI

        ''' <summary>
        ''' Specifies an overlapped window.
        ''' </summary>
        OverlappedWindow = (WindowEdge Or ClientEdge)

        ''' <summary>
        ''' Specifies a palette window, which is a modeless dialog box that presents an array of commands.
        ''' </summary>
        PaletteWindow = (WindowEdge Or ToolWindow Or TopMost)

        ''' <summary>
        ''' Specifies a window that has generic "right-aligned" properties. This depends on the window class.
        ''' <para></para>
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' <para></para>
        ''' Using the <see cref="WindowStylesEx.Right"/> style has the same effect as 
        ''' using the <c>SS_RIGHT</c> (static), <c>ES_RIGHT</c> (edit), 
        ''' and <c>BS_RIGHT</c>/<c>BS_RIGHTBUTTON</c> (button) control styles.
        ''' </summary>
        Right = &H1000UI

        ''' <summary>
        ''' Specifies a window with the vertical scroll bar (if present) to the right of the client area.
        ''' <para></para>
        ''' This is the default.
        ''' </summary>
        RightScrollbar = &H0UI

        ''' <summary>
        ''' Specifies a window that displays text using right-to-left reading-order properties.
        ''' <para></para>
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' </summary>
        RtlReading = &H2000UI

        ''' <summary>
        ''' Specifies a window with a three-dimensional border style intended to be used for 
        ''' items that do not accept user input.
        ''' </summary>
        StaticEdge = &H20000UI

        ''' <summary>
        ''' Specifies a window that is intended to be used as a floating toolbar.
        ''' <para></para>
        ''' A tool window has a title bar that is shorter than a normal title bar, 
        ''' and the window title is drawn using a smaller font.
        ''' <para></para>
        ''' A tool window does not appear in the taskbar or in the dialog that appears when the user presses <c>ALT</c>+<c>TAB</c>.
        ''' <para></para>
        ''' If a tool window has a system menu, its icon is not displayed on the title bar.
        ''' <para></para>
        ''' However, you can display the system menu by right-clicking or by typing <c>ALT</c>+<c>SPACE</c>.
        ''' </summary>
        ToolWindow = &H80UI

        ''' <summary>
        ''' Specifies a window that should be placed above all non-topmost windows and should stay above them, 
        ''' even when the window is deactivated.
        ''' <para></para>
        ''' To add or remove this style, use the <c>SetWindowPos</c> function.
        ''' </summary>
        TopMost = &H8UI

        ''' <summary>
        ''' Specifies a window that should not be painted until siblings beneath the window 
        ''' (that were created by the same thread) have been painted.
        ''' <para></para>
        ''' The window appears transparent because the bits of underlying sibling windows have already been painted.
        ''' <para></para>
        ''' To achieve transparency without these restrictions, use the <c>SetWindowRgn</c> function.
        ''' </summary>
        Transparent = &H20UI

        ''' <summary>
        ''' Specifies a window that has a border with a raised edge.
        ''' </summary>
        WindowEdge = &H100UI

    End Enum

End Namespace

#End Region
