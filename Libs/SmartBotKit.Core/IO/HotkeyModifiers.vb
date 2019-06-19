
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports SmartBotKit.Interop.Win32

#End Region

#Region " Hotkey Modifiers "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.IO


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
    Public Enum HotkeyModifiers As Short

        ''' <summary>
        ''' Specifies any modifier.
        ''' </summary>
        None = CShort(NativeHotkeyModifiers.None)

        ''' <summary>
        ''' The <c>ALT</c> keyboard key.
        ''' </summary>
        Alt = CShort(NativeHotkeyModifiers.Alt)

        ''' <summary>
        ''' The <c>CTRL</c> keyboard key.
        ''' </summary>
        Control = CShort(NativeHotkeyModifiers.Control)

        ''' <summary>
        ''' The <c>SHIFT</c> keyboard key.
        ''' </summary>
        Shift = CShort(NativeHotkeyModifiers.Shift)

        ''' <summary>
        ''' The <c>WIN</c> keyboard key.
        ''' </summary>
        Win = CShort(NativeHotkeyModifiers.Win)

        ''' <summary>
        ''' Changes the hotkey behavior so that the keyboard auto-repeat does not yield multiple hotkey notifications.
        ''' </summary>
        NoRepeat = CShort(NativeHotkeyModifiers.NoRepeat)

    End Enum

End Namespace

#End Region
