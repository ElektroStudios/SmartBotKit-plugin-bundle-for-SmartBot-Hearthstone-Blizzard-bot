' ***********************************************************************
' Author   : Elektro
' Modified : 06-May-2018
' ***********************************************************************

#Region " Public Members Summary "

#Region " Constructors "

' New(XElement)

#End Region

#Region " Properties "

' ContentType As ContentType
' DateCreated As Date
' DateUploaded As Date
' DownloadCount As Integer
' Id As String
' Label As String
' Name As String
' Raw As XElement
' Size As Long
' State As String
' Uploader As GitHubAuthor
' UriAsset As Uri
' UriDownload As Uri

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
Imports System.Globalization
Imports System.Linq
Imports System.Net.Mime
Imports System.Xml.Linq

#End Region

#Region " GitHubAsset "

' ReSharper disable once CheckNamespace

Namespace SmartBotKitUpdater.GitHub


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents an asset of a release on GitHub.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <seealso href="https://github.com"/>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class GitHubAsset

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the GitHub's unique identifier for this asset.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Id As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the name of this asset.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Name As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the label of this asset.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Label As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the state of this asset.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property State As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the size of this asset, in bytes.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Size As Long

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value indicating how many times this asset was downloaded.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property DownloadCount As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the content-type of this asset.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property ContentType As ContentType

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the page of this asset.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriAsset As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an <see cref="Uri"/> that points to the download page of this asset.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property UriDownload As Uri

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the creation datetime.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property DateCreated As Date

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the uploaded datetime.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property DateUploaded As Date

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the uploader of this asset.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property Uploader As GitHubAuthor

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the raw Xml content of this <see cref="GitHubAsset"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public ReadOnly Property Raw As XElement

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="GitHubAsset"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub New()
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="GitHubAsset"/> class.
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
            Me.Label = xml.<label>.Value
            Me.State = xml.<state>.Value

            Me.ContentType = New ContentType(xml.<content_type>.Value)

            Me.Size = CLng(xml.<size>.Value)
            Me.DownloadCount = CInt(xml.<download_count>.Value)

            Me.DateCreated = Date.Parse(xml.<created_at>.Value, CultureInfo.GetCultureInfo("en-US").DateTimeFormat)
            Me.DateUploaded = Date.Parse(xml.<updated_at>.Value, CultureInfo.GetCultureInfo("en-US").DateTimeFormat)

            Me.UriAsset = New Uri(xml.<url>.Value, UriKind.Absolute)
            Me.UriDownload = New Uri(xml.<browser_download_url>.Value, UriKind.Absolute)

            Me.Uploader = New GitHubAuthor(xml.<uploader>.Single())
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Returns a <see cref="String"/> that represents this asset.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A <see cref="String"/> that represents this asset.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Function ToString() As String
            Dim kvPairs As New NameValueCollection(EqualityComparer(Of String).Default) From {
                {NameOf(Me.Id), Me.Id},
                {NameOf(Me.Name), Me.Name},
                {NameOf(Me.Label), Me.Label},
                {NameOf(Me.State), Me.State},
                {NameOf(Me.Size), Me.Size.ToString()},
                {NameOf(Me.ContentType), Me.ContentType.ToString()},
                {NameOf(Me.DownloadCount), Me.DownloadCount.ToString()},
                {NameOf(Me.DateCreated), Me.DateCreated.ToString("MM/dd/yyyy HH:mm:ss")},
                {NameOf(Me.DateUploaded), Me.DateUploaded.ToString("MM/dd/yyyy HH:mm:ss")},
                {NameOf(Me.UriDownload), Me.UriDownload.AbsoluteUri}
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
        ''' <param name="asset1">
        ''' The first <see cref="GitHubAsset"/>.
        ''' </param>
        ''' 
        ''' <param name="asset2">
        ''' The second <see cref="GitHubAsset"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The result of the operator.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Operator <>(ByVal asset1 As GitHubAsset, ByVal asset2 As GitHubAsset) As Boolean

            Return (asset1.Id <> asset2.Id)

        End Operator

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Implements the operator =.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="asset1">
        ''' The first <see cref="GitHubAsset"/>.
        ''' </param>
        ''' 
        ''' <param name="asset2">
        ''' The second <see cref="GitHubAsset"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The result of the operator.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Operator =(ByVal asset1 As GitHubAsset, ByVal asset2 As GitHubAsset) As Boolean

            Return (asset1.Id = asset2.Id)

        End Operator

#End Region

    End Class

End Namespace

#End Region

