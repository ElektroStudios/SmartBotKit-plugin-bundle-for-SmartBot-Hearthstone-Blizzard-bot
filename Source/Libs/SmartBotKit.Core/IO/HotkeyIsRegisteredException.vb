﻿
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.Serialization
Imports System.Security.Permissions
Imports System.Xml.Serialization

#End Region

#Region " HotkeyIsRegistered Exception "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.IO


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Exception that is thrown when try to register a system-wide hotkey that is already registered.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <XmlRoot("Exception")>
    <ImmutableObject(True)>
    Public NotInheritable Class HotkeyIsRegisteredException : Inherits Exception

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="HotkeyIsRegisteredException"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
            MyBase.New("Unable to register because the hotkey is already registered.")
        End Sub

#End Region

#Region " ISerializable implementation "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="info">
        ''' The <see cref="SerializationInfo"/> to populate with data.
        ''' </param>
        ''' 
        ''' <param name="context">
        ''' The destination (see <see cref="StreamingContext"/>) for this serialization.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentNullException">
        ''' info
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <SecurityPermission(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerStepThrough>
        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)
            MyBase.GetObjectData(info, context)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="HotkeyIsRegisteredException"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <remarks>
        ''' This constructor is used to deserialize values.
        ''' </remarks>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="info">
        ''' The <see cref="SerializationInfo"/> to populate with data.
        ''' </param>
        ''' 
        ''' <param name="context">
        ''' The destination (see <see cref="StreamingContext"/>) for this deserialization.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentNullException">
        ''' info
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub

#End Region

    End Class

End Namespace

#End Region
