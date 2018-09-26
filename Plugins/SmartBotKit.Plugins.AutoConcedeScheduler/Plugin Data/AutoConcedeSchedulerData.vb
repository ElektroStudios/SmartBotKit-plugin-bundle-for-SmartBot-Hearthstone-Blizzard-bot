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

#Region " AutoConcedeSchedulerPluginData "

Namespace PluginTemplate

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="AutoConcedeSchedulerPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class AutoConcedeSchedulerPluginData : Inherits PluginDataContainer

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
        ''' ( of <see cref="AutoConcedeSchedulerPluginData.MaxRankedWins"/> or 
        ''' <see cref="AutoConcedeSchedulerPluginData.MaxUnrankedWins"/> 
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
        ''' ( of <see cref="AutoConcedeSchedulerPluginData.MaxRankedWins"/> and 
        ''' <see cref="AutoConcedeSchedulerPluginData.MaxUnrankedWins"/> ) 
        ''' will be reseted to zero after stopping the bot.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Behavior")>
        <DisplayName("Reset wins count after bot is stopped.")>
        <Browsable(True)>
        Public Property ResetWinsCountAfterBotStop As Boolean

#End Region

#Region " Ranked Mode "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether auto-concede settings are enabled for ranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode")>
        <DisplayName("Enable auto-concede for ranked mode")>
        <Browsable(True)>
        Public Property EnableRankedModeAutoConcede As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the required rank to apply auto-concede settings for ranked mode.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode")>
        <DisplayName("Don't auto-concede if below rank:")>
        <Browsable(True)>
        Public Property MinRank As Integer
            Get
                Return Me.minRankB
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Me.minRankB = 1
                ElseIf value > 25 Then
                    Me.minRankB = 25
                Else
                    Me.minRankB = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The required rank to apply auto-concede settings for ranked mode.
        ''' </summary>
        Private minRankB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine the required amount of wins in ranked mode 
        ''' to concede the next match.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Ranked Mode")>
        <DisplayName("Required amount of wins before conceding the next match:")>
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
        ''' The required amount of wins in ranked mode before conceding the next match.
        ''' </summary>
        Private maxRankedWinsB As Integer

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
        ''' Gets or sets a value that determine the required amount of wins in unranked mode 
        ''' to concede the next match.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Unranked Mode")>
        <DisplayName("Required amount of wins before conceding the next match:")>
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
        ''' The required amount of wins in unranked mode before conceding the next match.
        ''' </summary>
        Private maxUnrankedWinsB As Integer

#End Region

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="AutoConcedeSchedulerPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.AssemblyInfo.AssemblyName

            Me.EnableRankedModeAutoConcede = False
            Me.EnableUnrankedModeAutoConcede = False
            Me.ResetWinsCountAfterLose = False
            Me.ResetWinsCountAfterBotStop = False
            Me.maxRankedWinsB = 1
            Me.maxUnrankedWinsB = 1
            Me.minRankB = 25
        End Sub

#End Region

    End Class

End Namespace

#End Region
