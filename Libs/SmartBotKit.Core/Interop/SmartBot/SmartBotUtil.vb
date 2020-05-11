
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Windows.Automation

Imports SmartBot.Plugins
Imports SmartBot.Plugins.API
Imports SmartBotKit.Interop.Win32

#End Region

#Region " SmartBotUtil "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Interop

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides reusable automation utilities for SmartBot process.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class SmartBotUtil

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the SmartBot <see cref="Diagnostics.Process"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The SmartBot <see cref="Diagnostics.Process"/>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property Process As Process
            <DebuggerStepThrough>
            Get
                If (SmartBotUtil._processB Is Nothing) OrElse (SmartBotUtil._processB.HasExited) Then
                    SmartBotUtil._processB = Process.GetCurrentProcess()
                End If
                ' SmartBotUtil.processB.Refresh() ' Refresh window title and main window handle.
                Return SmartBotUtil._processB
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' Gets the SmartBot <see cref="Diagnostics.Process"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared _processB As Process

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value indicating whether the SmartBot process is displaying the 'Loading' splash screen.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' <see langword="True"/> if SmartBot window is displaying the splashscreen; otherwise, <see langword="False"/>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property IsInSplashScreen As Boolean
            Get
                Return (Process.GetCurrentProcess().MainWindowTitle.StartsWith("Loading", StringComparison.OrdinalIgnoreCase))
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets identifier of the thread that created the SmartBot main window; the UI thread.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The identifier of the thread that created the SmartBot main window; the UI thread.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property MainThreadId As Integer
            <DebuggerStepThrough>
            Get
                Return NativeMethods.GetWindowThreadProcessId(SmartBotUtil.Process.MainWindowHandle, New Integer)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="AutomationElement"/> that represents the 'TextBoxLog' control.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property UiElementTextBoxLog As AutomationElement
            Get
                Return SmartBotUtil.GetAutomationElement(SmartBotUtil.Process, "TextBoxLog")
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="AutomationElement"/> that represents the 'Statslabel' control.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property UiElementStatsLabel As AutomationElement
            Get
                Return SmartBotUtil.GetAutomationElement(SmartBotUtil.Process, "Statslabel")
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the text of the 'Statslabel' control.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property StatsLabelText As String
            <DebuggerStepThrough>
            Get
                Return SmartBotUtil.UiElementStatsLabel.Current.Name
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the text of the 'TextBoxLog' control.
        ''' <para></para>
        ''' Note that a call to <see cref="SmartBotUtil.TextBoxLogText"/> property will throw a
        ''' <see cref="NullReferenceException"/> exception if the 'TextBoxLog' control is not visible in the UI.
        ''' That is, if the 'Missplays', 'Changelog' or 'Debug' tab is the active one.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property TextBoxLogText As String
            <DebuggerStepThrough>
            Get
                Dim pattern As TextPattern = DirectCast(SmartBotUtil.UiElementTextBoxLog.GetCurrentPattern(TextPattern.Pattern), TextPattern)
                Return pattern.DocumentRange.GetText(Integer.MaxValue)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the content of the current logfile shown in SmartBot UI.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property CurrentLogContent As IEnumerable(Of String)
            Get
                Dim files As IEnumerable(Of FileInfo) = SmartBotUtil.LogsDir.GetFiles("*.txt", SearchOption.TopDirectoryOnly)
                Dim recentFile As FileInfo = (From fi As FileInfo In files
                                              Where fi.Name Like "#*-#*-####_#*-#*-#*_??.???"
                                              Order By fi.LastWriteTime Descending
                                             ).FirstOrDefault()

                If (recentFile Is Nothing) Then
                    Return Nothing ' Enumerable.Empty(Of String)
                End If

                Dim lines As IEnumerable(Of String)
                Try
                    lines = File.ReadLines(recentFile.FullName)

                Catch ex As IOException ' Unable to read log file because SmartBot has open it. This will occur at very specific circumstances.
                    ' Try to copy the file to a new location so we can finally read it.
                    Dim tmpFullPath As String = Path.Combine(Path.GetTempPath(), Path.GetTempFileName())
                    Try
                        My.Computer.FileSystem.CopyFile(recentFile.FullName, tmpFullPath, overwrite:=True)
                        lines = File.ReadLines(tmpFullPath)
                    Catch ' In case it also fails to copy or read, return nothing.
                        Return Nothing
                    End Try

                Catch ex As Exception
                    Throw

                End Try

                Return lines
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the last logfile line shown in SmartBot UI.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property LastLogLine As String
            Get
                Return SmartBotUtil.CurrentLogContent?.LastOrDefault()
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a <see cref="TimeSpan"/> that represents the exact hour that the SmartBot's server was down for last time.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property LastServerDownRecord As TimeSpan
            Get
                Dim serverIsDownString As String = "Board request sent for more than"

                Try
                    Dim lines As IEnumerable(Of String) = SmartBotUtil.CurrentLogContent
                    For Each line As String In lines?.Reverse()
                        If line.Contains(serverIsDownString) Then
                            Return TimeSpan.Parse(line.Substring(0, 10).TrimStart("["c).TrimEnd("]"c))
                        End If
                    Next
                Catch

                End Try

                Return Nothing
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current wins ratio percentage string.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The wins ratio percentage string.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property WinsRatio As String
            <DebuggerStepThrough>
            Get
                Return SmartBotUtil.GetWinsRatio()
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot window placement.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The Hearthstone window placement.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Property WindowPlacement As WindowPlacement
            <DebuggerStepThrough>
            Get
                Return SmartBotUtil.GetWindowPlacement(SmartBotUtil.Process.MainWindowHandle)
            End Get
            Set(value As WindowPlacement)
                Dim wpl As WindowPlacement = SmartBotUtil.GetWindowPlacement(SmartBotUtil.Process.MainWindowHandle)
                If (wpl.NormalPosition <> CType(value.NormalPosition, Rectangle)) OrElse
                   (wpl.WindowState <> value.WindowState) OrElse
                   (wpl.Flags <> value.Flags) OrElse
                   (wpl.MaxPosition <> CType(value.MaxPosition, Point)) OrElse
                   (wpl.MinPosition <> CType(value.MinPosition, Point)) Then

                    SmartBotUtil.SetWindowPlacement(SmartBotUtil.Process.MainWindowHandle, value)
                End If
            End Set
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot window position.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The Hearthstone window position.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Property WindowPosition As Point
            <DebuggerStepThrough>
            Get
                Return SmartBotUtil.GetWindowPosition(SmartBotUtil.Process.MainWindowHandle)
            End Get
            Set(value As Point)
                If (SmartBotUtil.GetWindowPosition(SmartBotUtil.Process.MainWindowHandle) <> value) Then
                    SmartBotUtil.SetWindowPosition(SmartBotUtil.Process.MainWindowHandle, value)
                End If
            End Set
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the SmartBot window size.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The Hearthstone window size.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Property WindowSize As Size
            <DebuggerStepThrough>
            Get
                Return SmartBotUtil.GetWindowSize(SmartBotUtil.Process.MainWindowHandle)
            End Get
            Set(value As Size)
                If (SmartBotUtil.GetWindowSize(SmartBotUtil.Process.MainWindowHandle) <> value) Then
                    SmartBotUtil.SetWindowSize(SmartBotUtil.Process.MainWindowHandle, value)
                End If
            End Set
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the directory where the SmartBot plugins are stored.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property PluginsDir As DirectoryInfo
            <DebuggerStepThrough>
            Get
                Return New DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the main directory of SmartBot.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property SmartBotDir As DirectoryInfo
            <DebuggerStepThrough>
            Get
                Return SmartBotUtil.PluginsDir.Parent
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the directory where the SmartBot crash logs are stored.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property CrashesDir As DirectoryInfo
            <DebuggerStepThrough>
            Get
                Return New DirectoryInfo(Path.Combine(SmartBotUtil.SmartBotDir.FullName, "Crashs"))
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the directory where the SmartBot logs are stored.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property LogsDir As DirectoryInfo
            <DebuggerStepThrough>
            Get
                Return New DirectoryInfo(Path.Combine(SmartBotUtil.SmartBotDir.FullName, "Logs"))
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the directory where the SmartBot seeds are stored.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property SeedsDir As DirectoryInfo
            <DebuggerStepThrough>
            Get
                Return New DirectoryInfo(Path.Combine(SmartBotUtil.SmartBotDir.FullName, "Seeds"))
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the directory where the SmartBot screenshots are stored.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property ScreenshotsDir As DirectoryInfo
            <DebuggerStepThrough>
            Get
                Return New DirectoryInfo(Path.Combine(SmartBotUtil.SmartBotDir.FullName, "Screenshots"))
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="SmartBotUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="AutomationElement"/> that has the specified automation id. in the SmartBot main window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="AutomationElement"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetAutomationElement(ByVal automationId As String) As AutomationElement
            Return SmartBotUtil.GetAutomationElement(SmartBotUtil.Process, automationId)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Ensures that SmartBot changes the specified mode or deck in Hearthstone.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="botMode">
        ''' The new bot mode.
        ''' </param>
        ''' 
        ''' <param name="deckName">
        ''' The new deck name.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Sub SafeChangeDeckOrMode(Optional botMode As Bot.Mode = Bot.Mode.None,
                                               Optional deckName As String = "")

            Dim currentMode As Bot.Mode = Bot.CurrentMode
            Dim currentDeckName As String = Bot.CurrentDeck?.Name

            If (botMode = Bot.Mode.None) Then
                botMode = currentMode
            End If

            If (deckName = "") Then
                deckName = currentDeckName
            End If

            If (currentMode <> botMode) OrElse (currentDeckName <> deckName) Then
                Bot.SuspendBot()

                Bot.ChangeMode(botMode)
                Bot.ChangeDeck(deckName)

                Thread.Sleep(2000)
                Bot.ResumeBot()
            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Finds and returns a running <see cref="Plugin"/> that matches 
        ''' the specified "Name" property value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim questerPlugin As Plugin = FindPluginByName("Quester", StringComparison.OrdinalIgnoreCase)
        ''' If questerPlugin IsNot Nothing Then
        '''     MsgBox("Plugin found.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The return value is a running <see cref="Plugin"/> that matches the specified "Name" property value, 
        ''' otherwise, it returns <see langword="Nothing"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function FindPluginByName(ByVal name As String, Optional stringComparison As StringComparison = StringComparison.Ordinal) As Plugin

            If String.IsNullOrWhiteSpace(name) Then
                Throw New ArgumentNullException(paramName:=NameOf(name))
            End If

            Dim plugins As List(Of Plugin) = Bot.GetPlugins()
            For Each plug As Plugin In plugins
                If plug.DataContainer.Name.Equals(name, stringComparison) Then
                    Return plug
                End If
            Next plug

            Return Nothing

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Finds and returns the first occurrence of a running <see cref="Plugin"/> that matches 
        ''' all the given property names.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim plug As Plugin = FindFirstPluginByPropertyNames("PropertyName1", "PropertyName2", "PropertyName3")
        ''' If plug IsNot Nothing Then
        '''     MsgBox("Plugin found.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The return value is the first occurrence of a running <see cref="Plugin"/> that matches 
        ''' all the given property names, 
        ''' otherwise, it returns <see langword="Nothing"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function FindFirstPluginByPropertyNames(ParamArray propertyNames As String()) As Plugin

            If (propertyNames.Count = 0) Then
                Throw New ArgumentNullException(paramName:=NameOf(propertyNames))
            End If

            Dim plugins As List(Of Plugin) = Bot.GetPlugins()
            For Each plug As Plugin In plugins
                Dim keys As Dictionary(Of String, Object).KeyCollection

                Try
                    keys = plug.GetProperties().Keys

                Catch ex As Exception
                    Throw

                End Try

                If propertyNames.All(Function(propertyName As String)
                                         Return Keys.Contains(propertyName)
                                     End Function) Then
                    Return plug
                End If
            Next plug

            Return Nothing
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Finds and returns all the occurrences of running <see cref="Plugin"/> objects that matches 
        ''' all the given property names.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim plugins As IEnumerable(Of Plugin) = FindPluginsByPropertyNames("PropertyName1", "PropertyName2", "PropertyName3")
        ''' 
        ''' If plugins IsNot Nothing Then
        '''     For Each plug As Plugin In plugins
        '''         ' ...
        '''     Next plug
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The return value is all the occurrences of running <see cref="Plugin"/> objects that matches 
        ''' all the given property names, 
        ''' otherwise, it returns <see langword="Nothing"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Iterator Function FindPluginsByPropertyNames(ParamArray propertyNames As String()) As IEnumerable(Of Plugin)

            If (propertyNames.Count = 0) Then
                Throw New ArgumentNullException(paramName:=NameOf(propertyNames))
            End If

            Dim plugins As List(Of Plugin) = Bot.GetPlugins()
            For Each plug As Plugin In plugins
                Dim keys As Dictionary(Of String, Object).KeyCollection

                Try
                    keys = plug.GetProperties().Keys

                Catch ex As Exception
                    Throw

                End Try

                If propertyNames.All(Function(propertyName As String)
                                         Return keys.Contains(propertyName)
                                     End Function) Then
                    Yield plug
                End If
            Next plug

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Finds and returns the first occurrence of a running <see cref="Plugin"/> that matches 
        ''' all the given property names with their specified values.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim properties As New Dictionary(Of String, Object)(StringComparer.Ordinal) From {
        '''     {"Author", "ElektroStudios"},
        '''     {"Path", ".\Plugins\BountyHunter.dll"},
        '''     {"Enabled", True}
        ''' }
        ''' 
        ''' Dim plug As Plugin = FindFirstPluginByPropertyValues(properties)
        ''' If plug IsNot Nothing Then
        '''     MsgBox("Plugin found.")
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The return value is a the first occurrence of a running <see cref="Plugin"/> that matches 
        ''' all the given property names with their specified values, 
        ''' otherwise, it returns <see langword="Nothing"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function FindFirstPluginByPropertyValues(properties As Dictionary(Of String, Object)) As Plugin

            Dim plugins As List(Of Plugin) = Bot.GetPlugins()
            For Each plug As Plugin In plugins

                Dim plugProps As Dictionary(Of String, Object) = plug.GetProperties()
                If properties.All(Function(prop As KeyValuePair(Of String, Object))
                                      Return (plugProps.ContainsKey(prop.Key) AndAlso
                                              plugProps(prop.Key).Equals(prop.Value))
                                  End Function) Then
                    Return plug
                End If

            Next plug

            Return Nothing

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Finds and returns all the occurrences of running <see cref="Plugin"/> objects that matches 
        ''' all the given property names with their specified values.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim properties As New Dictionary(Of String, Object)(StringComparer.Ordinal) From {
        '''     {"Author", "ElektroStudios"},
        '''     {"Path", ".\Plugins\BountyHunter.dll"},
        '''     {"Enabled", True}
        ''' }
        ''' 
        ''' Dim plugins As IEnumerable(Of Plugin) = FindPluginsByPropertyValues(properties)
        ''' 
        ''' If plugins IsNot Nothing Then
        '''     For Each plug As Plugin In plugins
        '''         ' ...
        '''     Next plug
        ''' End If
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The return value is a all the occurrences of running <see cref="Plugin"/> objects that matches 
        ''' all the given property names with their specified values, 
        ''' otherwise, it returns <see langword="Nothing"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Iterator Function FindPluginsByPropertyValues(properties As Dictionary(Of String, Object)) As IEnumerable(Of Plugin)

            Dim plugins As List(Of Plugin) = Bot.GetPlugins()
            For Each plug As Plugin In plugins

                Dim plugProps As Dictionary(Of String, Object) = plug.GetProperties()
                If properties.All(Function(prop As KeyValuePair(Of String, Object))
                                      Return (plugProps.ContainsKey(prop.Key) AndAlso
                                              plugProps(prop.Key).Equals(prop.Value))
                                  End Function) Then
                    Yield plug
                End If

            Next plug

        End Function

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current wins ratio percentage string.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The wins ratio percentage string.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Shared Function GetWinsRatio() As String
            Dim wins As Integer = API.Statistics.Wins
            Dim losses As Integer = API.Statistics.Losses
            Dim concedes As Integer = API.Statistics.ConcededTotal
            Dim winsRatio As Double = ((wins / (wins + losses + concedes)) * 100)

            If Double.IsNaN(winsRatio) Then
                winsRatio = 0R
            End If

            Dim str As String
            Select Case winsRatio

                Case = 0
                    str = "0%"

                Case = 100.0R
                    str = "100%"

                Case Else
                    str = $"{winsRatio:F2}%"

            End Select

            Return str
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="AutomationElement"/> that has the specified automation id. in the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="process">
        ''' The SmartBot <see cref="Diagnostics.Process"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="AutomationElement"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Shared Function GetAutomationElement(ByVal process As Process, ByVal automationId As String) As AutomationElement
            Dim window As AutomationElement = AutomationElement.FromHandle(process.MainWindowHandle)
            Dim condition As New PropertyCondition(AutomationElement.AutomationIdProperty, automationId)
            Dim element As AutomationElement = window.FindFirst(TreeScope.Subtree, condition)

            Return element
        End Function

        Private Shared Function GetWindowPlacement(ByVal hWnd As IntPtr) As WindowPlacement
            Dim wpl As New WindowPlacement()
            wpl.Length = Marshal.SizeOf(wpl)
            NativeMethods.GetWindowPlacement(hWnd, wpl)
            Return wpl
        End Function

        Private Shared Function SetWindowPlacement(ByVal hWnd As IntPtr, ByVal wpl As WindowPlacement) As Boolean
            Return NativeMethods.SetWindowPlacement(hWnd, wpl)
        End Function

        Private Shared Function GetWindowSize(ByVal hWnd As IntPtr) As Size
            Dim rc As Rectangle
            NativeMethods.GetWindowRect(hWnd, rc)
            Return rc.Size
        End Function

        Private Shared Function SetWindowSize(ByVal hWnd As IntPtr, ByVal sz As Size) As Boolean
            Dim rc As Rectangle
            NativeMethods.GetWindowRect(hWnd, rc)
            Return NativeMethods.SetWindowPos(hWnd, IntPtr.Zero,
                                              rc.Location.X, rc.Location.Y,
                                              sz.Width, sz.Height,
                                              SetWindowPosFlags.IgnoreMove)
        End Function

        Private Shared Function GetWindowPosition(ByVal hWnd As IntPtr) As Point
            Dim rc As Rectangle
            NativeMethods.GetWindowRect(hWnd, rc)
            Return rc.Location
        End Function

        Private Shared Function SetWindowPosition(ByVal hWnd As IntPtr, ByVal pt As Point) As Boolean
            Dim rc As Rectangle
            NativeMethods.GetWindowRect(hWnd, rc)
            Return NativeMethods.SetWindowPos(hWnd, IntPtr.Zero,
                                              pt.X, pt.Y,
                                              rc.Size.Width, rc.Size.Height,
                                              SetWindowPosFlags.IgnoreResize)
        End Function

#End Region

    End Class

End Namespace

#End Region
