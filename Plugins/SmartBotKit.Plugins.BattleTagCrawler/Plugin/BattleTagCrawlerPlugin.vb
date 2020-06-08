#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Text

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Bot

Imports SmartBotKit.Interop

Imports HearthMirror.Objects
Imports HearthMirror.Objects.MatchInfo

#End Region

#Region " BattleTagCrawlerPlugin "

' ReSharper disable once CheckNamespace

Namespace BattleTagCrawler

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

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="BattleTagCrawlerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ' ReSharper restore InconsistentNaming

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

                Dim hour As Integer = Date.Now.TimeOfDay.Hours
                If (hour >= Me.DataContainer.HourStart) AndAlso (hour < Me.DataContainer.HourEnd) Then

                    Dim dirInfo As New DirectoryInfo(Path.Combine(SmartBotUtil.LogsDir.FullName, "BattleTag Crawler"))
                    Dim filename As String = If(Me.DataContainer.UseSingleLogFile,
                                                "[BattleTag Crawler].csv",
                                                $"[BattleTag Crawler] {Date.Now.ToShortDateString().Replace("/"c, "-"c)}.csv")

                    Dim fileInfo As New FileInfo(Path.Combine(dirInfo.FullName, filename))

                    If (Not dirInfo.Exists) Then
                        Try
                            SmartBotUtil.LogsDir.Create()
                            dirInfo.Create()
                            dirInfo.Refresh()
                        Catch ex As Exception
                        End Try
                    End If

                    If (dirInfo.Exists) Then
                        Dim matchInfo As MatchInfo
                        Dim player As Player
                        Dim battletag As String = "#"
                        Dim standardRank As Integer
                        Dim wildrank As Integer

                        Try
                            matchInfo = HearthMirror.Reflection.GetMatchInfo()
                            If matchInfo Is Nothing Then
                                Throw New Exception(message:=$"'{NameOf(matchInfo)}' is null. (HDT libs are outdated?)")
                            End If

                            player = matchInfo.OpposingPlayer
                            If player Is Nothing Then
                                Throw New Exception(message:=$"'{NameOf(matchInfo.OpposingPlayer)}' is null. (HDT libs are outdated?)")
                            End If

                            battletag = $"{player.BattleTag.Name}#{player.BattleTag.Number}"
                            standardRank = player.StandardRank
                            wildrank = player.WildRank

                        Catch ex As Exception
                            Bot.Log($"[BattleTag Crawler] -> Error crawling BattleTag: {ex.Message}")
                        End Try

                        If Not Me.DataContainer.LogDuplicates Then
                            If (fileInfo.Exists) AndAlso (File.ReadAllText(fileInfo.FullName, Encoding.Unicode).Contains(battletag)) Then
                                Exit Sub
                            End If
                        End If

                        If Not (fileInfo.Exists) Then
                            Try
                                File.WriteAllText(fileInfo.FullName, "Date,Game Mode,Standard Rank,Wild Rank,BattleTag" & Environment.NewLine, Encoding.Unicode)

                            Catch ex As Exception
                                Bot.Log($"[BattleTag Crawler] -> Error writing to file: {ex.Message}")

                            End Try
                        End If

                        Dim newLine As String =
                                $"{Date.Now.ToString("yyyy-MM-dd HH\:mm\:ss")},{Bot.CurrentMode.ToString()},{standardRank},{ _
                                wildrank},{battletag}"

                        Try
                            Bot.Log($"[BattleTag Crawler] -> Logging opponent id: {battletag}")

                            If (Me.DataContainer.AddNewEntriesAtBeginningOfFile) Then
                                Dim lines As List(Of String) = File.ReadLines(fileInfo.FullName, Encoding.Unicode).ToList()
                                lines.Insert(1, newLine)
                                File.WriteAllLines(fileInfo.FullName, lines, Encoding.Unicode)

                            Else
                                File.AppendAllText(fileInfo.FullName, newLine & Environment.NewLine, Encoding.Unicode)

                            End If

                        Catch ex As Exception
                            Bot.Log($"[BattleTag Crawler] -> Error writing to file: {ex.Message}")

                        End Try

                    End If

                End If

            End If

            MyBase.OnGameBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="BattleTagCrawlerPlugin"/> instance.
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
