
#Region " Option Statements "

Imports System.ComponentModel

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " ITaskbarList4 "

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Exposes methods that control the taskbar.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd562040(v=vs.85).aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("C43DC798-95D1-4BEA-9030-BB99E2983A1A")>
    Public Interface ITaskbarList4

#Region " ITaskbarList "

        <PreserveSig>
        Sub HrInit()

        <PreserveSig>
        Sub AddTab(ByVal hwnd As IntPtr)

        <PreserveSig>
        Sub DeleteTab(ByVal hwnd As IntPtr)

        <PreserveSig>
        Sub ActivateTab(ByVal hwnd As IntPtr)

        <PreserveSig>
        Sub SetActiveAlt(ByVal hwnd As IntPtr)

#End Region

#Region " ITaskbarList2 "

        <PreserveSig>
        Sub MarkFullscreenWindow(ByVal hwnd As IntPtr, <MarshalAs(UnmanagedType.Bool)> ByVal fFullscreen As Boolean)

#End Region

#Region " ITaskbarList3 "

        <PreserveSig>
        Sub SetProgressValue(ByVal hwnd As IntPtr, ByVal ullCompleted As ULong, ByVal ullTotal As ULong)

        <PreserveSig>
        Sub SetProgressState(ByVal hwnd As IntPtr, ByVal tbpFlags As TaskbarProgressBarState)

        <PreserveSig>
        Sub RegisterTab(ByVal hwndTab As IntPtr, ByVal hwndMdi As IntPtr)

        <PreserveSig>
        Sub UnregisterTab(ByVal hwndTab As IntPtr)

        <PreserveSig>
        Sub SetTabOrder(ByVal hwndTab As IntPtr, ByVal hwndInsertBefore As IntPtr)

        <PreserveSig>
        Sub SetTabActive(ByVal hwndTab As IntPtr, ByVal hwndInsertBefore As IntPtr, ByVal dwReserved As UInteger)

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function Fake1() As Integer ' HResult
        ' Function ThumbBarAddButtons(byval hwnd As IntPtr, byval cButtons As UInteger, <MarshalAs(UnmanagedType.LPArray)> byval pButtons As ThumbButton()) As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function Fake2() As Integer ' HResult
        ' Function ThumbBarUpdateButtons(byval hwnd As IntPtr, byval cButtons As UInteger, <MarshalAs(UnmanagedType.LPArray)> byval pButtons As ThumbButton()) As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Sub Fake3()
        ' Sub ThumbBarSetImageList(byval hwnd As IntPtr, byval himl As IntPtr)

        <PreserveSig>
        Sub SetOverlayIcon(ByVal hwnd As IntPtr, ByVal hIcon As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDescription As String)

        <PreserveSig>
        Sub SetThumbnailTooltip(ByVal hwnd As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pszTip As String)

        <PreserveSig>
        Sub SetThumbnailClip(ByVal hwnd As IntPtr, ByVal prcClip As IntPtr)

#End Region

#Region " ITaskbarList4 "

        <EditorBrowsable(EditorBrowsableState.Never)>
        Sub Fake4()
        ' Sub SetTabProperties(byval hwndTab As IntPtr, byval stpFlags As SetTabPropertiesOption)

#End Region

    End Interface

End Namespace

#End Region
