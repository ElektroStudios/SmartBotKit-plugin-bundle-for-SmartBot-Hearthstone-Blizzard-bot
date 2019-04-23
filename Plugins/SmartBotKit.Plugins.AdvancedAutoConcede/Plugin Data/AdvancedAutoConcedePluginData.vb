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

Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

#End Region

#Region " AdvancedAutoConcedePluginData "

Namespace PluginTemplate

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

        <Browsable(False)>
        Public Shadows Property Name As String

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
                Return Me.maxRankedWinsB
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.maxRankedWinsB = 1
                ElseIf value > 99 Then
                    Me.maxRankedWinsB = 99
                Else
                    Me.maxRankedWinsB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required amount of wins in ranked mode to start conceding the next match.
        ''' </summary>
        Private maxRankedWinsB As Integer

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
                Return Me.maxRankedConcedesB
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.maxRankedConcedesB = 1
                ElseIf value > 99 Then
                    Me.maxRankedConcedesB = 99
                Else
                    Me.maxRankedConcedesB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The concede count for ranked mode.
        ''' </summary>
        Private maxRankedConcedesB As Integer

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
                Return Me.minRankStandardB
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.minRankStandardB = 1
                ElseIf value > 25 Then
                    Me.minRankStandardB = 25
                Else
                    Me.minRankStandardB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required rank to apply auto-concede settings for ranked mode.
        ''' </summary>
        Private minRankStandardB As Integer

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
                Return Me.maxRankStandardB
            End Get
            Set(ByVal value As Integer)
                If value < 0 Then
                    Me.maxRankStandardB = 0
                ElseIf value > 24 Then
                    Me.maxRankStandardB = 24
                Else
                    Me.maxRankStandardB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' A value that determine when to switch to unranked mode after reaching standard rank.
        ''' </summary>
        Private maxRankStandardB As Integer

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
                Return Me.minRankWildB
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.minRankWildB = 1
                ElseIf value > 25 Then
                    Me.minRankWildB = 25
                Else
                    Me.minRankWildB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required rank to apply auto-concede settings for ranked mode.
        ''' </summary>
        Private minRankWildB As Integer

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
                Return Me.maxRankWildB
            End Get
            Set(ByVal value As Integer)
                If value < 0 Then
                    Me.maxRankWildB = 0
                ElseIf value > 24 Then
                    Me.maxRankWildB = 24
                Else
                    Me.maxRankWildB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' A value that determine when to switch to unranked mode after reaching wild rank.
        ''' </summary>
        Private maxRankWildB As Integer

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
                Return Me.maxUnrankedWinsB
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.maxUnrankedWinsB = 1
                ElseIf value > 99 Then
                    Me.maxUnrankedWinsB = 99
                Else
                    Me.maxUnrankedWinsB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required amount of wins in unranked mode to start conceding the next match.
        ''' </summary>
        Private maxUnrankedWinsB As Integer

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
                Return Me.maxUnrankedConcedesB
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.maxUnrankedConcedesB = 1
                ElseIf value > 99 Then
                    Me.maxUnrankedConcedesB = 99
                Else
                    Me.maxUnrankedConcedesB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The concede count for unranked mode.
        ''' </summary>
        Private maxUnrankedConcedesB As Integer

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
            MyBase.Name = Me.AssemblyInfo.AssemblyName

            Me.EnableRankedModeAutoConcede = False
            Me.EnableUnrankedModeAutoConcede = False
            Me.ResetWinsCountAfterLose = False
            Me.ResetWinsCountAfterBotStop = False
            Me.maxRankedWinsB = 1
            Me.maxRankedConcedesB = 1
            Me.maxUnrankedWinsB = 1
            Me.maxUnrankedConcedesB = 1
            Me.minRankstandardB = 25
            Me.minRankWildB = 25
            Me.maxRankStandardB = 1
            Me.maxRankWildB = 1
        End Sub

#End Region

    End Class

End Namespace

#End Region
