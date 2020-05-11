
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text

Imports HearthMirror.Objects

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Audio
Imports SmartBotKit.Computer
Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.ReservedUse

#End Region

#Region " LegendRankHandlerPlugin "

' ReSharper disable once CheckNamespace

Namespace LegendRankHandler

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that handles what to do when the bot reachs legend rank.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class LegendRankHandlerPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As LegendRankHandlerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, LegendRankHandlerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="LegendRankHandlerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Flag to avoid redundant legend rank notifications.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private isLegendNotified As Boolean

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="LegendRankHandlerPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            UpdateUtil.RunUpdaterExecutable()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="LegendRankHandlerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Legend Rank Handler] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="LegendRankHandlerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Legend Rank Handler] -> Plugin enabled.")
                Else
                    Bot.Log("[Legend Rank Handler] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameEnd()

            Dim currentMode As Bot.Mode = Bot.CurrentMode
            Dim notifyLegend As Boolean =
                (((currentMode = Bot.Mode.RankedStandard) AndAlso (Bot.GetPlayerDatas().GetRank(wild:=False) = 0)) OrElse
                ((currentMode = Bot.Mode.RankedWild) AndAlso (Bot.GetPlayerDatas().GetRank(wild:=True) = 0)))

#If DEBUG Then
            ' notifyLegend = True
            ' Bot.Log("[Legend Rank Handler] [DEBUG] -> notifyLegend = True")
#End If

            If (notifyLegend AndAlso Not Me.isLegendNotified) Then
                Bot.Log("[Legend Rank Handler] -> Congrats, you reached legend rank!")
                Me.isLegendNotified = True

                If (Me.DataContainer.PlaySoundFile) Then
                    Dim file As New FileInfo(Path.Combine(SmartBotUtil.PluginsDir.FullName, "LegendRankHandler.mp3"))
                    If Not file.Exists() Then
                        Bot.Log($"[Legend Rank Handler] -> Audio file not found: '{file.FullName}'")
                    Else
                        AudioUtil.PlaySoundFile(file)
                    End If
                End If

                If Me.DataContainer.SendNotificationMail Then
                    Bot.Log($"[Legend Rank Handler] -> Sending notification e-mail to '{Me.DataContainer.ToAddress}'...")

                    Using client As New SmtpClient With {
                        .Host = Me.DataContainer.SmtpServer,
                        .Port = Me.DataContainer.SmtpPort,
                        .EnableSsl = Me.DataContainer.SmtpEnableSsl,
                        .Credentials = New NetworkCredential(Me.DataContainer.SmtpUserName, Me.DataContainer.SmtpPassword),
                        .Timeout = CInt(TimeSpan.FromSeconds(30).TotalMilliseconds)
                    }, msg As New MailMessage(Me.DataContainer.FromAddress, Me.DataContainer.ToAddress) With {
                            .IsBodyHtml = True,
                            .Subject = $"You got legend in Hearthstone!",
                            .BodyEncoding = Encoding.UTF8,
                            .Body = My.Resources.LegendAlter ' non-embedded images.
                        }

                        Dim btag As BattleTag = HearthMirror.Reflection.GetBattleTag()
                        Dim deck As API.Deck = Bot.CurrentDeck()
                        msg.Body = msg.Body.Replace("{name}", $"{btag.Name}#{btag.Number}").
                                            Replace("{date}", Date.Now.ToString()).
                                            Replace("{mode}", Bot.CurrentMode().ToString()).
                                            Replace("{hero}", deck.Class.ToString()).
                                            Replace("{deck}", deck.Name)

                        Try
                            client.Send(msg)
                            Bot.Log($"[Legend Rank Handler] -> Notification e-mail was successfully sent.")

                        Catch ex As Exception
                            Bot.Log($"[Legend Rank Handler] -> Error sending notification e-mail: '{ex.Message}'")

                        End Try

                    End Using

                End If

                If Me.DataContainer.StopSmartBot Then
                    Bot.Log("[Legend Rank Handler] -> Bot stopped.")
                    Bot.StopBot()
                End If

                Select Case Me.DataContainer.SetComputerState

                    Case ComputerState.Hibernate
                        Bot.Log("[Legend Rank Handler] -> Hibernating the computer...")
                        PowerUtil.Hibernate(force:=True)

                    Case ComputerState.Suspend
                        Bot.Log("[Legend Rank Handler] -> Suspending the computer...")
                        PowerUtil.Suspend(force:=True)

                    Case ComputerState.Shutdown
                        Bot.Log("[Legend Rank Handler] -> Powering off the computer...")
                        PowerUtil.Shutdown("", 0, "", ShutdownMode.ForceOthers, ShutdownReason.FagUserPlanned, ShutdownPlanning.Planned, True)

                    Case Else ' ComputerState.NoChange

                End Select

            End If

            MyBase.OnGameEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="LegendRankHandlerPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

    End Class

End Namespace

#End Region
