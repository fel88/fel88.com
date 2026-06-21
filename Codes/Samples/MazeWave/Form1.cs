using System.ComponentModel.Design;
using System.Data.Common;
using System.Security.Cryptography;

namespace MazeWave
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            StartPosition = FormStartPosition.CenterScreen;
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Start();
            timer.Tick += Timer_Tick;
            DoubleBuffered = true;
            MouseUp += Form1_MouseUp;
            Init();
            // fit window size
            ClientSize = new Size((int)(CellSize * map.GetLength(0)), (int)(CellSize * map.GetLength(1)));
        }

        #region Fields
        float CellSize = 30;
        Random Random = new Random();
        int[,] map = new int[16, 16];
        DateTime lastMove = DateTime.Now;
        int heroX = 10;
        int heroY = 10;
        const int N = 80;
        int[,] wave = new int[16, 16];
        List<Point> path = new List<Point>();
        #endregion

        private void Form1_MouseUp(object? sender, MouseEventArgs e)
        {
            var cursor = PointToClient(Cursor.Position);
            var row = (int)(cursor.Y / CellSize);
            var column = (int)(cursor.X / CellSize);

            if (map[column, row] != 0)
                return;

            BuildWave();
            BuildPath(column, row);
        }

        private void BuildPath(int targetX, int targetY)
        {
            // build path
            path = new List<Point>();
            path.Add(new Point(targetX, targetY));
            while (true)
            {
                var point = path.First();
                int level = wave[point.X, point.Y];

                if (point.X == heroX && point.Y == heroY)
                    break;

                bool exit = false;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Math.Abs(i + j) != 1)
                            continue;
                        if (wave[point.X + i, point.Y + j] == level - 1)
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
                    break;
                }
            }
        }

        private void BuildWave()
        {
            wave = new int[16, 16];
            Queue<(Point, int)> q = new Queue<(Point, int)>();
            q.Enqueue(new(new Point(heroX, heroY), 1));
            while (q.Any())
            {
                var deq = q.Dequeue();
                var p = deq.Item1;

                if (map[p.X, p.Y] != 0)
                    continue;

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

     
        public void Init()
        {
            InitBorder();
            RandomWallsInit();
            // init hero position
            do
            {
                heroX = Random.Next(1, map.GetLength(0) - 1);
                heroY = Random.Next(1, map.GetLength(1) - 1);
            } while (map[heroX, heroY] != 0);
        }

        public void InitBorder()
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                map[0, i] = 1;
                map[map.GetLength(0) - 1, i] = 1;
            }

            for (int i = 0; i < map.GetLength(0); i++)
            {
                map[i, 0] = 1;
                map[i, map.GetLength(1) - 1] = 1;
            }
        }


        public void RandomWallsInit()
        {
            for (int i = 0; i < N; i++)
            {
                var x = Random.Next(map.GetLength(0));
                var y = Random.Next(map.GetLength(1));
                map[x, y] = 1;
            }
        }

        public void UpdateScene()
        {            
            if (DateTime.Now.Subtract(lastMove).TotalMilliseconds <= 100)
                return;

            lastMove = DateTime.Now;
            int newHeroX = heroX;
            int newHeroY = heroY;
            if (path.Any())
            {
                var p = path.First();
                path.RemoveAt(0);

                newHeroX = p.X;
                newHeroY = p.Y;
            }

            if (map[newHeroX, newHeroY] == 0)
            {
                heroX = newHeroX;
                heroY = newHeroY;
            }
        }

        public void DrawHero(Graphics gr)
        {
            int heroSize = 24;
            var gap = (CellSize - heroSize) / 2;
            gr.FillEllipse(Brushes.LimeGreen, heroX * CellSize + gap, heroY * CellSize + gap, heroSize, heroSize);
            gr.DrawEllipse(Pens.Green, heroX * CellSize + gap, heroY * CellSize + gap, heroSize, heroSize);
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var cursor = PointToClient(Cursor.Position);
            var row = (int)(cursor.Y / CellSize);
            var column = (int)(cursor.X / CellSize);

            var gr = e.Graphics;
            gr.Clear(Color.Black);
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            UpdateScene();            

            //draw cursor
            gr.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.Green)), column * CellSize, row * CellSize, CellSize, CellSize);
            DrawPath(gr);

            //draw scene
            DrawMap(gr);
            DrawHero(gr);
        }

        private void DrawPath(Graphics gr)
        {
            foreach (var item in path)
            {
                gr.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.Blue)), item.X * CellSize, item.Y * CellSize, CellSize, CellSize);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }

        public void DrawBrick(Graphics gr, int x, int y)
        {
            gr.FillRectangle(Brushes.Firebrick, x * CellSize, y * CellSize, CellSize, CellSize);
            int brickHeight = 5;
            for (int i = 0; i < CellSize / brickHeight; i++)
            {
                gr.DrawLine(Pens.Silver, x * CellSize, y * CellSize + i * brickHeight, x * CellSize + CellSize, y * CellSize + i * brickHeight);
            }

            int brickLen = 15;
            for (int i = 0; i < CellSize / brickHeight; i += 2)
            {
                gr.DrawLine(Pens.Silver, x * CellSize + brickLen, y * CellSize + brickHeight * i, x * CellSize + brickLen, y * CellSize + brickHeight * (i + 1));
            }
            for (int i = 1; i < CellSize / brickHeight; i += 2)
            {
                gr.DrawLine(Pens.Silver, x * CellSize + brickLen / 2, y * CellSize + brickHeight * i, x * CellSize + brickLen / 2, y * CellSize + brickHeight * (i + 1));
                gr.DrawLine(Pens.Silver, x * CellSize + 3 * brickLen / 2, y * CellSize + brickHeight * i, x * CellSize + 3 * brickLen / 2, y * CellSize + brickHeight * (i + 1));
            }
        }



        public void DrawMap(Graphics gr, bool withLabels = false)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    gr.DrawRectangle(Pens.Silver, i * CellSize, j * CellSize, CellSize, CellSize);
                    if (withLabels)
                        gr.DrawString(wave[i, j].ToString(), SystemFonts.DefaultFont, Brushes.Silver, i * CellSize, j * CellSize);
                    if (map[i, j] == 1)
                        DrawBrick(gr, i, j);
                }
            }
        }
    }
}

