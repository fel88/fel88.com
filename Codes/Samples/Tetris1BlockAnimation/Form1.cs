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
            ClientSize = new Size(BlockSize * 6, BlockSize * 8);
            wall = new int[(int)(ClientRectangle.Width / BlockSize), (int)(ClientRectangle.Height / BlockSize)];
            ResetGame();

        }

        Timer timer = new Timer();

        Brush[] brushes = [
            Brushes.LimeGreen,
            Brushes.DeepPink,
            Brushes.Blue, 
            Brushes.Chocolate, 
            Brushes.Orange
        ];

        int brushIdx = 1;
        int BlockSize = 40;
        int[,] wall;

        int BrickX = 0;
        int BrickY = 0;

        Random Random = new Random();

        bool IsLineFilled(int lineIdx)
        {
            bool filled = true;
            for (int i = 0; i < wall.GetLength(0); i++)
            {
                if (wall[i, lineIdx] < 0)
                {
                    filled = false;
                    break;
                }
            }
            return filled;
        }


        private void DrawBlock(Graphics gr, int col, int row, Brush brush)
        {
            gr.FillRectangle(brush, col * BlockSize, row * BlockSize, BlockSize, BlockSize);
            int gap = 5;
            gr.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.White)), col * BlockSize + gap, row * BlockSize + gap, BlockSize - gap * 2, BlockSize - gap * 2);

            gr.FillPolygon(new SolidBrush(Color.FromArgb(128, Color.Black)), [
                new PointF((col+1) * BlockSize - gap,row*BlockSize+gap),
                new PointF((col+1) * BlockSize ,row*BlockSize),
                new PointF((col+1) * BlockSize ,(row+1)*BlockSize),
                new PointF((col+1) * BlockSize-gap ,(row+1)*BlockSize-gap),
            ]);

            gr.FillPolygon(new SolidBrush(Color.FromArgb(64, Color.Black)), [

                new PointF((col+1) * BlockSize ,(row+1)*BlockSize),
                new PointF((col+1) * BlockSize-gap ,(row+1)*BlockSize-gap),
                new PointF((col) * BlockSize+gap ,(row+1)*BlockSize-gap),
                new PointF((col) * BlockSize ,(row+1)*BlockSize),
            ]);

            gr.FillPolygon(new SolidBrush(Color.FromArgb(128, Color.White)), [

                new PointF((col+1) * BlockSize ,(row)*BlockSize),
                new PointF((col+1) * BlockSize-gap ,(row)*BlockSize+gap),
                new PointF((col) * BlockSize+gap ,(row)*BlockSize+gap),
                new PointF((col) * BlockSize ,(row)*BlockSize),
            ]);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (gameOver && keyData == Keys.R)
            {
                gameOver = false;
                //ResetGame();
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
                    wall[i, j] = -1;
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

            DrawBlock(gr, BrickX, BrickY, brushes[brushIdx]);

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
                wall[i, 0] = -1;
            }
        }

        private void DrawBlocks(Graphics gr)
        {
            for (int i = 0; i < wall.GetLength(0); i++)
                for (int j = 0; j < wall.GetLength(1); j++)
                    if (wall[i, j] >= 0)
                        DrawBlock(gr, i, j, brushes[wall[i, j]]);
        }

        bool gameOver = true;
        public void UpdateScene()
        {
            if (BrickY != wall.GetLength(1) - 1 && wall[BrickX, BrickY + 1] < 0)
            {
                BrickY++;
                return;
            }

            wall[BrickX, BrickY] = brushIdx;
            brushIdx = Random.Next(brushes.Length);

            if (!IsLineFilled(0))
            {
                BrickY = 0;
                do
                {
                    BrickX = Random.Next(wall.GetLength(0));
                } while (wall[BrickX, BrickY] >= 0);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }
    }

}
