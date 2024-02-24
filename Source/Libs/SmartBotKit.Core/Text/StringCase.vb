
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " String Case "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Text


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Specifies a string case.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public Enum StringCase As Integer

        ''' <summary>
        ''' LowerCase
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : ABCDEF
        ''' <para></para>
        ''' Output: abcdef
        ''' </summary>
        LowerCase = &H0

        ''' <summary>
        ''' UpperCase.
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : abcdef
        ''' <para></para>
        ''' Output: ABCDEF
        ''' </summary>
        UpperCase = &H1

        ''' <summary>
        ''' TitleCase.
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : abcdef
        ''' <para></para>
        ''' Output: Abcdef
        ''' </summary>
        TitleCase = &H2

        ''' <summary>
        ''' WordCase.
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : abc def
        ''' <para></para>
        ''' Output: Abc Def
        ''' </summary>
        WordCase = &H3

        ''' <summary>
        ''' CamelCase (With first letter to LowerCase).
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : ABC DEF
        ''' <para></para>
        ''' Output: abcDef
        ''' </summary>
        CamelCaseLower = &H4

        ''' <summary>
        ''' CamelCase (With first letter to UpperCase).
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : ABC DEF
        ''' <para></para>
        ''' Output: AbcDef
        ''' </summary>
        CamelCaseUpper = &H5

        ''' <summary>
        ''' MixedCase (With first letter to LowerCase).
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : ab cd ef
        ''' <para></para>
        ''' Output: aB Cd eF
        ''' </summary>
        MixedTitleCaseLower = &H6

        ''' <summary>
        ''' MixedCase (With first letter to UpperCase).
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : ab cd ef
        ''' <para></para>
        ''' Output: Ab cD Ef
        ''' </summary>
        MixedTitleCaseUpper = &H7

        ''' <summary>
        ''' MixedCase (With first letter of each word to LowerCase).
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : ab cd ef
        ''' <para></para>
        ''' Output: aB cD eF
        ''' </summary>
        MixedWordCaseLower = &H8

        ''' <summary>
        ''' MixedCase (With first letter of each word to UpperCase).
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : ab cd ef
        ''' <para></para>
        ''' Output: Ab Cd Ef
        ''' </summary>
        MixedWordCaseUpper = &H9

        ''' <summary>
        ''' ToggleCase.
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : abc def ghi
        ''' <para></para>
        ''' Output: aBC dEF gHI
        ''' </summary>
        ToggleCase = &H10

        ''' <summary>
        ''' Duplicates the characters.
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : Hello World!
        ''' <para></para>
        ''' Output: HHeelllloo  WWoorrlldd!!
        ''' </summary>
        DuplicateChars = &H11

        ''' <summary>
        ''' Alternates the characters.
        ''' <para></para>
        ''' 
        ''' [Example]
        ''' <para></para>
        ''' Input : Hello World!
        ''' <para></para>
        ''' Output: hELLO wORLD!
        ''' </summary>
        AlternateChars = &H12

    End Enum

End Namespace

#End Region
