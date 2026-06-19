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

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {

            //  if (movePressed)
            //   return;

            //movePressed = true;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    dir = MoveDirectoion.Left;
                    moveLeft = true;

                    break;
                case Keys.Right:
                    dir = MoveDirectoion.Right;
                    moveRight = true;
                    break;
                case Keys.Up:
                    dir = MoveDirectoion.Up;
                    moveUp = true;
                    break;
                case Keys.Down:
                    dir = MoveDirectoion.Down;
                    moveDown = true;
                    break;
            }

        }
        bool moveLeft;
        bool moveRight;
        bool moveUp;
        bool moveDown;

        private void Form1_KeyUp(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    dir = MoveDirectoion.Left;
                    moveLeft = false;

                    break;
                case Keys.Right:
                    dir = MoveDirectoion.Right;
                    moveRight = false;
                    break;
                case Keys.Up:
                    dir = MoveDirectoion.Up;
                    moveUp = false;
                    break;
                case Keys.Down:
                    dir = MoveDirectoion.Down;
                    moveDown = false;
                    break;
            }
            //if (!moveLeft && !moveDown && !moveRight && !moveUp)
            //lastMove = DateTime.Now.AddMinutes(-1);



        }
        public enum MoveDirectoion
        {
            None, Left, Right, Up, Down
        }
        MoveDirectoion dir;

        Bitmap hero = null;
        public void Init()
        {
            InitBorder();
            RandomWallsInit();
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

        const int N = 80;
        public void RandomWallsInit()
        {
            for (int i = 0; i < N; i++)
            {
                var x = Random.Next(map.GetLength(0));
                var y = Random.Next(map.GetLength(1));
                map[x, y] = 1;
            }
        }
        int heroX = 10;
        int heroY = 10;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            return base.ProcessCmdKey(ref msg, keyData);
        }

        bool movePressed = false;
        bool moveReleased = false;
        DateTime lastMove = DateTime.Now;
        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            var gr = e.Graphics;
            if (DateTime.Now.Subtract(lastMove).TotalMilliseconds > 100)
            {
                lastMove = DateTime.Now;
                int newHeroX = heroX;
                int newHeroY = heroY;

                if (moveLeft)

                    if (map[newHeroX - 1, newHeroY] == 0)
                    {
                        newHeroX--;
                    }
                if (moveRight)

                    if (map[newHeroX + 1, newHeroY] == 0)
                    {
                        newHeroX++;

                    }
                if (moveUp)

                    if (map[newHeroX, newHeroY - 1] == 0)
                    {
                        newHeroY--;

                    }
                if (moveDown)

                    if (map[newHeroX, newHeroY + 1] == 0)

                    {
                        newHeroY++;

                    }
                if (map[newHeroX, newHeroY] == 0)

                {
                    heroX = newHeroX;
                    heroY = newHeroY;
                }
            }


            DrawMap(gr);
            //gr.DrawImage(hero, new RectangleF(300,300,30,30),new RectangleF(0,0,hero.Width,hero.Height),GraphicsUnit.Pixel);
            gr.FillEllipse(Brushes.LimeGreen, heroX * CellSize + 3, heroY * CellSize + 3, 24, 24);
            gr.DrawEllipse(Pens.Green, heroX * CellSize + 3, heroY * CellSize + 3, 24, 24);

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }

        int[,] map = new int[16, 16];
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

        float CellSize = 30;

        Random Random = new Random();

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
