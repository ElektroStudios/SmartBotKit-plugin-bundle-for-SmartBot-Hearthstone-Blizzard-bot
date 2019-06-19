
#Region " Public Members Summary "

#Region " Construtors "

' Hotkey.New(HotkeyModifier, Keys)

#End Region

#Region " Events "

' Hotkey.Press As EventHandler(Of HotkeyPressEventArgs)

#End Region

#Region " Properties "

' Hotkey.Key As Keys
' Hotkey.Modifier As HotkeyModifier
' Hotkey.ID As Integer
' Hotkey.Count As Integer
' Hotkey.Tag As Object
' Hotkey.Handle As IntPtr

#End Region

#Region " Functions "

' Hotkey.IsRegistered() As Boolean

#End Region

#Region " Methods "

' Hotkey.Register()
' Hotkey.Unregister(Opt: Boolean)

#End Region

#End Region

#Region " Usage Examples "

'Public Class Form1 : Inherits Form
'
'    Private WithEvents hotkey As Hotkey
'
'    Public Sub Test() Handles MyBase.Shown
'
'        MyClass.InitializeComponent()
'
'        ' Registers a new global hotkey on the system. (Alt + Ctrl + A) 
'        hotkey = New Hotkey(HotkeyModifiers.Alt Or HotkeyModifiers.Control, Keys.A)
'
'        ' Replaces the current registered hotkey with a new one. (Alt + Escape)
'        hotkey = New Hotkey(DirectCast([Enum].Parse(GetType(HotkeyModifiers), "Alt", True), HotkeyModifiers),
'                            DirectCast([Enum].Parse(GetType(Keys), "Escape", True), Keys))
'
'        ' Set the tag property.
'        hotkey.Tag = "I'm a String tag"
'
'    End Sub
'
'    ''' ----------------------------------------------------------------------------------------------------
'    ''' <summary>
'    ''' Handles the <see cref="HotKey.Press"/> event of the HotKey control.
'    ''' </summary>
'    ''' ----------------------------------------------------------------------------------------------------
'    ''' <param name="sender">
'    ''' The source of the event.
'    ''' </param>
'    ''' 
'    ''' <param name="e">
'    ''' The <see cref="HotkeyPressEventArgs"/> instance containing the event data.
'    ''' </param>
'    ''' ----------------------------------------------------------------------------------------------------
'    Private Sub HotKey_Press(ByVal sender As Object, ByVal e As HotkeyPressEventArgs) _
'    Handles hotkey.Press
'
'        Dim sb As New Text.StringBuilder
'        With sb
'            .AppendLine(String.Format("Key.......: {0}", e.Key.ToString()))
'            .AppendLine(String.Format("Modifiers.: {0}", e.Modifiers.ToString()))
'            .AppendLine(String.Format("Identifier: {0}", e.Id))
'            .AppendLine(String.Format("Press-count: {0}", DirectCast(sender, Hotkey).Count))
'            .AppendLine(String.Format("Tag: {0}", DirectCast(sender, Hotkey).Tag.ToString()))
'        End With
'
'        MessageBox.Show(sb.ToString(), String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information)
'
'        ' Unregister the hotkey.
'        Me.hotkey.Unregister()
'
'        ' Is Registered?
'        Debug.WriteLine(Me.hotkey.IsRegistered)
'
'    End Sub
'
'End Class

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports SmartBotKit.Interop.Win32

#End Region

