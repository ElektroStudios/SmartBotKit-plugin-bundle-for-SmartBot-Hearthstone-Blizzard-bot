
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Reflection

Imports SmartBot.Plugins
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Interop

#End Region

#Region " HearthstoneResizerPluginData "

Namespace HearthstoneResizer

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="HearthstoneResizerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class HearthstoneResizerPluginData : Inherits PluginDataContainer

#Region " Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="HearthstoneResizerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Resolutions As New Dictionary(Of String, Size) From {
            {"256×192 (4:3)", New Size(256, 192)},
            {"320×240 (4:3)", New Size(320, 240)},
            {"640x400 (16:10)", New Size(640, 400)},
            {"640×480 (4:3)", New Size(640, 480)},
            {"720x480 (3:2)", New Size(720, 480)},
            {"800x480 (5:3)", New Size(800, 480)},
            {"800×600 (4:3)", New Size(800, 600)},
            {"960×720 (4:3)", New Size(960, 720)},
            {"1024×768 (4:3)", New Size(1024, 768)},
            {"1280x768 (5:3)", New Size(1280, 768)},
            {"1280x800 (16:10)", New Size(1280, 800)},
            {"1280×960 (4:3)", New Size(1280, 960)},
            {"1400×1050 (4:3)", New Size(1400, 1050)},
            {"1440x900 (16:10)", New Size(1440, 900)},
            {"1440x960 (3:2)", New Size(1440, 960)},
            {"1440×1080 (4:3)", New Size(1440, 1080)},
            {"1600×1200 (4:3)", New Size(1600, 1200)},
            {"1680x1050 (16:10)", New Size(1680, 1050)},
            {"1920x1200 (16:10)", New Size(1920, 1200)},
            {"1920×1440 (4:3)", New Size(1920, 1440)},
            {"2048×1536 (4:3)", New Size(2048, 1536)},
            {"2560x1600 (16:10)", New Size(2560, 1600)},
            {"2560×1920 (4:3)", New Size(2560, 1920)},
            {"2800×2100 (4:3)", New Size(2800, 2100)},
            {"3200×2400 (4:3)", New Size(3200, 2400)},
            {"3840x2400 (16:10)", New Size(3840, 2400)}
        }

#End Region

#Region " Properties "

#Region " Plugin "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="ApplicationServices.AssemblyInfo"/> for this assembly.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Friend ReadOnly Property AssemblyInfo As AssemblyInfo
            Get
                Return New AssemblyInfo(Assembly.GetExecutingAssembly())
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the assembly name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Assembly Name")>
        Public ReadOnly Property AssemblyName As String
            Get
                Return Path.ChangeExtension(Me.AssemblyInfo.AssemblyName, "dll")
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin description.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Description")>
        Public ReadOnly Property Description As String
            Get
                Return "Maintains a fixed size and location" & ControlChars.NewLine &
                       "for Hearthstone window."
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete log files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Version")>
        Public ReadOnly Property Version As String
            Get
                Return Me.AssemblyInfo.Version.ToString()
            End Get
        End Property

        <Browsable(False)>
        Public Shadows Property Name As String

#End Region

#Region " Info "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current Hearthstone's window position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Info")>
        <DisplayName("Current Hearthstone's window position")>
        Public ReadOnly Property CurrentPosition As Point
            Get
                Return HearthstoneUtil.WindowPosition
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current Hearthstone's window size.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Info")>
        <DisplayName("Current Hearthstone's window size")>
        Public ReadOnly Property CurrentSize As Size
            Get
                Return HearthstoneUtil.WindowSize
            End Get
        End Property

#End Region

#Region " Fixed Size "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin shouldn't resize Hearthstone window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Fixed Size")>
        <DisplayName("Enable Fixed Size")>
        Public Property EnableFixedSize As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the desired Hearthstone's window size.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Fixed Size")>
        <DisplayName("Size:")>
        <ItemsSource(GetType(SizeSource))>
        Public Property Size As String

#End Region

#Region " Fixed Position "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the plugin shouldn't move Hearthstone window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Fixed Position")>
        <DisplayName("Enable Fixed Position")>
        Public Property EnableFixedPos As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the desired Hearthstone's window position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Fixed Position")>
        <DisplayName("Position:")>
        Public Property Position As Point

#End Region

#Region " Behavior "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine at which event to resize Hearthstone's window size.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("SmartBot event at which fix Hearthstone size and position")>
        <ItemsSource(GetType(HearthstoneResizerEventSource))>
        Public Property HearthstoneResizerEvent As SmartBotEvent

#End Region

#Region " Timer Tick Settings "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to ignore ticks when the bot is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Timer Tick Settings" & ControlChars.NewLine &
                  "Note that these settings only has effect if the event you selected is " & NameOf(SmartBotEvent.TimerTick))>
        <DisplayName("Ignore ticks when the bot is stopped")>
        Public Property IgnoreTicksIfBotStopped As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the required tick count to change the Hearthstone's window size.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The required tick count to change the Hearthstone's window size.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Timer Tick Settings" & ControlChars.NewLine &
                  "Note that these settings only has effect if the event you selected is " & NameOf(SmartBotEvent.TimerTick))>
        <DisplayName("The required tick count to fix size and position")>
        Public Property TickCount As Integer
            Get
                Return Me.tickCountB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.tickCountB = 1
                ElseIf (value > 100) Then
                    Me.tickCountB = 100
                Else
                    Me.tickCountB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required tick count to change the Hearthstone's window size.
        ''' </summary>
        Private tickCountB As Integer

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="HearthstoneResizerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.AssemblyInfo.AssemblyName

            Me.EnableFixedPos = True
            Me.EnableFixedSize = True

            Me.Position = Point.Empty
            Me.Size = "800×600 (4:3)"

            Me.IgnoreTicksIfBotStopped = False
            Me.TickCount = 10
            Me.HearthstoneResizerEvent = SmartBotEvent.TimerTick
        End Sub

#End Region

    End Class

End Namespace

#End Region
