#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualBasic.ApplicationServices

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Reflection

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API

Imports Xceed.Wpf.Toolkit.PropertyGrid.Attributes

#End Region

#Region " MyPluginData "

Namespace PluginTemplate

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Plugin data for <see cref="MyPlugin"/> plugin class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso cref="PluginDataContainer"/>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <DebuggerNonUserCode>
    Public NotInheritable Class MyPluginData : Inherits PluginDataContainer

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
        ''' Gets the assembly name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Plugin")>
        <DisplayName("Assembly Name")>
        Public ReadOnly Property AssemblyName As String
            Get
                Return Path.ChangeExtension(Me.AssemblyInfo.AssemblyName, "dll")
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
                Return Me.AssemblyInfo.Description
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

        <Browsable(False)>
        Public Shadows Property Name As String

#End Region

#Region " Settings "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value that determine whether... { add description here }
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <Category("Settings")>
        <DisplayName("A test property")>
        <Browsable(True)>
        <ItemsSource(GetType(MyPluginDataSource))>
        Public Property TestProperty As Bot.Mode

#End Region

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="MyPluginData"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.Name = Me.AssemblyInfo.AssemblyName
        End Sub

#End Region

    End Class

End Namespace

#End Region
