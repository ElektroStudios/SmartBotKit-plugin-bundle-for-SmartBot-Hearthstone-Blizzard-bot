
#Region " Public Members Summary "

#Region " Constructors "

' New(Keys, HotkeyModifier, Integer)

#End Region

#Region " Properties "

' Key As Keys
' Modifier As HotkeyModifier
' Id As Integer

#End Region

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel

#End Region

#Region " HotkeyPress EventArgs "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.IO


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains the event data for the <see cref="Hotkey.Press"/> event.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    Public NotInheritable Class HotkeyPressEventArgs : Inherits EventArgs

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the key assigned to the hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The key assigned to the hotkey.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Key As Keys

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the key-modifiers assigned to the hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The key-modifiers assigned to the hotkey.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Modifiers As HotkeyModifiers

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the unique identifier assigned to the hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The unique identifier assigned to the hotkey.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Id As Integer

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="HotkeyPressEventArgs"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="key">
        ''' The key assigned to the hotkey.
        ''' </param>
        ''' 
        ''' <param name="modifiers">
        ''' The key-modifiers assigned to the hotkey.
        ''' </param>
        ''' 
        ''' <param name="id">
        ''' The unique identifier assigned to the hotkey.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub New(ByVal key As Keys, ByVal modifiers As HotkeyModifiers, ByVal id As Integer)

            Me.Key = key
            Me.Modifiers = modifiers
            Me.Id = id

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="HotkeyPressEventArgs"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub New()
        End Sub

#End Region

    End Class

End Namespace

#End Region
