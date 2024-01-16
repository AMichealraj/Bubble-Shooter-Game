Public Class Settings

    Public radius As Integer = 15

    Public gamesWon As Integer = 0

    Public Sub New()
    End Sub

    Public Sub Save()
        VBGame.XMLIO.Write("Settings.xml", Me)
    End Sub

    Public Sub insertScore(score As Score)
        Dim i As Integer = 0
        For Each escore As Score In leaderboard
            Try
                If score.score > escore.score Then

                    For x As Integer = leaderboard.Length - 1 To i Step -1
                        Try
                            leaderboard(x + 1) = leaderboard(x)
                        Catch ex As Exception
                        End Try
                    Next

                    leaderboard(i) = score

                    Exit For
                End If
                i += 1
            Catch
                leaderboard(i) = score
                Exit For
            End Try
        Next
        Save()

    End Sub

    Public Sub handleScore(grid As Grid)
        Dim highscore As Boolean = False

        For Each Score As Score In leaderboard
            If IsNothing(Score) Then
                highscore = True
                Exit For
            Else
                If grid.score > Score.score Then
                    highscore = True
                    Exit For
                End If
            End If
        Next

        Dim pscore As Score

        ' Display "You Win" if the player's score is >= 180
        If grid.score >= 180 Then
            MsgBox("You Win!" & vbCrLf & "Score: " & CStr(grid.score))
        Else
            MsgBox("Game Over." & vbCrLf & "Score: " & CStr(grid.score))
        End If



        ' High score name ask


        'If highscore Then
        '    pscore = New Score()
        '    While IsNothing(pscore.name) OrElse pscore.name.Count > 6
        '        pscore = New Score(InputBox("New Highscore!" & vbCrLf & "Enter name:"), grid.score, grid.won)
        '        If pscore.name.Count > 6 Then
        '            MsgBox("Name is maximum of 6 characters.")
        '        End If
        '    End While
        '    If pscore.name <> "" Then
        '        insertScore(pscore)
        '    End If
        'End If

    End Sub

    Public leaderboard(9) As Score

End Class

Public Class Score
    Public name As String
    Public score As Integer
    Public won As Boolean

    Public Sub New()
    End Sub

    Public Sub New(name As String, score As Integer, won As Boolean)
        Me.name = name
        Me.score = score
        Me.won = won
    End Sub

End Class