#Region " Hotkey "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.IO


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Creates a system-wide hotkey for the current application.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <example> This is a code example.
    ''' <code>
    ''' Public Class Form1 : Inherits Form
    ''' 
    '''     Private WithEvents hotkey As Hotkey
    ''' 
    '''     Public Sub Test() Handles MyBase.Shown
    ''' 
    '''         MyClass.InitializeComponent()
    ''' 
    '''         ' Registers a new global hotkey on the system. (Alt + Ctrl + A) 
    '''         hotkey = New Hotkey(HotkeyModifiers.Alt Or HotkeyModifiers.Control, Keys.A)
    ''' 
    '''         ' Replaces the current registered hotkey with a new one. (Alt + Escape)
    '''         hotkey = New Hotkey(DirectCast([Enum].Parse(GetType(HotkeyModifiers), "Alt", True), HotkeyModifiers),
    '''                             DirectCast([Enum].Parse(GetType(Keys), "Escape", True), Keys))
    ''' 
    '''         ' Set the tag property.
    '''         hotkey.Tag = "I'm a String tag"
    ''' 
    '''     End Sub
    ''' 
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     ''' &lt;summary&gt;
    '''     ''' Handles the &lt;see cref="HotKey.Press"/&gt; event of the HotKey control.
    '''     ''' &lt;/summary&gt;
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     ''' &lt;param name="sender"&gt;
    '''     ''' The source of the event.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="e"&gt;
    '''     ''' The &lt;see cref="HotkeyPressEventArgs"/&gt; instance containing the event data.
    '''     ''' &lt;/param&gt;
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     Private Sub HotKey_Press(ByVal sender As Object, ByVal e As HotkeyPressEventArgs) _
    '''     Handles hotkey.Press
    ''' 
    '''         Dim sb As New Text.StringBuilder
    '''         With sb
    '''             .AppendLine(String.Format("Key.......: {0}", e.Key.ToString()))
    '''             .AppendLine(String.Format("Modifiers.: {0}", e.Modifiers.ToString()))
    '''             .AppendLine(String.Format("Identifier: {0}", e.Id))
    '''             .AppendLine(String.Format("Press-count: {0}", DirectCast(sender, Hotkey).Count))
    '''             .AppendLine(String.Format("Tag: {0}", DirectCast(sender, Hotkey).Tag.ToString()))
    '''         End With
    ''' 
    '''         MessageBox.Show(sb.ToString(), String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''' 
    '''         ' Unregister the hotkey.
    '''         Me.hotkey.Unregister()
    ''' 
    '''         ' Is Registered?
    '''         Debug.WriteLine(Me.hotkey.IsRegistered)
    ''' 
    '''     End Sub
    ''' 
    ''' End Class
    ''' </code>
    ''' </example>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(False)>
    Public Class Hotkey : Inherits NativeWindow : Implements IDisposable

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the handle for the window that owns this <see cref="Hotkey"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The handle.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overridable Shadows ReadOnly Property Handle As IntPtr
            <DebuggerStepThrough>
            Get
                Return MyBase.Handle
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the key assigned to the hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The key assigned to the hotkey.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overridable ReadOnly Property Key As Keys
            <DebuggerStepThrough>
            Get
                Return Me.key_
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' The key assigned to the hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly key_ As Keys

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the key-modifiers assigned to the hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The key-modifiers assigned to the hotkey.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overridable ReadOnly Property Modifier As HotkeyModifiers
            <DebuggerStepThrough>
            Get
                Return Me.modifier_
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' The key-modifier assigned to the hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly modifier_ As HotkeyModifiers

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the unique identifier assigned to the hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The unique identifier assigned to the hotkey.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overridable ReadOnly Property Id As Integer
            <DebuggerStepThrough>
            Get
                Return Me.id_
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' The unique identifier assigned to the hotkey.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly id_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets an user-defined data associated with this object.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The user-defined data associated with this object.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overridable Property Tag As Object

#End Region

#Region " Events "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A list of event delegates.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Protected ReadOnly Events As EventHandlerList

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Occurs when the hotkey is pressed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Custom Event Press As EventHandler(Of HotkeyPressEventArgs)

            <DebuggerNonUserCode>
            <DebuggerStepThrough>
            AddHandler(ByVal value As EventHandler(Of HotkeyPressEventArgs))
                Me.Events.AddHandler("PressEvent", value)
            End AddHandler

            <DebuggerNonUserCode>
            <DebuggerStepThrough>
            RemoveHandler(ByVal value As EventHandler(Of HotkeyPressEventArgs))
                Me.Events.RemoveHandler("PressEvent", value)
            End RemoveHandler

            <DebuggerNonUserCode>
            <DebuggerStepThrough>
            RaiseEvent(ByVal sender As Object, ByVal e As HotkeyPressEventArgs)
                Dim handler As EventHandler(Of HotkeyPressEventArgs) =
                    DirectCast(Me.Events("PressEvent"), EventHandler(Of HotkeyPressEventArgs))

                If (handler IsNot Nothing) Then
                    handler.Invoke(sender, e)
                End If
            End RaiseEvent

        End Event

#End Region

#Region " Constructors "

        ' ReSharper disable UnusedMember.Local
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="Hotkey"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
            ' ReSharper restore UnusedMember.Local
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="Hotkey"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="modifier">
        ''' One or more key-modifiers to assign to the hotkey.
        ''' </param>
        '''
        ''' <param name="key">
        ''' The key to assign to the hotkey.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub New(ByVal modifier As HotkeyModifiers,
                       ByVal key As Keys)

            MyBase.CreateHandle(New CreateParams())

            Me.Events = New EventHandlerList()
            Me.key_ = key
            Me.modifier_ = modifier
            Me.id_ = MyBase.GetHashCode()

        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether this hotkey is registered on the system.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if this hotkey is registered; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Overridable Function IsRegistered() As Boolean

            ' Try to unregister the hotkey.
            Select Case NativeMethods.UnregisterHotKey(Me.Handle, Me.id_)

                Case False ' Unregistration failed.
                    Return False

                Case Else ' Unregistration succeeds.
                    Me.Register() ' Re-Register the hotkey before return.
                    Return True

            End Select

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Registers this hotkey on the system.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="HotkeyIsRegisteredException">
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Overridable Sub Register()

            If Not NativeMethods.RegisterHotKey(Me.Handle, Me.id_, CUInt(Me.modifier_), CUInt(Me.key_)) Then
                Throw New HotkeyIsRegisteredException()
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Unregisters this hotkey from the system. So after calling this method the hotkey turns unavailable.
        ''' <para></para>
        ''' Note that the hotkey can be re-registered at any time calling the <see cref="Register"/> method.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="HotkeyIsNotRegisteredException">
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Overridable Sub Unregister()

            If Not (NativeMethods.UnregisterHotKey(Me.Handle, Me.id_)) Then
                Throw New HotkeyIsNotRegisteredException()
            End If

        End Sub

#End Region

#Region " Event Invocators "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Raises <see cref="Press"/> event.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="e">
        ''' The <see cref="HotkeyPressEventArgs"/> instance containing the event data.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Protected Overridable Sub OnHotkeyPress(ByVal e As HotkeyPressEventArgs)

            RaiseEvent Press(Me, e)

        End Sub

#End Region

#Region " WndProc "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Invokes the default window procedure associated with this window to process windows messages.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="m">
        ''' A <see cref="Message"/> that is associated with the current Windows message.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Protected Overrides Sub WndProc(ByRef m As Message)

            Select Case m.Msg

                Case WindowsMessages.WM_Hotkey ' A hotkey is pressed.
                    ' Raise the Press event.
                    Me.OnHotkeyPress(New HotkeyPressEventArgs(Me.key_, Me.modifier_, Me.id_))

                Case Else
                    MyBase.WndProc(m)

            End Select

        End Sub

#End Region

#Region " IDisposable Implementation "

        ' ReSharper disable InconsistentNaming
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Flag to detect redundant calls when disposing.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Protected isDisposed As Boolean
        ' ReSharper restore InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub Dispose() Implements IDisposable.Dispose

            Me.Dispose(isDisposing:=True)
            GC.SuppressFinalize(obj:=Me)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged Global.System.Resources.
        ''' Releases unmanaged and, optionally, managed Global.System.Resources.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="isDisposing">
        ''' <see langword="True"/>  to release both managed and unmanaged Global.System.Resources. 
        ''' <see langword="False"/> to release only unmanaged Global.System.Resources.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Protected Overridable Sub Dispose(ByVal isDisposing As Boolean)

            If (Not Me.isDisposed) AndAlso (isDisposing) Then

                Me.Events.Dispose()
                Try
                    ' ReSharper disable once UnusedVariable
                    Dim success As Boolean = NativeMethods.UnregisterHotKey(Me.Handle, Me.id_)
                    ' Dim win32err As Integer = Marshal.GetLastWin32Error()
                Catch ex As Exception
                End Try
                MyBase.ReleaseHandle()
                MyBase.DestroyHandle()

            End If

            Me.isDisposed = True

        End Sub

#End Region

    End Class

End Namespace

#End Region
