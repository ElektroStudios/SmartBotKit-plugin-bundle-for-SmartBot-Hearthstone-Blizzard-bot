
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Runtime.CompilerServices

#End Region

#Region " IEnumerable Extensions "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Extensions.IEnumerableExtensions


    ' ReSharper disable InconsistentNaming

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with a <see cref="IEnumerable"/> type.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    <HideModuleName>
    Public Module IEnumerableExtensions

        ' ReSharper restore InconsistentNaming

#Region " Private Fields "

        ' ReSharper disable InconsistentNaming

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A <see cref="Random"/> instance to generate random secuences of numbers.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Rng As Random

        ' ReSharper restore InconsistentNaming

#End Region

#Region " Public Extension Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Randomizes the elements of the source <see cref="IEnumerable(Of T)"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim col As IEnumerable(Of Integer) = {1, 2, 3, 4, 5, 6, 7, 8, 9}
        ''' Debug.WriteLine(String.Join(", ", col.Randomize))
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <typeparam name="T">
        ''' The type.
        ''' </typeparam>
        ''' 
        ''' <param name="sender">
        ''' The source collection.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see cref="IEnumerable(Of T)"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Randomize(Of T)(ByVal sender As IEnumerable(Of T)) As IEnumerable(Of T)

            If (IEnumerableExtensions.Rng Is Nothing) Then
                IEnumerableExtensions.Rng = New Random(Seed:=Environment.TickCount)
            End If

            Return From item As T In sender
                   Order By Rng.Next()

        End Function

#End Region

    End Module

End Namespace

#End Region
