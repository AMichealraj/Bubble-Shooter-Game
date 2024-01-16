Public Class Level
    Public Property BubbleSpeed As Integer
    Public Property TargetScore As Integer

    Public Sub New(speed As Integer, score As Integer)
        BubbleSpeed = speed
        TargetScore = score
    End Sub
End Class

Public Class LevelsManager
    Private levels As New List(Of Level)
    Private currentLevelIndex As Integer = 0

    Public ReadOnly Property CurrentLevel As Level
        Get
            If currentLevelIndex >= 0 AndAlso currentLevelIndex < levels.Count Then
                Return levels(currentLevelIndex)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Sub AddLevel(speed As Integer, score As Integer)
        levels.Add(New Level(speed, score))
    End Sub

    Public Sub LoadNextLevel()
        If currentLevelIndex < levels.Count - 1 Then
            currentLevelIndex += 1
        End If
    End Sub

    ' Add any other methods or properties related to level management

End Class
