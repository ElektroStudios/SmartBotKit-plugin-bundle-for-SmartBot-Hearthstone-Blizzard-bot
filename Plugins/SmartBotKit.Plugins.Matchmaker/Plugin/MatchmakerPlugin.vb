
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.IO
Imports System.Threading

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports SmartBotKit.Audio
Imports SmartBotKit.Interop
Imports SmartBotKit.Interop.Win32
Imports SmartBotKit.ReservedUse

#End Region

#Region " MatchmakerPlugin "

' ReSharper disable once CheckNamespace

Namespace Matchmaker

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that helps you find your favorite opponent match.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class MatchmakerPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As MatchmakerPluginData
            Get
                Return DirectCast(MyBase.DataContainer, MatchmakerPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="MatchmakerPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="MatchmakerPlugin"/> class.
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
        ''' Called when this <see cref="MatchmakerPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Matchmaker] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="MatchmakerPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Matchmaker] -> Plugin enabled.")
                    'Me.RunPluginComatibilityCheck()
                Else
                    Bot.Log("[Matchmaker] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is about to handle mulligan (to decide which card to replace) before a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="choices">
        ''' The mulligan choices.
        ''' </param>
        ''' 
        ''' <param name="opponentClass">
        ''' The opponent class.
        ''' </param>
        ''' 
        ''' <param name="ownClass">
        ''' Our hero class.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnHandleMulligan(ByVal choices As List(Of Card.Cards), ByVal opponentClass As Card.CClass, ByVal ownClass As Card.CClass)

            Dim allowedClasses As ReadOnlyCollection(Of Card.CClass) = Me.GetAllowedClasses()
            If Not allowedClasses.Contains(opponentClass) Then
                Bot.Concede()
                Bot.Log($"[Matchmaker] -> Game conceded to '{opponentClass.ToString()}' opponent.")

            Else
                If (Me.DataContainer.PlaySoundFile) Then
                    Dim file As New FileInfo(Path.Combine(SmartBotUtil.PluginsDir.FullName, "MatchMaker.mp3"))
                    If Not file.Exists() Then
                        Bot.Log($"[Matchmaker] -> Audio file not found: '{file.FullName}'")
                    Else
                        AudioUtil.PlaySoundFile(file)
                    End If
                End If

                If Me.DataContainer.ActivateWindow Then
                    Dim p As Process = HearthstoneUtil.Process
                    If (p Is Nothing) OrElse (p.HasExited) Then
                        Exit Sub
                    End If

                    NativeMethods.SetForegroundWindow(p.MainWindowHandle)
                    NativeMethods.BringWindowToTop(p.MainWindowHandle)
                    Interaction.AppActivate(p.Id) ' Double ensure the window get visible and focused, because I experienced sometimes it don't get.
                End If

                Bot.Log($"[Matchmaker] -> Match found with '{opponentClass.ToString()}' opponent!")
                Bot.StopBot()
                Bot.Log("[Matchmaker] -> Bot stopped. You can play the game now.")

            End If

            MyBase.OnHandleMulligan(choices, opponentClass, ownClass)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is started.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStarted()
            Me.RunPluginComatibilityCheck()

            Bot.SuspendBot()
            Bot.ChangeMode(Me.DataContainer.Mode)

            If Me.DataContainer.Deck.Equals("None", StringComparison.OrdinalIgnoreCase) Then
                Bot.Log("[Matchmaker] -> Please select a deck.")
            Else
                Bot.ChangeDeck(Me.DataContainer.Deck)
            End If

            Thread.Sleep(2000)
            Bot.ResumeBot()

            Bot.Log($"[Matchmaker] -> Using mode: '{Bot.CurrentMode.ToString()}'")
            Bot.Log($"[Matchmaker] -> Using deck: '{Bot.CurrentDeck.Name}'")

            MyBase.OnStarted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="MatchmakerPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Runs a compatibility check that analyzes the current loaded plugins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub RunPluginComatibilityCheck()

            Dim failTest As Boolean

            ' AdvancedAutoConcede Plugin
            Dim advancedAutoConcedeProps As New Dictionary(Of String, Object)(StringComparer.Ordinal) From {
                {"Author", "ElektroStudios"},
                {"AssemblyName", "AdvancedAutoConcede.dll"},
                {"Enabled", True}
            }
            Dim advancedAutoConcedePlugin As Plugin = SmartBotUtil.FindFirstPluginByPropertyValues(advancedAutoConcedeProps)
            If advancedAutoConcedePlugin IsNot Nothing Then
                Bot.Log($"[Matchmaker] -> This plugin will not work when '{advancedAutoConcedeProps("AssemblyName")}' plugin is enabled.")
                failTest = True
            End If

            ' BountyHunter Plugin
            Dim bountyHunterProps As New Dictionary(Of String, Object) From {
                {"Author", "ElektroStudios"},
                {"AssemblyName", "BountyHunter.dll"},
                {"Enabled", True}
            }
            Dim bountyHunterPlugin As Plugin = SmartBotUtil.FindFirstPluginByPropertyValues(bountyHunterProps)
            If bountyHunterPlugin IsNot Nothing Then
                Bot.Log($"[Matchmaker] -> This plugin will not work when '{bountyHunterProps("AssemblyName")}' plugin is enabled.")
                failTest = True
            End If

            ' Quester Plugin
            Dim questerProps As New Dictionary(Of String, Object) From {
                {"Name", "Quester"},
                {"Enabled", True}
            }
            Dim questerPlugin As Plugin = SmartBotUtil.FindFirstPluginByPropertyValues(questerProps)
            If questerPlugin IsNot Nothing Then
                Bot.Log($"[Matchmaker] -> This plugin will not work when '{questerProps("Name")}' plugin is enabled.")
                failTest = True
            End If

            ' AdvancedQuester Plugin
            Dim advancedQuesterProps As New Dictionary(Of String, Object) From {
                {"Name", "AdvancedQuester"},
                {"Enabled", True}
            }
            Dim advancedQuesterPlugin As Plugin = SmartBotUtil.FindFirstPluginByPropertyValues(advancedQuesterProps)
            If advancedQuesterPlugin IsNot Nothing Then
                Bot.Log($"[Matchmaker] -> This plugin will not work when '{advancedQuesterProps("Name")}' plugin is enabled.")
                failTest = True
            End If

            If failTest Then
                Me.DataContainer.Enabled = False
                Me.OnDataContainerUpdated()
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the allowed classes to match with.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Function GetAllowedClasses() As ReadOnlyCollection(Of Card.CClass)

            Dim l As New List(Of Card.CClass)

            If Me.DataContainer.AllowDemonHunter Then
                l.Add(Card.CClass.DEMONHUNTER)
            End If

            If Me.DataContainer.AllowDruid Then
                l.Add(Card.CClass.DRUID)
            End If

            If Me.DataContainer.AllowHunter Then
                l.Add(Card.CClass.HUNTER)
            End If

            If Me.DataContainer.AllowMage Then
                l.Add(Card.CClass.MAGE)
            End If

            If Me.DataContainer.AllowPaladin Then
                l.Add(Card.CClass.PALADIN)
            End If

            If Me.DataContainer.AllowPriest Then
                l.Add(Card.CClass.PRIEST)
            End If

            If Me.DataContainer.AllowRogue Then
                l.Add(Card.CClass.ROGUE)
            End If

            If Me.DataContainer.AllowShaman Then
                l.Add(Card.CClass.SHAMAN)
            End If

            If Me.DataContainer.AllowWarlock Then
                l.Add(Card.CClass.WARLOCK)
            End If

            If Me.DataContainer.AllowWarrior Then
                l.Add(Card.CClass.WARRIOR)
            End If

            Return l.AsReadOnly()

        End Function

#End Region

    End Class

End Namespace

#End Region
