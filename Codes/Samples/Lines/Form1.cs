using System.Data.Common;
using System.IO;

namespace Lines
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Start();
            timer.Tick += Timer_Tick;
            Paint += Form1_Paint;
            ClientSize = new Size(CellSize * 9 + 1, CellSize * 9 + 1);
            MouseUp += Form1_MouseUp;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ResetGame();
        }


        List<Point> path = new List<Point>();
        int[,] wave = new int[9, 9];
        int[,] map = new int[9, 9];
        int scores = 0;
        int maxScores = 0;

        Point? selectedBall = null;
        Color[] colors = new[] { Color.Red, Color.Green, Color.Orange, Color.Blue, Color.Yellow, Color.Violet, Color.LimeGreen };
        Random rand = new Random();
        const int topGap = 30;

        const int CellSize = 50;

        private bool BuildPath(int targetX, int targetY, int ballX, int ballY)
        {
            // build path
            path = new List<Point>();
            path.Add(new Point(targetX, targetY));
            while (true)
            {
                var point = path.First();
                int level = wave[point.X, point.Y];

                if (point.X == ballX && point.Y == ballY)
                    break;

                bool exit = false;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Math.Abs(i + j) != 1)
                            continue;
                        var newX = point.X + i;
                        var newY = point.Y + j;
                        if (newX < 0 || newY < 0)
                            continue;
                        if (newX >= wave.GetLength(0) || newY >= wave.GetLength(1))
                            continue;

                        if (wave[newX, newY] == level - 1)
                        {
                            path.Insert(0, new Point(point.X + i, point.Y + j));
                            exit = true;
                            break;
                        }

                    }
                    if (exit)
                        break;
                }
                if (!exit) //path not found
                {
                    path.Clear();
                    return false;
                }
            }
            path.RemoveAt(0);
            return true;
        }

        private void BuildWave(int ballX, int ballY)
        {
            wave = new int[9, 9];
            Queue<(Point, int)> q = new Queue<(Point, int)>();
            q.Enqueue(new(new Point(ballX, ballY), 1));
            bool first = true;
            while (q.Any())
            {
                var deq = q.Dequeue();
                var p = deq.Item1;
                if (p.X < 0 || p.Y < 0 || p.X >= map.GetLength(0) || p.Y >= map.GetLength(1))
                    continue;

                if (!first && map[p.X, p.Y] != 0)
                    continue;

                first = false;

                if (wave[p.X, p.Y] != 0)
                    continue;

                int level = deq.Item2;
                wave[p.X, p.Y] = level;

                q.Enqueue((new Point(p.X + 1, p.Y), level + 1));
                q.Enqueue((new Point(p.X - 1, p.Y), level + 1));
                q.Enqueue((new Point(p.X, p.Y + 1), level + 1));
                q.Enqueue((new Point(p.X, p.Y - 1), level + 1));

            }
        }
        enum GameState
        {
            Idle, BallMove, GameOver
        }
        GameState state = GameState.Idle;

        private void Form1_MouseUp(object? sender, MouseEventArgs e)
        {
            if (state == GameState.GameOver)
            {
                ResetGame();
                return;
            }

            if (state != GameState.Idle)
                return;

            var cursor = e.Location;
            int row = (cursor.Y) / CellSize;
            int column = cursor.X / CellSize;
            if (map[column, row] != 0)
                selectedBall = new Point(column, row);

            if (selectedBall != null && map[column, row] == 0)
            {
                ////calc path
                BuildWave(selectedBall.Value.X, selectedBall.Value.Y);
                if (BuildPath(column, row, selectedBall.Value.X, selectedBall.Value.Y))//path exist                    
                {
                    state = GameState.BallMove;
                }
            }
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Text = $"fel88.com - lines   score: {scores}  max scores: {maxScores} ";
        }

        const int BallInLine = 3;
        private void CheckLines()
        {
            bool[,] deleteMap = new bool[9, 9];
            //find all 5-len lines
            //check rows
            for (int i = 0; i < 9; i++)
            {
                int accum = 0;
                int startColor = 0;
                for (int j = 0; j < 9; j++)
                {
                    if (map[i, j] != startColor)
                    {
                        accum = 0;
                        startColor = map[i, j];
                    }
                    if (startColor != 0)
                        accum++;

                    if (accum >= BallInLine)
                    {
                        //remove line                        
                        for (int k = 0; k < accum; k++)
                        {
                            //map[i, j - k] = 0;
                            deleteMap[i, j - k] = true;
                        }

                    }
                }
            }

            //check columns
            for (int j = 0; j < 9; j++)
            {
                int accum = 0;
                int startColor = 0;

                for (int i = 0; i < 9; i++)
                {
                    if (map[i, j] != startColor)
                    {
                        accum = 0;
                        startColor = map[i, j];
                    }
                    if (startColor != 0)
                        accum++;

                    if (accum >= BallInLine)
                    {
                        //remove line

                        for (int k = 0; k < accum; k++)
                        {
                            //map[i, j - k] = 0;
                            deleteMap[i - k, j] = true;
                        }
                    }
                }
            }

            //check diags 1
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    int accum = 0;
                    int startColor = 0;
                    for (int q = 0; q < 9; q++)
                    {
                        var column = i + q;
                        var row = j + q;
                        if (column >= 9 || row >= 9)
                            break;

                        if (map[i + q, j + q] != startColor)
                        {
                            accum = 0;
                            startColor = map[i + q, j + q];
                        }
                        if (startColor != 0)
                            accum++;

                        if (accum >= BallInLine)
                        {
                            //remove line

                            for (int k = 0; k < accum; k++)
                            {
                                deleteMap[i + k, j + k] = true;
                            }
                        }
                    }
                }
            }

            //check diags 2
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    int accum = 0;
                    int startColor = 0;
                    for (int q = 0; q < 9; q++)
                    {
                        var column = i - q;
                        var row = j + q;
                        if (column < 0 || row >= 9)
                            break;

                        if (map[i - q, j + q] != startColor)
                        {
                            accum = 0;
                            startColor = map[i - q, j + q];
                        }
                        if (startColor != 0)
                            accum++;

                        if (accum >= BallInLine)
                        {
                            //remove line

                            for (int k = 0; k < accum; k++)
                            {
                                deleteMap[i - k, j + k] = true;
                            }
                        }
                    }
                }
            }

            //remove deleted
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (deleteMap[i, j])
                    {
                        scores++;
                        map[i, j] = 0;
                    }
                }
            }

            maxScores = Math.Max(maxScores, scores);
        }

        private void ResetGame()
        {
            state = GameState.Idle;
            map = new int[9, 9];
            for (int i = 0; i < 3; i++)
            {
                NewBall();
            }
        }

        public void DrawMap(Graphics gr)
        {
            int ballGap = 3;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    gr.DrawRectangle(Pens.White, i * CellSize, j * CellSize, CellSize, CellSize);
                    if (map[i, j] > 0)
                        gr.FillEllipse(new SolidBrush(colors[map[i, j] - 1]), i * CellSize + ballGap, j * CellSize + ballGap, CellSize - ballGap * 2, CellSize - ballGap * 2);

                    //gr.DrawString(wave[i, j].ToString(), SystemFonts.DefaultFont, Brushes.White, i * CellSize, j * CellSize);
                }
            }
        }


        public void NewBall()
        {
            int x = 0;
            int y = 0;
            do
            {
                x = rand.Next(map.GetLength(0));
                y = rand.Next(map.GetLength(1));
            } while (map[x, y] != 0);
            map[x, y] = rand.Next(colors.Length) + 1;
        }

        public int FreeCells()
        {
            int ret = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0)
                        ret++;
                }
            }
            return ret;
        }


        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);
            gr.ResetTransform();
            //gr.DrawString($"Scores: {scores}", SystemFonts.DefaultFont, Brushes.White, 5, 5);
            //gr.TranslateTransform(0, topGap);

            UpdateScene();
            DrawMap(gr);
            if (state == GameState.GameOver)
            {
                var text = "GAME OVER";
                var font = new Font("Press Start 2P", 24);
                var ms = gr.MeasureString(text, font);
                gr.FillRectangle(new SolidBrush(Color.FromArgb(220, Color.Black)), 0, 0, ClientSize.Width, ClientSize.Height);
                gr.DrawString(text, font, Brushes.DeepPink, ClientSize.Width / 2 - ms.Width / 2, ClientSize.Height / 2 - ms.Height / 2);
            }

        }
        DateTime lastMove = new DateTime() ;
        const int MoveAnimatinDelayMs = 100;
        private void UpdateScene()
        {
            if (state != GameState.BallMove)
                return;

            if (DateTime.Now.Subtract(lastMove).TotalMilliseconds < MoveAnimatinDelayMs)
                return;

            lastMove = DateTime.Now;

            if (!path.Any())
            {
                selectedBall = null;
                //check lines
                CheckLines();
                
                //generate new balls
                var freeQty = FreeCells();
                if (freeQty <= 3) //gameOver                    
                    state = GameState.GameOver;

                for (int i = 0; i < Math.Min(freeQty, 3); i++)
                {
                    NewBall();
                }
                state = GameState.Idle;
                return;
            }

            var nextCell = path.First();
            path.RemoveAt(0);
            //move ball to new position
            map[nextCell.X, nextCell.Y] = map[selectedBall.Value.X, selectedBall.Value.Y];
            map[selectedBall.Value.X, selectedBall.Value.Y] = 0;
            selectedBall = nextCell;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
