Imports System.Threading
Imports System.IO

Public Class Form1

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg <> &HF Then
            MyBase.WndProc(m)
        End If
    End Sub

    Public display As VBGame.Display
    Public thread As New Thread(AddressOf mainloop)
    Public settings As New Settings

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Show the login form first
        Dim loginForm As New LoginForm()
        If loginForm.ShowDialog() = DialogResult.OK Then
            ' Start the game only if the login is successful
            InitializeGame()
        Else
            ' Close the application if the login is canceled
            Me.Close()
        End If

        settings = New Settings()
        If Not File.Exists("Settings.xml") Then
            settings.Save()
        Else
            VBGame.XMLIO.Read("Settings.xml", settings)
        End If
        ' Create VBGame.Display after the window handle has been created
        Me.BeginInvoke(New Action(Sub()
                                      Bubble.images.Add(VBGame.Images.load("assets/images/bubbles/red.png"))
                                      Bubble.images.Add(VBGame.Images.load("assets/images/bubbles/yellow.png"))
                                      Bubble.images.Add(VBGame.Images.load("assets/images/bubbles/purple.png"))
                                      Bubble.images.Add(VBGame.Images.load("assets/images/bubbles/blue.png"))
                                      Bubble.images.Add(VBGame.Images.load("assets/images/bubbles/green.png"))
                                      Bubble.images.Add(VBGame.Images.load("assets/images/bubbles/cyan.png"))

                                      VBGame.Assets.sounds.Add("shoot", New VBGame.Sound("assets/sounds/shoot.mp3"))
                                      VBGame.Assets.sounds.Add("pop", New VBGame.Sound("assets/sounds/pop.mp3"))
                                      VBGame.Assets.sounds.Add("bounce", New VBGame.Sound("assets/sounds/bounce.mp3"))
                                      VBGame.Assets.sounds.Add("snap", New VBGame.Sound("assets/sounds/snap.mp3"))
                                      VBGame.Assets.sounds.Add("advance", New VBGame.Sound("assets/sounds/advance.mp3"))
                                      VBGame.Assets.sounds.Add("music", New VBGame.Sound("assets/sounds/music/Marty_Gots_a_Plan.mp3"))
                                      VBGame.Assets.sounds.Add("victory", New VBGame.Sound("assets/sounds/music/victory.mp3"))

                                      VBGame.Assets.images.Add("bg", VBGame.Images.load("assets/images/bg.png"))
                                      VBGame.Assets.images.Add("sidebar", VBGame.Images.load("assets/images/sidebar.png"))
                                      VBGame.Assets.images.Add("star", VBGame.Images.load("assets/images/star.png"))
                                      VBGame.Assets.images.Add("bg_gold", VBGame.Images.load("assets/images/bg_gold.png"))

                                      Me.Icon = New Icon("assets/images/icon.ico")

                                      display = New VBGame.Display(Me, New Size(800, 600), "Bubble-VB")
                                      thread.Start()
                                  End Sub))
    End Sub

    Private Sub InitializeGame()
        ' Add your game initialization logic here
        ' For example, creating the initial game state
    End Sub

    Public Sub mainloop()
        While True
            settings.handleScore(gameloop())
            display.getMouseEvents()
        End While
    End Sub

    Public Sub drawScore(grid As Grid)
        Dim color As Color
        If grid.won Then
            color = Color.Gold
        Else
            color = VBGame.Colors.grey
        End If

        Dim scoreText As String = "Score: " & CStr(grid.score)
        Dim levelText As String = "Level: " & grid.CurrentLevelNumber.ToString()

        display.drawText(New Point(display.width - 205, 10), scoreText, color, New Font("Xpress Heavy SF", 16))
        display.drawText(New Point(display.width - 205, 30), levelText, color, New Font("Xpress Heavy SF", 16))
    End Sub

    Public Function getBackground(Optional won As Boolean = False) As VBGame.Surface

        Dim background As New VBGame.Surface(display.getRect(), display)

        background.blit(VBGame.Assets.images("bg"), New Point(0, 0))
        If won Then
            background.blit(VBGame.Assets.images("bg_gold"), New Point(0, 0))
        End If

        background.blit(VBGame.Assets.images("sidebar"), New Point(585, 0))

        Dim tx As Integer = display.width - 205
        Dim ty As Integer = 64

        Dim color As Color


        'Display the Leaderboard

        'background.drawText(New Point(tx, ty), "Leaderboard:", VBGame.Colors.grey, New Font("Xpress Heavy SF", 16))
        'ty += 16
        'For Each Score As Score In settings.leaderboard
        '    ty += 16
        '    If Not IsNothing(Score) Then
        '        If Score.won Then
        '            color = color.Gold
        '        Else
        '            color = VBGame.Colors.grey
        '        End If
        '        background.drawText(New Point(tx, ty), Score.name, color, New Font("Xpress Heavy SF", 16))
        '        background.drawText(New Point(tx + 100, ty), Score.score, color, New Font("Xpress Heavy SF", 16))
        '    End If
        'Next

        If settings.gamesWon > 0 Then
            background.blit(VBGame.Assets.images("star"), New Point(display.width - 160, 390))
            background.drawText(New Rectangle(display.width - 160, 390 + (VBGame.Assets.images("star").Height * (1 / 2)), VBGame.Assets.images("star").Width, VBGame.Assets.images("star").Height), settings.gamesWon, color.Gold, New Font("Xpress Heavy SF", 16))
        End If

        Return background

    End Function

    Public Function createGrid() As Grid
        Return New Grid(New Size(display.width / 40 - 1, 22), settings.radius, 0)
    End Function

    Public Function gameloop() As Grid
        VBGame.Assets.sounds("music").play(True)

        Dim grid As Grid = createGrid()
        Dim frames As Integer = 0
        Dim player As New Player(New Size(585, display.height), settings.radius)
        Dim background As VBGame.Surface = getBackground()

        grid.calculateExposed()
        Dim bubbles As New List(Of Bubble)
        Dim toCheck As New List(Of Cell)
        Dim startingRows As Integer = 11
        Dim rowsToAdd As Integer = startingRows
        Dim reset As New VBGame.Button(display, "Restart", New Rectangle(display.width - 205, 350, 195, 32))

        reset.fontname = "Xpress Heavy SF"
        reset.fontsize = 16
        reset.setColor(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(100, 100, 100))
        reset.setTextColor(VBGame.Colors.grey, VBGame.Colors.grey)

        While True
            frames += 1

            background.update()

            If Not player.ready AndAlso bubbles.Count = 0 Then
                player.ready = True
            End If

            For Each e As VBGame.MouseEvent In display.getMouseEvents()
                If e.location.X < display.width - 215 Then
                    player.handleMouseControls(e, bubbles, grid.updateList)
                End If

                If reset.handle(e) Then
                    Return grid
                End If
            Next

            For Each Cell As Cell In grid.updateList.ToList()
                Cell.Bubble.Draw(display)
                Cell.Bubble.scale -= 0.1
                If Cell.Bubble.scale <= 0.1 Then
                    Cell.Bubble = Nothing
                    grid.updateList.Remove(Cell)
                    toCheck.AddRange(grid.getNeighbors(Cell.ix, Cell.iy))
                End If
            Next

            If toCheck.Count <> 0 AndAlso grid.updateList.Count = 0 Then
                grid.update()
                toCheck.Clear()
            End If

            grid.draw(display)
            player.draw(display)
            drawScore(grid)

            For Each Bubble As Bubble In bubbles.ToList()
                Bubble.Draw(display)
                If Bubble.Handle(grid, player) Then
                    bubbles.Remove(Bubble)
                End If
            Next

            If player.queue.Count = 0 AndAlso bubbles.Count = 0 Then
                rowsToAdd = 1
                player.populateQueue()
            End If

            If grid.lost Then
                Return grid
            End If

            If grid.exposed.Count = 0 AndAlso frames > 10 Then
                VBGame.Assets.sounds("victory").play()
                settings.gamesWon += 1
                settings.Save()
                background = getBackground(True)
                grid.won = True
                grid.score += 100000
                grid.addRow()
                rowsToAdd = startingRows - 1
            End If

            If rowsToAdd > 0 AndAlso frames Mod 2 = 0 Then
                grid.addRow()
                rowsToAdd -= 1
            End If

            ' Check if the player's score is greater than or equal to 180
            If grid.score >= 180 Then
                Return grid
            End If

            reset.draw()
            display.update()
            display.clockTick(60)
        End While

        ' This will never be reached, but keeping it for completeness
        Return grid
    End Function

End Class