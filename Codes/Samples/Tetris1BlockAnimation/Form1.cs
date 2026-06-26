using Timer = System.Windows.Forms.Timer;

namespace Tetris1BlockAnimation
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
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Paint += Form1_Paint;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(BrickSize * 6, BrickSize * 8);
            wall = new int[(int)(ClientRectangle.Width / BrickSize), (int)(ClientRectangle.Height / BrickSize)];
            BrickX = Random.Next(wall.GetLength(0));
        }

        Timer timer = new Timer();

        Color[] colors = new Color[] { Color.LimeGreen, Color.DeepPink, Color.Blue, Color.Chocolate, Color.Orange };
        int colorIdx = 1;
        int BrickSize = 40;
        int[,] wall;

        int BrickX = 0;
        int BrickY = 0;

        Random Random = new Random();

        bool IsLineFilled(int lineIdx)
        {
            bool allFilled = true;
            for (int i = 0; i < wall.GetLength(0); i++)
            {
                if (wall[i, lineIdx] == 0)
                {
                    allFilled = false;
                    break;
                }
            }
            return allFilled;
        }


        private void DrawBlock(Graphics gr, int col, int row, int colorIndex)
        {
            gr.FillRectangle(new SolidBrush(colors[colorIndex]), col * BrickSize, row * BrickSize, BrickSize, BrickSize);
            int gap = 5;
            gr.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.White)), col * BrickSize + gap, row * BrickSize + gap, BrickSize - gap * 2, BrickSize - gap * 2);

            gr.FillPolygon(new SolidBrush(Color.FromArgb(128, Color.Black)), [
                new PointF((col+1) * BrickSize - gap,row*BrickSize+gap),
                new PointF((col+1) * BrickSize ,row*BrickSize),
                new PointF((col+1) * BrickSize ,(row+1)*BrickSize),
                new PointF((col+1) * BrickSize-gap ,(row+1)*BrickSize-gap),
            ]);

            gr.FillPolygon(new SolidBrush(Color.FromArgb(64, Color.Black)), [

                new PointF((col+1) * BrickSize ,(row+1)*BrickSize),
                new PointF((col+1) * BrickSize-gap ,(row+1)*BrickSize-gap),
                new PointF((col) * BrickSize+gap ,(row+1)*BrickSize-gap),
                new PointF((col) * BrickSize ,(row+1)*BrickSize),
            ]);

            gr.FillPolygon(new SolidBrush(Color.FromArgb(128, Color.White)), [

                new PointF((col+1) * BrickSize ,(row)*BrickSize),
                new PointF((col+1) * BrickSize-gap ,(row)*BrickSize+gap),
                new PointF((col) * BrickSize+gap ,(row)*BrickSize+gap),
                new PointF((col) * BrickSize ,(row)*BrickSize),
            ]);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (gameOver && keyData == Keys.R)
            {
                ResetGame();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ResetGame()
        {
            gameOver = false;
            for (int i = 0; i < wall.GetLength(0); i++)
            {
                for (int j = 0; j < wall.GetLength(1); j++)
                {
                    wall[i, j] = 0;
                }
            }

            BrickY = 0;
            BrickX = Random.Next(wall.GetLength(0));
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (!gameOver)
                UpdateScene();

            e.Graphics.Clear(Color.Black);
            var gr = e.Graphics;

            DrawBlock(gr, BrickX, BrickY, colorIdx);

            DrawBlocks(gr);

            if (IsLineFilled(wall.GetLength(1) - 1))
                RemoveBottomLine();

            if (IsLineFilled(0))
                gameOver = true;
        }

        private void RemoveBottomLine()
        {
            //shift all rows
            for (int i = wall.GetLength(1) - 1; i > 0; i--)
            {
                for (int j = 0; j < wall.GetLength(0); j++)
                {
                    wall[j, i] = wall[j, i - 1];
                }
            }

            //clear first line
            for (int i = 0; i < wall.GetLength(0); i++)
            {
                wall[i, 0] = 0;
            }
        }

        private void DrawBlocks(Graphics gr)
        {
            for (int i = 0; i < wall.GetLength(0); i++)
                for (int j = 0; j < wall.GetLength(1); j++)
                {
                    if (wall[i, j] != 0)
                    {
                        DrawBlock(gr, i, j, wall[i, j] - 1);
                    }

                }
        }

        bool gameOver = false;
        public void UpdateScene()
        {
            if (BrickY != wall.GetLength(1) - 1 && wall[BrickX, BrickY + 1] == 0)
            {
                BrickY++;
                return;
            }

            wall[BrickX, BrickY] = colorIdx + 1;
            colorIdx = Random.Next(colors.Length);
            if (!IsLineFilled(0))
            {
                BrickY = 0;
                do
                {
                    BrickX = Random.Next(wall.GetLength(0));
                } while (wall[BrickX, BrickY] != 0);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }
    }

}
