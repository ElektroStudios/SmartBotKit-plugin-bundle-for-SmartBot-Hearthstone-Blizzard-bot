'Imports System.Net
'Imports System.Reflection
'Imports SmartBotKit.NET.GitHub

'Namespace SmartBotKit.ReservedUse

'    ''' ----------------------------------------------------------------------------------------------------
'    ''' <summary>
'    ''' Provides utilities for version update check of SmartBotKit plugin bundle.
'    ''' <para></para>
'    ''' Note: the usage of this class is reserved by SmartBotKit plugins, don't use it by yourself.
'    ''' </summary>
'    ''' ----------------------------------------------------------------------------------------------------
'    Public NotInheritable Class UpdateUtil

'#Region " Fields "

'        ''' ----------------------------------------------------------------------------------------------------
'        ''' <summary>
'        ''' Flag to prevent redundant calls from multiple plugins checking for updates.
'        ''' <para></para>
'        ''' Note: the usage of this field is reserved by SmartBotKit plugins, don't use it by yourself.
'        ''' </summary>
'        ''' ----------------------------------------------------------------------------------------------------
'        Public Shared IsUpdateChecked As Boolean

'#End Region

'#Region " Constructors "

'        ''' ----------------------------------------------------------------------------------------------------
'        ''' <summary>
'        ''' Prevents a default instance of the <see cref="UpdateUtil"/> class from being created.
'        ''' <para></para>
'        ''' Note: the usage of this class is reserved by SmartBotKit plugins, don't use it by yourself.
'        ''' </summary>
'        ''' ----------------------------------------------------------------------------------------------------
'        <DebuggerNonUserCode>
'        Private Sub New()
'        End Sub

'#End Region

'#Region " Public Methods "

'        ''' ----------------------------------------------------------------------------------------------------
'        ''' <summary>
'        ''' Checks for SmartBotKit available updates on GitHub.
'        ''' <para></para>
'        ''' If a update is detected, a <see cref="MessageBox"/> will be shown to ask the user navigate to the GitHub repository.
'        ''' <para></para>
'        ''' Note: the usage of this method is reserved by SmartBotKit plugins, don't use it by yourself.
'        ''' </summary>
'        ''' ----------------------------------------------------------------------------------------------------
'        Public Shared Async Sub CheckForSmartBotKitUpdatesAsync()
'            Throw New NotImplementedException("This method is not yet finished. Im experiencing SSL/TLS issues to be able connect to the GitHub API from SmartBot process.")

'            If (UpdateUtil.IsUpdateChecked) Then
'                Exit Sub
'            Else
'                UpdateUtil.IsUpdateChecked = True
'            End If

'            Dim release As GitHubRelease = Await GitHubUtil.GetLatestReleaseAsync("ElektroStudios", "SmartBotKit")

'            If (release.Version > Assembly.GetExecutingAssembly().GetName().Version) Then

'                If (My.Settings.LastDateNotified = Nothing) Then
'                    My.Settings.LastDateNotified = Date.Now

'                ElseIf (My.Settings.LastDateNotified.Subtract(Date.Now).TotalDays > -7) Then ' Notify once every 7 days.
'                    Exit Sub

'                End If

'                My.Settings.LastDateNotified = Date.Now

'                Dim dlgResult As DialogResult =
'                    MessageBox.Show("New version of SmartBotKit plugin bundle is available." &
'                                    Environment.NewLine & Environment.NewLine &
'                                    "Do you want to open the webbrowser and navigate to the GitHub repository now?." &
'                                    Environment.NewLine & Environment.NewLine &
'                                    "( If you decline, this notification will be shown again the next week. )",
'                                    "SmartBotKit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

'                If (dlgResult = DialogResult.Yes) Then
'                    Try
'                        Process.Start(release.UriHtml.AbsoluteUri)
'                    Catch ex As Exception
'                    End Try
'                End If

'            End If

'        End Sub

'#End Region

'    End Class

'End Namespace
