
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Linq
Imports System.Runtime.CompilerServices

#End Region

#Region " Enum Extensions "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Extensions.EnumExtensions

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with <see cref="System.[Enum]"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <HideModuleName>
    Public Module EnumExtensions

#Region " Public Extension Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the current flags of the source enumeration.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim value As FileAttributes = FileAttributes.ReadOnly Or FileAttributes.Hidden Or FileAttributes.System
        ''' Dim flags As FileAttributes() = value.Flags(Of FileAttributes)
        ''' 
        ''' For Each flag As FileAttributes In flags
        '''     MsgBox(flag.ToString())
        ''' Next
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <typeparam name="T">
        ''' The type.
        ''' </typeparam>
        ''' 
        ''' <param name="sender">
        ''' The source <see cref="System.[Enum]"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The current flags.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Flags(Of T)(sender As System.[Enum]) As T()

            Return (From flag In sender.ToString().Split({","c}, StringSplitOptions.RemoveEmptyEntries)
                    Select DirectCast(System.[Enum].Parse(sender.GetType, flag.Trim(" "c)), T)
                   ).ToArray()

            ' Original Loop:
            '
            'Dim result As New List(Of T)
            '
            'For Each flag As String In sender.ToString().Split({","c}, StringSplitOptions.RemoveEmptyEntries)
            '    result.Add(DirectCast(System.[Enum].Parse(sender.GetType, flag.Trim(" "c)), T))
            'Next flag
            '
            'Return result.ToArray()

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Performs the specified action for each flag of the source enumeration.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim flags As FileAttributes = FileAttributes.ReadOnly Or FileAttributes.Hidden Or FileAttributes.System
        ''' 
        ''' flags.ForEachFlag(Of FileAttributes)(
        '''     Sub(ByVal x As FileAttributes)
        '''         MsgBox(x.ToString())
        '''     End Sub)
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <typeparam name="T">
        ''' The type.
        ''' </typeparam>
        ''' 
        ''' <param name="sender">
        ''' The source <see cref="System.[Enum]"/>.
        ''' </param>
        ''' 
        ''' <param name="action">
        ''' The  <see cref="System.Action"/> to perform on each flag.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Sub ForEachFlag(Of T)(sender As System.[Enum],
                                     action As Action(Of T))

            For Each flag As T In sender.Flags(Of T)
                action.Invoke(flag)
            Next flag

        End Sub

#End Region

    End Module

End Namespace

#End Region
