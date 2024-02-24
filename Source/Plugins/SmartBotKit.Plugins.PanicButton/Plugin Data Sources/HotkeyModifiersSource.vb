
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.IO

#End Region

#Region " HotkeyModifiersSource "

' ReSharper disable once CheckNamespace

Namespace PanicButton


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides the collection of values represented by a ComboBox for the 
    ''' <see cref="PanicButtonPluginData.ModifierA"/> and <see cref="PanicButtonPluginData.ModifierB"/> properties.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="IItemsSource"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class HotkeyModifiersSource : Implements IItemsSource

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="HotkeyModifiersSource"/> class.
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
            For Each modifierKey As HotkeyModifiers In [Enum].GetValues(GetType(HotkeyModifiers))
                If (modifierKey <> HotkeyModifiers.NoRepeat) Then
                    collection.Add(modifierKey)
                End If
            Next modifierKey
            Return collection

        End Function

#End Region

    End Class

End Namespace

#End Region
