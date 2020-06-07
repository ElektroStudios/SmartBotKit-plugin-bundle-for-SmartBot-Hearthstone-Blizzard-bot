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

#Region " Behavior "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the current wins count 
        ''' ( of <see cref="AdvancedAutoConcedePluginData.MaxRankedWins"/> or 
        ''' <see cref="AdvancedAutoConcedePluginData.MaxUnrankedWins"/> 
        ''' depending on the current mode. ) will be reseted to zero after losing a match.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("Reset wins count after losing a match.")>
        <Browsable(True)>
        Public Property ResetWinsCountAfterLose As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the current wins count 
        ''' ( of <see cref="AdvancedAutoConcedePluginData.MaxRankedWins"/> and 
        ''' <see cref="AdvancedAutoConcedePluginData.MaxUnrankedWins"/> ) 
        ''' will be reseted to zero after stopping the bot.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("Reset wins count after bot is stopped.")>
        <Browsable(True)>
        Public Property ResetWinsCountAfterBotStop As Boolean

#End Region

#Region " Ranked Mode (General Settings) "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether auto-concede settings are enabled for ranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode (General Settings)")>
        <DisplayName("Enable auto-concede for ranked mode")>
        <Browsable(True)>
        Public Property EnableRankedModeAutoConcede As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the required amount of wins in ranked mode to start conceding the next match.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode (General Settings)")>
        <DisplayName("Amount of Wins:")>
        <Browsable(True)>
        Public Property MaxRankedWins As Integer
            Get
                Return Me.maxRankedWins_
            End Get
            Set(ByVal value As Integer)
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
        ''' The required amount of wins in ranked mode to start conceding the next match.
        ''' </summary>
        Private maxRankedWins_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the concede count for ranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode (General Settings)")>
        <DisplayName("Amount of Concedes:")>
        <Browsable(True)>
        Public Property MaxRankedConcedes As Integer
            Get
                Return Me.maxRankedConcedes_
            End Get
            Set(ByVal value As Integer)
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

#End Region

#Region " Ranked Mode (Standard) "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the required rank to apply auto-concede settings for ranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode (Standard)")>
        <DisplayName("Don't auto-concede if below standard rank:")>
        <Browsable(True)>
        Public Property MinRankStandard As Integer
            Get
                Return Me.minRankStandard_
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.minRankStandard_ = 1
                ElseIf value > 25 Then
                    Me.minRankStandard_ = 25
                Else
                    Me.minRankStandard_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required rank to apply auto-concede settings for ranked mode.
        ''' </summary>
        Private minRankStandard_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine when to switch to unranked mode after reaching standard rank.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode (Standard)")>
        <DisplayName("Switch to unranked mode after reaching standard rank:")>
        <Browsable(True)>
        Public Property MaxRankStandard As Integer
            Get
                Return Me.maxRankStandard_
            End Get
            Set(ByVal value As Integer)
                If value < 0 Then
                    Me.maxRankStandard_ = 0
                ElseIf value > 24 Then
                    Me.maxRankStandard_ = 24
                Else
                    Me.maxRankStandard_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' A value that determine when to switch to unranked mode after reaching standard rank.
        ''' </summary>
        Private maxRankStandard_ As Integer

#End Region

#Region " Ranked Mode (Wild) "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the required rank to apply auto-concede settings for ranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode (Wild)")>
        <DisplayName("Don't auto-concede if below wild rank:")>
        <Browsable(True)>
        Public Property MinRankWild As Integer
            Get
                Return Me.minRankWild_
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.minRankWild_ = 1
                ElseIf value > 25 Then
                    Me.minRankWild_ = 25
                Else
                    Me.minRankWild_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required rank to apply auto-concede settings for ranked mode.
        ''' </summary>
        Private minRankWild_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine when to switch to unranked mode after reaching wild rank.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode (Wild)")>
        <DisplayName("Switch to unranked mode after reaching wild rank:")>
        <Browsable(True)>
        Public Property MaxRankWild As Integer
            Get
                Return Me.maxRankWild_
            End Get
            Set(ByVal value As Integer)
                If value < 0 Then
                    Me.maxRankWild_ = 0
                ElseIf value > 24 Then
                    Me.maxRankWild_ = 24
                Else
                    Me.maxRankWild_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' A value that determine when to switch to unranked mode after reaching wild rank.
        ''' </summary>
        Private maxRankWild_ As Integer

#End Region

#Region " Unranked Mode "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether auto-concede settings are enabled for unranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Unranked Mode")>
        <DisplayName("Enable auto-concede for unranked mode")>
        <Browsable(True)>
        Public Property EnableUnrankedModeAutoConcede As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the required amount of wins in unranked mode to start conceding the next match.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Unranked Mode")>
        <DisplayName("Amount of Wins:")>
        <Browsable(True)>
        Public Property MaxUnrankedWins As Integer
            Get
                Return Me.maxUnrankedWins_
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.maxUnrankedWins_ = 1
                ElseIf value > 99 Then
                    Me.maxUnrankedWins_ = 99
                Else
                    Me.maxUnrankedWins_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required amount of wins in unranked mode to start conceding the next match.
        ''' </summary>
        Private maxUnrankedWins_ As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the concede count for unranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Unranked Mode")>
        <DisplayName("Amount of Concedes:")>
        <Browsable(True)>
        Public Property MaxUnrankedConcedes As Integer
            Get
                Return Me.maxUnrankedConcedes_
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.maxUnrankedConcedes_ = 1
                ElseIf value > 99 Then
                    Me.maxUnrankedConcedes_ = 99
                Else
                    Me.maxUnrankedConcedes_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The concede count for unranked mode.
        ''' </summary>
        Private maxUnrankedConcedes_ As Integer

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
            Me.EnableUnrankedModeAutoConcede = False
            Me.ResetWinsCountAfterLose = False
            Me.ResetWinsCountAfterBotStop = False
            Me.maxRankedWins_ = 1
            Me.maxRankedConcedes_ = 1
            Me.maxUnrankedWins_ = 1
            Me.maxUnrankedConcedes_ = 1
            Me.minRankStandard_ = 25
            Me.minRankWild_ = 25
            Me.maxRankStandard_ = 0
            Me.maxRankWild_ = 0
        End Sub

#End Region

    End Class

End Namespace

#End Region
