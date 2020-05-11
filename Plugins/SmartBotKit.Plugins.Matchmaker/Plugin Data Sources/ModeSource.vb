
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports SmartBot.Plugins.API
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

#End Region

#Region " ModeSource "

' ReSharper disable once CheckNamespace

Namespace Matchmaker

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides the collection of values represented by a ComboBox for the <see cref="MatchmakerPluginData.Mode"/> property.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="IItemsSource"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class ModeSource : Implements IItemsSource

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="ModeSource"/> class.
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
            Return New ItemCollection From {
                Bot.Mode.RankedStandard,
                Bot.Mode.UnrankedStandard,
                Bot.Mode.RankedWild,
                Bot.Mode.UnrankedWild
            }
        End Function

#End Region

    End Class

End Namespace

#End Region
