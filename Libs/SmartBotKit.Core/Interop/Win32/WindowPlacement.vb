
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Runtime.InteropServices

#End Region

#Region " Window Placement "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains information about the placement of a window on the screen.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms632611%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <DebuggerStepThrough>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure WindowPlacement

#Region " Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The length of the structure, in bytes.
        ''' <para></para>
        ''' Before calling the <see cref="NativeMethods.GetWindowPlacement"/> 
        ''' or NativeMethods.SetWindowPlacement functions, 
        ''' set this member to <c>Marshal.SizeOf(WindowPlacement)</c>.
        ''' <para></para>
        ''' <see cref="NativeMethods.GetWindowPlacement"/> and NativeMethods.SetWindowPlacement 
        ''' fail if this member is not set correctly.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Length As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Specifies flags that control the position of the minimized window 
        ''' and the method by which the window is restored.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Flags As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The current show state of the window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public WindowState As NativeWindowState

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The coordinates of the window's upper-left corner when the window is minimized.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public MinPosition As NativePoint

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The coordinates of the window's upper-left corner when the window is maximized.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public MaxPosition As NativePoint

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The window's coordinates when the window is in the restored position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public NormalPosition As NativeRectangle

#End Region

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the default (empty) value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property [Default]() As WindowPlacement
            Get
                Dim result As New WindowPlacement
                result.Length = Marshal.SizeOf(result)
                Return result
            End Get
        End Property

#End Region

    End Structure

End Namespace

#End Region
