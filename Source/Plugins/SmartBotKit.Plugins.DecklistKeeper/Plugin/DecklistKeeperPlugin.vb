
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Linq

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBotKit.Interop
' Imports SmartBotKit.ReservedUse

#End Region

#Region " DecklistKeeperPlugin "

' ReSharper disable once CheckNamespace

Namespace DecklistKeeper

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A plugin that prevents the bot from changing your deck-list.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="Plugin"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class DecklistKeeperPlugin : Inherits Plugin

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
        Public Shadows ReadOnly Property DataContainer As DecklistKeeperPluginData
            Get
                Return DirectCast(MyBase.DataContainer, DecklistKeeperPluginData)
            End Get
        End Property

#End Region

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Keeps track of the last <see cref="DecklistKeeperPluginData.Enabled"/> value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private lastEnabled As Boolean

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="DecklistKeeperPlugin"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.IsDll = True
            ' UpdateUtil.RunUpdaterExecutable()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when this <see cref="DecklistKeeperPlugin"/> is created by the SmartBot plugin manager.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnPluginCreated()
            Me.lastEnabled = Me.DataContainer.Enabled
            If (Me.lastEnabled) Then
                Bot.Log("[Deck-list Keeper] -> Plugin initialized.")
            End If
            MyBase.OnPluginCreated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the properties of <see cref="DecklistKeeperPluginData"/> are updated.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnDataContainerUpdated()
            Dim enabled As Boolean = Me.DataContainer.Enabled
            If (enabled <> Me.lastEnabled) Then
                If (enabled) Then
                    Bot.Log("[Deck-list Keeper] -> Plugin enabled.")
                Else
                    Bot.Log("[Deck-list Keeper] -> Plugin disabled.")
                End If
                Me.lastEnabled = enabled
            End If
            MyBase.OnDataContainerUpdated()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Called when the bot injects Hearthstone process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub OnInjection()
            If Me.DataContainer.Enabled Then
                Me.RestoreSelectedDecks()
            End If
            MyBase.OnInjection()
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
        Public Overrides Sub OnMulliganCardsReplaced(replacedCards As List(Of Card.Cards))
            If Me.DataContainer.Enabled Then
                Me.RestoreSelectedDecks()
            End If
            MyBase.OnMulliganCardsReplaced(replacedCards)
        End Sub

        '''' ----------------------------------------------------------------------------------------------------
        '''' <summary>
        '''' Called when a game begins.
        '''' </summary>
        '''' ----------------------------------------------------------------------------------------------------
        'Public Overrides Sub OnGameBegin()
        '    If Me.DataContainer.Enabled Then
        '        Me.RestoreSelectedDecks()
        '    End If
        '    MyBase.OnGameBegin()
        'End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Releases all the resources used by this <see cref="DecklistKeeperPlugin"/> instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Dispose()
            MyBase.Dispose()
        End Sub

#End Region

#Region " Private Methods "

        Private Sub RestoreSelectedDecks()

            Dim deckNames As String() = {
                    Me.DataContainer.Deck1, Me.DataContainer.Deck2,
                    Me.DataContainer.Deck3, Me.DataContainer.Deck4,
                    Me.DataContainer.Deck5, Me.DataContainer.Deck6,
                    Me.DataContainer.Deck7, Me.DataContainer.Deck8,
                    Me.DataContainer.Deck9, Me.DataContainer.Deck10
                }.Where(Function(name As String) Not String.IsNullOrEmpty(name) AndAlso Not name.Equals("None", StringComparison.OrdinalIgnoreCase)).ToArray()

            If Not deckNames.Any() Then
                Bot.Log("[Deck-list Keeper] -> No decks specified in plugin settings.")
                Exit Sub
            End If

            Dim deckList As New List(Of SmartBot.Plugins.API.Deck)
            For Each deck As Deck In (From d As Deck In Bot.GetDecks() Where deckNames.Contains(d.Name) Select d)
                deckList.Add(deck)
            Next deck

            Dim selectedDeck As Deck = deckList(New Random().Next(0, deckList.Count - 1))
            SmartBotUtil.SafeChangeDeckOrMode(Bot.CurrentMode, selectedDeck.Name)

        End Sub

#End Region

    End Class

End Namespace

#End Region
