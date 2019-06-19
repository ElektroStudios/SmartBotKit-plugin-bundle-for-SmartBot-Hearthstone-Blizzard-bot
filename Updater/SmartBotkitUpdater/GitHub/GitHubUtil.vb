' ***********************************************************************
' Author   : Elektro
' Modified : 06-May-2018
' ***********************************************************************

#Region " Public Members Summary "

#Region " Functions "

' GetLatestRelease(String, String) As GitHubRelease
' GetLatestReleaseAsync(String, String) As Task(Of GitHubRelease)

' GetRelease(String, String, Version) As GitHubRelease
' GetReleaseAsync(String, String, Version) As Task(Of GitHubRelease)

' GetReleases(String, String) As ReadOnlyCollection(Of GitHubRelease)
' GetReleasesAsync(String, String) As Task(Of ReadOnlyCollection(Of GitHubRelease))

' IsUpdateAvailable(String, String, Version) As Boolean
' IsUpdateAvailableAsync(String, String, Version) As Task(Of Boolean)

#End Region

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Reflection
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web
Imports System.Xml
Imports System.Xml.Linq

#End Region

#Region " GitHub Util "

' ReSharper disable once CheckNamespace

Namespace SmartBotKitUpdater.GitHub


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains GitHub related utilities.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    Public NotInheritable Class GitHubUtil

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="GitHubUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Asynchronously gets the releases from the specified repository on GitHub.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim releases As ReadOnlyCollection(Of GitHubRelease) = Await GetReleasesAsync("ElektroStudios", "SmartBotKit")
        ''' 
        ''' For Each release As GitHubRelease In releases
        '''     Console.WriteLine("RELEASE: {0}", release.ToString())
        ''' 
        '''     For Each asset As GitHubAsset In release.Assets
        '''         Console.WriteLine("ASSET  : {0}", asset.ToString())
        '''     Next asset
        '''     Console.WriteLine()
        ''' Next release
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="userName">
        ''' The user name.
        ''' </param>
        ''' 
        ''' <param name="repositoryName">
        ''' The repository name.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A <see cref="Task(Of ReadOnlyCollection(Of GitHubRelease))"/> containing the releases.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="HttpException">
        ''' JSON validation error.
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Async Function GetReleasesAsync(ByVal userName As String, ByVal repositoryName As String) As Task(Of ReadOnlyCollection(Of GitHubRelease))

            Dim uri As New Uri($"http://api.github.com/repos/{userName}/{repositoryName}/releases", UriKind.Absolute)

            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(uri), HttpWebRequest)
            request.UserAgent = userName

            Using response As WebResponse = Await request.GetResponseAsync(),
              sr As New StreamReader(response.GetResponseStream()),
              xmlReader As XmlDictionaryReader = JsonReaderWriterFactory.CreateJsonReader(sr.BaseStream, Encoding.UTF8, New XmlDictionaryReaderQuotas, Nothing)

                Dim xml As XElement = XElement.Load(xmlReader)
                If (xml.IsEmpty) Then
                    Dim errMsg As String = $"JSON validation error. ""{uri.ToString()}"""
                    Throw New HttpException(HttpStatusCode.NotFound, errMsg)
                End If

                Dim releases As New Collection(Of GitHubRelease)
                Dim elements As IEnumerable(Of XElement) = xml.<item>
                For Each element As XElement In elements
                    Dim release As New GitHubRelease(element)
                    releases.Add(release)
                Next

                Return New ReadOnlyCollection(Of GitHubRelease)(releases)

            End Using

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the releases from the specified repository on GitHub.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim releases As ReadOnlyCollection(Of GitHubRelease) = GetReleases("ElektroStudios", "SmartBotKit")
        ''' 
        ''' For Each release As GitHubRelease In releases
        '''     Console.WriteLine("RELEASE: {0}", release.ToString())
        ''' 
        '''     For Each asset As GitHubAsset In release.Assets
        '''         Console.WriteLine("ASSET  : {0}", asset.ToString())
        '''     Next asset
        '''     Console.WriteLine()
        ''' Next release
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="userName">
        ''' The user name.
        ''' </param>
        ''' 
        ''' <param name="repositoryName">
        ''' The repository name.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A <see cref="ReadOnlyCollection(Of GitHubRelease)"/> collection containing the releases.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function GetReleases(ByVal userName As String, ByVal repositoryName As String) As ReadOnlyCollection(Of GitHubRelease)

            Dim t As Task(Of ReadOnlyCollection(Of GitHubRelease)) = Task.Run(Function() GitHubUtil.GetReleases(userName, repositoryName))
            t.Wait(Timeout.Infinite)

            Return t.Result

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Asynchronously gets a release that matches the specified version from the specified repository on GitHub.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim release As GitHubRelease = Await GetReleaseAsync("ElektroStudios", "SmartBotKit", New Version("1.3"))
        ''' Console.WriteLine("RELEASE: {0}", release.ToString())
        ''' Console.WriteLine()
        ''' 
        ''' Console.WriteLine("BODY:")
        ''' Console.WriteLine(release.Body)
        ''' Console.WriteLine()
        ''' 
        ''' For Each asset As GitHubAsset In release.Assets
        '''     Console.WriteLine("ASSET: {0}", asset.ToString())
        ''' Next asset
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="userName">
        ''' The user name.
        ''' </param>
        ''' 
        ''' <param name="repositoryName">
        ''' The repository name.
        ''' </param>
        ''' 
        ''' <param name="version">
        ''' The version of the release.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="GitHubRelease"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Async Function GetReleaseAsync(ByVal userName As String, ByVal repositoryName As String, ByVal version As Version) As Task(Of GitHubRelease)

            Return (From release As GitHubRelease In Await GetReleasesAsync(userName, repositoryName)
                    Where release.Version = version).
                DefaultIfEmpty(Nothing).
                SingleOrDefault()

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a release that matches the specified version from the specified repository on GitHub.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim release As GitHubRelease = GetRelease("ElektroStudios", "SmartBotKit", New Version("1.3"))
        ''' Console.WriteLine("RELEASE: {0}", release.ToString())
        ''' Console.WriteLine()
        ''' 
        ''' Console.WriteLine("BODY:")
        ''' Console.WriteLine(release.Body)
        ''' Console.WriteLine()
        ''' 
        ''' For Each asset As GitHubAsset In release.Assets
        '''     Console.WriteLine("ASSET: {0}", asset.ToString())
        ''' Next asset
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="userName">
        ''' The user name.
        ''' </param>
        ''' 
        ''' <param name="repositoryName">
        ''' The repository name.
        ''' </param>
        ''' 
        ''' <param name="version">
        ''' The version of the release.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="GitHubRelease"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function GetRelease(ByVal userName As String, ByVal repositoryName As String, ByVal version As Version) As GitHubRelease

            Dim t As Task(Of GitHubRelease) = Task.Run(Function() GitHubUtil.GetRelease(userName, repositoryName, version))
            t.Wait(Timeout.Infinite)

            Return t.Result

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Asynchronously gets the latest release from the specified repository on GitHub.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim release As GitHubRelease = Await GetLatestReleaseAsync("ElektroStudios", "SmartBotKit")
        ''' Console.WriteLine("RELEASE: {0}", release.ToString())
        ''' Console.WriteLine()
        ''' 
        ''' Console.WriteLine("BODY:")
        ''' Console.WriteLine(release.Body)
        ''' Console.WriteLine()
        ''' 
        ''' For Each asset As GitHubAsset In release.Assets
        '''     Console.WriteLine("ASSET: {0}", asset.ToString())
        ''' Next asset
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="userName">
        ''' The user name.
        ''' </param>
        ''' 
        ''' <param name="repositoryName">
        ''' The repository name.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="GitHubRelease"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="HttpException">
        ''' JSON validation error.
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Async Function GetLatestReleaseAsync(ByVal userName As String, ByVal repositoryName As String) As Task(Of GitHubRelease)

            Dim uri As New Uri($"https://api.github.com/repos/{userName}/{repositoryName}/releases/latest", UriKind.Absolute)

            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(uri), HttpWebRequest)
            request.UserAgent = userName

            Using response As WebResponse = Await request.GetResponseAsync(),
              sr As New StreamReader(response.GetResponseStream()),
              xmlReader As XmlDictionaryReader = JsonReaderWriterFactory.CreateJsonReader(sr.BaseStream, Encoding.UTF8, New XmlDictionaryReaderQuotas, Nothing)

                Dim xml As XElement = XElement.Load(xmlReader)
                If (xml.IsEmpty) Then
                    Dim errMsg As String = $"JSON validation error. ""{uri.ToString()}"""
                    Throw New HttpException(HttpStatusCode.NotFound, errMsg)
                End If

                Return New GitHubRelease(xml)
            End Using

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the latest release from the specified repository on GitHub.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim release As GitHubRelease = GetLatestRelease("ElektroStudios", "SmartBotKit")
        ''' Console.WriteLine("RELEASE: {0}", release.ToString())
        ''' Console.WriteLine()
        ''' 
        ''' Console.WriteLine("BODY:")
        ''' Console.WriteLine(release.Body)
        ''' Console.WriteLine()
        ''' 
        ''' For Each asset As GitHubAsset In release.Assets
        '''     Console.WriteLine("ASSET: {0}", asset.ToString())
        ''' Next asset
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="userName">
        ''' The user name.
        ''' </param>
        ''' 
        ''' <param name="repositoryName">
        ''' The repository name.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <see cref="GitHubRelease"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function GetLatestRelease(ByVal userName As String, ByVal repositoryName As String) As GitHubRelease

            Dim t As Task(Of GitHubRelease) = Task.Run(Function() GitHubUtil.GetLatestReleaseAsync(userName, repositoryName))
            t.Wait(Timeout.Infinite)

            Return t.Result

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Asynchronously gets a value that determine whether exists a new version available of the specified reository on GitHub.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim user As String = "ElektroStudios"
        ''' Dim repo As String = "SmartBotKit"
        ''' Dim currentVersion As Version = Assembly.GetExecutingAssembly().GetName().Version
        ''' 
        ''' Dim isUpdateAvailable As Boolean = Await IsUpdateAvailableAsync(user, repo, currentVersion)
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="userName">
        ''' The user name.
        ''' </param>
        ''' 
        ''' <param name="repositoryName">
        ''' The repository name.
        ''' </param>
        ''' 
        ''' <param name="currentVersion">
        ''' The current version.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if exists a new version available on GitHub; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Async Function IsUpdateAvailableAsync(ByVal userName As String, ByVal repositoryName As String, ByVal currentVersion As Version) As Task(Of Boolean)

            Dim latestRelease As GitHubRelease = Await GitHubUtil.GetLatestReleaseAsync(userName, repositoryName)
            Return (latestRelease.Version > currentVersion)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether exists a new version available of the specified reository on GitHub.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim user As String = "ElektroStudios"
        ''' Dim repo As String = "SmartBotKit"
        ''' Dim currentVersion As Version = Assembly.GetExecutingAssembly().GetName().Version
        ''' 
        ''' Dim isUpdateAvailable As Boolean = IsUpdateAvailable(user, repo, currentVersion)
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="userName">
        ''' The user name.
        ''' </param>
        ''' 
        ''' <param name="repositoryName">
        ''' The repository name.
        ''' </param>
        ''' 
        ''' <param name="currentVersion">
        ''' The current version.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if exists a new version available on GitHub; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Function IsUpdateAvailable(ByVal userName As String, ByVal repositoryName As String, ByVal currentVersion As Version) As Boolean

            Dim t As Task(Of Boolean) = Task.Run(Function() GitHubUtil.IsUpdateAvailableAsync(userName, repositoryName, currentVersion))
            t.Wait(Timeout.Infinite)

            Return t.Result

        End Function

#End Region

    End Class

End Namespace

#End Region
