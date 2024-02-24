
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Hotkey Modifiers "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop.Win32


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies a key-modifier to assign for a hotkey.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <see href="http://msdn.microsoft.com/es-es/library/windows/desktop/ms646309%28v=vs.85%29.aspx"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    <Flags>
    Public Enum NativeHotkeyModifiers As UInteger

        ''' <summary>
        ''' Specifies any modifier.
        ''' </summary>
        None = 0

        ''' <summary>
        ''' The <c>ALT</c> keyboard key.
        ''' </summary>
        Alt = 1

        ''' <summary>
        ''' The <c>CTRL</c> keyboard key.
        ''' </summary>
        Control = 2

        ''' <summary>
        ''' The <c>SHIFT</c> keyboard key.
        ''' </summary>
        Shift = 4

        ''' <summary>
        ''' The <c>WIN</c> keyboard key.
        ''' </summary>
        Win = 8

        ''' <summary>
        ''' Changes the hotkey behavior so that the keyboard auto-repeat does not yield multiple hotkey notifications.
        ''' </summary>
        NoRepeat = &H4000

    End Enum

End Namespace

#End Region
