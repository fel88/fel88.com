namespace MazeKeyboard
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
            KeyUp += Form1_KeyUp;
            KeyDown += Form1_KeyDown;
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

        bool moveLeft;
        bool moveRight;
        bool moveUp;
        bool moveDown;
        #endregion

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    moveLeft = true;
                    break;
                case Keys.Right:
                    moveRight = true;
                    break;
                case Keys.Up:
                    moveUp = true;
                    break;
                case Keys.Down:
                    moveDown = true;
                    break;
            }
        }


        private void Form1_KeyUp(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    moveLeft = false;
                    break;
                case Keys.Right:
                    moveRight = false;
                    break;
                case Keys.Up:
                    moveUp = false;
                    break;
                case Keys.Down:
                    moveDown = false;
                    break;
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

            if (moveLeft && map[newHeroX - 1, newHeroY] == 0)
                newHeroX--;

            if (moveRight && map[newHeroX + 1, newHeroY] == 0)
                newHeroX++;

            if (moveUp && map[newHeroX, newHeroY - 1] == 0)
                newHeroY--;

            if (moveDown && map[newHeroX, newHeroY + 1] == 0)
                newHeroY++;

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
            var gr = e.Graphics;
            gr.Clear(Color.Black);
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            UpdateScene();
            //draw scene
            DrawMap(gr);
            DrawHero(gr);
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



        public void DrawMap(Graphics gr)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    gr.DrawRectangle(Pens.Silver, i * CellSize, j * CellSize, CellSize, CellSize);
                    if (map[i, j] == 1)
                        DrawBrick(gr, i, j);
                }
            }
        }
    }
}
