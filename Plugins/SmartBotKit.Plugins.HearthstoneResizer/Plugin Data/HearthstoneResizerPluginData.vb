
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
Imports System.Linq

#End Region

#Region " HearthstoneResizerPluginData "

' ReSharper disable once CheckNamespace

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
        ''' 3:2 resolutions.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared ReadOnly Resolutions_3_2 As New Dictionary(Of String, Size) From {
            {"720×480 (3:2)", New Size(720, 480)},
            {"960×640 (3:2)", New Size(960, 640)},
            {"1152×768 (3:2)", New Size(1152, 768)},
            {"1440×960 (3:2)", New Size(1440, 960)},
            {"1920×1280 (3:2)", New Size(1920, 1280)}
        }

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' 4:3 resolutions.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared ReadOnly Resolutions_4_3 As New Dictionary(Of String, Size) From {
            {"256×192 (4:3)", New Size(256, 192)},
            {"320×240 (4:3)", New Size(320, 240)},
            {"640×480 (4:3)", New Size(640, 480)},
            {"800×600 (4:3)", New Size(800, 600)},
            {"960×720 (4:3)", New Size(960, 720)},
            {"1024×768 (4:3)", New Size(1024, 768)},
            {"1280×960 (4:3)", New Size(1280, 960)},
            {"1400×1050 (4:3)", New Size(1400, 1050)},
            {"1440×1080 (4:3)", New Size(1440, 1080)},
            {"1600×1200 (4:3)", New Size(1600, 1200)},
            {"1920×1440 (4:3)", New Size(1920, 1440)},
            {"2048×1536 (4:3)", New Size(2048, 1536)},
            {"2560×1920 (4:3)", New Size(2560, 1920)},
            {"2800×2100 (4:3)", New Size(2800, 2100)},
            {"3200×2400 (4:3)", New Size(3200, 2400)}
        }

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' 5:3 resolutions.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared ReadOnly Resolutions_5_3 As New Dictionary(Of String, Size) From {
            {"800×480 (5:3)", New Size(800, 480)},
            {"1280×768 (5:3)", New Size(1280, 768)}
        }

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' 5:4 resolutions.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared ReadOnly Resolutions_5_4 As New Dictionary(Of String, Size) From {
            {"550×440 (5:4)", New Size(550, 440)},
            {"320×256 (5:4)", New Size(320, 256)},
            {"600×480 (5:4)", New Size(600, 480)}
        }

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' 8:5 resolutions.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared ReadOnly Resolutions_8_5 As New Dictionary(Of String, Size) From {
            {"768×480 (8:5)", New Size(768, 480)},
            {"1024×640 (8:5)", New Size(1024, 640)},
            {"1152×720 (8:5)", New Size(1152, 720)},
            {"1280×800 (8:5)", New Size(1280, 800)},
            {"1440×900 (8:5)", New Size(1440, 900)}
        }


        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' 16:9 resolutions.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared ReadOnly Resolutions_16_9 As New Dictionary(Of String, Size) From {
            {"640×360 (16:9)", New Size(640, 360)},
            {"960×540 (16:9)", New Size(960, 540)},
            {"1024×576 (16:9)", New Size(1024, 576)},
            {"1280×720 (16:9)", New Size(1280, 720)}
        }

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' 16:10 resolutions.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared ReadOnly Resolutions_16_10 As New Dictionary(Of String, Size) From {
            {"640×400 (16:10)", New Size(640, 400)},
            {"1280×800 (16:10)", New Size(1280, 800)},
            {"1440×900 (16:10)", New Size(1440, 900)},
            {"1680×1050 (16:10)", New Size(1680, 1050)},
            {"1920×1200 (16:10)", New Size(1920, 1200)},
            {"2560×1600 (16:10)", New Size(2560, 1600)},
            {"3840×2400 (16:10)", New Size(3840, 2400)}
        }

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Available window resolutions.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Resolutions As Dictionary(Of String, Size) =
            (Resolutions_3_2.
             Union(Resolutions_4_3).
             Union(Resolutions_5_3).
             Union(Resolutions_5_4).
             Union(Resolutions_8_5).
             Union(Resolutions_16_9).
             Union(Resolutions_16_10)
            ).ToDictionary(Function(item) item.Key, Function(item) item.Value)

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
        ''' Gets the assembly path.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <Browsable(False)>
        <DisplayName("Assembly Name")>
        Public ReadOnly Property AssemblyName As String
            Get
                Return System.IO.Path.ChangeExtension(Me.AssemblyInfo.AssemblyName, "dll")
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the assembly path.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Path")>
        Public ReadOnly Property Path As String
            Get
                Return $".\{SmartBotUtil.PluginsDir.Name}\{Me.AssemblyName}"
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin title.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("[Title]")>
        Public ReadOnly Property Title As String
            Get
                Return Me.AssemblyInfo.Title
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Name")>
        <Browsable(False)>
        <[ReadOnly](True)>
        Public Shadows ReadOnly Property Name As String
            Get
                Return Me.AssemblyInfo.AssemblyName
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Author")>
        Public ReadOnly Property Author As String
            Get
                Return Me.AssemblyInfo.CompanyName
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
                Return Me.tickCount_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.tickCount_ = 1
                ElseIf (value > 100) Then
                    Me.tickCount_ = 100
                Else
                    Me.tickCount_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required tick count to change the Hearthstone's window size.
        ''' </summary>
        Private tickCount_ As Integer

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="HearthstoneResizerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.EnableFixedPos = True
            Me.EnableFixedSize = True

            Me.Position = Point.Empty
            Me.Size = "640×480 (4:3)"

            Me.IgnoreTicksIfBotStopped = False
            Me.TickCount = 10
            Me.HearthstoneResizerEvent = SmartBotEvent.TimerTick
        End Sub

#End Region

    End Class

End Namespace

#End Region
