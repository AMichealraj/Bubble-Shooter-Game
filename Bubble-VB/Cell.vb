Public Class Cell

    Private b As Bubble

    Public hasBubble As Boolean = False

    Property Bubble As Bubble
        Get
            Return b
        End Get
        Set(value As Bubble)
            hasBubble = Not IsNothing(value)
            If hasBubble Then
                value.x = rx
                value.y = ry
            End If
            b = value
        End Set
    End Property

    Public ix As Integer
    Public iy As Integer

    Public rx As Integer
    Public ry As Integer

    Public radius As Integer

    Public Property X As Integer
        Get
            Return rx
        End Get
        Set(value As Integer)
            b.x += value - rx
            rx = value
        End Set
    End Property

    Property y As Integer
        Get
            Return ry
        End Get
        Set(value As Integer)
            b.y += value - ry
            ry = value
        End Set
    End Property

    Public Sub Tile()
        'If Not IsNothing(b) Then
        '    b.moving = False
        '    b.x = rx
        '    b.x += ((iy + 2) Mod 2) * b.radius
        '    b.y = ry
        '    b.y -= iy * 2 * b.radius * (1 - Math.Cos(Math.PI / 6))
        '    rx = b.x
        '    ry = b.y
        'End If
        rx += ((iy + 2) Mod 2) * radius
        ry -= iy * 2 * radius * (1 - Math.Cos(Math.PI / 6))
    End Sub

    Public Sub pop(grid As Grid, Optional addToUpdateList As Boolean = True)
        grid.score += 10
        Bubble.popped = True
        If addToUpdateList Then
            grid.updateList.Add(Me)
        End If
        VBGame.Assets.sounds("pop").play()
    End Sub

    Public Sub New(bubble As Bubble, x As Integer, y As Integer, ix As Integer, iy As Integer, radius As Integer)
        Me.radius = radius
        Me.ix = ix
        Me.iy = iy
        rx = x
        ry = y
        Tile()
        Me.Bubble = bubble
    End Sub

End Class
