
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.CompilerServices

#End Region

#Region " TimeSpan Extensions "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.Extensions.TimeSpanExtensions


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with a <see cref="TimeSpan"/> type.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    <HideModuleName>
    Public Module TimeSpanExtensions

#Region " Public Extension Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determine whether the current time (hours, minutes, seconds and milliseconds, not including days) 
        ''' in the source <see cref="TimeSpan"/> is 
        ''' in range between the specified start time and end time range.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim input As TimeSpan = TimeSpan.Parse("00:00:00")
        ''' Dim start As TimeSpan = TimeSpan.Parse("23:59:59")
        ''' Dim [end] As TimeSpan = TimeSpan.Parse("00:00:01")
        ''' 
        ''' Dim result As Boolean = input.IsHourInRange(start, [end])
        ''' Console.WriteLine(result) ' result = True
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="TimeSpan"/>.
        ''' </param>
        ''' 
        ''' <param name="start">
        ''' A <see cref="TimeSpan"/> that represents the start time.
        ''' </param>
        ''' 
        ''' <param name="[end]">
        ''' A <see cref="TimeSpan"/> that represents the end time.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the current time in the source <see cref="TimeSpan"/> is 
        ''' in range between the specified start time and end time range, 
        ''' <see langword="False"/> otherwise.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function IsHourInRange(ByVal sender As TimeSpan, start As TimeSpan, [end] As TimeSpan) As Boolean

            ' Remove days.
            sender = sender.Add(-TimeSpan.FromDays(sender.Days))
            start = start.Add(-TimeSpan.FromDays(start.Days))
            [end] = [end].Add(-TimeSpan.FromDays([end].Days))

            If (sender <= start) AndAlso (sender <= [end]) AndAlso ([end] <= start) Then
                Return True

            ElseIf (sender <= start) AndAlso (sender <= [end]) AndAlso ([end] >= start) Then
                Return False

            ElseIf (sender >= start) AndAlso (sender >= [end]) AndAlso ([end] <= start) Then
                Return True

            Else
                Return (sender >= start) AndAlso (sender <= [end])

            End If

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determine whether the current time (days, hours, minutes, seconds and milliseconds) 
        ''' in the source <see cref="TimeSpan"/> is 
        ''' in range between the specified start time and end time range.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim input As TimeSpan = TimeSpan.Parse("1.00:00:00")
        ''' Dim start As TimeSpan = TimeSpan.Parse("0.23:59:59")
        ''' Dim [end] As TimeSpan = TimeSpan.Parse("1.00:00:01")
        ''' 
        ''' Dim result As Boolean = input.IsInRange(start, [end])
        ''' Console.WriteLine(result)
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="TimeSpan"/>.
        ''' </param>
        ''' 
        ''' <param name="start">
        ''' A <see cref="TimeSpan"/> that represents the start time.
        ''' </param>
        ''' 
        ''' <param name="[end]">
        ''' A <see cref="TimeSpan"/> that represents the end time.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the current time in the source <see cref="TimeSpan"/> is 
        ''' in range between the specified start time and end time range, 
        ''' <see langword="False"/> otherwise.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function IsInRange(ByVal sender As TimeSpan, start As TimeSpan, [end] As TimeSpan) As Boolean

            Return (sender >= start) AndAlso (sender <= [end])

        End Function

#End Region

    End Module

End Namespace

#End Region
