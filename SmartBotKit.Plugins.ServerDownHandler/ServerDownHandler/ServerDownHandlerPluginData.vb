
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
        ''' Gets a value that indicate whether the bot should be stopped if server is down and current mode is 
        ''' <see cref="Bot.Mode.ArenaAuto"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Stop the bot if current mode is: AutoArena.")>
        <Browsable(True)>
        Public Property StopTheBotIfArena As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that indicate whether the bot should be stopped if server is down and current mode is 
        ''' <see cref="Bot.Mode.RankedStandard"/> or <see cref="Bot.Mode.RankedWild"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Stop the bot if current mode is: RankedStandard or RankedWild.")>
        <Browsable(True)>
        Public Property StopTheBotIfRanked As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that indicate whether the bot should be stopped if server is down and current mode is 
        ''' <see cref="Bot.Mode.UnrankedStandard"/> or <see cref="Bot.Mode.UnrankedWild"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Stop the bot if current mode is: UnrankedStandard or UnrankedWild.")>
        <Browsable(True)>
        Public Property StopTheBotIfUnranked As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to play a wave sound file when a server down is detected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Play sound file when server down is detected.")>
        Public Property PlaySoundFile As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="ServerDownHandlerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name

            Me.StopTheBotIfArena = False
            Me.StopTheBotIfRanked = False
            Me.StopTheBotIfUnranked = False

            Me.PlaySoundFile = True
        End Sub

#End Region

    End Class

End Namespace

#End Region
