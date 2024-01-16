Public Class Player
    Inherits VBGame.Sprite

    Public queue As New List(Of Bubble)

    Public ready As Boolean = True

    Private angleMin As Integer = 185
    Private angleMax As Integer = 355

    Public maxQueue As Integer = 5

    Public radius As Integer

    Public Sub New(bounds As Size, radius As Integer)
        Me.radius = radius
        x = bounds.Width / 2
        y = bounds.Height * (39 / 40)
        width = 0
        height = 0
        angle = 270
        speed = 40
        populateQueue()
        color = VBGame.Colors.grey
    End Sub

    Public Overrides Sub draw(display As VBGame.Display)
        Dim move As PointF = calcMove(True)
        Dim brush As New SolidBrush(color)
        display.displaybuffer.Graphics.FillPolygon(brush, {New Point(x - 5, y), New Point(x + 5, y), New Point(move.X, move.Y)})
        brush.Dispose()
        Dim xmod As Integer = 0
        For Each Bubble As Bubble In queue
            display.blitCentered(Bubble.scaledImage, New Point(x + xmod, y))
            xmod += Bubble.radius * 2
        Next
    End Sub

    Public Sub clampAngle()
        If angle < angleMin Then
            angle = angleMin
        ElseIf angle > angleMax Then
            angle = angleMax
        End If
    End Sub

    Public Sub shoot(ByRef bubbles As List(Of Bubble))
        Dim bubble As Bubble = queue(0)
        bubble.angle = angle
        bubbles.Add(bubble)
        queue.RemoveAt(0)
        VBGame.Assets.sounds("shoot").play()
    End Sub

    Public Sub populateQueue()
        While queue.Count < maxQueue
            addToQueue()
        End While
    End Sub

    Public Sub addToQueue()
        queue.Add(New Bubble(x - radius, y - radius, 0, radius))
    End Sub

    Public Sub handleMouseControls(e As VBGame.MouseEvent, bubbles As List(Of Bubble), updateList As List(Of Cell))
        angle = Math.Atan2(y - e.location.Y, x - e.location.X) * (180 / Math.PI) + 180
        clampAngle()

        If ready AndAlso e.action = VBGame.MouseEvent.actions.down AndAlso e.button = VBGame.MouseEvent.buttons.left AndAlso updateList.Count = 0 Then
            shoot(bubbles)
            ready = False
            'populateQueue()
        End If
    End Sub

End Class
