#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Runtime.CompilerServices

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBot.Plugins.API.Bot

#End Region

#Region " MyPlugin "

' ReSharper disable once CheckNamespace

Namespace PluginTemplate

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin template for SmartBot.
    ''' </summary>
    ''' <example></example>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class MyPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As MyPluginData
            Get
                Return DirectCast(MyBase.DataContainer, MyPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A <see cref="Boolean"/> flag for testing purposes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly testFlag As Boolean

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="MyPlugin"/> class.
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
        ''' Called when this <see cref="MyPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.LogMethodName()
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot injects Hearthstone process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnInjection()
            Me.LogMethodName()
            MyBase.OnInjection()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is started.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStarted()
            Me.LogMethodName()
            MyBase.OnStarted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot is stopped.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnStopped()
            Me.LogMethodName()
            MyBase.OnStopped()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot timer is ticked, every 300 milliseconds.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTick()
            Me.LogMethodName()
            MyBase.OnTick()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameBegin()
            Me.LogMethodName()
            MyBase.OnGameBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameEnd()
            Me.LogMethodName()
            MyBase.OnGameEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a turn begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTurnBegin()
            Me.LogMethodName()
            MyBase.OnTurnBegin()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a turn ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnTurnEnd()
            Me.LogMethodName()
            MyBase.OnTurnEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot determines the hero has a lethal move to perform in the current turn.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnLethal()
            Me.LogMethodName()
            MyBase.OnLethal()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when our hero is defeated by the opponent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDefeat()
            Me.LogMethodName()
            MyBase.OnDefeat()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when our hero wins the opponent.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnVictory()
            Me.LogMethodName()
            MyBase.OnVictory()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a game is conceded by our hero.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnConcede()
            Me.LogMethodName()
            MyBase.OnConcede()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the results from a simulation are received.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="actions">
        ''' The source <see cref="List(Of Actions.Action)"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <seealso cref="Plugin.OnSimulation()"/>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnActionStackReceived(actions As List(Of Actions.Action))
            Me.LogMethodName()
            MyBase.OnActionStackReceived(actions)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot executes an action ingame.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="action">
        ''' The source <see cref="Actions.Action"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnActionExecute(action As Actions.Action)
            Me.LogMethodName()
            MyBase.OnActionExecute(action)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called at the beginning of each simulations.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnSimulation()
            Me.LogMethodName()
            MyBase.OnSimulation()
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
            Me.LogMethodName()
            MyBase.OnHandleMulligan(choices, opponentClass, ownClass)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot finishes to do mulligan before a game begins.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="replacedCards">
        ''' The cards that were replaced during mulligan.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnMulliganCardsReplaced(ByVal replacedCards As List(Of Card.Cards))
            Me.LogMethodName()
            MyBase.OnMulliganCardsReplaced(replacedCards)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a daily quest is completed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnQuestCompleted()
            Me.LogMethodName()
            MyBase.OnQuestCompleted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when all daily quests are completed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnAllQuestsCompleted()
            Me.LogMethodName()
            MyBase.OnAllQuestsCompleted()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when there's not enough gold to buy a new Arena ticket.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnArenaTicketPurchaseFailed()
            Me.LogMethodName()
            MyBase.OnArenaTicketPurchaseFailed()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when <see cref="Mode.ArenaAuto"/> mode ends.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnArenaEnd()
            Me.LogMethodName()
            MyBase.OnArenaEnd()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the opponent send us a emote.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="emoteType">
        ''' The emote received.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnReceivedEmote(ByVal emoteType As EmoteType)
            Me.LogMethodName()
            MyBase.OnReceivedEmote(emoteType)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when a friend send us a chat message.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="[friend]">
        ''' The source <see cref="[Friend]"/>.
        ''' </param>
        ''' 
        ''' <param name="message">
        ''' The message received.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnWhisperReceived(ByVal [friend] As [Friend], ByVal message As String)
            Me.LogMethodName()
            MyBase.OnWhisperReceived([friend], message)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when you get a new friend request.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="request">
        ''' The source <see cref="FriendRequest"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnFriendRequestReceived(ByVal request As FriendRequest)
            Me.LogMethodName()
            MyBase.OnFriendRequestReceived(request)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when you accept a friend request.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="[friend]">
        ''' The source <see cref="[Friend]"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnFriendRequestAccepted(ByVal [friend] As [Friend])
            Me.LogMethodName()
            MyBase.OnFriendRequestAccepted([friend])
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the Hearthstone's main window reolution is changed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="width">
        ''' The new resolution width.
        ''' </param>
        ''' 
        ''' <param name="height">
        ''' The new resolution height.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGameResolutionUpdate(ByVal width As Integer, ByVal height As Integer)
            Me.LogMethodName()
            MyBase.OnGameResolutionUpdate(width, height)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the current gold amount is changed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnGoldAmountChanged()
            Me.LogMethodName()
            MyBase.OnGoldAmountChanged()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="MyPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Me.LogMethodName()
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the Global.System.Resources.used by this <see cref="MyPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            Me.LogMethodName()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Writes a log message with the name of the method that triggered.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="methodName">
        ''' The name of the method that triggered.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Sub LogMethodName(<CallerMemberName> ByVal Optional methodName As String = "")
            If (Me.DataContainer.Enabled) Then
                Bot.Log($"[{NameOf(MyPlugin)}] -> {methodName}")
            End If
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Tests the specified value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="value">
        ''' A <see cref="Boolean"/> value.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting value.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepperBoundary>
        Private Function TestFunction(ByVal value As Boolean) As Boolean
            Return value
        End Function

#End Region

    End Class

End Namespace

#End Region
