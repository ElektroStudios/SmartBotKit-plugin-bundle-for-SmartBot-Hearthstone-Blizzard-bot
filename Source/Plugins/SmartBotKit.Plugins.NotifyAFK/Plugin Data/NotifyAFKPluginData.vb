
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Reflection

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Interop
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Collections.Generic
Imports System.Text
Imports System.Linq

#End Region

#Region " NotifyAFKPluginData "

' ReSharper disable once CheckNamespace

Namespace NotifyAFK

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="NotifyAFKPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class NotifyAFKPluginData : Inherits PluginDataContainer

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
                Return "Notifies when the current opponent is AFK."
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

#Region " Settings "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the amount of turns that the opponent is AFK to notify it.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value between 2 and 10.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("The amount of turns that the opponent is AFK to notify it")>
        <Browsable(True)>
        Public Property TurnsAFK As Integer
            Get
                Return Me.turnsAFK_
            End Get
            Set(value As Integer)
                If value < 2 Then
                    Me.turnsAFK_ = 2
                ElseIf value > 10 Then
                    Me.turnsAFK_ = 10
                Else
                    Me.turnsAFK_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' A value that determine the amount of turns that the opponent is AFK to notify it.
        ''' </summary>
        Private turnsAFK_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the amount of turns that the opponent is not AFK to concede the current game.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value between 2 and 10.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("The amount of turns that the opponent is not AFK to concede the current game")>
        <Browsable(True)>
        Public Property TurnsNotAFK As Integer
            Get
                Return Me.turnsNotAFK_
            End Get
            Set(value As Integer)
                If value < 1 Then
                    Me.turnsNotAFK_ = 1
                ElseIf value > 10 Then
                    Me.turnsNotAFK_ = 10
                Else
                    Me.turnsNotAFK_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' A value that determine the amount of opponent AFK turns to start notify.
        ''' </summary>
        Private turnsNotAFK_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to notify an AFK game when matchmaking the specified battletag names.
        ''' <para></para>
        ''' ( One battletag per line. Unicode encoding. Case insensitive. Format: 'name#id' or 'name' ) 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Always notify when matchmaking BattleTags specified in file:")>
        <Browsable(True)>
        <[ReadOnly](True)>
        Public ReadOnly Property NotifyToBattletagsInFile As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to concede the current game if a emote is received.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Concede current game if emote is received")>
        Public Property ConcedeIfEmoteReceived As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to concede the current game if mulligan takes less than 60 seconds.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Concede current game if mulligan takes less than 60 seconds")>
        Public Property ConcedeIfMulliganTakesLessThan60Seconds As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether to concede the current game if mulligan takes more than 60 seconds.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("Notify AFK if mulligan takes more than 60 seconds")>
        Public Property NotifyAFKIfMulliganTakesMoreThan60Seconds As Boolean

#End Region

#Region " Notification "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to play a wave sound file when a match is found.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Notification")>
        <DisplayName("Play sound file when opponent is AFK")>
        Public Property PlaySoundFile As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to activate Hearthstone window when a match is found.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings: Notification")>
        <DisplayName("Activate Hearthstone window")>
        Public Property ActivateWindow As Boolean

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="NotifyAFKPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.TurnsAFK = 3
            Me.TurnsNotAFK = 1
            Me.ConcedeIfEmoteReceived = True
            Me.ConcedeIfMulliganTakesLessThan60Seconds = True
            Me.NotifyAFKIfMulliganTakesMoreThan60Seconds = True

            Me.ActivateWindow = True
            Me.PlaySoundFile = True

            Me.NotifyToBattletagsInFile = $"{SmartBotUtil.PluginsDir.FullName}\NotifyAFK.txt"
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the battletags from <see cref="AdvancedAutoConcedePluginData.ConcedeToBattletagsInFile"/> as a list.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function GetBattletags() As ReadOnlyCollection(Of String)

            If File.Exists(Me.NotifyToBattletagsInFile) Then
                Dim lines As IEnumerable(Of String)
                Try
                    lines = File.ReadLines(Me.NotifyToBattletagsInFile, Encoding.Unicode)
                    Return (From line As String In lines
                            Where Not String.IsNullOrWhiteSpace(line) AndAlso Not line.StartsWith("#"c)
                            Select line.ToLower().Trim(" "c)
                            ).ToList().AsReadOnly()

                Catch ex As Exception
                    Bot.Log($"[Advanced Auto Concede] -> Error reading file: {Me.NotifyToBattletagsInFile}'")

                End Try
            End If

            Return Nothing

        End Function

#End Region

    End Class

End Namespace

#End Region
