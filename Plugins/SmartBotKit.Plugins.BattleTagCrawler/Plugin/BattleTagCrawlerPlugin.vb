#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Text

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Bot

Imports HearthMirror.Objects.MatchInfo

Imports SmartBotKit.Extensions.StringExtensions
Imports SmartBotKit.Interop
Imports SmartBotKit.Text
Imports System.Collections.Generic
Imports System.Linq

#End Region

#Region " BattleTagCrawlerPlugin "

Namespace PluginTemplate

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that collects BattleTag ids from opponents into a csv file.
    ''' </summary>
    ''' <example></example>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class BattleTagCrawlerPlugin : Inherits Plugin

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin's data container.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The plugin's data container.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shadows ReadOnly Property DataContainer As BattleTagCrawlerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, BattleTagCrawlerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="BattleTagCrawlerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="BattleTagCrawlerPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Public Sub New()
            Me.IsDll = True
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="BattleTagCrawlerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[BattleTag Crawler] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="BattleTagCrawlerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[BattleTag Crawler] -> Plugin enabled.")
                Else
                    Bot.Log("[BattleTag Crawler] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameBegin()
            If (Me.DataContainer.Enabled) Then

                Select Case Bot.CurrentMode
                    Case Mode.RankedStandard
                        If Not Me.DataContainer.CrawlRankedStandardGames Then
                            Exit Sub
                        End If

                    Case Mode.UnrankedStandard
                        If Not Me.DataContainer.CrawlUnrankedStandardGames Then
                            Exit Sub
                        End If

                    Case Mode.RankedWild
                        If Not Me.DataContainer.CrawlRankedWildGames Then
                            Exit Sub
                        End If

                    Case Mode.UnrankedWild
                        If Not Me.DataContainer.CrawlUnrankedWildGames Then
                            Exit Sub
                        End If

                    Case Else ' Mode.Arena, Mode.ArenaAuto, Mode.Practice
                        Exit Sub
                End Select

                Dim dirInfo As New DirectoryInfo(Path.Combine(SmartBotUtil.LogsDir.FullName, "BattleTag Crawler"))
                Dim filename As String = If(Me.DataContainer.UseSingleLogFile,
                                            "[BattleTag Crawler].csv",
                                            String.Format("[BattleTag Crawler] {0}.csv", Date.Now.ToShortDateString().Replace("/"c, "-"c)))

                Dim fileInfo As New FileInfo(Path.Combine(dirInfo.FullName, filename))

                If (Not dirInfo.Exists) Then
                    Try
                        SmartBotUtil.LogsDir.Create()
                        dirInfo.Create()
                        dirInfo.Refresh()
                    Catch ex As Exception
                        Bot.Log(String.Format("[BattleTag Crawler] -> Error creating log directory: {0}", ex.Message))
                    End Try
                End If

                If (dirInfo.Exists) Then
                    Dim player As Player = HearthMirror.Reflection.GetMatchInfo().OpposingPlayer
                    Dim battletag As String = String.Format("{0}#{1}", player.BattleTag.Name, player.BattleTag.Number)
                    Dim standardRank As Integer = player.StandardRank
                    Dim wildrank As Integer = player.WildRank

                    If Not Me.DataContainer.LogDuplicates Then
                        If (fileInfo.Exists) AndAlso (File.ReadAllText(fileInfo.FullName, Encoding.Unicode).Contains(battletag)) Then
                            Exit Sub
                        End If
                    End If

                    If Not (fileInfo.Exists) Then
                        Try
                            File.WriteAllText(fileInfo.FullName, "Date,Game Mode,Standard Rank,Wild Rank,BattleTag" & Environment.NewLine, Encoding.Unicode)

                        Catch ex As Exception
                            Bot.Log(String.Format("[BattleTag Crawler] -> Error writing to file: {0}", ex.Message))

                        End Try
                    End If

                    Dim newLine As String = String.Format("{0},{1},{2},{3},{4}",
                                                          Date.Now.ToString("yyyy-MM-dd hh\:mm\:ss"),
                                                          Bot.CurrentMode.ToString(),
                                                          standardRank, wildrank,
                                                          battletag)

                    Try
                        Bot.Log(String.Format("[BattleTag Crawler] -> Logging opponent id: {0}", battletag))

                        If (Me.DataContainer.AddNewEntriesAtBeginningOfFile) Then
                            Dim lines As List(Of String) = File.ReadLines(fileInfo.FullName, Encoding.Unicode).ToList()
                            lines.Insert(1, newLine)
                            File.WriteAllLines(fileInfo.FullName, lines, Encoding.Unicode)

                        Else
                            File.AppendAllText(fileInfo.FullName, newLine & Environment.NewLine, Encoding.Unicode)

                        End If

                    Catch ex As Exception
                        Bot.Log(String.Format("[BattleTag Crawler] -> Error writing to file: {0}", ex.Message))

                    End Try
                End If

            End If

            MyBase.OnGameBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="BattleTagCrawlerPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

#End Region

    End Class

End Namespace

#End Region
