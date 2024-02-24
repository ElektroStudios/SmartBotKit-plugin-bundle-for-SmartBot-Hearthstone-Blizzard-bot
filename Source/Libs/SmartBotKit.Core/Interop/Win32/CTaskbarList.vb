
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " CTaskbarList "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' <c>CLSID_TaskbarList</c> from <c>shobjidl.h</c> headers.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ComImport>
    <ClassInterface(ClassInterfaceType.None)>
    <Guid("56FDF344-FD6D-11d0-958A-006097C9A090")>
    Public Class CTaskbarList
    End Class

End Namespace

#End Region
