using Timer = System.Windows.Forms.Timer;

namespace Chessboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            Timer timer = new Timer();
            timer.Start();
            timer.Tick += Timer_Tick;
            timer.Interval = 10;
            DoubleBuffered = true;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }

        double angle = 0;
        float radius = 100;
        float circleWidth = 10;

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var gr = e.Graphics;
            var point = PointToClient(Cursor.Position);

            var centerX = ClientSize.Width / 2;
            var centerY = ClientSize.Height / 2;

            angle += 0.5;
            angle %= 360;

            var radians = angle / 180.0 * Math.PI;

            var circleX = (float)(centerX + radius * Math.Cos(radians));
            var circleY = (float)(centerY + radius * Math.Sin(radians));

            gr.DrawEllipse(Pens.Black, centerX - radius, centerY - radius, radius * 2, radius * 2);
            gr.FillEllipse(Brushes.Blue, circleX - circleWidth, circleY - circleWidth, circleWidth * 2, circleWidth * 2);
        }
    }
}
