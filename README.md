# Blazing Websockets
Websocket Server and Client for VB.NET that can send multiple files bidirectionally with progress bar

## Overview ##
Sending files programmatically has always been something that involves a tedious amount of work. Of course, there's great cloud tools like Google Drive or Dropbox. However, for enterprise solution, sometimes a dedicated solution is more secure and has lower latency. This repo contains a demonstration program that sends and receives files within itself. Obviously, the server and client can be split into 2 applications and run on separate machines.

## Usage ##
Make sure to have the `BlazingWebsockets` file in your VB.NET program. Then include it in your main code:

```vbnet
Imports FileWebsockets.BlazingWebsockets
```

Now implement the interfaces to attach the callbacks to your code. You'll want to rate limit your UI updates as shown, so that the form doesn't freeze up:
```vbnet
Implements IBlazingWebsocketsCallbacks
Private websockets As BlazingWebsockets

Dim lastUpdateServer As DateTime = DateTime.MinValue
Dim lastUpdateClient As DateTime = DateTime.MinValue

Public Sub UpdateServerInterface(details As TransferInfo) Implements IBlazingWebsocketsCallbacks.UpdateServerInterface
    Me.Invoke(Sub()
                  Dim rawPercent As Double = (details.bytesCurrent / details.bytesTotal) * 100
                  Dim percent As Integer = CInt(Math.Max(0, Math.Min(100, rawPercent)))   'constrain between 0-100
                  ProgressBar1.Value = percent

                  Dim now = DateTime.Now
                  If (now - lastUpdateServer).TotalMilliseconds > 300 Or percent > 98 Then
                      Dim humanRecvBytes As String = websockets.BytesToString(details.bytesCurrent)
                      Dim humanTotalFileSize As String = websockets.BytesToString(details.bytesTotal)
                      lblServerProgress.Text = percent.ToString & "% (" & humanRecvBytes & " of " & humanTotalFileSize & ")"
                      lblServerStatus.Text = details.status
                      Me.Refresh()
                      lastUpdateServer = now
                  End If

              End Sub)
End Sub

Public Sub UpdateClientInterface(details As TransferInfo) Implements IBlazingWebsocketsCallbacks.UpdateClientInterface
    Me.Invoke(Sub()
                  Dim rawPercent As Double = (details.bytesCurrent / details.bytesTotal) * 100
                  Dim percent As Integer = CInt(Math.Max(0, Math.Min(100, rawPercent)))   'constrain between 0-100
                  ProgressBar2.Value = percent

                  Dim now = DateTime.Now
                  If (now - lastUpdateClient).TotalMilliseconds > 500 Or percent > 98 Then
                      Dim humanRecvBytes As String = websockets.BytesToString(details.bytesCurrent)
                      Dim humanTotalFileSize As String = websockets.BytesToString(details.bytesTotal)
                      lblClientProgress.Text = percent.ToString & "% (" & humanRecvBytes & " of " & humanTotalFileSize & ")"
                      lblClientStatus.Text = details.status
                      Me.Refresh()
                      lastUpdateClient = now
                  End If

              End Sub)
End Sub

Public Sub ServerIncomingMsg(msg As String) Implements IBlazingWebsocketsCallbacks.ServerIncomingMsg
    Me.Invoke(Sub()
                  appendServerLog(msg)
              End Sub)
End Sub

Public Sub ClientIncomingMsg(msg As String) Implements IBlazingWebsocketsCallbacks.ClientIncomingMsg
    Me.Invoke(Sub()
                  appendClientLog(msg)
              End Sub)
End Sub
```

Then write this to initiate the server:
```vbnet
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    websockets = New BlazingWebsockets(Me)
End Sub
```

Optionally, add these helper functions to print out the logs from the server/client:
```vbnet
Private Sub appendServerLog(ByVal line As String)
    ' Append the new text and a new line
    RichTextBox1.AppendText(line & Environment.NewLine)
    ' Scroll to the bottom
    RichTextBox1.SelectionStart = RichTextBox1.TextLength
    RichTextBox1.ScrollToCaret()
End Sub

Private Sub appendClientLog(ByVal line As String)
    ' Append the new text and a new line
    RichTextBox2.AppendText(line & Environment.NewLine)
    ' Scroll to the bottom
    RichTextBox2.SelectionStart = RichTextBox2.TextLength
    RichTextBox2.ScrollToCaret()
End Sub
```

To specify your host/port and start the respective server/client, do the following:
```vbnet
    Private Sub btnStartServer_Click(sender As Object, e As EventArgs) Handles btnStartServer.Click
        websockets.serverPort = txtServerPort.Text
        websockets.StartServer()
    End Sub

    Private Sub btnStartClient_Click(sender As Object, e As EventArgs) Handles btnStartClient.Click
        websockets.clientHost = txtClientHost.Text
        websockets.clientPort = txtClientPort.Text
        websockets.StartClient()
    End Sub

```

To send text or files from server/client respectively, do the following:
```vbnet
    Private Async Sub btnServerSendFiles_ClickAsync(sender As Object, e As EventArgs) Handles btnServerSendFiles.Click
        Dim filesList As List(Of String) = ListBox1.Items.Cast(Of String)().ToList()
        Await websockets.ServerSendFiles(websockets.setClientID, filesList)
    End Sub

    Private Async Sub btnClientSendFiles_ClickAsync(sender As Object, e As EventArgs) Handles btnClientSendFiles.Click
        Dim filesList As List(Of String) = ListBox2.Items.Cast(Of String)().ToList()
        Await websockets.ClientSendFiles(websockets.clientWS, filesList)
    End Sub

    Private Async Sub btnServerSendText_ClickAsync(sender As Object, e As EventArgs) Handles btnServerSendText.Click
        Dim clientID As String = websockets.lastClientID
        Await websockets.ServerSendString(websockets.clientSockets(lastClientID), txtServerText.Text)
    End Sub

    Private Async Sub btnClientSendText_ClickAsync(sender As Object, e As EventArgs) Handles btnClientSendText.Click
        Await websockets.ClientSendString(websockets.clientWS, txtClientText.Text)
    End Sub
```