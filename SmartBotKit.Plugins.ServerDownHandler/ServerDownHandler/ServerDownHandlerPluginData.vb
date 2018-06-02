
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Reflection

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop

#End Region

#Region " ServerDownHandlerPluginData "

Namespace ServerDownHandler

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="ServerDownHandlerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class ServerDownHandlerPluginData : Inherits PluginDataContainer

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the last known server down time record for the current session.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Info")>
        <DisplayName("Last known server down time record for the current session.")>
        <Browsable(True)>
        Public ReadOnly Property LastServerDownRecord As String
            Get
                Dim ts As TimeSpan = SmartBotUtil.LastServerDownRecord
                If (ts = Nothing) Then
                    Return "None"
                End If
                Return ts.ToString()
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to play a wave sound file when a server down is detected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Notification")>
        <DisplayName("Play sound file when server down is detected.")>
        Public Property PlaySoundFile As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that indicate whether the bot should be stopped if server is down and current mode is 
        ''' <see cref="Bot.Mode.ArenaAuto"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Disconnection")>
        <DisplayName("Stop the bot if current mode is: AutoArena.")>
        <Browsable(True)>
        Public Property StopTheBotIfArena As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that indicate whether the bot should be stopped if server is down and current mode is 
        ''' <see cref="Bot.Mode.RankedStandard"/> or <see cref="Bot.Mode.RankedWild"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Disconnection")>
        <DisplayName("Stop the bot if current mode is: RankedStandard or RankedWild.")>
        <Browsable(True)>
        Public Property StopTheBotIfRanked As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that indicate whether the bot should be stopped if server is down and current mode is 
        ''' <see cref="Bot.Mode.UnrankedStandard"/> or <see cref="Bot.Mode.UnrankedWild"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Disconnection")>
        <DisplayName("Stop the bot if current mode is: UnrankedStandard or UnrankedWild.")>
        <Browsable(True)>
        Public Property StopTheBotIfUnranked As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the bot should resume and try reconnect to the server after the 
        ''' specified time interval.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reconnection")>
        <DisplayName("Enable bot resume.")>
        Public Property ResumeEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the time interval, in minutes, to resume the bot and try reconnect to the server.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value between 5 and 1440 (24 hours).
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reconnection")>
        <DisplayName("The time interval, in minutes, to resume the bot and try reconnect to the server.")>
        <Browsable(True)>
        Public Property ResumeInterval() As Integer
            Get
                Return Me.resumeIntervalB
            End Get
            Set(ByVal value As Integer)
                If (value < 5) Then
                    Me.resumeIntervalB = 5
                ElseIf value > 1440 Then
                    Me.resumeIntervalB = 1440
                Else
                    Me.resumeIntervalB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The time interval, in minutes, to resume the bot and try reconnect to the server.
        ''' </summary>
        Private resumeIntervalB As Integer

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="ServerDownHandlerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name

            Me.StopTheBotIfArena = True
            Me.StopTheBotIfRanked = True
            Me.StopTheBotIfUnranked = False

            Me.PlaySoundFile = True

            Me.ResumeEnabled = True
            Me.ResumeInterval = 60
        End Sub

#End Region

    End Class

End Namespace

#End Region
