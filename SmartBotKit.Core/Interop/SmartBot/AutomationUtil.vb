
#Region " Public Members Summary "

#Region " Properties "

' Process As Process
' MainThreadId As Integer
' Statistics As String
' WinsRatio As String

#End Region

#Region " Methods "

' GetAutomationElement(String) As AutomationElement

#End Region

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Windows.Automation

Imports SmartBot.Plugins

Imports SmartBotKit.Interop.Win32

#End Region

#Region " AutomationUtil "

Namespace SmartBotKit.Interop.SmartBot

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides reusable automation utilities for SmartBot.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class AutomationUtil

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
                If (AutomationUtil.processB Is Nothing) OrElse (AutomationUtil.processB.HasExited) Then
                    AutomationUtil.processB = Diagnostics.Process.GetCurrentProcess()
                End If
                ' AutomationUtil.processB.Refresh() ' Refresh window title and main window handle.
                Return AutomationUtil.processB
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' Gets the SmartBot <see cref="Diagnostics.Process"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared processB As Process

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
                Return NativeMethods.GetWindowThreadProcessId(AutomationUtil.processB.MainWindowHandle, New Integer)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the statistics string shown in the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The statistics string shown in the SmartBot window.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Property Statistics As String
            <DebuggerStepThrough>
            Get
                Return AutomationUtil.GetStatisticsString(AutomationUtil.Process)
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
                Return AutomationUtil.GetWinsRatio()
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="AutomationUtil"/> class from being created.
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
            Return AutomationUtil.GetAutomationElement(AutomationUtil.Process, automationId)
        End Function

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the statistics string shown in the SmartBot window.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="process">
        ''' The SmartBot <see cref="Diagnostics.Process"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The statistics string shown in the SmartBot window.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepperBoundary>
        Private Shared Function GetStatisticsString(ByVal process As Process) As String
            Dim element As AutomationElement = AutomationUtil.GetAutomationElement(process, "Statslabel")

            Return element.Current.Name
        End Function

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
                    str = String.Format("{0:F2}%", winsRatio)

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

#End Region

    End Class

End Namespace

#End Region
