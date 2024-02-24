
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " LuId And Attributes "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents a locally unique identifier (LUID) and its attributes.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa379263%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <StructLayout(LayoutKind.Sequential)>
    Public Structure LuidAndAttributes

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Specifies a <see cref="Luid"/> (locally unique identifier) value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Luid As Luid

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Specifies attributes of the <see cref="Luid"/>.
        ''' <para></para> 
        ''' This value contains up to 32 one-bit flags.
        ''' Its meaning is dependent on the definition and use of the <see cref="Luid"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Attributes As PrivilegeStates

    End Structure

End Namespace

#End Region
