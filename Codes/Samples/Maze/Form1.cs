using System.Runtime.CompilerServices;
using System.Security;

namespace Maze
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 200;
            timer.Start();
            timer.Tick += Timer_Tick;
            DoubleBuffered = true;
            Init();
            // fit window size
            ClientSize = new Size((int)(CellSize * map.GetLength(0)), (int)(CellSize * map.GetLength(1)));
        }

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

        const int N = 40;
        public void RandomWallsInit()
        {
            for (int i = 0; i < N; i++)
            {
                var x = Random.Next(map.GetLength(0));
                var y = Random.Next(map.GetLength(1));
                map[x, y] = 1;
            }
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var gr = e.Graphics;
            DrawMap(gr);
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
