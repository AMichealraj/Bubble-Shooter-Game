Public Class LoginForm
    ' Add WithEvents to the controls
    Private WithEvents btnLogin As New Button()
    Private WithEvents txtUsername As New TextBox()
    Private WithEvents txtPassword As New TextBox()
    Private WithEvents lblUsername As New Label()
    Private WithEvents lblPassword As New Label()

    Public Sub New()
        ' Set up the form and controls in the constructor
        Me.Text = "Login"
        Me.Size = New Size(300, 150)

        lblUsername.Text = "Username:"
        lblUsername.Size = New Size(80, 20)
        lblUsername.Location = New Point(30, 20)

        lblPassword.Text = "Password:"
        lblPassword.Size = New Size(80, 20)
        lblPassword.Location = New Point(30, 50)

        txtUsername.Size = New Size(150, 20)
        txtUsername.Location = New Point(110, 20)

        txtPassword.Size = New Size(150, 20)
        txtPassword.Location = New Point(110, 50)
        txtPassword.PasswordChar = "*"c ' Set to '*' to hide password

        btnLogin.Text = "Login"
        btnLogin.Size = New Size(80, 30)
        btnLogin.Location = New Point(110, 80)

        ' Add controls to the form's controls collection
        Me.Controls.Add(lblUsername)
        Me.Controls.Add(lblPassword)
        Me.Controls.Add(txtUsername)
        Me.Controls.Add(txtPassword)
        Me.Controls.Add(btnLogin)
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' Perform login validation
        If ValidateLogin(txtUsername.Text, txtPassword.Text) Then
            ' Set DialogResult to OK if login is successful
            DialogResult = DialogResult.OK
            Close()
        Else
            MessageBox.Show("Invalid username or password. Please try again.")
        End If
    End Sub

    Private Function ValidateLogin(username As String, password As String) As Boolean
        ' Add your login validation logic here
        ' For simplicity, let's assume a fixed username and password
        Return username = "admin" AndAlso password = "password"
    End Function
End Class
