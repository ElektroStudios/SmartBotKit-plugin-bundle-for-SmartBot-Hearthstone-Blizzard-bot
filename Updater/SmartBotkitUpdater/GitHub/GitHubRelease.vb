' ***********************************************************************
' Author   : Elektro
' Modified : 06-May-2018
' ***********************************************************************

#Region " Public Members Summary "

#Region " Constructors "

' New(XElement)

#End Region

#Region " Properties "

' Assets As ReadOnlyCollection(Of GitHubAsset)
' Author As GitHubAuthor
' Body As String
' DateCreated As Date
' DatePublished As Date
' Id As String
' IsDraft As Boolean
' IsPreRelease As Boolean
' Name As String
' Raw As XElement
' TagName As String
' TargetCommitish As String
' UriAssets As Uri
' UriHtml As Uri
' UriRelease As Uri
' UriTarball As Uri
' UriZipball As Uri
' Version As Version

#End Region

#Region " Functions "

' ToString()

#End Region

#Region " Operators "

' <>
' =

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
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Globalization
Imports System.Linq
Imports System.Xml.Linq

#End Region

#Region " GitHubRelease "

' ReSharper disable once CheckNamespace

Namespace SmartBotKitUpdater.GitHub


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents a release on GitHub.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso href="https://github.com"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class GitHubRelease

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the GitHub's unique identifier for this release.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Id As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the release tag name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property TagName As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the release name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Name As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the release version.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' The version is derived from <see cref="GitHubRelease.TagName"/>, which should follow semantic versioning guidelines.
        ''' <para></para>
        ''' See for more info about semantic versioning: 
        ''' <para></para>
        ''' <see href="https://semver.org/"/>
        ''' <para></para>
        ''' <see href="https://help.github.com/articles/about-releases/"/>
        ''' <para></para>
        ''' <see href="https://git-scm.com/book/en/v2/Git-Basics-Tagging"/>
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Version As Version
            Get
                ' Remove prefixes and suffixes from tagname, like: "v1.0", "1.0-alpha" or "1.0-beta".
                Dim str As String = Me.TagName.ToLower().
                                           Trim("abcdefghijklmnopqrstuvwxyz !·$%&/()=?\|@#~'^*¨;:,.{}[]+".ToArray())

                Dim result As Version = Nothing
                Version.TryParse(str, result)
                Return result
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the body, in MarkDown format.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Body As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the commition target.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property TargetCommitish As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the release page.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriRelease As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the assets page of this release.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriAssets As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the tarball file of this release.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriTarball As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the zipball file of this release.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriZipball As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the release page for a web-browser.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriHtml As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether this release is a draft. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property IsDraft As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether this release is a pre-release. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property IsPreRelease As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the creation datetime.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property DateCreated As Date

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the published datetime. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property DatePublished As Date

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the author of this release.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Author As GitHubAuthor

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the assets of this release.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Assets As ReadOnlyCollection(Of GitHubAsset)

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the raw Xml content of this <see cref="GitHubRelease"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public ReadOnly Property Raw As XElement

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="GitHubRelease"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub New()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="GitHubRelease"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="xml">
        ''' An <see cref="XElement"/> that contains the fields to parse.
        ''' <para></para>
        ''' See: <see href="https://api.github.com/repos/{user}/{repo}/releases"/>
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New(ByVal xml As XElement)
            Me.Raw = xml

            Me.Id = xml.<id>.Value
            Me.Name = xml.<name>.Value
            Me.TagName = xml.<tag_name>.Value
            Me.Body = xml.<body>.Value
            Me.TargetCommitish = xml.<target_commitish>.Value

            Me.IsDraft = CBool(xml.<draft>.Value)
            Me.IsPreRelease = CBool(xml.<prerelease>.Value)

            Me.DateCreated = Date.Parse(xml.<created_at>.Value, CultureInfo.GetCultureInfo("en-US").DateTimeFormat)
            Me.DatePublished = Date.Parse(xml.<published_at>.Value, CultureInfo.GetCultureInfo("en-US").DateTimeFormat)

            Me.UriRelease = New Uri(xml.<url>.Value, UriKind.Absolute)
            Me.UriAssets = New Uri(xml.<assets_url>.Value, UriKind.Absolute)
            Me.UriHtml = New Uri(xml.<html_url>.Value, UriKind.Absolute)
            Me.UriTarball = New Uri(xml.<tarball_url>.Value, UriKind.Absolute)
            Me.UriZipball = New Uri(xml.<zipball_url>.Value, UriKind.Absolute)

            Me.Author = New GitHubAuthor(xml.<author>.Single())

            Dim assets As New Collection(Of GitHubAsset)()
            Dim elements As IEnumerable(Of XElement) = xml.<assets>.<item>
            For Each element As XElement In elements
                Dim asset As New GitHubAsset(element)
                assets.Add(asset)
            Next
            Me.Assets = New ReadOnlyCollection(Of GitHubAsset)(assets)
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Returns a <see cref="String"/> that represents this release.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A <see cref="String"/> that represents this release.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Function ToString() As String
            Dim kvPairs As New NameValueCollection(EqualityComparer(Of String).Default) From {
            {NameOf(Me.Id), Me.Id},
            {NameOf(Me.TagName), Me.TagName},
            {NameOf(Me.Name), Me.Name},
            {NameOf(Me.TargetCommitish), Me.TargetCommitish},
            {NameOf(Me.IsDraft), Me.IsDraft.ToString()},
            {NameOf(Me.IsPreRelease), Me.IsPreRelease.ToString()},
            {NameOf(Me.DateCreated), Me.DateCreated.ToString("MM/dd/yyyy HH:mm:ss")},
            {NameOf(Me.DatePublished), Me.DatePublished.ToString("MM/dd/yyyy HH:mm:ss")},
            {NameOf(Me.UriHtml), Me.UriHtml.AbsoluteUri}
        }

            Return $"{{{String.Join(", ", (From key In kvPairs.AllKeys, value In kvPairs.GetValues(key)
                                           Select $"{key}={value}"))}}}"
        End Function

#End Region

#Region " Operator Overloading "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Implements the operator &lt;&gt;.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="release1">
        ''' The first <see cref="GitHubRelease"/>.
        ''' </param>
        ''' 
        ''' <param name="release2">
        ''' The second <see cref="GitHubRelease"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The result of the operator.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Operator <>(ByVal release1 As GitHubRelease, ByVal release2 As GitHubRelease) As Boolean

            Return (release1.Id <> release2.Id)

        End Operator

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Implements the operator =.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="release1">
        ''' The first <see cref="GitHubRelease"/>.
        ''' </param>
        ''' 
        ''' <param name="release2">
        ''' The second <see cref="GitHubRelease"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The result of the operator.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Operator =(ByVal release1 As GitHubRelease, ByVal release2 As GitHubRelease) As Boolean

            Return (release1.Id = release2.Id)

        End Operator

#End Region

    End Class

End Namespace

#End Region

