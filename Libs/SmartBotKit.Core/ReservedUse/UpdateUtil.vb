
#Region " Imports "

Imports System.IO

Imports SmartBotKit.Interop

#End Region

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.ReservedUse


    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides utilities for version update check of SmartBotKit plugin bundle.
    ''' <para></para>
    ''' Note: the usage of this class is reserved by SmartBotKit plugins, don't use it by yourself.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class UpdateUtil

#Region " Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Flag to prevent redundant calls from multiple plugins checking for updates.
        ''' <para></para>
        ''' Note: the usage of this field is reserved by SmartBotKit plugins, don't use it by yourself.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared IsUpdateChecked As Boolean

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="UpdateUtil"/> class from being created.
        ''' <para></para>
        ''' Note: the usage of this class is reserved by SmartBotKit plugins, don't use it by yourself.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Runs the SmartBotkitUpdater.exe file.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Sub RunUpdaterExecutable()

            If (UpdateUtil.IsUpdateChecked) Then
                Exit Sub
            End If
            UpdateUtil.IsUpdateChecked = True

            Dim srcDir As String = Path.Combine(SmartBotUtil.PluginsDir().FullName, "libs")
            Dim exeName As String = "SmartBotkitUpdater.exe"
            Try
                Using p As New Process
                    With p.StartInfo
                        .FileName = Path.Combine(srcDir, exeName)
                        .WorkingDirectory = srcDir
                        .ErrorDialog = False
                        .CreateNoWindow = True
                        .UseShellExecute = False
                        .WindowStyle = ProcessWindowStyle.Hidden
                    End With

                    p.Start()
                End Using

            Catch ex As Exception
                ' Do nothing.

            End Try

        End Sub

#End Region

    End Class

End Namespace
