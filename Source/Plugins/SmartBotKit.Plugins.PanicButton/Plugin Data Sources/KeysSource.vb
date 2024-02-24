
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Windows.Forms

Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

#End Region

#Region " KeysSource "

' ReSharper disable once CheckNamespace

Namespace PanicButton


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides the collection of values represented by a ComboBox for the <see cref="PanicButtonPluginData.Key"/> property.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="IItemsSource"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class KeysSource : Implements IItemsSource

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="KeysSource"/> class.
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
            Return New ItemCollection() From {
                {Keys.A},
                {Keys.B},
                {Keys.C},
                {Keys.D},
                {Keys.E},
                {Keys.F},
                {Keys.G},
                {Keys.H},
                {Keys.I},
                {Keys.J},
                {Keys.K},
                {Keys.L},
                {Keys.M},
                {Keys.N},
                {Keys.O},
                {Keys.P},
                {Keys.Q},
                {Keys.R},
                {Keys.S},
                {Keys.T},
                {Keys.U},
                {Keys.V},
                {Keys.W},
                {Keys.X},
                {Keys.Y},
                {Keys.Z},
                {Keys.D0},
                {Keys.D1},
                {Keys.D2},
                {Keys.D3},
                {Keys.D4},
                {Keys.D5},
                {Keys.D6},
                {Keys.D7},
                {Keys.D8},
                {Keys.D9},
                {Keys.NumPad0},
                {Keys.NumPad1},
                {Keys.NumPad2},
                {Keys.NumPad3},
                {Keys.NumPad4},
                {Keys.NumPad5},
                {Keys.NumPad6},
                {Keys.NumPad7},
                {Keys.NumPad8},
                {Keys.NumPad9},
                {Keys.F1},
                {Keys.F2},
                {Keys.F3},
                {Keys.F4},
                {Keys.F5},
                {Keys.F6},
                {Keys.F7},
                {Keys.F8},
                {Keys.F9},
                {Keys.F10},
                {Keys.F11},
                {Keys.F12},
                {Keys.Space},
                {Keys.Up},
                {Keys.Down},
                {Keys.Left},
                {Keys.Right},
                {Keys.Home},
                {Keys.End},
                {Keys.Return},
                {Keys.VolumeDown},
                {Keys.VolumeUp},
                {Keys.VolumeMute}
            }
        End Function

#End Region

    End Class

End Namespace

#End Region
