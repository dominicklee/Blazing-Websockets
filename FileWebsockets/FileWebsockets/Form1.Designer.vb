<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblServerStatus = New System.Windows.Forms.Label()
        Me.lblServerProgress = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.btnServerSendText = New System.Windows.Forms.Button()
        Me.lblServerText = New System.Windows.Forms.Label()
        Me.txtServerText = New System.Windows.Forms.TextBox()
        Me.btnServerSendFiles = New System.Windows.Forms.Button()
        Me.btnStartServer = New System.Windows.Forms.Button()
        Me.btnServerRemove = New System.Windows.Forms.Button()
        Me.btnServerAdd = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.btnServerBrowse = New System.Windows.Forms.Button()
        Me.lblServerFile = New System.Windows.Forms.Label()
        Me.txtServerFile = New System.Windows.Forms.TextBox()
        Me.txtServerPort = New System.Windows.Forms.TextBox()
        Me.lblServerPort = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblClientStatus = New System.Windows.Forms.Label()
        Me.lblClientProgress = New System.Windows.Forms.Label()
        Me.ProgressBar2 = New System.Windows.Forms.ProgressBar()
        Me.RichTextBox2 = New System.Windows.Forms.RichTextBox()
        Me.btnClientSendText = New System.Windows.Forms.Button()
        Me.lblClientText = New System.Windows.Forms.Label()
        Me.txtClientText = New System.Windows.Forms.TextBox()
        Me.btnClientSendFiles = New System.Windows.Forms.Button()
        Me.btnStartClient = New System.Windows.Forms.Button()
        Me.txtClientHost = New System.Windows.Forms.TextBox()
        Me.lblHost = New System.Windows.Forms.Label()
        Me.btnClientRemove = New System.Windows.Forms.Button()
        Me.btnClientAdd = New System.Windows.Forms.Button()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.btnClientBrowse = New System.Windows.Forms.Button()
        Me.lblClientFile = New System.Windows.Forms.Label()
        Me.txtClientFile = New System.Windows.Forms.TextBox()
        Me.txtClientPort = New System.Windows.Forms.TextBox()
        Me.lblClientPort = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblServerStatus)
        Me.GroupBox1.Controls.Add(Me.lblServerProgress)
        Me.GroupBox1.Controls.Add(Me.ProgressBar1)
        Me.GroupBox1.Controls.Add(Me.RichTextBox1)
        Me.GroupBox1.Controls.Add(Me.btnServerSendText)
        Me.GroupBox1.Controls.Add(Me.lblServerText)
        Me.GroupBox1.Controls.Add(Me.txtServerText)
        Me.GroupBox1.Controls.Add(Me.btnServerSendFiles)
        Me.GroupBox1.Controls.Add(Me.btnStartServer)
        Me.GroupBox1.Controls.Add(Me.btnServerRemove)
        Me.GroupBox1.Controls.Add(Me.btnServerAdd)
        Me.GroupBox1.Controls.Add(Me.ListBox1)
        Me.GroupBox1.Controls.Add(Me.btnServerBrowse)
        Me.GroupBox1.Controls.Add(Me.lblServerFile)
        Me.GroupBox1.Controls.Add(Me.txtServerFile)
        Me.GroupBox1.Controls.Add(Me.txtServerPort)
        Me.GroupBox1.Controls.Add(Me.lblServerPort)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(300, 387)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Server"
        '
        'lblServerStatus
        '
        Me.lblServerStatus.Location = New System.Drawing.Point(164, 215)
        Me.lblServerStatus.Name = "lblServerStatus"
        Me.lblServerStatus.Size = New System.Drawing.Size(130, 13)
        Me.lblServerStatus.TabIndex = 14
        Me.lblServerStatus.Text = "Status: N/A"
        Me.lblServerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblServerProgress
        '
        Me.lblServerProgress.Location = New System.Drawing.Point(91, 202)
        Me.lblServerProgress.Name = "lblServerProgress"
        Me.lblServerProgress.Size = New System.Drawing.Size(130, 13)
        Me.lblServerProgress.TabIndex = 13
        Me.lblServerProgress.Text = "0%"
        Me.lblServerProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(9, 176)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(285, 23)
        Me.ProgressBar1.TabIndex = 12
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Location = New System.Drawing.Point(9, 261)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(285, 120)
        Me.RichTextBox1.TabIndex = 11
        Me.RichTextBox1.Text = ""
        '
        'btnServerSendText
        '
        Me.btnServerSendText.Location = New System.Drawing.Point(224, 233)
        Me.btnServerSendText.Name = "btnServerSendText"
        Me.btnServerSendText.Size = New System.Drawing.Size(70, 23)
        Me.btnServerSendText.TabIndex = 10
        Me.btnServerSendText.Text = "Send Text"
        Me.btnServerSendText.UseVisualStyleBackColor = True
        '
        'lblServerText
        '
        Me.lblServerText.AutoSize = True
        Me.lblServerText.Location = New System.Drawing.Point(9, 238)
        Me.lblServerText.Name = "lblServerText"
        Me.lblServerText.Size = New System.Drawing.Size(31, 13)
        Me.lblServerText.TabIndex = 9
        Me.lblServerText.Text = "Text:"
        '
        'txtServerText
        '
        Me.txtServerText.Location = New System.Drawing.Point(41, 235)
        Me.txtServerText.Name = "txtServerText"
        Me.txtServerText.Size = New System.Drawing.Size(180, 20)
        Me.txtServerText.TabIndex = 8
        '
        'btnServerSendFiles
        '
        Me.btnServerSendFiles.Location = New System.Drawing.Point(224, 147)
        Me.btnServerSendFiles.Name = "btnServerSendFiles"
        Me.btnServerSendFiles.Size = New System.Drawing.Size(70, 23)
        Me.btnServerSendFiles.TabIndex = 7
        Me.btnServerSendFiles.Text = "Send Files"
        Me.btnServerSendFiles.UseVisualStyleBackColor = True
        '
        'btnStartServer
        '
        Me.btnStartServer.Location = New System.Drawing.Point(224, 20)
        Me.btnStartServer.Name = "btnStartServer"
        Me.btnStartServer.Size = New System.Drawing.Size(70, 23)
        Me.btnStartServer.TabIndex = 5
        Me.btnStartServer.Text = "Start"
        Me.btnStartServer.UseVisualStyleBackColor = True
        '
        'btnServerRemove
        '
        Me.btnServerRemove.Location = New System.Drawing.Point(224, 104)
        Me.btnServerRemove.Name = "btnServerRemove"
        Me.btnServerRemove.Size = New System.Drawing.Size(70, 23)
        Me.btnServerRemove.TabIndex = 7
        Me.btnServerRemove.Text = "Remove"
        Me.btnServerRemove.UseVisualStyleBackColor = True
        '
        'btnServerAdd
        '
        Me.btnServerAdd.Location = New System.Drawing.Point(224, 75)
        Me.btnServerAdd.Name = "btnServerAdd"
        Me.btnServerAdd.Size = New System.Drawing.Size(70, 23)
        Me.btnServerAdd.TabIndex = 6
        Me.btnServerAdd.Text = "Add"
        Me.btnServerAdd.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(9, 75)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(212, 95)
        Me.ListBox1.TabIndex = 5
        '
        'btnServerBrowse
        '
        Me.btnServerBrowse.Location = New System.Drawing.Point(224, 46)
        Me.btnServerBrowse.Name = "btnServerBrowse"
        Me.btnServerBrowse.Size = New System.Drawing.Size(70, 23)
        Me.btnServerBrowse.TabIndex = 4
        Me.btnServerBrowse.Text = "Browse"
        Me.btnServerBrowse.UseVisualStyleBackColor = True
        '
        'lblServerFile
        '
        Me.lblServerFile.AutoSize = True
        Me.lblServerFile.Location = New System.Drawing.Point(9, 51)
        Me.lblServerFile.Name = "lblServerFile"
        Me.lblServerFile.Size = New System.Drawing.Size(26, 13)
        Me.lblServerFile.TabIndex = 3
        Me.lblServerFile.Text = "File:"
        '
        'txtServerFile
        '
        Me.txtServerFile.Location = New System.Drawing.Point(41, 48)
        Me.txtServerFile.Name = "txtServerFile"
        Me.txtServerFile.Size = New System.Drawing.Size(180, 20)
        Me.txtServerFile.TabIndex = 2
        '
        'txtServerPort
        '
        Me.txtServerPort.Location = New System.Drawing.Point(41, 22)
        Me.txtServerPort.Name = "txtServerPort"
        Me.txtServerPort.Size = New System.Drawing.Size(100, 20)
        Me.txtServerPort.TabIndex = 1
        Me.txtServerPort.Text = "3800"
        '
        'lblServerPort
        '
        Me.lblServerPort.AutoSize = True
        Me.lblServerPort.Location = New System.Drawing.Point(6, 25)
        Me.lblServerPort.Name = "lblServerPort"
        Me.lblServerPort.Size = New System.Drawing.Size(29, 13)
        Me.lblServerPort.TabIndex = 0
        Me.lblServerPort.Text = "Port:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblClientStatus)
        Me.GroupBox2.Controls.Add(Me.lblClientProgress)
        Me.GroupBox2.Controls.Add(Me.ProgressBar2)
        Me.GroupBox2.Controls.Add(Me.RichTextBox2)
        Me.GroupBox2.Controls.Add(Me.btnClientSendText)
        Me.GroupBox2.Controls.Add(Me.lblClientText)
        Me.GroupBox2.Controls.Add(Me.txtClientText)
        Me.GroupBox2.Controls.Add(Me.btnClientSendFiles)
        Me.GroupBox2.Controls.Add(Me.btnStartClient)
        Me.GroupBox2.Controls.Add(Me.txtClientHost)
        Me.GroupBox2.Controls.Add(Me.lblHost)
        Me.GroupBox2.Controls.Add(Me.btnClientRemove)
        Me.GroupBox2.Controls.Add(Me.btnClientAdd)
        Me.GroupBox2.Controls.Add(Me.ListBox2)
        Me.GroupBox2.Controls.Add(Me.btnClientBrowse)
        Me.GroupBox2.Controls.Add(Me.lblClientFile)
        Me.GroupBox2.Controls.Add(Me.txtClientFile)
        Me.GroupBox2.Controls.Add(Me.txtClientPort)
        Me.GroupBox2.Controls.Add(Me.lblClientPort)
        Me.GroupBox2.Location = New System.Drawing.Point(318, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(300, 387)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Client"
        '
        'lblClientStatus
        '
        Me.lblClientStatus.Location = New System.Drawing.Point(164, 215)
        Me.lblClientStatus.Name = "lblClientStatus"
        Me.lblClientStatus.Size = New System.Drawing.Size(130, 13)
        Me.lblClientStatus.TabIndex = 15
        Me.lblClientStatus.Text = "Status: N/A"
        Me.lblClientStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblClientProgress
        '
        Me.lblClientProgress.Location = New System.Drawing.Point(91, 202)
        Me.lblClientProgress.Name = "lblClientProgress"
        Me.lblClientProgress.Size = New System.Drawing.Size(130, 13)
        Me.lblClientProgress.TabIndex = 14
        Me.lblClientProgress.Text = "0%"
        Me.lblClientProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ProgressBar2
        '
        Me.ProgressBar2.Location = New System.Drawing.Point(9, 176)
        Me.ProgressBar2.Name = "ProgressBar2"
        Me.ProgressBar2.Size = New System.Drawing.Size(285, 23)
        Me.ProgressBar2.TabIndex = 13
        '
        'RichTextBox2
        '
        Me.RichTextBox2.Location = New System.Drawing.Point(9, 261)
        Me.RichTextBox2.Name = "RichTextBox2"
        Me.RichTextBox2.Size = New System.Drawing.Size(285, 120)
        Me.RichTextBox2.TabIndex = 14
        Me.RichTextBox2.Text = ""
        '
        'btnClientSendText
        '
        Me.btnClientSendText.Location = New System.Drawing.Point(224, 233)
        Me.btnClientSendText.Name = "btnClientSendText"
        Me.btnClientSendText.Size = New System.Drawing.Size(70, 23)
        Me.btnClientSendText.TabIndex = 13
        Me.btnClientSendText.Text = "Send Text"
        Me.btnClientSendText.UseVisualStyleBackColor = True
        '
        'lblClientText
        '
        Me.lblClientText.AutoSize = True
        Me.lblClientText.Location = New System.Drawing.Point(9, 238)
        Me.lblClientText.Name = "lblClientText"
        Me.lblClientText.Size = New System.Drawing.Size(31, 13)
        Me.lblClientText.TabIndex = 12
        Me.lblClientText.Text = "Text:"
        '
        'txtClientText
        '
        Me.txtClientText.Location = New System.Drawing.Point(41, 235)
        Me.txtClientText.Name = "txtClientText"
        Me.txtClientText.Size = New System.Drawing.Size(180, 20)
        Me.txtClientText.TabIndex = 11
        '
        'btnClientSendFiles
        '
        Me.btnClientSendFiles.Location = New System.Drawing.Point(224, 147)
        Me.btnClientSendFiles.Name = "btnClientSendFiles"
        Me.btnClientSendFiles.Size = New System.Drawing.Size(70, 23)
        Me.btnClientSendFiles.TabIndex = 8
        Me.btnClientSendFiles.Text = "Send Files"
        Me.btnClientSendFiles.UseVisualStyleBackColor = True
        '
        'btnStartClient
        '
        Me.btnStartClient.Location = New System.Drawing.Point(224, 20)
        Me.btnStartClient.Name = "btnStartClient"
        Me.btnStartClient.Size = New System.Drawing.Size(70, 23)
        Me.btnStartClient.TabIndex = 6
        Me.btnStartClient.Text = "Start"
        Me.btnStartClient.UseVisualStyleBackColor = True
        '
        'txtClientHost
        '
        Me.txtClientHost.Location = New System.Drawing.Point(41, 22)
        Me.txtClientHost.Name = "txtClientHost"
        Me.txtClientHost.Size = New System.Drawing.Size(72, 20)
        Me.txtClientHost.TabIndex = 5
        Me.txtClientHost.Text = "localhost"
        '
        'lblHost
        '
        Me.lblHost.AutoSize = True
        Me.lblHost.Location = New System.Drawing.Point(6, 25)
        Me.lblHost.Name = "lblHost"
        Me.lblHost.Size = New System.Drawing.Size(32, 13)
        Me.lblHost.TabIndex = 4
        Me.lblHost.Text = "Host:"
        '
        'btnClientRemove
        '
        Me.btnClientRemove.Location = New System.Drawing.Point(224, 104)
        Me.btnClientRemove.Name = "btnClientRemove"
        Me.btnClientRemove.Size = New System.Drawing.Size(70, 23)
        Me.btnClientRemove.TabIndex = 8
        Me.btnClientRemove.Text = "Remove"
        Me.btnClientRemove.UseVisualStyleBackColor = True
        '
        'btnClientAdd
        '
        Me.btnClientAdd.Location = New System.Drawing.Point(224, 75)
        Me.btnClientAdd.Name = "btnClientAdd"
        Me.btnClientAdd.Size = New System.Drawing.Size(70, 23)
        Me.btnClientAdd.TabIndex = 7
        Me.btnClientAdd.Text = "Add"
        Me.btnClientAdd.UseVisualStyleBackColor = True
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(9, 75)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(212, 95)
        Me.ListBox2.TabIndex = 6
        '
        'btnClientBrowse
        '
        Me.btnClientBrowse.Location = New System.Drawing.Point(224, 46)
        Me.btnClientBrowse.Name = "btnClientBrowse"
        Me.btnClientBrowse.Size = New System.Drawing.Size(70, 23)
        Me.btnClientBrowse.TabIndex = 5
        Me.btnClientBrowse.Text = "Browse"
        Me.btnClientBrowse.UseVisualStyleBackColor = True
        '
        'lblClientFile
        '
        Me.lblClientFile.AutoSize = True
        Me.lblClientFile.Location = New System.Drawing.Point(9, 51)
        Me.lblClientFile.Name = "lblClientFile"
        Me.lblClientFile.Size = New System.Drawing.Size(26, 13)
        Me.lblClientFile.TabIndex = 5
        Me.lblClientFile.Text = "File:"
        '
        'txtClientFile
        '
        Me.txtClientFile.Location = New System.Drawing.Point(41, 48)
        Me.txtClientFile.Name = "txtClientFile"
        Me.txtClientFile.Size = New System.Drawing.Size(180, 20)
        Me.txtClientFile.TabIndex = 4
        '
        'txtClientPort
        '
        Me.txtClientPort.Location = New System.Drawing.Point(154, 22)
        Me.txtClientPort.Name = "txtClientPort"
        Me.txtClientPort.Size = New System.Drawing.Size(67, 20)
        Me.txtClientPort.TabIndex = 3
        Me.txtClientPort.Text = "3800"
        '
        'lblClientPort
        '
        Me.lblClientPort.AutoSize = True
        Me.lblClientPort.Location = New System.Drawing.Point(119, 25)
        Me.lblClientPort.Name = "lblClientPort"
        Me.lblClientPort.Size = New System.Drawing.Size(29, 13)
        Me.lblClientPort.TabIndex = 2
        Me.lblClientPort.Text = "Port:"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 411)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FileWebsockets"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btnServerBrowse As Button
    Friend WithEvents lblServerFile As Label
    Friend WithEvents txtServerFile As TextBox
    Friend WithEvents txtServerPort As TextBox
    Friend WithEvents lblServerPort As Label
    Friend WithEvents btnClientBrowse As Button
    Friend WithEvents lblClientFile As Label
    Friend WithEvents txtClientFile As TextBox
    Friend WithEvents txtClientPort As TextBox
    Friend WithEvents lblClientPort As Label
    Friend WithEvents btnServerRemove As Button
    Friend WithEvents btnServerAdd As Button
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents btnClientRemove As Button
    Friend WithEvents btnClientAdd As Button
    Friend WithEvents ListBox2 As ListBox
    Friend WithEvents txtClientHost As TextBox
    Friend WithEvents lblHost As Label
    Friend WithEvents btnStartServer As Button
    Friend WithEvents btnStartClient As Button
    Friend WithEvents btnServerSendFiles As Button
    Friend WithEvents btnClientSendFiles As Button
    Friend WithEvents btnServerSendText As Button
    Friend WithEvents lblServerText As Label
    Friend WithEvents txtServerText As TextBox
    Friend WithEvents btnClientSendText As Button
    Friend WithEvents lblClientText As Label
    Friend WithEvents txtClientText As TextBox
    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents RichTextBox2 As RichTextBox
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents ProgressBar2 As ProgressBar
    Friend WithEvents lblServerProgress As Label
    Friend WithEvents lblClientProgress As Label
    Friend WithEvents lblServerStatus As Label
    Friend WithEvents lblClientStatus As Label
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
End Class
