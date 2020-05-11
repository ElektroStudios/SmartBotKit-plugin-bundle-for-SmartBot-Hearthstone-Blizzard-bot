
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Runtime.InteropServices

Imports SmartBotKit.Interop.Win32

Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Card

#End Region

#Region " HearthstoneUtil "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides reusable automation utilities for HearthstoneUtil.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class HearthstoneUtil

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the Hearthstone <see cref="Diagnostics.Process"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The Hearthstone <see cref="Diagnostics.Process"/>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property Process As Process
            <DebuggerStepThrough>
            Get

                If (HearthstoneUtil.process_ Is Nothing) OrElse (HearthstoneUtil.process_.HasExited) Then
                    HearthstoneUtil.process_ = Process.GetProcessesByName("Hearthstone").DefaultIfEmpty(Nothing).SingleOrDefault()
                End If
                ' HearthstoneUtil.process_.Refresh() ' Refresh window title and main window handle.
                Return HearthstoneUtil.process_
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' Gets the Hearthstone <see cref="Diagnostics.Process"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared process_ As Process

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets identifier of the thread that created the Hearthstone main window; the UI thread.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The identifier of the thread that created the Hearthstone main window; the UI thread.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property MainThreadId As Integer
            <DebuggerStepThrough>
            Get
                Return NativeMethods.GetWindowThreadProcessId(HearthstoneUtil.Process.MainWindowHandle, New Integer)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the Hearthstone window placement.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The Hearthstone window placement.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Property WindowPlacement As WindowPlacement
            <DebuggerStepThrough>
            Get
                Return HearthstoneUtil.GetWindowPlacement(HearthstoneUtil.Process.MainWindowHandle)
            End Get
            Set(value As WindowPlacement)
                Dim wpl As WindowPlacement = HearthstoneUtil.GetWindowPlacement(HearthstoneUtil.Process.MainWindowHandle)
                If (wpl.NormalPosition <> CType(value.NormalPosition, Rectangle)) OrElse
                   (wpl.WindowState <> value.WindowState) OrElse
                   (wpl.Flags <> value.Flags) OrElse
                   (wpl.MaxPosition <> CType(value.MaxPosition, Point)) OrElse
                   (wpl.MinPosition <> CType(value.MinPosition, Point)) Then

                    HearthstoneUtil.SetWindowPlacement(HearthstoneUtil.Process.MainWindowHandle, value)
                End If
            End Set
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the Hearthstone window position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The Hearthstone window position.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Property WindowPosition As Point
            <DebuggerStepThrough>
            Get
                Return HearthstoneUtil.GetWindowPosition(HearthstoneUtil.Process.MainWindowHandle)
            End Get
            Set(value As Point)
                If (HearthstoneUtil.GetWindowPosition(HearthstoneUtil.Process.MainWindowHandle) <> value) Then
                    HearthstoneUtil.SetWindowPosition(HearthstoneUtil.Process.MainWindowHandle, value)
                End If
            End Set
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the Hearthstone window size.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The Hearthstone window size.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Property WindowSize As Size
            <DebuggerStepThrough>
            Get
                Return HearthstoneUtil.GetWindowSize(HearthstoneUtil.Process.MainWindowHandle)
            End Get
            Set(value As Size)
                If (HearthstoneUtil.GetWindowSize(HearthstoneUtil.Process.MainWindowHandle) <> value) Then
                    HearthstoneUtil.SetWindowSize(HearthstoneUtil.Process.MainWindowHandle, value)
                End If
            End Set
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the Hearthstone window is fullscreen.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value that determine whether the Hearthstone window is fullscreen.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsFullscreen As Boolean
            <DebuggerStepThrough>
            Get
                Dim hWnd As IntPtr = HearthstoneUtil.Process.MainWindowHandle
                If (hWnd = IntPtr.Zero) Then
                    Return False
                End If
                Dim scr As Screen = Screen.FromHandle(hWnd)
                Dim rc As Rectangle
                NativeMethods.GetWindowRect(hWnd, rc)
                Return (rc.Size = scr.Bounds.Size) AndAlso (rc.Location = scr.Bounds.Location)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determine whether the player has obtained a golden hero portrait for the specified <see cref="CClass"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="heroClass">
        ''' The source <see cref="CClass"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the player has obtained a golden hero portrait for the specified <see cref="CClass"/>;
        ''' otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function IsGoldenHero(heroClass As CClass) As Boolean

            Select Case heroClass

                Case CClass.NONE
                    Return False

                Case CClass.DRUID
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().Druid

                Case CClass.HUNTER
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().Hunter

                Case CClass.MAGE
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().Mage

                Case CClass.PALADIN
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().Paladin

                Case CClass.PRIEST
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().Priest

                Case CClass.ROGUE
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().Rogue

                Case CClass.SHAMAN
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().Shaman

                Case CClass.WARLOCK
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().Warlock

                Case CClass.WARRIOR
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().Warrior

                Case CClass.DEMONHUNTER
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().DemonHunter

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(heroClass), heroClass, GetType(CClass))

            End Select

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve the amount of ranked/arena wins for the specified <see cref="CClass"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="heroClass">
        ''' The source <see cref="CClass"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The amount of ranked/arena wins for the specified <see cref="CClass"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function GetHeroWins(heroClass As CClass) As Integer

            Select Case heroClass

                Case CClass.NONE
                    Return 0

                Case CClass.DRUID
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().DruidWins

                Case CClass.HUNTER
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().HunterWins

                Case CClass.MAGE
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().MageWins

                Case CClass.PALADIN
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().PaladinWins

                Case CClass.PRIEST
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().PriestWins

                Case CClass.ROGUE
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().RogueWins

                Case CClass.SHAMAN
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().ShamanWins

                Case CClass.WARLOCK
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().WarlockWins

                Case CClass.WARRIOR
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().WarriorWins

                Case CClass.DEMONHUNTER
                    Return Bot.GetPlayerDatas().GetGoldenHeroesDatas().DemonHunterWins

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(heroClass), heroClass, GetType(CClass))

            End Select

        End Function

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="HearthstoneUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Private Methods "

        Private Shared Function GetWindowPlacement(ByVal hWnd As IntPtr) As WindowPlacement
            Dim wpl As New WindowPlacement()
            wpl.Length = Marshal.SizeOf(wpl)
            NativeMethods.GetWindowPlacement(hWnd, wpl)
            Return wpl
        End Function

        Private Shared Function SetWindowPlacement(ByVal hWnd As IntPtr, ByVal wpl As WindowPlacement) As Boolean
            Return NativeMethods.SetWindowPlacement(hWnd, wpl)
        End Function

        Private Shared Function GetWindowSize(ByVal hWnd As IntPtr) As Size
            Dim rc As Rectangle
            NativeMethods.GetWindowRect(hWnd, rc)
            Return rc.Size
        End Function

        Private Shared Function SetWindowSize(ByVal hWnd As IntPtr, ByVal sz As Size) As Boolean
            Dim rc As Rectangle
            NativeMethods.GetWindowRect(hWnd, rc)
            Return NativeMethods.SetWindowPos(hWnd, IntPtr.Zero,
                                              rc.Location.X, rc.Location.Y,
                                              sz.Width, sz.Height,
                                              SetWindowPosFlags.IgnoreMove Or
                                              SetWindowPosFlags.DontSendChangingEvent Or
                                              SetWindowPosFlags.DontRedraw)
        End Function

        Private Shared Function GetWindowPosition(ByVal hWnd As IntPtr) As Point
            Dim rc As Rectangle
            NativeMethods.GetWindowRect(hWnd, rc)
            Return rc.Location
        End Function

        Private Shared Function SetWindowPosition(ByVal hWnd As IntPtr, ByVal pt As Point) As Boolean
            Dim rc As Rectangle
            NativeMethods.GetWindowRect(hWnd, rc)
            Return NativeMethods.SetWindowPos(hWnd, IntPtr.Zero,
                                              pt.X, pt.Y,
                                              rc.Size.Width, rc.Size.Height,
                                              SetWindowPosFlags.IgnoreResize Or
                                              SetWindowPosFlags.DontSendChangingEvent Or
                                              SetWindowPosFlags.DontRedraw)
        End Function

#End Region

    End Class

End Namespace

#End Region
