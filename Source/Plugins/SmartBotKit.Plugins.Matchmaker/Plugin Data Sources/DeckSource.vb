﻿
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Linq

Imports SmartBot.Plugins.API

Imports SmartBotKit.Plugins.Matchmaker

Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

#End Region

#Region " DeckSource "

' ReSharper disable once CheckNamespace

Namespace Matchmaker

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides the collection of values represented by a ComboBox for the <see cref="MatchmakerPluginData.Deck"/> property.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="IItemsSource"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class DeckSource : Implements IItemsSource

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="DeckSource"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the collection of values.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="ItemCollection"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Function GetValues() As ItemCollection Implements IItemsSource.GetValues
            Dim items As New ItemCollection From {
                {"None", "None"}
            }

            For Each deck As Deck In (From d As Deck In Bot.GetDecks())
                items.Add(deck.Name, deck.Name)
            Next deck

            Return items
        End Function

#End Region

    End Class

End Namespace

#End Region
