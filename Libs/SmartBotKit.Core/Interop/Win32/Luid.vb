
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " Luid "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' An LUID is a 64-bit value guaranteed to be unique only on the system on which it was generated. 
    ''' The uniqueness of a locally unique identifier (LUID) is guaranteed only until the system is restarted.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa379261%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <StructLayout(LayoutKind.Sequential)>
    Public Structure Luid

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The Low-order bits.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public LowPart As UInteger

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The High-order bits.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public HighPart As Integer

    End Structure

End Namespace

#End Region
