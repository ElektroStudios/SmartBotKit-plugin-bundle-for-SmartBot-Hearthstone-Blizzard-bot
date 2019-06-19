
#Region " Option Statements "

Imports System.ComponentModel

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " ITaskbarList4 "

' ReSharper disable once CheckNamespace

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
        Function HrInit() As HResult

        <PreserveSig>
        Function AddTab(ByVal hwnd As IntPtr) As HResult

        <PreserveSig>
        Function DeleteTab(ByVal hwnd As IntPtr) As HResult

        <PreserveSig>
        Function ActivateTab(ByVal hwnd As IntPtr) As HResult

        <PreserveSig>
        Function SetActiveAlt(ByVal hwnd As IntPtr) As HResult

#End Region

#Region " ITaskbarList2 "

        <PreserveSig>
        Function MarkFullscreenWindow(ByVal hwnd As IntPtr, <MarshalAs(UnmanagedType.Bool)> ByVal fFullscreen As Boolean) As HResult

#End Region

#Region " ITaskbarList3 "

        <PreserveSig>
        Function SetProgressValue(ByVal hwnd As IntPtr, ByVal ullCompleted As ULong, ByVal ullTotal As ULong) As HResult

        <PreserveSig>
        Function SetProgressState(ByVal hwnd As IntPtr, ByVal tbpFlags As TaskbarProgressBarState) As HResult

        <PreserveSig>
        Function RegisterTab(ByVal hwndTab As IntPtr, ByVal hwndMdi As IntPtr) As HResult

        <PreserveSig>
        Function UnregisterTab(ByVal hwndTab As IntPtr) As HResult

        <PreserveSig>
        Function SetTabOrder(ByVal hwndTab As IntPtr, ByVal hwndInsertBefore As IntPtr) As HResult

        <PreserveSig>
        Function SetTabActive(ByVal hwndTab As IntPtr, ByVal hwndInsertBefore As IntPtr, ByVal dwReserved As UInteger) As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function Fake1() As HResult
        ' Function ThumbBarAddButtons(byval hwnd As IntPtr, byval cButtons As UInteger, <MarshalAs(UnmanagedType.LPArray)> byval pButtons As ThumbButton()) As HResult

        <EditorBrowsable(EditorBrowsableState.Never)>
        <PreserveSig>
        Function Fake2() As HResult
        ' Function ThumbBarUpdateButtons(byval hwnd As IntPtr, byval cButtons As UInteger, <MarshalAs(UnmanagedType.LPArray)> byval pButtons As ThumbButton()) As HResult

        <PreserveSig>
        Function ThumbBarSetImageList(ByVal hwnd As IntPtr, ByVal himl As IntPtr) As HResult

        <PreserveSig>
        Function SetOverlayIcon(ByVal hwnd As IntPtr, ByVal hIcon As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDescription As String) As HResult

        <PreserveSig>
        Function SetThumbnailTooltip(ByVal hwnd As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pszTip As String) As HResult

        <PreserveSig>
        Function SetThumbnailClip(ByVal hwnd As IntPtr, ByRef refClip As NativeRectangle) As HResult

#End Region

#Region " ITaskbarList4 "

        <EditorBrowsable(EditorBrowsableState.Never)>
        Sub Fake4()
        ' Function SetTabProperties(byval hwndTab As IntPtr, byval stpFlags As SetTabPropertiesOption) As HResult

#End Region

    End Interface

End Namespace

#End Region
