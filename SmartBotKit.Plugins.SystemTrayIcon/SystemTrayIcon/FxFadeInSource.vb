
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Interop.Win32

#End Region

#Region " FxFadeInSource "

Namespace SystemTrayIcon

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides the collection of values represented by a ComboBox for the <see cref="SystemTrayIconPluginData.FxFadeIn"/> property.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="IItemsSource"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class FxFadeInSource : Implements IItemsSource

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="FxFadeInSource"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Throw New NotImplementedException("Fade-In/Fade-Out feature is not yet implemented.")
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
            Dim collection As New ItemCollection From {
                WindowAnimation.None
            }

            For Each animation As WindowAnimation In [Enum].GetValues(GetType(WindowAnimation))
                If (animation.ToString().StartsWith("Show", StringComparison.OrdinalIgnoreCase)) Then
                    collection.Add(animation)
                End If
            Next animation

            Return collection
        End Function

#End Region

    End Class

End Namespace

#End Region
