using System.Drawing.Drawing2D;

namespace XOX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(CellWidth * 3, CellWidth * 3);
            Paint += Form1_Paint;
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            timer.Start();
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            MouseUp += Form1_MouseUp;
            ResetGame();
        }

        const int CellGap = 25;
        const int WinLineGap = 10;
        const int CellWidth = 100;
        Random rand = new Random();
        const int PenThickness = 5;

        Pen borderPen = new Pen(Color.Silver, PenThickness);
        Pen winLinePen = new Pen(Color.White, PenThickness)
        {
            EndCap = LineCap.Round,
            StartCap = LineCap.Round
        };

        Pen penX = new Pen(Color.LimeGreen, PenThickness)
        {
            EndCap = LineCap.Round,
            StartCap = LineCap.Round
        };

        Pen penO = new Pen(Color.Red, PenThickness);

        int[,] board = new int[3, 3];
        bool gameOver = false;
        public enum GameState
        {
            Idle, PlayerMove, ComputerMove, GameOver
        }

        private void Form1_MouseUp(object? sender, MouseEventArgs e)
        {
            if (gameOver)
            {
                ResetGame();
                return;
            }

            var col = e.Location.X / CellWidth;
            var row = e.Location.Y / CellWidth;

            if (board[col, row] != 0)
                return;

            board[col, row] = 1;
            if (!IsGameOver())
                ComputerMove();
        }

        private void ResetGame()
        {
            gameOver = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = 0;

                }
            }

            if (rand.Next(100) > 50)
                ComputerMove();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }

        void ComputerMove()
        {
            int x;
            int y;
            //check free cells available
            int qty = FreeCells();
            if (qty == 0)
                return;
            do
            {
                x = rand.Next(3);
                y = rand.Next(3);

            } while (board[x, y] != 0);
            board[x, y] = 2;
        }

        private int FreeCells()
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                        count++;
                }
            }
            return count;
        }

        public void DrawBoard(Graphics gr)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gr.DrawRectangle(borderPen, i * CellWidth, j * CellWidth, CellWidth, CellWidth);
                    if (board[i, j] == 1)//draw X
                    {
                        DrawX(gr, i, j);
                    }
                    else if (board[i, j] == 2)//draw O
                    {
                        DrawO(gr, i, j);
                    }
                }
            }
        }

        void DrawWinLine(Graphics gr)
        {
            for (int i = 0; i < 3; i++)
            {
                if (IsRowWin(i))
                    gr.DrawLine(winLinePen, WinLineGap, i * CellWidth + CellWidth / 2, ClientSize.Width - WinLineGap, i * CellWidth + CellWidth / 2);

                if (IsColumnWin(i))
                    gr.DrawLine(winLinePen, i * CellWidth + CellWidth / 2, WinLineGap, i * CellWidth + CellWidth / 2, ClientSize.Height - WinLineGap);
            }

            if (IsDiagWin())
                gr.DrawLine(winLinePen, WinLineGap, WinLineGap, ClientSize.Width - WinLineGap, ClientSize.Height - WinLineGap);

            if (IsBackDiagWin())
                gr.DrawLine(winLinePen, WinLineGap, ClientSize.Height - WinLineGap, ClientSize.Width - WinLineGap, WinLineGap);
        }


        private void DrawX(Graphics gr, int i, int j)
        {
            gr.DrawLine(penX, i * CellWidth + CellGap, j * CellWidth + CellGap, (i + 1) * CellWidth - CellGap, (j + 1) * CellWidth - CellGap);
            gr.DrawLine(penX, i * CellWidth + CellGap, (j + 1) * CellWidth - CellGap, (i + 1) * CellWidth - CellGap, j * CellWidth + CellGap);
        }

        private void DrawO(Graphics gr, int i, int j)
        {
            gr.DrawEllipse(penO, i * CellWidth + CellGap, j * CellWidth + CellGap, CellWidth - CellGap * 2, CellWidth - CellGap * 2);
        }


        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gameOver = IsGameOver();

            gr.Clear(Color.Black);
            DrawBoard(gr);
            DrawWinLine(gr);
        }

        bool AreNumbersEqual(int v1, int v2, int v3) => v1 == v2 && v2 == v3;
        bool IsRowWin(int row) => board[0, row] != 0 && AreNumbersEqual(board[0, row], board[1, row], board[2, row]);
        bool IsColumnWin(int column) => board[column, 0] != 0 && AreNumbersEqual(board[column, 0], board[column, 1], board[column, 2]);
        bool IsDiagWin() => board[0, 0] != 0 && AreNumbersEqual(board[0, 0], board[1, 1], board[2, 2]);
        bool IsBackDiagWin() => board[2, 0] != 0 && AreNumbersEqual(board[2, 0], board[1, 1], board[0, 2]);

        private bool IsGameOver()
        {
            for (int i = 0; i < 3; i++)
            {
                if (IsRowWin(i) || IsColumnWin(i))
                {
                    return true;
                }
            }

            if (IsDiagWin() || IsBackDiagWin())
                return true;

            if (FreeCells() == 0)
                return true;

            return false;
        }

    }
}
