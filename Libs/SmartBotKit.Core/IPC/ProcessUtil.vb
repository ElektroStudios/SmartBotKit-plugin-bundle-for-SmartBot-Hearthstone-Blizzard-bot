
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.InteropServices

Imports SmartBotKit.Extensions.EnumExtensions
Imports SmartBotKit.Interop.Win32

#End Region

#Region " Process Util "

' ReSharper disable once CheckNamespace

Namespace SmartBotKit.IPC

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains related Process utilities.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    Public NotInheritable Class ProcessUtil


#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="ProcessUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Enable or disable a process privilege for the specified process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="handle">
        ''' The process handle.
        ''' </param>
        ''' 
        ''' <param name="privileges">
        ''' The process privileges to set.
        ''' </param>
        ''' 
        ''' <param name="privilegeState">
        ''' The new state for the privileges.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub SetProcessPrivileges(handle As IntPtr,
                                               privileges As ProcessPrivileges,
                                               privilegeState As PrivilegeStates)

            privileges.ForEachFlag(
                Sub(priv As ProcessPrivileges)
                    ProcessUtil.SetProcessPrivilege(Environment.MachineName, handle, $"Se{priv.ToString()}", privilegeState)
                End Sub)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Enable or disable a process privilege for the specified process on the target computer.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The computer on which to set the privileges for the process.
        ''' <para></para>
        ''' If this value is empty, it sets the privileges for the current computer.
        ''' </param>
        ''' 
        ''' <param name="handle">
        ''' The process handle.
        ''' </param>
        ''' 
        ''' <param name="privileges">
        ''' The process privileges to enable.
        ''' </param>
        ''' 
        ''' <param name="privilegeState">
        ''' The new state for the privileges.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub SetProcessPrivileges(computer As String,
                                               handle As IntPtr,
                                               privileges As ProcessPrivileges,
                                               privilegeState As PrivilegeStates)

            If String.IsNullOrEmpty(computer) Then
                Throw New ArgumentNullException(computer)

            Else
                privileges.ForEachFlag(
                Sub(priv As ProcessPrivileges)
                    ProcessUtil.SetProcessPrivilege(computer, handle, $"Se{priv.ToString()}", privilegeState)
                End Sub)

            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Enable or disable a process privilege for the specified process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pid">
        ''' The process identifier (PID).
        ''' </param>
        ''' 
        ''' <param name="privileges">
        ''' The process privileges to set.
        ''' </param>
        ''' 
        ''' <param name="privilegeState">
        ''' The new state for the privileges.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub SetProcessPrivileges(pid As Integer,
                                               privileges As ProcessPrivileges,
                                               privilegeState As PrivilegeStates)

            ProcessUtil.SetProcessPrivileges(Environment.MachineName, pid, privileges, privilegeState)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Enable or disable a process privilege for the specified process on the target computer.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The computer on which to set the privileges for the process.
        ''' <para></para>
        ''' If this value is empty, it sets the privileges for the current computer.
        ''' </param>
        ''' 
        ''' <param name="pid">
        ''' The process identifier (PID).
        ''' </param>
        ''' 
        ''' <param name="privileges">
        ''' The process privileges to enable.
        ''' </param>
        ''' 
        ''' <param name="privilegeState">
        ''' The new state for the privileges.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub SetProcessPrivileges(computer As String,
                                               pid As Integer,
                                               privileges As ProcessPrivileges,
                                               privilegeState As PrivilegeStates)

            If String.IsNullOrEmpty(computer) Then
                Throw New ArgumentNullException(computer)

            Else
                Dim proc As Process = Process.GetProcessById(pid)
                If (proc Is Nothing) Then
                    Throw New ArgumentException(paramName:=NameOf(pid),
                                                message:="Process not found by the specified PID")

                Else
                    ProcessUtil.SetProcessPrivileges(computer, proc.Handle, privileges, privilegeState)

                End If

            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Enable or disable a process privilege for the caller process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="privileges">
        ''' The process privileges to enable.
        ''' </param>
        ''' 
        ''' <param name="privilegeState">
        ''' The new state for the privileges.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub SetCurrentProcessPrivileges(privileges As ProcessPrivileges,
                                                      privilegeState As PrivilegeStates)

            ProcessUtil.SetProcessPrivileges(Process.GetCurrentProcess().Handle, privileges, privilegeState)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Enable or disable a process privilege for the caller process on the target computer.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The computer on which to set the privileges for the process.
        ''' <para></para>
        ''' If this value is empty, it sets the privileges for the current computer.
        ''' </param>
        ''' 
        ''' <param name="privileges">
        ''' The process privileges to enable.
        ''' </param>
        ''' 
        ''' <param name="privilegeState">
        ''' The new state for the privileges.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub SetCurrentProcessPrivileges(computer As String,
                                                      privileges As ProcessPrivileges,
                                                      privilegeState As PrivilegeStates)

            If String.IsNullOrEmpty(computer) Then
                Throw New ArgumentNullException(computer)

            Else
                ProcessUtil.SetProcessPrivileges(computer, Process.GetCurrentProcess().Handle, privileges, privilegeState)

            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the state of a privilege of the specified process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="handle">
        ''' The process handle.
        ''' </param>
        ''' 
        ''' <param name="privilege">
        ''' The privileges to check.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The privilege state.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetProcessPrivilegeState(handle As IntPtr,
                                                        privilege As ProcessPrivileges) As PrivilegeStates

            Return ProcessUtil.GetProcessPrivilegeState(Environment.MachineName, handle, $"Se{privilege.ToString()}")

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the state of a privilege of the specified process on the target computer.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The computer on which to retrieve the privileges of the process.
        ''' <para></para>
        ''' If this value is empty, it checks the privileges for the current computer.
        ''' </param>
        ''' 
        ''' <param name="handle">
        ''' The process handle.
        ''' </param>
        ''' 
        ''' <param name="privilege">
        ''' The privileges to check.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The privilege state.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetProcessPrivilegeState(computer As String,
                                                        handle As IntPtr,
                                                        privilege As ProcessPrivileges) As PrivilegeStates

            If String.IsNullOrEmpty(computer) Then
                Throw New ArgumentNullException(computer)

            Else
                Return ProcessUtil.GetProcessPrivilegeState(computer, handle, $"Se{privilege.ToString()}")

            End If

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the state of a privilege of the specified process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pid">
        ''' The process identifier (PID).
        ''' </param>
        ''' 
        ''' <param name="privilege">
        ''' The privileges to check.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The privilege state.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetProcessPrivilegeState(pid As Integer,
                                                        privilege As ProcessPrivileges) As PrivilegeStates

            Return ProcessUtil.GetProcessPrivilegeState(Environment.MachineName, pid, privilege)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the state of a privilege of the specified process on the target computer.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The computer on which to retrieve the privileges of the process.
        ''' <para></para>
        ''' If this value is empty, it checks the privileges for the current computer.
        ''' </param>
        ''' 
        ''' <param name="pid">
        ''' The process identifier (PID).
        ''' </param>
        ''' 
        ''' <param name="privilege">
        ''' The privileges to check.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The privilege state.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetProcessPrivilegeState(computer As String,
                                                        pid As Integer,
                                                        privilege As ProcessPrivileges) As PrivilegeStates

            If String.IsNullOrEmpty(computer) Then
                Throw New ArgumentNullException(computer)

            Else
                Dim proc As Process = Process.GetProcessById(pid)
                If (proc Is Nothing) Then
                    Throw New ArgumentException(paramName:=NameOf(pid),
                                                message:="Process not found by the specified PID")

                Else
                    Return ProcessUtil.GetProcessPrivilegeState(computer, proc.Handle, $"Se{privilege.ToString()}")

                End If

            End If

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the state of a privilege of the caller process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="privilege">
        ''' The privileges to check.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The privilege state.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetCurrentProcessPrivilegeState(privilege As ProcessPrivileges) As PrivilegeStates

            Return ProcessUtil.GetProcessPrivilegeState(Environment.MachineName, Process.GetCurrentProcess().Handle,
                                                        $"Se{privilege.ToString()}")

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the state of a privilege of the caller process on the target computer.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The computer on which to retrieve the privileges of the process.
        ''' <para></para>
        ''' If this value is empty, it checks the privileges for the current computer.
        ''' </param>
        ''' 
        ''' <param name="privilege">
        ''' The privileges to check.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The privilege state.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function GetCurrentProcessPrivilegeState(computer As String,
                                                               privilege As ProcessPrivileges) As PrivilegeStates

            If String.IsNullOrEmpty(computer) Then
                Throw New ArgumentNullException(computer)

            Else
                Return ProcessUtil.GetProcessPrivilegeState(computer, Process.GetCurrentProcess().Handle,
                                                            $"Se{privilege.ToString()}")

            End If

        End Function

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Enable or disable a process privilege for the specified process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The computer on which to set the privileges for the process.
        ''' <para></para>
        ''' If this value is empty, it sets the privileges for the current computer.
        ''' </param>
        ''' 
        ''' <param name="handle">
        ''' The process handle.
        ''' </param>
        ''' 
        ''' <param name="privilegeName">
        ''' The process privilege name to enable.
        ''' </param>
        ''' 
        ''' <param name="privilegeState">
        ''' The privilege attributes.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Shared Sub SetProcessPrivilege(computer As String,
                                               handle As IntPtr,
                                               privilegeName As String,
                                               privilegeState As PrivilegeStates)
            Dim win32Err As Integer

            Dim luid As Luid
            Dim hToken As IntPtr
            Dim luAttr As LuidAndAttributes
            Dim newState As TokenPrivileges
            Dim prevState As TokenPrivileges
            Dim returnLength As IntPtr

            Try
                ' Get the LUID that corresponds to the privilege, if it exists.
                If Not NativeMethods.LookupPrivilegeValue(computer, privilegeName, luid) Then
                    win32Err = Marshal.GetLastWin32Error()
                    Throw New Win32Exception(win32Err)
                End If

                ' Get the source process token.
                If Not NativeMethods.OpenProcessToken(handle, TokenAccess.AdjustPrivileges Or TokenAccess.Query, hToken) Then
                    win32Err = Marshal.GetLastWin32Error()
                    Throw New Win32Exception(win32Err)
                End If

                ' Set up a LuidAndAttributes structure containing the source privilege.
                luAttr = New LuidAndAttributes With {
                    .Luid = luid,
                    .Attributes = privilegeState
                }

                ' Set up a TokenPrivileges structure containing only the source privilege.
                newState = New TokenPrivileges With {
                    .PrivilegeCount = 1,
                    .Privileges = New LuidAndAttributes() {luAttr}
                }

                ' Set up a TokenPrivileges structure for the returned (modified) privileges.
                prevState = New TokenPrivileges
                ReDim prevState.Privileges(CInt(newState.PrivilegeCount))

                ' Apply the TokenPrivileges structure to the source process token.
                If Not NativeMethods.AdjustTokenPrivileges(hToken, False, newState, Marshal.SizeOf(prevState), prevState, returnLength) Then
                    win32Err = Marshal.GetLastWin32Error()
                    Throw New Win32Exception(win32Err)
                End If

            Catch ex As Win32Exception
                Throw

            Finally
                If (hToken <> IntPtr.Zero) Then
                    NativeMethods.CloseHandle(hToken)
                End If

            End Try

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the state of a privilege of the specified process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="computer">
        ''' The computer on which to retrieve the privileges of the process.
        ''' <para></para>
        ''' If this value is empty, it checks the privileges for the current computer.
        ''' </param>
        ''' 
        ''' <param name="handle">
        ''' The process handle.
        ''' </param>
        ''' 
        ''' <param name="privilegeName">
        ''' The privilege name to check.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The privilege state.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Private Shared Function GetProcessPrivilegeState(computer As String,
                                                         handle As IntPtr,
                                                         privilegeName As String) As PrivilegeStates
            Dim win32Err As Integer

            Dim hToken As IntPtr
            Dim tkp As New TokenPrivileges
            Dim tkpHandle As IntPtr = IntPtr.Zero ' will be set later.
            Dim tkInfoLength As Integer = 0
            Dim privilegeState As PrivilegeStates

            Try

                ' Get the source process token.
                If Not NativeMethods.OpenProcessToken(handle, TokenAccess.Query, hToken) Then
                    win32Err = Marshal.GetLastWin32Error()
                    Throw New Win32Exception(win32Err)
                End If

                ' Here we call GetTokenInformation the first time to receive the length of the data it would like to store.
                NativeMethods.GetTokenInformation(hToken, TokenInformationClass.TokenPrivileges, IntPtr.Zero, tkInfoLength, tkInfoLength)
                win32Err = Marshal.GetLastWin32Error()

                ' Since the current length we pass is 0, we'll always get expected error-code "122", which is fine.
                ' We also get the required length returned.
                If (win32Err <> 122) Then
                    Throw New Win32Exception(win32Err)

                Else
                    ' Here we allocate memory for receiving the actual data.
                    ' By Now, "tkInfoLength" contains the size of the memory block we need to allocate.
                    tkpHandle = Marshal.AllocHGlobal(tkInfoLength)

                    ' This time, we shouldn't get an error-code "122", because this time we have set the correct buffer size. 
                    ' "GetTokenInformation" should now write the data into the memory block we just allocated.
                    If Not NativeMethods.GetTokenInformation(hToken, TokenInformationClass.TokenPrivileges, tkpHandle, tkInfoLength, tkInfoLength) Then
                        win32Err = Marshal.GetLastWin32Error()
                        Throw New Win32Exception(win32Err)

                    Else
                        ' We will now ask "PtrToStructure" to read the raw data out of the memory block 
                        ' and convert it to a managed structure of type "TokenPrivileges" which we can use in our code.
                        tkp = DirectCast(Marshal.PtrToStructure(tkpHandle, GetType(TokenPrivileges)), TokenPrivileges)

                        ' We have to iterate over all privileges listed in the "TokenPrivileges" structure 
                        ' to find the one we are looking for.
                        Dim found As Boolean = False
                        For i As Integer = 0 To CInt(tkp.PrivilegeCount - 1I)
                            ' There is a problem: 
                            ' "Marshal.PtrToStructure" can't marshal variable-length structures, 
                            ' but the array "TokenPrivileges::Privileges" has a variable length determined by the value of "TokenPrivileges::PrivilegeCount",
                            ' Since we don't know the size at compile time, the size of the array was hardcoded to 1, 
                            ' which means that we would only be able to access the first element of the array.
                            '
                            ' To work around this, we calculate the raw memory offset pointing to the array element we need 
                            ' and load it separately into a "LuidAndAttributes" structure.
                            ' The way this works is: The contents of the "TokenPrivilege" structure or stored in memory one after another, like this:
                            '   PrivilegeCount (type: UInteger)
                            '   Privileges(0) (type: LuidAndAttributes)
                            '   Privileges(1) (type: LuidAndAttributes) << these and all further we normally can't access
                            '   Privileges(2) (type: LuidAndAttributes)
                            ' ...and so on.
                            ' We are now calculating the offset into the structure for a specific array element. 
                            ' Let's use Privileges(2) as example:
                            ' To get to it, we need to take the pointer to the beginning of the structure and add the sizes of all previous elements,
                            ' which would be once the size of "PrivilegeCount" and then 2 times the size of a "LuidAndAttributes" structure.
                            Dim directPointer As New IntPtr(tkpHandle.ToInt64() + Len(tkp.PrivilegeCount) + i * Marshal.SizeOf(GetType(LuidAndAttributes)))
                            Dim luidAndAttributes As LuidAndAttributes = DirectCast(Marshal.PtrToStructure(directPointer, GetType(LuidAndAttributes)), LuidAndAttributes)

                            ' Get the privilege name. We first call "LookupPrivilegeName" with a zero size to get the real size we need, then reserve space, then get the actual data.
                            '
                            ' NOTE: This will not be really necessary in case we already have the right privilege's LUID assigned in "luAttr.Luid" field.
                            Dim privNameLen As Integer = 0
                            Dim sb As New System.Text.StringBuilder()
                            NativeMethods.LookupPrivilegeName(Nothing, luidAndAttributes.Luid, sb, privNameLen)
                            sb.EnsureCapacity(privNameLen + 1)
                            If Not NativeMethods.LookupPrivilegeName(Nothing, luidAndAttributes.Luid, sb, privNameLen) Then
                                win32Err = Marshal.GetLastWin32Error()
                                Throw New Win32Exception(win32Err)
                            End If

                            ' Now that we have the name, we can check if it's the one we are looking for.
                            If (sb.ToString() = privilegeName) Then
                                ' Found! So we can finally get the status of the privilege!
                                found = True
                                privilegeState = luidAndAttributes.Attributes
                                Exit For
                            End If

                        Next i

                        If Not (found) Then
                            privilegeState = PrivilegeStates.PrivilegeRemoved
                        End If

                        Return privilegeState

                    End If

                End If

            Catch ex As Win32Exception
                Throw

            Finally ' Make sure the memory blocks are freed again (even when an error occured).
                If (tkpHandle <> IntPtr.Zero) Then
                    Marshal.FreeHGlobal(tkpHandle)
                End If

                If (hToken <> IntPtr.Zero) Then
                    NativeMethods.CloseHandle(hToken)
                End If

            End Try

        End Function

#End Region

    End Class

End Namespace

#End Region
