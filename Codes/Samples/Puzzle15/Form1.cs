using System.ComponentModel.Design;
using System.Text;

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
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ClientSize = new Size(4 * CellSize, 4 * CellSize);
            ResetGame();
            Text = $"fel88.com - puzzle 15 (level: {difficultyLevel})";

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


        static int findEmptyRowIdx(int[,] puzzle)
        {
            for (int i = puzzle.GetUpperBound(0); i >= 0; i--)
            {
                for (int j = puzzle.GetUpperBound(1); j >= 0; j--)
                {
                    if (puzzle[i, j] == 0)
                        return puzzle.GetUpperBound(0) - i;
                }
            }
            return -1;
        }


        static bool isSolvable(int[,] puzzle)
        {
            List<int> puzzle1D = new List<int>();
            for (int i = 0; i < puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < puzzle.GetLength(1); j++)
                {
                    if (puzzle[i, j] == 0)
                        continue;

                    puzzle1D.Add(puzzle[i, j]);
                }
            }

            //calc inversions
            int invQty = 0;
            for (int i = 0; i < puzzle1D.Count; i++)
            {
                for (int j = i + 1; j < puzzle1D.Count; j++)
                {
                    if (puzzle1D[i] > puzzle1D[j])
                        invQty++;
                }
            }

            return (findEmptyRowIdx(puzzle) + invQty) % 2 == 0;
        }


        private Point? GetEmptyPoint(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == 0)
                        return new Point(i, j);
                }
            }
            return null;
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

        int difficultyLevel = 0;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.R)
            {
                ResetGame();
            }
            if (keyData == Keys.D)
            {
                difficultyLevel++;
                difficultyLevel %= 3;
                Text = $"fel88.com - puzzle 15 (level: {difficultyLevel})";
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        const int EasyLevelTurnsQty = 30;
        const int MediumLevelTurnsQty = 200;
        private void ResetGame()
        {
            gameOver = false;
            InitBoard();
            if (difficultyLevel == 0)
                RandomMoves(EasyLevelTurnsQty);

            else
            if (difficultyLevel == 1)
                RandomMoves(MediumLevelTurnsQty);

            else
            {
                if (rand.Next(100) > 80)                
                    SetBoard(hardBoard);                
                else
                    do
                    {
                        MakeRandomBoard();
                    } while (!isSolvable(board));
            }
        }

        void SetBoard(int[] b)
        {
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    board[j, i] = b[index++];
                }
            }
        }

        int[] hardBoard = new int[] { // 80 moves board
                0,12,9,13,
                15,11,10,14,
                3,7,2,5,
                4,8,6,1
            };

        private void MakeRandomBoard()
        {
            List<int> remains = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                remains.Add(i);
            }

            //shuffle
            remains = remains.OrderBy(z => rand.Next(100)).ToList();
            //assign
            for (int i = 0; i < remains.Count; i++)
            {
                int col = i / 4;
                int row = i % 4;
                board[col, row] = remains[i];
            }
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

        private void InitBoard()
        {
            for (int i = 0; i < 15; i++)
            {
                int row = i / 4;
                int col = i % 4;
                board[col, row] = i + 1;
            }
            board[3, 3] = 0;
        }

        public void RandomMoves(int N)
        {
            for (int i = 0; i < N; i++)
            {
                var empty = GetEmptyPoint(board).Value;

                var up = new Point(empty.X, empty.Y - 1);
                var down = new Point(empty.X, empty.Y + 1);
                var left = new Point(empty.X - 1, empty.Y);
                var right = new Point(empty.X + 1, empty.Y);
                Point[] moves = new[] { up, down, left, right };
                moves = moves.Where(z => z.X >= 0 && z.X <= board.GetUpperBound(0)
                && z.Y >= 0 && z.Y <= board.GetUpperBound(1)
                ).ToArray();
                var move = moves[rand.Next(moves.Length)];
                Turn(move.X, move.Y);
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
