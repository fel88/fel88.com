namespace Snake
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
            ResetGame();
        }

        const int MaxApples = 10;
        int Rows => (int)(ClientRectangle.Height / CellSize);
        int Cols => (int)(ClientRectangle.Width / CellSize);
        bool gameOver = false;
        SnakeNode Head = new SnakeNode();
        public List<Apple> Apples = new List<Apple>();
        MoveDirection dir;
        float CellSize = 30;
        Random Random = new Random();

        public void NewApple()
        {
            int x = 0;
            int y = 0;
            do
            {
                x = Random.Next(1, Cols - 1);
                y = Random.Next(1, Rows - 1);
            }
            while (Apples.Any(z => z.X == x && z.Y == y) || SnakeHas(x, y));

            Apples.Add(new Apple() { X = x, Y = y });
        }

        private bool SnakeHas(int x, int y)
        {
            var node = Head;
            while (node != null)
            {
                if (node.X == x && node.Y == y)
                    return true;

                node = node.Next;
            }
            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    if (dir != MoveDirection.Right)
                        dir = MoveDirection.Left;
                    break;
                case Keys.Right:
                    if (dir != MoveDirection.Left)
                        dir = MoveDirection.Right;
                    break;
                case Keys.Up:
                    if (dir != MoveDirection.Bottom)
                        dir = MoveDirection.Top;
                    break;
                case Keys.Down:
                    if (dir != MoveDirection.Top)
                        dir = MoveDirection.Bottom;
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!gameOver)
                UpdateScene();

            Invalidate();
        }

        private void UpdateScene()
        {
            SnakeNode newHead = new SnakeNode();

            newHead.X = Head.X;
            newHead.Y = Head.Y;
            switch (dir)
            {
                case MoveDirection.Left:
                    newHead.X--;
                    break;
                case MoveDirection.Right:
                    newHead.X++;
                    break;
                case MoveDirection.Top:
                    newHead.Y--;
                    break;
                case MoveDirection.Bottom:
                    newHead.Y++;
                    break;
            }

            if (CheckCollisions(newHead))
                return;

            newHead.Next = Head;
            Head = newHead;

            bool removeLastNode = true;
            if (Apples.Any(z => z.X == Head.X && z.Y == Head.Y))
            {
                Apples.RemoveAll(z => z.X == Head.X && z.Y == Head.Y);
                NewApple();
                removeLastNode = false;
            }

            if (!removeLastNode)
                return;

            //remove last node
            var node = Head;
            while (true)
            {
                if (node.Next != null && node.Next.Next == null)
                {
                    node.Next = null;
                    break;
                }
                node = node.Next;
            }
        }

        public void DrawWall(Graphics gr, int x, int y)
        {
            gr.DrawRectangle(Pens.Black, x * CellSize, y * CellSize, CellSize, CellSize);
            gr.FillRectangle(Brushes.Thistle, x * CellSize, y * CellSize, CellSize, CellSize);
        }

        public void DrawAllWalls(Graphics gr)
        {
            for (int i = 0; i < Cols; i++)
            {
                DrawWall(gr, i, 0);
                DrawWall(gr, i, Rows - 1);
            }
            for (int i = 0; i < Rows; i++)
            {
                DrawWall(gr, 0, i);
                DrawWall(gr, Cols - 1, i);
            }
        }

        public void DrawNode(SnakeNode node, Graphics gr, float size, Brush brush)
        {
            gr.DrawEllipse(Pens.Black, node.X * CellSize + (CellSize - size) / 2, node.Y * CellSize + (CellSize - size) / 2, size, size);
            gr.FillEllipse(brush, node.X * CellSize + (CellSize - size) / 2, node.Y * CellSize + (CellSize - size) / 2, size, size);
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var gr = e.Graphics;
            DrawAllWalls(gr);
            DrawSnake(gr);
            DrawApples(gr);

            var font = new Font("Helvetica", 35);
            var ms = gr.MeasureString("GAME OVER", font);
            if (gameOver)
                gr.DrawString("GAME OVER", font, Brushes.Red, ClientRectangle.Width / 2 - ms.Width / 2, ClientRectangle.Height / 2 - ms.Height / 2);
        }

        private void DrawApples(Graphics gr)
        {
            foreach (var apple in Apples)
            {
                gr.FillEllipse(Brushes.Tomato, apple.X * CellSize, apple.Y * CellSize, CellSize, CellSize);
            }
        }

        private bool CheckCollisions(SnakeNode head)
        {
            if (head.X == 0 || head.Y == 0 || head.X == Cols - 1 || head.Y == Rows - 1)
            {
                gameOver = true;
                return true;
            }

            var node = Head;
            while (node != null)
            {
                if (node.X == head.X && node.Y == head.Y)
                {
                    gameOver = true;
                    return true;
                }
                node = node.Next;
            }

            return false;
        }

        private void DrawSnake(Graphics gr)
        {
            DrawNode(Head, gr, CellSize, Brushes.Orange);
            var node = Head.Next;
            while (node != null)
            {
                DrawNode(node, gr, CellSize * 0.9f, Brushes.LightGreen);
                node = node.Next;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameOver)
            {
                ResetGame();
            }
        }

        private void ResetGame()
        {
            Apples.Clear();
            Head = new SnakeNode();
            gameOver = false;
            Head.X = (int)(ClientRectangle.Width / CellSize / 2);
            Head.Y = (int)(ClientRectangle.Height / CellSize / 2);

            var w = (int)(ClientRectangle.Width % CellSize);
            var h = (int)(ClientRectangle.Height % CellSize);
            Size = new Size(Size.Width - w, Size.Height - h);

            dir = MoveDirection.Left;

            for (int i = 0; i < MaxApples; i++)
                NewApple();

        }
    }
}
