Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Net.WebSockets
Imports System.Threading
Imports Newtonsoft.Json.Linq
Imports System.Security.Cryptography

Public Class BlazingWebsockets
    'Server vars
    Public clientSockets As New Dictionary(Of String, WebSocket)
    Public clientFiles As New Dictionary(Of String, List(Of String))
    Public clientFileSizes As New Dictionary(Of String, List(Of String))
    Public clientHashes As New Dictionary(Of String, List(Of String))
    Public clientRecvIndex As New Dictionary(Of String, Integer)
    Public clientTransfer As New Dictionary(Of String, TransferInfo)
    Public serverOutbox As New List(Of String)
    Public setServerID As String = "server"
    Public lastClientID As String = ""
    Public serverRecvPath As String = Application.StartupPath & "\server"
    Public serverPort As String = "3800"
    Public knownPassword As String = "password123"  'password which server expects from client

    'Client vars
    Public serverFiles As List(Of String)
    Public serverFileSizes As List(Of String)
    Public serverHashes As List(Of String)
    Public serverRecvIndex As Integer
    Public serverTransfer As TransferInfo
    Public clientOutbox As New List(Of String)
    Public setClientID As String = "client123"
    Public setPassword As String = "password123"    'password which client sends to server
    Public clientRecvPath As String = Application.StartupPath & "\client"
    Public clientHost As String = "localhost"
    Public clientPort As String = "3800"
    Public clientWS = New ClientWebSocket()

    'Common
    Public Class TransferInfo
        Public bytesCurrent As Long
        Public bytesTotal As Long
        Public filesCurrent As Integer
        Public filesTotal As Integer
        Public fileName As String
        Public status As String
    End Class

    Public Function validateUser(clientID As String, password As String) As Boolean
        If password = knownPassword Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetMD5Hash(filePath As String) As String
        Using md5 As MD5 = MD5.Create()
            Using stream = File.OpenRead(filePath)
                Dim hash = md5.ComputeHash(stream)
                Return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant()
            End Using
        End Using
    End Function

    Public Sub MakeDirectoryIfNotExist(ByVal dirPath As String)
        Try
            If My.Computer.FileSystem.DirectoryExists(dirPath) <> True Then
                My.Computer.FileSystem.CreateDirectory(dirPath)
            End If
        Catch
        End Try
    End Sub

    Public Function IsValidJson(ByVal str As String) As Boolean
        Try
            JToken.Parse(str)
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Function BytesToString(ByVal byteCount As Long) As String
        Dim sizes As String() = {"B", "KB", "MB", "GB", "TB"}
        Dim order As Integer = 0

        While byteCount >= 1024 AndAlso order < sizes.Length - 1
            order += 1
            byteCount /= 1024
        End While

        Return String.Format("{0:0.0} {1}", byteCount, sizes(order))
    End Function

    Public Interface IBlazingWebsocketsCallbacks
        Sub UpdateServerInterface(ByVal details As TransferInfo)
        Sub UpdateClientInterface(ByVal details As TransferInfo)
        Sub ServerIncomingMsg(ByVal msg As String)
        Sub ClientIncomingMsg(ByVal msg As String)
    End Interface

    Private callbacks As IBlazingWebsocketsCallbacks

    Public Sub New(callbacks As IBlazingWebsocketsCallbacks)
        Me.callbacks = callbacks
    End Sub

    Public Async Sub StartServer()
        Try
            Dim listener As New HttpListener()
            listener.Prefixes.Add("http://*:" & serverPort & "/")
            listener.Start()

            Dim activeSocket As WebSocket = Nothing

            While True
                Try
                    Dim context = Await listener.GetContextAsync()
                    If context.Request.IsWebSocketRequest Then
                        Dim webSocketContext = Await context.AcceptWebSocketAsync(subProtocol:=Nothing)
                        Dim socket = webSocketContext.WebSocket
                        activeSocket = socket 'update socket to catch any potential errors

                        Dim buffer As New ArraySegment(Of Byte)(New Byte(8191) {})

                        While socket.State = WebSocketState.Open
                            Dim result = Await socket.ReceiveAsync(buffer, CancellationToken.None)
                            'Console.WriteLine("Server - result.count: " & result.Count)

                            If result.MessageType = WebSocketMessageType.Text Then
                                Dim receivedMsg = System.Text.Encoding.UTF8.GetString(buffer.Array, 0, result.Count)
                                If IsValidJson(receivedMsg) = True Then
                                    Dim missingParameter As Boolean = False

                                    ' Deserialize JSON and validate password, etc.
                                    Dim header As JObject = JObject.Parse(receivedMsg)

                                    Dim clientIDToken As JToken = Nothing
                                    Dim passwordToken As JToken = Nothing
                                    Dim fileListToken As JToken = Nothing
                                    Dim fileSizesToken As JToken = Nothing
                                    Dim fileHashesToken As JToken = Nothing
                                    Dim actionToken As JToken = Nothing
                                    Dim clientID As String = ""
                                    Dim password As String = ""
                                    Dim fileList As New List(Of String)
                                    Dim fileSizes As New List(Of String)
                                    Dim fileHashes As New List(Of String)
                                    Dim actionText As String = ""

                                    'Gracefully get JSON value
                                    If header.TryGetValue("clientID", clientIDToken) Then
                                        clientID = clientIDToken.ToString()
                                        lastClientID = clientID
                                    Else
                                        missingParameter = True
                                    End If

                                    If header.TryGetValue("password", passwordToken) Then
                                        password = passwordToken.ToString()
                                    Else
                                        missingParameter = True
                                    End If

                                    If header.TryGetValue("fileList", fileListToken) Then
                                        fileList = fileListToken.ToObject(Of List(Of String))()
                                    Else
                                        missingParameter = True
                                    End If

                                    If header.TryGetValue("fileSizes", fileSizesToken) Then
                                        fileSizes = fileSizesToken.ToObject(Of List(Of String))()
                                    Else
                                        missingParameter = True
                                    End If

                                    If header.TryGetValue("fileHashes", fileHashesToken) Then
                                        fileHashes = fileHashesToken.ToObject(Of List(Of String))()
                                    Else
                                        missingParameter = True
                                    End If

                                    If header.TryGetValue("action", actionToken) Then
                                        actionText = actionToken.ToString()
                                    End If

                                    Console.WriteLine("Client ID: " & clientID)

                                    If missingParameter = True Then
                                        If actionText = "intro" Then
                                            missingParameter = False
                                            clientSockets(clientID) = socket
                                            Await ServerSendString(socket, "Welcome " & clientID)
                                            callbacks.ServerIncomingMsg("Client connected: " & clientID)
                                        Else
                                            Console.WriteLine("MissingParameters")
                                            ' Invalid JSON
                                            Await ServerSendString(socket, "MissingParameters")
                                        End If
                                    Else
                                        If validateUser(clientID, password) Then
                                            clientSockets(clientID) = socket
                                            clientRecvIndex(clientID) = 0   'reset index
                                            ' Assign the lists directly
                                            clientFiles(clientID) = fileList
                                            clientFileSizes(clientID) = fileSizes
                                            clientHashes(clientID) = fileHashes

                                            ' Make sure file count matches file sizes
                                            If fileList.Count = fileSizes.Count And fileList.Count = fileHashes.Count And fileList.Count > 0 Then    ' Great, it matches
                                                clientTransfer(clientID) = New TransferInfo
                                                clientTransfer(clientID).filesTotal = fileList.Count
                                                clientTransfer(clientID).status = "Ready To Receive"
                                                clientTransfer(clientID).fileName = fileList(0)
                                                clientTransfer(clientID).filesCurrent = 1

                                                For Each fileName In fileList
                                                    Console.WriteLine("File: " & fileName.ToString)
                                                Next
                                                For Each fileSize In fileSizes
                                                    Console.WriteLine("File Size: " & fileSize.ToString)
                                                Next
                                                For Each hash In fileHashes
                                                    Console.WriteLine("MD5: " & hash.ToString)
                                                Next

                                                ' Respond to the client with some message if you want, and prepare to receive files
                                                Await ServerSendString(socket, "ReadyToReceive")
                                            Else
                                                ' Files do not match
                                                Await ServerSendString(socket, "FileCountMismatch")
                                                Await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None)
                                            End If

                                        Else    ' Access denied. Close the socket for this user.
                                            Await ServerSendString(socket, "WrongPassword")
                                            Await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None)
                                        End If
                                    End If
                                Else    'Primitive indicators
                                    ' Handle received message and decide whether to send or receive files
                                    If receivedMsg = "ReadyToReceive" Then
                                        Await Task.Delay(1000)
                                        ' Proceed to send the file
                                        Console.WriteLine("Client is ready. Sending now...")
                                        If lastClientID <> "" Then
                                            Dim ind As Integer = 0
                                            For Each filepath In serverOutbox
                                                clientTransfer(lastClientID).filesCurrent = ind + 1
                                                clientTransfer(lastClientID).status = "Sending file " & clientTransfer(lastClientID).filesCurrent.ToString & " of " & clientTransfer(lastClientID).filesTotal
                                                Await ServerSendFile(socket, filepath)
                                                ind += 1
                                            Next
                                            ' All files sent
                                            clientTransfer(lastClientID).status = "All files sent!"
                                            'Invoke callback
                                            callbacks.UpdateServerInterface(clientTransfer(lastClientID))
                                        End If
                                    ElseIf receivedMsg = "TransferSuccessful" Then
                                        'Great, lets request to close the connection
                                        'Await Task.Delay(1000)
                                        'Await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None)
                                    Else
                                        'Invoke callback
                                        callbacks.ServerIncomingMsg(receivedMsg)
                                    End If
                                End If


                            ElseIf result.MessageType = WebSocketMessageType.Binary Then
                                'Find the clientID by looking up the socket
                                Dim keyValue = clientSockets.FirstOrDefault(Function(kvp) kvp.Value Is socket)

                                If Not keyValue.Equals(New KeyValuePair(Of String, WebSocket)(Nothing, Nothing)) Then
                                    Dim clientID As String = keyValue.Key

                                    If clientRecvIndex.ContainsKey(clientID) AndAlso clientFiles.ContainsKey(clientID) AndAlso clientFileSizes.ContainsKey(clientID) Then
                                        Dim recvIndex As Integer = clientRecvIndex(clientID)
                                        Dim recvName As String = clientFiles(clientID)(recvIndex)
                                        Dim recvBytes As Long = clientFileSizes(clientID)(recvIndex)
                                        Dim totalFiles As Integer = clientFiles(clientID).Count
                                        Dim expectedHash As String = clientHashes(clientID)(recvIndex)

                                        clientTransfer(clientID).status = "Receiving file " & (recvIndex + 1).ToString & " of " & totalFiles
                                        clientTransfer(clientID).bytesTotal = recvBytes
                                        clientTransfer(clientID).bytesCurrent = 0
                                        clientTransfer(clientID).fileName = recvName
                                        clientTransfer(clientID).filesCurrent = recvIndex + 1
                                        clientTransfer(clientID).filesTotal = totalFiles

                                        ' Update user file(s) being received
                                        'Invoke callback
                                        callbacks.UpdateServerInterface(clientTransfer(clientID))

                                        MakeDirectoryIfNotExist(serverRecvPath)
                                        Dim recvPath As String = serverRecvPath & "\" & recvName
                                        Console.WriteLine("Server: Storing file " & recvPath)
                                        Await ServerReceiveFile(socket, recvPath, recvBytes, buffer.Array, result.Count)
                                        Dim checksum As String = GetMD5Hash(recvPath)
                                        If checksum <> expectedHash Then
                                            MsgBox("The file (" & recvName & ") may be corrupted. Try sending again")
                                        End If

                                        recvIndex += 1
                                        clientRecvIndex(clientID) += recvIndex 'advance to the next file

                                        ' Check if all files have been received
                                        If recvIndex >= totalFiles Then
                                            ' All files received
                                            clientTransfer(clientID).status = "All files received!"
                                            'Invoke callback
                                            callbacks.UpdateServerInterface(clientTransfer(clientID))
                                            Await ServerSendString(socket, "TransferSuccessful")
                                        End If
                                    Else
                                        ' If the clientID is not found in the dictionaries
                                        ' Close the socket, as it's not authorized to send binary data
                                        Await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Unauthorized client", CancellationToken.None)
                                    End If
                                End If
                            ElseIf result.MessageType = WebSocketMessageType.Close Then
                                Await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Acknowledged close", CancellationToken.None)
                            End If
                        End While
                    Else
                        context.Response.StatusCode = 400
                        context.Response.Close()
                    End If
                Catch wsEx As WebSocketException
                    If wsEx.WebSocketErrorCode = WebSocketError.Success OrElse wsEx.WebSocketErrorCode = WebSocketError.ConnectionClosedPrematurely Then
                        If activeSocket IsNot Nothing Then  ' <- Use it here
                            Dim clientID As String = clientSockets.FirstOrDefault(Function(kvp) kvp.Value Is activeSocket).Key
                            If Not String.IsNullOrEmpty(clientID) Then
                                callbacks.ServerIncomingMsg("Client disconnected: " & clientID)
                                'Console.WriteLine("ClientID " & clientID & " disconnected. Removing from list.")
                                RemoveClientFromList(clientID)
                            End If
                        End If
                        Continue While
                    Else
                        ' Handle other WebSocket exceptions here, if needed
                        Console.WriteLine($"Websocket Ex: {wsEx.WebSocketErrorCode.ToString }")
                    End If
                Catch ex As Exception
                    ' Handle other general exceptions here
                    Console.WriteLine($"Websocket Error: {ex.Message}")
                End Try
            End While

        Catch ex As Exception
            MsgBox("Failed to start server! " & ex.Message)
            Exit Sub
        End Try
    End Sub

    Public Sub RemoveClientFromList(clientID As String)

        If clientSockets.ContainsKey(clientID) Then
            clientSockets.Remove(clientID)
            lastClientID = ""
        End If

        If clientFiles.ContainsKey(clientID) Then
            clientFiles.Remove(clientID)
        End If

        If clientFileSizes.ContainsKey(clientID) Then
            clientFileSizes.Remove(clientID)
        End If

        If clientHashes.ContainsKey(clientID) Then
            clientHashes.Remove(clientID)
        End If

        If clientRecvIndex.ContainsKey(clientID) Then
            clientRecvIndex.Remove(clientID)
        End If

        If clientTransfer.ContainsKey(clientID) Then
            clientTransfer.Remove(clientID)
        End If
    End Sub

    Public Async Function ServerSendString(socket As WebSocket, msg As String) As Task
        Dim serverMsgBytes = System.Text.Encoding.UTF8.GetBytes(msg)
        Await socket.SendAsync(New ArraySegment(Of Byte)(serverMsgBytes), WebSocketMessageType.Text, True, CancellationToken.None)
    End Function

    Public Async Function ServerSendFiles(clientID As String, filepaths As List(Of String)) As Task
        If Not clientSockets.ContainsKey(clientID) Then
            Console.WriteLine("ClientID not found.")
            Return
        End If

        Dim socket As WebSocket = clientSockets(clientID)

        If socket.State <> WebSocketState.Open Then
            Console.WriteLine("WebSocket is not connected!")
            Return
        End If

        serverOutbox = filepaths
        Dim sendFiles As New List(Of String)
        Dim sendFileSizes As New List(Of String)
        Dim sendFileHashes As New List(Of String)

        For Each filepath In filepaths
            Dim fileBytes As Long = My.Computer.FileSystem.GetFileInfo(filepath).Length
            Dim fileName As String = Path.GetFileName(filepath)
            Dim checksum As String = GetMD5Hash(filepath)
            sendFiles.Add(fileName)
            sendFileSizes.Add(fileBytes)
            sendFileHashes.Add(checksum)
        Next

        clientTransfer(clientID) = New TransferInfo
        clientTransfer(clientID).filesTotal = sendFiles.Count
        clientTransfer(clientID).status = "Ready To Send"
        clientTransfer(clientID).fileName = sendFiles(0)
        clientTransfer(clientID).filesCurrent = 1

        ' Sending Header
        Dim header As New JObject(
                New JProperty("clientID", setServerID),
                New JProperty("password", setPassword),
                New JProperty("fileList", New JArray(sendFiles.ToArray())),
                New JProperty("fileSizes", New JArray(sendFileSizes.ToArray())),
                New JProperty("fileHashes", New JArray(sendFileHashes.ToArray()))
            )

        Dim headerBytes = System.Text.Encoding.UTF8.GetBytes(header.ToString())
        Await socket.SendAsync(New ArraySegment(Of Byte)(headerBytes), WebSocketMessageType.Text, True, CancellationToken.None)
        lastClientID = clientID

    End Function

    Private Async Function ServerSendFile(client As WebSocket, filepath As String) As Task
        Try
            Using fs As New FileStream(filepath, FileMode.Open, FileAccess.Read)
                Dim buffer As New ArraySegment(Of Byte)(New Byte(8191) {})
                'Console.WriteLine($"File length: {fs.Length}")  ' Debug line 1
                'Console.WriteLine($"Buffer size: {buffer.Array.Length}")  ' Debug line 2
                Dim bytesRead As Integer = fs.Read(buffer.Array, 0, buffer.Array.Length)
                While bytesRead > 0
                    Await client.SendAsync(New ArraySegment(Of Byte)(buffer.Array, 0, bytesRead), WebSocketMessageType.Binary, True, CancellationToken.None)
                    ' Update ProgressBar
                    Dim clientID As String = clientSockets.FirstOrDefault(Function(kvp) kvp.Value Is client).Key

                    clientTransfer(clientID).bytesTotal = fs.Length  ' Entire file size
                    clientTransfer(clientID).bytesCurrent = fs.Position  ' Current read position
                    clientTransfer(clientID).fileName = Path.GetFileName(filepath)

                    'Invoke callback
                    callbacks.UpdateServerInterface(clientTransfer(clientID))

                    bytesRead = fs.Read(buffer.Array, 0, buffer.Array.Length)
                End While
            End Using
            Console.WriteLine("Server Sending complete")
        Catch ex As Exception
            Console.WriteLine("Error in Server SendFile: " + ex.Message)
        End Try
    End Function

    Private Async Function ServerReceiveFile(socket As WebSocket, filename As String, TotalFileLength As Long, initialData As Byte(), initialCount As Integer) As Task
        Using fs As New FileStream(filename, FileMode.Create, FileAccess.Write)
            fs.Write(initialData, 0, initialCount)  ' Write the initial data chunk
            fs.Flush()

            Dim buffer As Byte() = New Byte(8191) {}
            Dim bytesReceived As Long = initialCount  ' Initialize bytesReceived with initialCount
            Dim receivedBuffer As New ArraySegment(Of Byte)(buffer)

            While bytesReceived < TotalFileLength
                Dim result = Await socket.ReceiveAsync(receivedBuffer, CancellationToken.None)

                If result.MessageType = WebSocketMessageType.Binary Then
                    fs.Write(buffer, 0, result.Count)
                    fs.Flush() ' Flush the stream
                    bytesReceived += result.Count

                    Dim clientID As String = clientSockets.FirstOrDefault(Function(kvp) kvp.Value Is socket).Key
                    clientTransfer(clientID).bytesCurrent = bytesReceived
                    clientTransfer(clientID).bytesTotal = TotalFileLength

                    'Invoke callback
                    callbacks.UpdateServerInterface(clientTransfer(clientID))

                ElseIf result.MessageType = WebSocketMessageType.Close Then
                    Await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "File received", CancellationToken.None)
                    fs.Flush() ' Flush the stream
                    fs.Close() ' Explicitly close the FileStream
                    Exit While
                Else
                End If
            End While
        End Using
    End Function

    Public Async Sub StartClient()
        Try
            clientWS = New ClientWebSocket()
            Dim cts As New CancellationTokenSource()

            Await clientWS.ConnectAsync(New Uri("ws://" & clientHost & ":" & clientPort), cts.Token)

            'Prepare to send some files
            'clientOutbox = ListBox1.Items.Cast(Of String)().ToList()
            'Await ClientSendFiles(clientWS, clientOutbox)
            Await ClientSendIntro(clientWS)

            ' Prepare to receive or send files
            Dim buffer As New ArraySegment(Of Byte)(New Byte(8191) {})
            While clientWS.State = WebSocketState.Open
                Dim result = Await clientWS.ReceiveAsync(buffer, cts.Token)

                If result.MessageType = WebSocketMessageType.Text Then
                    Dim receivedMsg = System.Text.Encoding.UTF8.GetString(buffer.Array, 0, result.Count)
                    If IsValidJson(receivedMsg) = True Then
                        Dim missingParameter As Boolean = False

                        ' Deserialize JSON and validate password, etc.
                        Dim recvHeader As JObject = JObject.Parse(receivedMsg)

                        Dim clientIDToken As JToken = Nothing
                        Dim passwordToken As JToken = Nothing
                        Dim fileListToken As JToken = Nothing
                        Dim fileSizesToken As JToken = Nothing
                        Dim fileHashesToken As JToken = Nothing
                        Dim clientID As String = ""
                        Dim password As String = ""
                        Dim fileList As New List(Of String)
                        Dim fileSizes As New List(Of String)
                        Dim fileHashes As New List(Of String)

                        'Gracefully get JSON value
                        If recvHeader.TryGetValue("clientID", clientIDToken) Then
                            clientID = clientIDToken.ToString()
                        Else
                            missingParameter = True
                        End If

                        If recvHeader.TryGetValue("password", passwordToken) Then
                            password = passwordToken.ToString()
                        Else
                            missingParameter = True
                        End If

                        If recvHeader.TryGetValue("fileList", fileListToken) Then
                            fileList = fileListToken.ToObject(Of List(Of String))()
                        Else
                            missingParameter = True
                        End If

                        If recvHeader.TryGetValue("fileSizes", fileSizesToken) Then
                            fileSizes = fileSizesToken.ToObject(Of List(Of String))()
                        Else
                            missingParameter = True
                        End If

                        If recvHeader.TryGetValue("fileHashes", fileHashesToken) Then
                            fileHashes = fileHashesToken.ToObject(Of List(Of String))()
                        Else
                            missingParameter = True
                        End If

                        Console.WriteLine("Client ID: " & clientID)

                        If missingParameter = True Then
                            Console.WriteLine("MissingParameters")
                            Await clientWS.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None)
                        Else
                            If validateUser(clientID, password) Then
                                serverRecvIndex = 0   'reset index
                                ' Assign the lists directly
                                serverFiles = fileList
                                serverFileSizes = fileSizes
                                serverHashes = fileHashes

                                ' Make sure file count matches file sizes
                                If fileList.Count = fileSizes.Count And fileList.Count = fileHashes.Count And fileList.Count > 0 Then    ' Great, it matches
                                    serverTransfer = New TransferInfo
                                    serverTransfer.filesTotal = fileList.Count
                                    serverTransfer.status = "Ready To Receive"
                                    serverTransfer.fileName = fileList(0)
                                    serverTransfer.filesCurrent = 1

                                    For Each fileName In fileList
                                        Console.WriteLine("File: " & fileName.ToString)
                                    Next
                                    For Each fileSize In fileSizes
                                        Console.WriteLine("File Size: " & fileSize.ToString)
                                    Next
                                    For Each hash In fileHashes
                                        Console.WriteLine("MD5: " & hash.ToString)
                                    Next

                                    ' Respond to the client with some message if you want, and prepare to receive files
                                    Await ClientSendString(clientWS, "ReadyToReceive")
                                Else
                                    ' Files do not match
                                    Await ClientSendString(clientWS, "FileCountMismatch")
                                End If

                            Else    ' Access denied. Close the socket for this user.
                                Await ClientSendString(clientWS, "WrongPassword")
                                Await clientWS.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None)
                            End If
                        End If

                    Else
                        ' Handle received message and decide whether to send or receive files
                        If receivedMsg = "ReadyToReceive" Then
                            Console.WriteLine("Client: " & receivedMsg)
                            Await Task.Delay(1000)
                            ' Proceed to send the file
                            Console.WriteLine("Server is ready. Sending now...")
                            Dim ind As Integer = 0
                            For Each filepath In clientOutbox
                                serverTransfer.filesCurrent = ind + 1
                                serverTransfer.status = "Sending file " & serverTransfer.filesCurrent.ToString & " of " & serverTransfer.filesTotal
                                Await ClientSendFile(clientWS, filepath)
                                ind += 1
                            Next

                            ' All files sent
                            serverTransfer.status = "All files sent!"
                            'Invoke callback
                            callbacks.UpdateClientInterface(serverTransfer)

                        ElseIf receivedMsg = "TransferSuccessful" Then
                            'Great, lets request to close the connection
                            'Await Task.Delay(1000)
                            'Await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None)
                        Else
                            'Invoke callback
                            callbacks.ClientIncomingMsg(receivedMsg)
                        End If
                    End If

                ElseIf result.MessageType = WebSocketMessageType.Binary Then
                    Console.WriteLine("Client: I'm receiving a file...")

                    Dim recvIndex As Integer = serverRecvIndex
                    Dim recvName As String = serverFiles(recvIndex)
                    Dim recvBytes As Long = serverFileSizes(recvIndex)
                    Dim totalFiles As Integer = serverFiles.Count
                    Dim expectedHash As String = serverHashes(recvIndex)

                    serverTransfer.status = "Receiving file " & (recvIndex + 1).ToString & " of " & totalFiles
                    serverTransfer.bytesTotal = recvBytes
                    serverTransfer.bytesCurrent = 0
                    serverTransfer.fileName = recvName
                    serverTransfer.filesCurrent = recvIndex + 1
                    serverTransfer.filesTotal = totalFiles

                    'Invoke callback
                    callbacks.UpdateClientInterface(serverTransfer)

                    MakeDirectoryIfNotExist(clientRecvPath)
                    Dim recvPath As String = clientRecvPath & "\" & recvName
                    Console.WriteLine("Client: Storing file " & recvPath)
                    Await ClientReceiveFile(clientWS, recvPath, recvBytes, buffer.Array, result.Count)
                    Dim checksum As String = GetMD5Hash(recvPath)
                    If checksum <> expectedHash Then
                        MsgBox("The file (" & recvName & ") may be corrupted. Try sending again")
                    End If

                    recvIndex += 1
                    serverRecvIndex += recvIndex 'advance to the next file

                    ' Check if all files have been received
                    If recvIndex >= totalFiles Then
                        ' All files received
                        serverTransfer.status = "All files received!"
                        'Invoke callback
                        callbacks.UpdateClientInterface(serverTransfer)

                        Await ClientSendString(clientWS, "TransferSuccessful")
                    End If
                End If
            End While
            Console.WriteLine("Client connection closed")

        Catch ex As Exception
            ' Handle exception (for instance, if server is offline)
            MessageBox.Show("An error occurred: " + ex.Message)
        End Try
    End Sub

    Public Async Function ClientSendString(client As ClientWebSocket, msg As String) As Task
        Dim clientMsgBytes = System.Text.Encoding.UTF8.GetBytes(msg)
        Await client.SendAsync(New ArraySegment(Of Byte)(clientMsgBytes), WebSocketMessageType.Text, True, CancellationToken.None)
    End Function

    Public Async Function ClientSendIntro(client As ClientWebSocket) As Task
        If client.State <> WebSocketState.Open Then
            Console.WriteLine("WebSocket is not connected!")
            Return
        End If

        ' Sending Header
        Dim header As New JObject(
                New JProperty("clientID", setClientID),
                New JProperty("password", setPassword),
                New JProperty("action", "intro")
        )

        Dim headerBytes = System.Text.Encoding.UTF8.GetBytes(header.ToString())
        Await client.SendAsync(New ArraySegment(Of Byte)(headerBytes), WebSocketMessageType.Text, True, CancellationToken.None)

    End Function

    Public Async Function ClientSendFiles(client As ClientWebSocket, filepaths As List(Of String)) As Task
        If client.State <> WebSocketState.Open Then
            Console.WriteLine("WebSocket is not connected!")
            Return
        End If

        clientOutbox = filepaths
        Dim sendFiles As New List(Of String)
        Dim sendFileSizes As New List(Of String)
        Dim sendFileHashes As New List(Of String)

        For Each filepath In filepaths
            Dim fileBytes As Long = My.Computer.FileSystem.GetFileInfo(filepath).Length
            Dim fileName As String = Path.GetFileName(filepath)
            Dim checksum As String = GetMD5Hash(filepath)
            sendFiles.Add(fileName)
            sendFileSizes.Add(fileBytes)
            sendFileHashes.Add(checksum)
        Next

        serverTransfer = New TransferInfo
        serverTransfer.filesTotal = sendFiles.Count
        serverTransfer.status = "Ready To Send"
        serverTransfer.fileName = sendFiles(0)
        serverTransfer.filesCurrent = 1

        ' Sending Header
        Dim header As New JObject(
                New JProperty("clientID", setClientID),
                New JProperty("password", setPassword),
                New JProperty("fileList", New JArray(sendFiles.ToArray())),
                New JProperty("fileSizes", New JArray(sendFileSizes.ToArray())),
                New JProperty("fileHashes", New JArray(sendFileHashes.ToArray()))
            )

        Dim headerBytes = System.Text.Encoding.UTF8.GetBytes(header.ToString())
        Await client.SendAsync(New ArraySegment(Of Byte)(headerBytes), WebSocketMessageType.Text, True, CancellationToken.None)

    End Function

    Private Async Function ClientSendFile(client As ClientWebSocket, filepath As String) As Task
        If client.State <> WebSocketState.Open Then
            MsgBox("Websocket is not connected!")
            Return
        End If

        Try
            Using fs As New FileStream(filepath, FileMode.Open, FileAccess.Read)
                Dim buffer As New ArraySegment(Of Byte)(New Byte(8191) {})
                'Console.WriteLine($"File length: {fs.Length}")  ' Debug line 1
                'Console.WriteLine($"Buffer size: {buffer.Array.Length}")  ' Debug line 2
                Dim bytesRead As Integer = fs.Read(buffer.Array, 0, buffer.Array.Length)
                While bytesRead > 0
                    Await client.SendAsync(New ArraySegment(Of Byte)(buffer.Array, 0, bytesRead), WebSocketMessageType.Binary, True, CancellationToken.None)

                    serverTransfer.bytesTotal = fs.Length  ' Entire file size
                    serverTransfer.bytesCurrent = fs.Position  ' Current read position
                    serverTransfer.fileName = Path.GetFileName(filepath)

                    'Invoke callback
                    callbacks.UpdateClientInterface(serverTransfer)

                    bytesRead = fs.Read(buffer.Array, 0, buffer.Array.Length)
                End While
            End Using
            Console.WriteLine("Sending complete")
        Catch ex As Exception
            Console.WriteLine("Error in Client SendFile: " + ex.Message)
        End Try
    End Function

    Private Async Function ClientReceiveFile(client As ClientWebSocket, filename As String, TotalFileLength As Long, initialData As Byte(), initialCount As Integer) As Task
        Using fs As New FileStream(filename, FileMode.Create, FileAccess.Write)
            fs.Write(initialData, 0, initialCount)  ' Write the initial data chunk
            fs.Flush()

            Dim buffer As Byte() = New Byte(8191) {}
            Dim bytesReceived As Long = initialCount  ' Initialize bytesReceived with initialCount
            Dim receivedBuffer As New ArraySegment(Of Byte)(buffer)

            While bytesReceived < TotalFileLength
                Dim result = Await client.ReceiveAsync(receivedBuffer, CancellationToken.None)

                If result.MessageType = WebSocketMessageType.Binary Then
                    fs.Write(buffer, 0, result.Count)
                    fs.Flush() ' Flush the stream
                    bytesReceived += result.Count

                    serverTransfer.bytesCurrent = bytesReceived
                    serverTransfer.bytesTotal = TotalFileLength

                    'Invoke callback
                    callbacks.UpdateClientInterface(serverTransfer)

                ElseIf result.MessageType = WebSocketMessageType.Close Then
                    Await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "File received", CancellationToken.None)
                    fs.Flush() ' Flush the stream
                    fs.Close() ' Explicitly close the FileStream
                    Exit While
                Else
                End If
            End While
        End Using
    End Function
End Class
