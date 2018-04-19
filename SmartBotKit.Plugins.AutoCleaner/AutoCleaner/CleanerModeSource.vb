
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

#End Region

#Region " CleanerMode "

Namespace AutoCleaner

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides the collection of values represented by a ComboBox for the <see cref="AutoCleanerPluginData.CleanerMode"/> property.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="IItemsSource"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class CleanerModeSource : Implements IItemsSource

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="CleanerModeSource"/> class.
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
            Dim collection As New ItemCollection()

            For Each mode As CleanerMode In [Enum].GetValues(GetType(CleanerMode))
                collection.Add(mode)
            Next mode

            Return collection
        End Function

#End Region

    End Class

End Namespace

#End Region
