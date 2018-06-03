
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Linq
Imports System.Reflection

Imports SmartBot.Plugins

#End Region

#Region " PlayAFriendChallengeNotifierPluginData "

Namespace PlayAFriendChallengeNotifier

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="PlayAFriendChallengeNotifierPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class PlayAFriendChallengeNotifierPluginData : Inherits PluginDataContainer

#Region " Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The supported resolutions for this plugin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly SupportedResolutions As New Dictionary(Of Size, Point) From {
            {New Size(640, 480), New Point(4, 3)},
            {New Size(800, 600), New Point(4, 3)},
            {New Size(1280, 768), New Point(5, 3)}
        } ' { Resolution, Aspect Ratio }

#End Region

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the supported resolutions for this plugin.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Info")>
        <DisplayName("These are the supported resolutions for this plugin." & ControlChars.NewLine &
                     "If you are running Hearthstone in any other resolution not listed here, then this plugin will not work." & ControlChars.NewLine &
                     "Note that you can use my plugin 'Hearthstone Resizer' to resize Hearthstone window to any of the resolutions listed here.")>
        Public ReadOnly Property SupportedResolutionsFormatted As String
            Get
                Dim stringValues As IEnumerable(Of String) =
                    From sz As Size In SupportedResolutions.Keys
                    Select String.Format("{0}x{1}", sz.Width, sz.Height)

                Return String.Join(Environment.NewLine, stringValues)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the interval, in seconds, to scan Hearthstone for new received challenge invitations of the 'Play a Friend' quest.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The interval, in seconds, to scan Hearthstone for new received challenge invitations of the 'Play a Friend' quest.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("The interval, in seconds, to check for new challenge invitations of the 'Play a Friend' quest.")>
        <Browsable(True)>
        Public Property Interval() As Integer
            Get
                Return Me.intervalB
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.intervalB = 1
                ElseIf (value > 10) Then
                    Me.intervalB = 10
                Else
                    Me.intervalB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The interval, in seconds, to scan Hearthstone for new received challenge invitations of the 'Play a Friend' quest.
        ''' </summary>
        Private intervalB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to play a wave sound file when a challenge is detected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Play sound file when a challenge is detected.")>
        Public Property PlaySoundFile As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to send Hearthstone window to foreground when a challenge is detected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Activate Hearthstone window when a challenge is detected.")>
        Public Property SetHearthstoneWindowToForeground As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to maximize Hearthstone window when a challenge is detected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Maximize Hearthstone window when a challenge is detected.")>
        Public Property SetHearthstoneWindowMaximized As Boolean

#If DEBUG Then

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to set Hearthstone window to maximized when a challenge is detected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("[DEBUG] Settings")>
        <DisplayName("Save captured screenshots to hard drive." & ControlChars.NewLine &
                     "The screenshots are saved into a subfolder inside the plugins directory.")>
        Public Property SaveScreenshots As Boolean

#End If

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="PlayAFriendChallengeNotifierPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name

            Me.Interval = 2
            Me.PlaySoundFile = True
            Me.SetHearthstoneWindowToForeground = True
            Me.SetHearthstoneWindowMaximized = True

#If DEBUG Then
            Me.SaveScreenshots = False
#End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
