namespace Puzzle15
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            DoubleBuffered = true;
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            timer.Start();
            Paint += Form1_Paint;
            MouseUp += Form1_MouseUp;
            ClientSize = new Size(4 * CellSize, 4 * CellSize);
            InitBoard();
        }

        private void Form1_MouseUp(object? sender, MouseEventArgs e)
        {
            if (gameOver)
            {
                ResetGame();
                return;
            }
            int row = e.Location.Y / CellSize;
            int col = e.Location.X / CellSize;

            Turn(col, row);
        }


        public void Turn(int col, int row)
        {
            var val = board[col, row];
            //check 4 directions
            if (row > 0 && board[col, row - 1] == 0) // check up and move if empty
            {
                board[col, row] = 0;
                board[col, row - 1] = val;
            }
            else
            if (col > 0 && board[col - 1, row] == 0) // check left and move if empty
            {
                board[col, row] = 0;
                board[col - 1, row] = val;
            }
            else
            if (row < 3 && board[col, row + 1] == 0) // check down and move if empty
            {
                board[col, row] = 0;
                board[col, row + 1] = val;
            }
            else
            if (col < 3 && board[col + 1, row] == 0) // check right and move if empty
            {
                board[col, row] = 0;
                board[col + 1, row] = val;
            }
        }

        private void ResetGame()
        {
            gameOver = false;
            InitBoard();
        }

        bool IsGameOver()
        {
            for (int i = 0; i < 15; i++)
            {
                int row = i / 4;
                int col = i % 4;
                if (board[col, row] != i + 1)
                    return false;
            }

            return true;
        }

        bool gameOver = false;
        const int InitMovesQty = 200;
        private void InitBoard()
        {
            for (int i = 0; i < 15; i++)
            {
                int row = i / 4;
                int col = i % 4;
                board[col, row] = i + 1;
            }

            for (int i = 0; i < InitMovesQty; i++)
            {
                int x = rand.Next(4);
                int y = rand.Next(4);
                Turn(x, y);
            }
        }

        Random rand = new Random();
        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            DrawBoard(e.Graphics);
            gameOver = IsGameOver();
            if (gameOver)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Black)), 0, 0, ClientSize.Width, ClientSize.Height);

                DrawCentredString(e.Graphics, "Game Over", new Font("Press Start 2P", 24), Brushes.Orange, ClientSize.Width / 2, ClientSize.Height / 2);                
            }
        }

        private void DrawBlock(Graphics gr, int col, int row, Brush brush, int gap)
        {            
            gr.FillRectangle(brush, col * CellSize, row * CellSize, CellSize, CellSize);
            
            gr.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.White)), col * CellSize + gap, row * CellSize + gap, CellSize - gap * 2, CellSize - gap * 2);

            gr.FillPolygon(new SolidBrush(Color.FromArgb(128, Color.Black)), [
                new PointF((col+1) * CellSize - gap,row*CellSize+gap),
                new PointF((col+1) * CellSize ,row*CellSize),
                new PointF((col+1) * CellSize ,(row+1)*CellSize),
                new PointF((col+1) * CellSize-gap ,(row+1)*CellSize-gap),
            ]);

            gr.FillPolygon(new SolidBrush(Color.FromArgb(64, Color.Black)), [

                new PointF((col+1) * CellSize ,(row+1)*CellSize),
                new PointF((col+1) * CellSize-gap ,(row+1)*CellSize-gap),
                new PointF((col) * CellSize+gap ,(row+1)*CellSize-gap),
                new PointF((col) * CellSize ,(row+1)*CellSize),
            ]);

            gr.FillPolygon(new SolidBrush(Color.FromArgb(128, Color.White)), [

                new PointF((col+1) * CellSize ,(row)*CellSize),
                new PointF((col+1) * CellSize-gap ,(row)*CellSize+gap),
                new PointF((col) * CellSize+gap ,(row)*CellSize+gap),
                new PointF((col) * CellSize ,(row)*CellSize),
            ]);
        }

        public void DrawCentredString(Graphics gr, string text, Font font, Brush brush, int centerX, int centerY)
        {
            var textSize = gr.MeasureString(text, font);
            var left = centerX - textSize.Width / 2;
            var top = centerY - textSize.Height / 2;
            gr.DrawString(text, font, brush, left, top);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }

        const int blockVolumeGap = 10;
        const int CellSize = 90;
        int[,] board = new int[4, 4];
        Font font = new Font("Arial", 30);
        Pen borderPen = new Pen(Color.White, 2);
        void DrawBoard(Graphics gr)
        {
            gr.Clear(Color.Black);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {              
                    //gr.DrawRectangle(borderPen, i * CellSize, j * CellSize, CellSize, CellSize);

                    if (board[i, j] == 0)
                        continue;

                    //gr.FillRectangle(Brushes.Goldenrod, i * CellSize, j * CellSize, CellSize, CellSize);
                    DrawBlock(gr, i, j, Brushes.Green, blockVolumeGap);

                    //DrawCentredString(gr, board[i, j].ToString(), font, Brushes.LightGoldenrodYellow, i * CellSize + CellSize / 2, j * CellSize + CellSize / 2);
                    DrawCentredString(gr, board[i, j].ToString(), font, Brushes.LightGoldenrodYellow, i * CellSize + CellSize / 2, j * CellSize + CellSize / 2);

                }
            }
        }

    }
}
