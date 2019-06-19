#Region " Imports "

Imports System.Reflection
Imports System.Windows.Forms

Imports SmartBotKitUpdater.GitHub

#End Region

Public Module Updater

    ' ReSharper disable once UnusedMember.Local
    Private ReadOnly CurrentVersion As New Version("1.7.0.0")

    Public Sub Main()
        Updater.CheckForSmartBotKitUpdates()
    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Checks for SmartBotKit available updates on GitHub.
    ''' <para></para>
    ''' If a update is detected, a <see cref="MessageBox"/> will be shown to ask the user navigate to the GitHub repository.
    ''' <para></para>
    ''' Note: the usage of this method is reserved by SmartBotKit plugins, don't use it by yourself.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public Sub CheckForSmartBotKitUpdates()

        Dim startupDir As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        Dim coreAssemblyPath As String = Path.Combine(startupDir, "SmartBotKit.Core.dll")

        If Not File.Exists(coreAssemblyPath) Then
            Console.WriteLine("Can't find SmartBotKit.Core.dll file. Program is exiting now...")
            Environment.Exit(1)
        End If

        Dim ass As Assembly = Nothing
        Try
            ass = Assembly.LoadFile(coreAssemblyPath)
        Catch ex As Exception
            Console.WriteLine("Error while loading SmartBotKit.Core.dll file. Error message: {0}", ex.Message)
            Console.WriteLine("Program is exiting now...")
            Environment.Exit(1)
        End Try

        Dim currentVersion As Version = ass.GetName().Version
        Console.WriteLine("SmartBotKit.Core.dll version: {0}", currentVersion.ToString())

        Dim release As GitHubRelease = GitHubUtil.GetLatestRelease("ElektroStudios", "SmartBotKit")

        If (release.Version > currentVersion) Then
            Console.WriteLine("New update is available, version: {0}", release.Version.ToString())

            If (My.Settings.LastDateNotified = Nothing) Then
                My.Settings.LastDateNotified = Date.Now

            ElseIf (My.Settings.LastDateNotified.Subtract(Date.Now).TotalDays > -7) Then ' Notify once every 7 days.
                Exit Sub

            End If

            My.Settings.LastDateNotified = Date.Now

            Dim dlgResult As DialogResult =
                MessageBox.Show("New version of SmartBotKit plugin bundle is available." &
                                Environment.NewLine & Environment.NewLine &
                                "Do you want to open the webbrowser and navigate to the GitHub repository now?." &
                                Environment.NewLine & Environment.NewLine &
                                "( If you decline, this notification will be shown again in the next 7 days. )",
                                "SmartBotKit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If (dlgResult = DialogResult.Yes) Then
                Try
                    Process.Start(release.UriHtml.AbsoluteUri)
                Catch ex As Exception
                End Try
            End If

        Else
            Console.WriteLine("No update is available.")

        End If

        Environment.Exit(0)
    End Sub

End Module
