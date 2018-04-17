
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Reflection

Imports SmartBot.Plugins
Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

Imports SmartBotKit.Interop.Win32

#End Region

#Region " SystemTrayIconPluginData "

Namespace SystemTrayIcon

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="SystemTrayIconPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class SystemTrayIconPluginData : Inherits PluginDataContainer

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the animation effect to use when displaying the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Browsable(False)>
        <Category("Fx")>
        <DisplayName("The animation effect to use when displaying the SmartBot window.")>
        <ItemsSource(GetType(FxFadeInSource))>
        Public Property FxFadeIn As WindowAnimation
            Get
                Return WindowAnimation.None
            End Get
            Set(value As WindowAnimation)
                Throw New NotImplementedException("Fade-In/Fade-Out feature is not yet implemented.")
            End Set
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the animation effect to use when hidding the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Browsable(False)>
        <Category("Fx")>
        <DisplayName("The animation effect to use when hidding the SmartBot window.")>
        <ItemsSource(GetType(FxFadeOutSource))>
        Public Property FxFadeOut As WindowAnimation
            Get
                Return WindowAnimation.None
            End Get
            Set(value As WindowAnimation)
                Throw New NotImplementedException("Fade-In/Fade-Out feature is not yet implemented.")
            End Set
        End Property

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="SystemTrayIconPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            Me.Name = Assembly.GetExecutingAssembly().GetName().Name
        End Sub

#End Region

    End Class

End Namespace

#End Region
