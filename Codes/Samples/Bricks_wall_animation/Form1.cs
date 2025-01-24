using Timer = System.Windows.Forms.Timer;

namespace Chessboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer.Start();
            timer.Tick += Timer_Tick;
            timer.Interval = 50;
            DoubleBuffered = true;
            Paint += Form1_Paint;
            wall = new int[(int)(ClientRectangle.Width / BrickSize), (int)(ClientRectangle.Height / BrickSize)];
            BrickX = Random.Next(wall.GetLength(0));
        }

        Timer timer = new Timer();

        float BrickSize = 40;
        int[,] wall;

        int BrickX = 0;
        int BrickY = 0;

        Random Random = new Random();

        bool AllFilled()
        {
            bool allFilled = true;
            for (int i = 0; i < wall.GetLength(0); i++)
            {
                if (wall[i, 0] == 0)
                {
                    allFilled = false;
                    break;
                }
            }
            return allFilled;
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            e.Graphics.Clear(Color.White);
            var gr = e.Graphics;

            gr.FillRectangle(Brushes.Blue, BrickX * BrickSize, BrickY * BrickSize, BrickSize, BrickSize);

            for (int i = 0; i < wall.GetLength(0); i++)
                for (int j = 0; j < wall.GetLength(1); j++)
                {
                    if (wall[i, j] == 1)
                        gr.FillRectangle(Brushes.Blue, i * BrickSize, j * BrickSize, BrickSize, BrickSize);
                }
            if (AllFilled())
                timer.Stop();
        }

        public void UpdateScene()
        {
            if (BrickY == wall.GetLength(1) - 1 || wall[BrickX, BrickY + 1] == 1)
            {
                wall[BrickX, BrickY] = 1;
                if (!AllFilled())
                {
                    BrickY = 0;
                    do
                    {
                        BrickX = Random.Next(wall.GetLength(0));
                    } while (wall[BrickX, BrickY] == 1);
                }
            }
            else
                BrickY++;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!AllFilled())
                UpdateScene();

            Invalidate();
        }        
    }
}
