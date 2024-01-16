Public Class Bubble
    Inherits VBGame.Sprite

    Public moving As Boolean
    Public radius As Integer
    Public realRadius As Integer
    Public scaledImage As Image
    Public popped As Boolean
    Public scale As Single = 4 / 5
    Private oscale As Single = 4 / 5
    Public Shared images As New List(Of Image)
    Private Shared random As New Random() ' Add this line to create an instance of Random


    Public Sub New(x As Integer, y As Integer, angle As Integer, radius As Integer)
        Me.radius = radius
        realRadius = radius * scale
        Me.x = x
        Me.y = y
        Me.angle = angle
        width = radius * 2
        height = radius * 2
        moving = True
        Dim c As Integer = Random.Next(0, images.Count)
        image = images(c)
        color = Color.FromArgb(c)
        speed = 10
        scaledImage = VBGame.Images.resizeImage(image, scale, False)
    End Sub

    Public Shadows Sub Draw(Display As VBGame.Display)
        If scale = oscale Then
            Display.blitCentered(scaledImage, getCenter())
        Else
            Display.blitCentered(VBGame.Images.resizeImage(scaledImage, scale, False), getCenter())
        End If
    End Sub

    Public Function Handle(ByRef grid As Grid, player As Player)
        move(True)
        If keepInBounds(New Rectangle(New Point(0, 0), grid.bounds), True, True) Then
            VBGame.Assets.sounds("bounce").play(False, True)
            If y = 0 Then
                grid.snapBubble(Me, player)
                VBGame.Assets.sounds("snap").play(False, True)
                Return True
            End If
        End If
        For Each Cell As Cell In grid.exposed
            If VBGame.Collisions.rect(getRect(), Cell.Bubble.getRect()) Then
                If VBGame.Collisions.circle(New VBGame.Circle(getCenter(), realRadius), New VBGame.Circle(Cell.Bubble.getCenter(), Cell.Bubble.realRadius)) Then
                    grid.snapBubble(Me, player)
                    VBGame.Assets.sounds("snap").play(False, True)
                    Return True
                End If
            End If
        Next
        Return False
    End Function

End Class