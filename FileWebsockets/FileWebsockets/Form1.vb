Imports FileWebsockets.BlazingWebsockets

Public Class Form1

    Implements IBlazingWebsocketsCallbacks
    Private websockets As BlazingWebsockets

    Public Sub UpdateServerInterface(details As TransferInfo) Implements IBlazingWebsocketsCallbacks.UpdateServerInterface
        Dim rawPercent As Double = (details.bytesCurrent / details.bytesTotal) * 100
        Dim percent As Integer = CInt(Math.Max(0, Math.Min(100, rawPercent)))   'constrain between 0-100
        Dim humanRecvBytes As String = websockets.BytesToString(details.bytesCurrent)
        Dim humanTotalFileSize As String = websockets.BytesToString(details.bytesTotal)

        ProgressBar1.Value = percent
        lblServerProgress.Text = percent.ToString & "% (" & humanRecvBytes & " of " & humanTotalFileSize & ")"
        lblServerStatus.Text = details.status
    End Sub

    Public Sub UpdateClientInterface(details As TransferInfo) Implements IBlazingWebsocketsCallbacks.UpdateClientInterface
        Dim rawPercent As Double = (details.bytesCurrent / details.bytesTotal) * 100
        Dim percent As Integer = CInt(Math.Max(0, Math.Min(100, rawPercent)))   'constrain between 0-100
        Dim humanRecvBytes As String = websockets.BytesToString(details.bytesCurrent)
        Dim humanTotalFileSize As String = websockets.BytesToString(details.bytesTotal)

        ProgressBar2.Value = percent
        lblClientProgress.Text = percent.ToString & "% (" & humanRecvBytes & " of " & humanTotalFileSize & ")"
        lblClientStatus.Text = details.status
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

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        websockets = New BlazingWebsockets(Me)
    End Sub

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

    Private Sub btnServerBrowse_Click(sender As Object, e As EventArgs) Handles btnServerBrowse.Click
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.FileName <> "" Then
            txtServerFile.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub btnClientBrowse_Click(sender As Object, e As EventArgs) Handles btnClientBrowse.Click
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.FileName <> "" Then
            txtClientFile.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub btnServerAdd_Click(sender As Object, e As EventArgs) Handles btnServerAdd.Click
        ListBox1.Items.Add(txtServerFile.Text)
        txtServerFile.Clear()
    End Sub

    Private Sub btnClientAdd_Click(sender As Object, e As EventArgs) Handles btnClientAdd.Click
        ListBox2.Items.Add(txtClientFile.Text)
        txtClientFile.Clear()
    End Sub

    Private Sub btnServerRemove_Click(sender As Object, e As EventArgs) Handles btnServerRemove.Click
        ListBox1.Items.Remove(ListBox1.SelectedItem)
    End Sub

    Private Sub btnClientRemove_Click(sender As Object, e As EventArgs) Handles btnClientRemove.Click
        ListBox2.Items.Remove(ListBox2.SelectedItem)
    End Sub

    Private Sub btnStartServer_Click(sender As Object, e As EventArgs) Handles btnStartServer.Click
        websockets.serverPort = txtServerPort.Text
        websockets.StartServer()
    End Sub

    Private Sub btnStartClient_Click(sender As Object, e As EventArgs) Handles btnStartClient.Click
        websockets.clientHost = txtClientHost.Text
        websockets.clientPort = txtClientPort.Text
        websockets.StartClient()
    End Sub

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
        Await websockets.ServerSendString(websockets.clientSockets(clientID), txtServerText.Text)
    End Sub

    Private Async Sub btnClientSendText_ClickAsync(sender As Object, e As EventArgs) Handles btnClientSendText.Click
        Await websockets.ClientSendString(websockets.clientWS, txtClientText.Text)
    End Sub
End Class
