#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Reflection

Imports SmartBot.Plugins

Imports SmartBotKit.Interop

#End Region

#Region " AutoInjectorPluginData "

' ReSharper disable once CheckNamespace

Namespace AutoInjector

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="AutoInjectorPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class AutoInjectorPluginData : Inherits PluginDataContainer

#Region " Properties "

#Region " Plugin "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="ApplicationServices.AssemblyInfo"/> for this assembly.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Friend ReadOnly Property AssemblyInfo As AssemblyInfo
            Get
                Return New AssemblyInfo(Assembly.GetExecutingAssembly())
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the assembly path.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <Browsable(False)>
        <DisplayName("Assembly Name")>
        Public ReadOnly Property AssemblyName As String
            Get
                Return System.IO.Path.ChangeExtension(Me.AssemblyInfo.AssemblyName, "dll")
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the assembly path.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Path")>
        Public ReadOnly Property Path As String
            Get
                Return $".\{SmartBotUtil.PluginsDir.Name}\{Me.AssemblyName}"
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin title.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("[Title]")>
        Public ReadOnly Property Title As String
            Get
                Return Me.AssemblyInfo.Title
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Name")>
        <Browsable(False)>
        <[ReadOnly](True)>
        Public Shadows ReadOnly Property Name As String
            Get
                Return Me.AssemblyInfo.AssemblyName
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Author")>
        Public ReadOnly Property Author As String
            Get
                Return Me.AssemblyInfo.CompanyName
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the plugin description.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Description")>
        Public ReadOnly Property Description As String
            Get
                Return "Automate SmartBot injection" & ControlChars.NewLine &
                       "when Hearthstone process is detected."
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether the plugin should delete log files.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Version")>
        Public ReadOnly Property Version As String
            Get
                Return Me.AssemblyInfo.Version.ToString()
            End Get
        End Property

#End Region

#Region " Injection "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether the bot should start after injection.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Injection")>
        <DisplayName("Auto-start bot after injected")>
        Public Property AutoStartBotAfterInjected As Boolean

#End Region

#Region " Process Discovery "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the time interval, in seconds, to discover new Hearthstone processes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' A value between 1 and 10.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Process Discovery")>
        <DisplayName("The interval, in seconds, to discover new Hearthstone processes.")>
        <Browsable(True)>
        Public Property ProcessDiscoverInterval() As Integer
            Get
                Return Me.processDiscoverInterval_
            End Get
            Set(ByVal value As Integer)
                If (value < 1) Then
                    Me.processDiscoverInterval_ = 1
                ElseIf value > 10 Then
                    Me.processDiscoverInterval_ = 10
                Else
                    Me.processDiscoverInterval_ = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The time interval, in seconds, to discover new Hearthstone processes.
        ''' </summary>
        Private processDiscoverInterval_ As Integer

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="AutoInjectorPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.Name

            Me.AutoStartBotAfterInjected = False
            Me.processDiscoverInterval_ = 5
        End Sub

#End Region

    End Class

End Namespace

#End Region
