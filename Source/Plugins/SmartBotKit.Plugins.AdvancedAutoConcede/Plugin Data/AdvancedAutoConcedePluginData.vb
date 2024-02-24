#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Text

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Interop

#End Region

#Region " AdvancedAutoConcedePluginData "

' ReSharper disable once CheckNamespace

Namespace AdvancedAutoConcede

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="AdvancedAutoConcedePlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class AdvancedAutoConcedePluginData : Inherits PluginDataContainer

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
                Return "A plugin to schedule auto-concede after winning a match"
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

#Region " Info "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current concede count.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category(".Info")>
        <DisplayName("Current concede count")>
        <Browsable(True)>
        <[ReadOnly](True)>
        Public ReadOnly Property CurrentConcedeCount As Integer
            Get
                Return AdvancedAutoConcedePlugin.concedesCount
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current ranked wins count.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category(".Info")>
        <DisplayName("Current ranked wins count")>
        <Browsable(True)>
        <[ReadOnly](True)>
        Public ReadOnly Property CurrentRankedWinsCount As Integer
            Get
                Return AdvancedAutoConcedePlugin.rankedWinsCount
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current unranked wins count.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category(".Info")>
        <DisplayName("Current unranked wins count")>
        <Browsable(True)>
        <[ReadOnly](True)>
        Public ReadOnly Property CurrentCasualWinsCount As Integer
            Get
                Return AdvancedAutoConcedePlugin.casualWinsCount
            End Get
        End Property

#End Region

