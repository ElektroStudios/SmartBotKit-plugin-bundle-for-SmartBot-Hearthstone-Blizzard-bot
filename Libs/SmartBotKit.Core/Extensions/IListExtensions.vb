
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

#Region " IList Extensions "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Extensions.IListExtensions


    ' ReSharper disable InconsistentNaming

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with a <see cref="IList"/> type.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    <HideModuleName>
    Public Module IListExtensions

        ' ReSharper restore InconsistentNaming

#Region " Public Extension Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Randomizes the elements of the source <see cref="IList(Of T)"/>.
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
        ''' <see cref="IList(Of T)"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function Randomize(Of T)(ByVal sender As IList(Of T)) As IList(Of T)

            Return IEnumerableExtensions.Randomize(sender).ToList()

        End Function

#End Region

    End Module

End Namespace

#End Region
