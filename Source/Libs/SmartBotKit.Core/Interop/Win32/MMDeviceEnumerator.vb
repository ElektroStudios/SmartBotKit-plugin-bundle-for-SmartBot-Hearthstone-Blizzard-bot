#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " MMDeviceEnumerator "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' <c>CLSID_MMDeviceEnumerator</c>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ComImport>
    <Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")>
    Public Class MMDeviceEnumerator
    End Class

End Namespace

#End Region
