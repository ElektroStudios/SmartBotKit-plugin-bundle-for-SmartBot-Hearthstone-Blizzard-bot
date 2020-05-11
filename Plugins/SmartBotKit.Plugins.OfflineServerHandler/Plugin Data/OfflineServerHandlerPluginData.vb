
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Reflection

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

#End Region

#Region " OfflineServerHandlerPluginData "

' ReSharper disable once CheckNamespace

Namespace OfflineServerHandler

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="OfflineServerHandlerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class OfflineServerHandlerPluginData : Inherits PluginDataContainer

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
                Return "Handles the bot behavior when the server gets down" & ControlChars.NewLine &
                       "(not lag, ﻿local network inactivity﻿﻿ neither ﻿auth. problems)."
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
        ''' Gets the last known server down time record for the current session.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Info")>
        <DisplayName("Last known server down time record for the current session")>
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

#End Region

#Region " Notification "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to play a wave sound file when a server down is detected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Notification")>
        <DisplayName("Play sound file when server down is detected")>
        Public Property PlaySoundFile As Boolean

#End Region

#Region " Disconnection "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that indicate whether the bot should be stopped if server is down and current mode is 
        ''' <see cref="Bot.Mode.ArenaAuto"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Disconnection")>
        <DisplayName("Stop the bot if current mode is: AutoArena")>
        <Browsable(True)>
        Public Property StopTheBotIfArena As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that indicate whether the bot should be stopped if server is down and current mode is 
        ''' <see cref="Bot.Mode.RankedStandard"/> or <see cref="Bot.Mode.RankedWild"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Disconnection")>
        <DisplayName("Stop the bot if current mode is: RankedStandard or RankedWild")>
        <Browsable(True)>
        Public Property StopTheBotIfRanked As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that indicate whether the bot should be stopped if server is down and current mode is 
        ''' <see cref="Bot.Mode.UnrankedStandard"/> or <see cref="Bot.Mode.UnrankedWild"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Disconnection")>
        <DisplayName("Stop the bot if current mode is: UnrankedStandard or UnrankedWild")>
        <Browsable(True)>
        Public Property StopTheBotIfUnranked As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the new computer state if server is down.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Disconnection")>
        <DisplayName("Set the computer state")>
        <ItemsSource(GetType(ComputerStateSource))>
        Public Property SetComputerState As ComputerState

#End Region

#Region " Reconnection "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the bot should resume and try reconnect to the server after the 
        ''' specified time interval.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Reconnection")>
        <DisplayName("Enable bot resumption")>
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
        <DisplayName("The time interval, in minutes, to resume the bot and try reconnect to the server")>
        <Browsable(True)>
        Public Property ResumeInterval() As Integer
            Get
                Return Me.resumeInterval_
            End Get
            Set(ByVal value As Integer)
                If (value < 5) Then
                    Me.resumeInterval_ = 5
                ElseIf value > 1440 Then
                    Me.resumeInterval_ = 1440
                Else
                    Me.resumeInterval_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The time interval, in minutes, to resume the bot and try reconnect to the server.
        ''' </summary>
        Private resumeInterval_ As Integer

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="OfflineServerHandlerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.StopTheBotIfArena = True
            Me.StopTheBotIfRanked = True
            Me.StopTheBotIfUnranked = True

            Me.SetComputerState = ComputerState.NoChange

            Me.PlaySoundFile = True

            Me.ResumeEnabled = True
            Me.ResumeInterval = 60
        End Sub

#End Region

    End Class

End Namespace

#End Region
