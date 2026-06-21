namespace DragRectangle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var timer = new System.Windows.Forms.Timer();
            StartPosition = FormStartPosition.CenterScreen;
            timer.Interval = 5;
            timer.Start();
            timer.Tick += Timer_Tick;
            DoubleBuffered = true;
            Paint += Form1_Paint;
            MouseDown += Form1_MouseDown;
            MouseUp += Form1_MouseUp;

            posX = ClientSize.Width / 2 - rectWidth / 2;
            posY = ClientSize.Height / 2 - rectHeight / 2;

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }

        int startPosX;
        int startPosY;
        int startCursorX;
        int startCursorY;

        int posX = 50;
        int posY = 50;
        int rectWidth = 200;
        int rectHeight = 150;

        bool isDrag = false;
        private void Form1_MouseUp(object? sender, MouseEventArgs e)
        {
            isDrag = false;
        }

        private void Form1_MouseDown(object? sender, MouseEventArgs e)
        {
            var cursor = PointToClient(Cursor.Position);
            RectangleF rect = new RectangleF(posX, posY, rectWidth, rectHeight);
            if (rect.Contains(cursor))
            {
                startPosX = posX;
                startPosY = posY;
                startCursorX = cursor.X;
                startCursorY = cursor.Y;
                isDrag = true;
            }
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);
            var cursor = PointToClient(Cursor.Position);
            if (isDrag)
            {
                var cursorDiffX = cursor.X - startCursorX;
                var cursorDiffY = cursor.Y - startCursorY;
                posX = startPosX + cursorDiffX;
                posY = startPosY + cursorDiffY;
            }
            RectangleF rect = new RectangleF(posX, posY, rectWidth, rectHeight);
            if (rect.Contains(cursor))
            {
                gr.FillRectangle(Brushes.LimeGreen, posX, posY, rectWidth, rectHeight);
            }
            gr.DrawRectangle(Pens.White, posX, posY, rectWidth, rectHeight);
        }
    }
}
