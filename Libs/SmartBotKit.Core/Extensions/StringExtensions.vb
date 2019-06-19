
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Globalization
Imports System.Linq
Imports System.Runtime.CompilerServices

Imports SmartBotKit.Text

#End Region

#Region " String Extensions "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Extensions.StringExtensions


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with a <see cref="String"/> datatype.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    <HideModuleName>
    Public Module StringExtensions

#Region " Public Extension Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Renames a string to the specified StringCase.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim str As String = "Hello World".Rename(StringCase.UpperCase)
        ''' 
        ''' MessageBox.Show(str)
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="String"/>.
        ''' </param>
        ''' 
        ''' <param name="stringCase">
        ''' The <see cref="StringCase"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The renamed string.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Rename(ByVal sender As String,
                               ByVal stringCase As StringCase) As String

            Select Case stringCase

                Case StringCase.LowerCase
                    Return sender.ToLower

                Case StringCase.UpperCase
                    Return sender.ToUpper

                Case StringCase.TitleCase
                    Return $"{Char.ToUpper(sender.First())}{sender.Remove(0, 1).ToLower()}"

                Case StringCase.WordCase
                    Return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sender.ToLower())

                Case StringCase.CamelCaseLower
                    Return _
                        $"{Char.ToLower(sender.First())}{ _
                            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sender.ToLower()).
                                Replace(" "c, String.Empty).
                                Remove(0, 1)}"

                Case StringCase.CamelCaseUpper
                    Return _
                        $"{Char.ToUpper(sender.First())}{ _
                            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sender.ToLower()).
                                Replace(" "c, String.Empty).
                                Remove(0, 1)}"

                Case StringCase.MixedTitleCaseLower
                    Dim sb As New System.Text.StringBuilder
                    For i As Integer = 0 To (sender.Length - 1) Step 2
                        If Not (i + 1) >= sender.Length Then
                            sb.AppendFormat("{0}{1}", Char.ToLower(sender(i)), Char.ToUpper(sender(i + 1)))
                        Else
                            sb.Append(Char.ToLower(sender(i)))
                        End If
                    Next i
                    Return sb.ToString()

                Case StringCase.MixedTitleCaseUpper
                    Dim sb As New System.Text.StringBuilder
                    For i As Integer = 0 To (sender.Length - 1) Step 2
                        If Not (i + 1) >= sender.Length Then
                            sb.AppendFormat("{0}{1}", Char.ToUpper(sender(i)), Char.ToLower(sender(i + 1)))
                        Else
                            sb.Append(Char.ToUpper(sender(i)))
                        End If
                    Next i
                    Return sb.ToString()

                Case StringCase.MixedWordCaseLower
                    Dim sb As New System.Text.StringBuilder
                    For Each word As String In sender.Split
                        sb.AppendFormat("{0} ", Rename(word, StringCase.MixedTitleCaseLower))
                    Next word
                    Return sb.ToString()

                Case StringCase.MixedWordCaseUpper
                    Dim sb As New System.Text.StringBuilder
                    For Each word As String In sender.Split
                        sb.AppendFormat("{0} ", Rename(word, StringCase.MixedTitleCaseUpper))
                    Next word
                    Return sb.ToString()

                Case StringCase.ToggleCase
                    Dim sb As New System.Text.StringBuilder
                    For Each word As String In sender.Split
                        sb.AppendFormat("{0}{1} ", Char.ToLower(word.First()), word.Remove(0, 1).ToUpper)
                    Next word
                    Return sb.ToString()

                Case StringCase.DuplicateChars
                    Dim sb As New System.Text.StringBuilder
                    For Each c As Char In sender
                        sb.Append(New String(c, 2))
                    Next c
                    Return sb.ToString()

                Case StringCase.AlternateChars
                    Dim sb As New System.Text.StringBuilder
                    For Each c As Char In sender
                        Select Case Char.IsLower(c)
                            Case True
                                sb.Append(Char.ToUpper(c))
                            Case Else
                                sb.Append(Char.ToLower(c))
                        End Select
                    Next c
                    Return sb.ToString()

                Case Else
                    Return sender

            End Select

        End Function

#End Region

    End Module

End Namespace

#End Region
