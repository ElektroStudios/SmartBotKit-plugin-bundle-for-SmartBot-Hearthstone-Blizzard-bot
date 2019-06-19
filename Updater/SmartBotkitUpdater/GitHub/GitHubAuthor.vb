' ***********************************************************************
' Author   : Elektro
' Modified : 06-May-2018
' ***********************************************************************

#Region " Public Members Summary "

#Region " Constructors "

' New(XElement)

#End Region

#Region " Properties "

' GravatarId As String
' Id As String
' IsSiteAdministrator As Boolean
' Name As String
' Raw As XElement
' Type As String
' UriAuthor As Uri
' UriAvatar As Uri
' UriFollowers As Uri
' UriHtml As Uri
' UriOrganizations As Uri
' UriReceivedEvents As Uri
' UriRepositories As Uri
' UriSubscriptions As Uri

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
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Linq
Imports System.Xml.Linq

#End Region

#Region " GitHubAuthor "

' ReSharper disable once CheckNamespace

Namespace SmartBotKitUpdater.GitHub


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents the author of a release or the uploader of an asset on GitHub.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso href="https://github.com"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class GitHubAuthor

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the GitHub's unique identifier for this author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Id As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the author name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Name As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the type of user.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property [Type] As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value that determine whether this author is a site administrator. 
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property IsSiteAdministrator As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the unique identifier of the Gravatar.
        ''' <para></para>
        ''' See for more info: <see href="https://gravatar.com/"/>
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property GravatarId As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the author page.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriAuthor As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the avatar page of this author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriAvatar As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the followers page of this author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriFollowers As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the subscriptions page of this author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriSubscriptions As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the organizations page of this author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriOrganizations As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the repositories page of this author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriRepositories As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the received events page of this author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriReceivedEvents As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the author page for a web-browser.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriHtml As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the raw Xml content of this <see cref="GitHubAuthor"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public ReadOnly Property Raw As XElement

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="GitHubAuthor"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub New()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="GitHubAuthor"/> class.
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
            Me.Name = xml.<login>.Value
            Me.Type = xml.<type>.Value

            Me.IsSiteAdministrator = CBool(xml.<site_admin>.Value)
            Me.GravatarId = xml.<gravatar_id>.Value

            Me.UriAuthor = New Uri(xml.<url>.Value, UriKind.Absolute)
            Me.UriAvatar = New Uri(xml.<avatar_url>.Value, UriKind.Absolute)
            Me.UriSubscriptions = New Uri(xml.<subscriptions_url>.Value, UriKind.Absolute)
            Me.UriOrganizations = New Uri(xml.<organizations_url>.Value, UriKind.Absolute)
            Me.UriRepositories = New Uri(xml.<repos_url>.Value, UriKind.Absolute)
            Me.UriReceivedEvents = New Uri(xml.<received_events_url>.Value, UriKind.Absolute)
            Me.UriHtml = New Uri(xml.<html_url>.Value, UriKind.Absolute)
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Returns a <see cref="String"/> that represents this author.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A <see cref="String"/> that represents this author.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Function ToString() As String
            Dim kvPairs As New NameValueCollection(EqualityComparer(Of String).Default) From {
            {NameOf(Me.Id), Me.Id},
            {NameOf(Me.Name), Me.Name},
            {NameOf(Me.Type), Me.Type},
            {NameOf(Me.IsSiteAdministrator), Me.IsSiteAdministrator.ToString()},
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
        ''' <param name="author1">
        ''' The first <see cref="GitHubAuthor"/>.
        ''' </param>
        ''' 
        ''' <param name="author2">
        ''' The second <see cref="GitHubAuthor"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The result of the operator.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Operator <>(ByVal author1 As GitHubAuthor, ByVal author2 As GitHubAuthor) As Boolean

            Return (author1.Id <> author2.Id)

        End Operator

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Implements the operator =.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="author1">
        ''' The first <see cref="GitHubAuthor"/>.
        ''' </param>
        ''' 
        ''' <param name="author2">
        ''' The second <see cref="GitHubAuthor"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The result of the operator.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Operator =(ByVal author1 As GitHubAuthor, ByVal author2 As GitHubAuthor) As Boolean

            Return (author1.Id = author2.Id)

        End Operator

#End Region

    End Class

End Namespace

#End Region