#Region " Behavior "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the current wins count 
        ''' ( of <see cref="AdvancedAutoConcedePluginData.MaxRankedWins"/> or 
        ''' <see cref="AdvancedAutoConcedePluginData.MaxCasualWins"/> 
        ''' depending on the current mode. ) will be reseted to zero after losing a match.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("Reset wins count after losing a match")>
        <Browsable(True)>
        Public Property ResetWinsCountAfterLose As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the current wins count 
        ''' ( of <see cref="AdvancedAutoConcedePluginData.MaxRankedWins"/> and 
        ''' <see cref="AdvancedAutoConcedePluginData.MaxCasualWins"/> ) 
        ''' will be reseted to zero after stopping the bot.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("Reset wins count after bot is stopped")>
        <Browsable(True)>
        Public Property ResetWinsCountAfterBotStop As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to concede game when matchmaking the specified battletag names.
        ''' <para></para>
        ''' ( One battletag per line. Unicode encoding. Case insensitive. Format: 'name#id' or 'name' ) 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("Always concede when matchmaking BattleTags specified in file:")>
        <Browsable(True)>
        <[ReadOnly](True)>
        Public ReadOnly Property ConcedeToBattletagsInFile As String

#End Region

#Region " Ranked Modes "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether auto-concede settings are enabled for ranked modes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("* Enable auto-concede for Standard, Wild and Twist modes")>
        <Browsable(True)>
        Public Property EnableRankedModeAutoConcede As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the required amount of wins in ranked modes to start conceding the next match.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Amount of Wins:")>
        <Browsable(True)>
        Public Property MaxRankedWins As Integer
            Get
                Return Me.maxRankedWins_
            End Get
            Set(value As Integer)
                If value < 1 Then
                    Me.maxRankedWins_ = 1
                ElseIf value > 99 Then
                    Me.maxRankedWins_ = 99
                Else
                    Me.maxRankedWins_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required amount of wins in ranked modes to start conceding the next match.
        ''' </summary>
        Private maxRankedWins_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the concede count for ranked modes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Amount of Concedes:")>
        <Browsable(True)>
        Public Property MaxRankedConcedes As Integer
            Get
                Return Me.maxRankedConcedes_
            End Get
            Set(value As Integer)
                If value < 1 Then
                    Me.maxRankedConcedes_ = 1
                ElseIf value > 99 Then
                    Me.maxRankedConcedes_ = 99
                Else
                    Me.maxRankedConcedes_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The concede count for ranked mode.
        ''' </summary>
        Private maxRankedConcedes_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine when to switch to casual mode after reaching rank.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Switch to casual mode after reaching rank:")>
        <Browsable(True)>
        Public Property MaxRank As Integer
            Get
                Return Me.maxRank_
            End Get
            Set(value As Integer)
                If value < 0 Then
                    Me.maxRank_ = 0
                ElseIf value > 24 Then
                    Me.maxRank_ = 24
                Else
                    Me.maxRank_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' A value that determine when to switch to casual mode after reaching rank.
        ''' </summary>
        Private maxRank_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Death Knight.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Death Knight. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Death Knight")>
        <Browsable(True)>
        Public Property AlwaysConcedeToDeathKnight As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a DemonHunter.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a DemonHunter. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to DemonHunter")>
        <Browsable(True)>
        Public Property AlwaysConcedeToDemonHunter As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Druid.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Druid. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Druid")>
        <Browsable(True)>
        Public Property AlwaysConcedeToDruid As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Hunter.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Hunter. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Hunter")>
        <Browsable(True)>
        Public Property AlwaysConcedeToHunter As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Mage.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Mage. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Mage")>
        <Browsable(True)>
        Public Property AlwaysConcedeToMage As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Paladin.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Paladin. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Paladin")>
        <Browsable(True)>
        Public Property AlwaysConcedeToPaladin As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Priest.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Priest. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Priest")>
        <Browsable(True)>
        Public Property AlwaysConcedeToPriest As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Rogue.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Rogue. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Rogue")>
        <Browsable(True)>
        Public Property AlwaysConcedeToRogue As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Shaman.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Shaman. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Shaman")>
        <Browsable(True)>
        Public Property AlwaysConcedeToShaman As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Warlock.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Warlock. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Warlock")>
        <Browsable(True)>
        Public Property AlwaysConcedeToWarlock As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether to always concede when matchmaking a Warrior.
        ''' <para></para>
        ''' If this value is <see langword="True"/>, a game will be always conceded when matchmaking a Warrior. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Standard, Wild and Twist Modes")>
        <DisplayName("Always concede to Warrior")>
        <Browsable(True)>
        Public Property AlwaysConcedeToWarrior As Boolean

#End Region

#Region " Casual Mode "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether auto-concede settings are enabled for casual mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Casual Mode")>
        <DisplayName("* Enable auto-concede for casual mode")>
        <Browsable(True)>
        Public Property EnableCasualModeAutoConcede As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the required amount of wins in casual mode to start conceding the next match.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Casual Mode")>
        <DisplayName("Amount of Wins:")>
        <Browsable(True)>
        Public Property MaxCasualWins As Integer
            Get
                Return Me.maxCasualWins_
            End Get
            Set(value As Integer)
                If value < 1 Then
                    Me.maxCasualWins_ = 1
                ElseIf value > 99 Then
                    Me.maxCasualWins_ = 99
                Else
                    Me.maxCasualWins_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required amount of wins in casual mode to start conceding the next match.
        ''' </summary>
        Private maxCasualWins_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the concede count for casual mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Casual Mode")>
        <DisplayName("Amount of Concedes:")>
        <Browsable(True)>
        Public Property MaxCasualConcedes As Integer
            Get
                Return Me.maxCasualConcedes_
            End Get
            Set(value As Integer)
                If value < 1 Then
                    Me.maxCasualConcedes_ = 1
                ElseIf value > 99 Then
                    Me.maxCasualConcedes_ = 99
                Else
                    Me.maxCasualConcedes_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The concede count for casual mode.
        ''' </summary>
        Private maxCasualConcedes_ As Integer

#End Region

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="AdvancedAutoConcedePluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.EnableRankedModeAutoConcede = False
            Me.EnableCasualModeAutoConcede = False
            Me.ResetWinsCountAfterLose = False
            Me.ResetWinsCountAfterBotStop = False
            Me.maxRankedWins_ = 1
            Me.maxRankedConcedes_ = 1
            Me.maxCasualWins_ = 1
            Me.maxCasualConcedes_ = 1
            Me.maxRank_ = 0

            Me.AlwaysConcedeToDeathKnight = False
            Me.AlwaysConcedeToDemonHunter = False
            Me.AlwaysConcedeToDruid = False
            Me.AlwaysConcedeToHunter = False
            Me.AlwaysConcedeToMage = False
            Me.AlwaysConcedeToPaladin = False
            Me.AlwaysConcedeToPriest = False
            Me.AlwaysConcedeToRogue = False
            Me.AlwaysConcedeToShaman = False
            Me.AlwaysConcedeToWarlock = False
            Me.AlwaysConcedeToWarrior = False

            Me.ConcedeToBattletagsInFile = $"{SmartBotUtil.PluginsDir.FullName}\AdvancedAutoConcede.txt"
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the battletags from <see cref="AdvancedAutoConcedePluginData.ConcedeToBattletagsInFile"/> as a list.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function GetBattletags() As ReadOnlyCollection(Of String)

            If File.Exists(Me.ConcedeToBattletagsInFile) Then
                Dim lines As IEnumerable(Of String)
                Try
                    lines = File.ReadLines(Me.ConcedeToBattletagsInFile, Encoding.Unicode)
                    Return (From line As String In lines
                            Where Not String.IsNullOrWhiteSpace(line) AndAlso Not line.StartsWith("#"c)
                            Select line.ToLower().Trim(" "c)
                            ).ToList().AsReadOnly()

                Catch ex As Exception
                    Bot.Log($"[Advanced Auto Concede] -> Error reading file: {Me.ConcedeToBattletagsInFile}'")

                End Try
            End If

            Return Nothing

        End Function

#End Region

    End Class

End Namespace

#End Region
