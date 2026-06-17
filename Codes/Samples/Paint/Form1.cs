using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            gr = Graphics.FromImage(bmp);
            DoubleBuffered = true;
            gr.Clear(Color.Black);
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Start();
            timer.Tick += Timer_Tick;
            MouseDown += Form1_MouseDown;
            MouseUp += Form1_MouseUp;
            MouseWheel += Form1_MouseWheel;
            Paint += Form1_Paint;
        }

        bool isPressed = false;
        bool isPressed2 = false;
        Bitmap bmp;
        Graphics gr;
        PointF? lastPoint = null;
        float brushSize = 16;
        Color brushColor = Color.Orange;


        private void Form1_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isPressed = true;
            else if (e.Button == MouseButtons.Right)
                isPressed2 = true;
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var cursor = PointToClient(Cursor.Position);
            Color color = isPressed ? brushColor : Color.Black;
            //gr.FillEllipse(new SolidBrush(color), cur.X - brushSize / 2, cur.Y - brushSize / 2, brushSize, brushSize);
            if (lastPoint != null && (isPressed || isPressed2))
            {
                gr.DrawLine(new Pen(color, brushSize), cursor.X, cursor.Y, lastPoint.Value.X, lastPoint.Value.Y);
                gr.FillEllipse(new SolidBrush(color), cursor.X - brushSize / 2, cursor.Y - brushSize / 2, brushSize, brushSize);
            }

            lastPoint = cursor;


            e.Graphics.DrawImageUnscaled(bmp, 0, 0);
            e.Graphics.DrawEllipse(Pens.White, cursor.X - brushSize / 2, cursor.Y - brushSize / 2, brushSize, brushSize);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.D1:
                    brushColor = Color.Red;
                    break;
                case Keys.D2:
                    brushColor = Color.LimeGreen;
                    break;
                case Keys.D3:
                    brushColor = Color.Blue;
                    break;
                case Keys.D4:
                    brushColor = Color.Violet;
                    break;
                case Keys.D5:
                    brushColor = Color.Orange;
                    break;
                case Keys.D6:
                    brushColor = Color.White;
                    break;
                case Keys.D7:
                    brushColor = Color.Yellow;
                    break;                
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_MouseWheel(object? sender, MouseEventArgs e)
        {
            const int brushSizeStep = 4;
            brushSize += e.Delta > 0 ? brushSizeStep : -brushSizeStep;
        }

        private void Form1_MouseUp(object? sender, MouseEventArgs e)
        {
            isPressed = false;
            isPressed2 = false;
            lastPoint = null;
        }

     
        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }

      
    }
}
