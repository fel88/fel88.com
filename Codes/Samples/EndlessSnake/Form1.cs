namespace EndlessSnake
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Paint += Form1_Paint;
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }

        float snakeAngle = 0;
        int snakeQty = 15;
        float snakeRadius = 20;
        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            var a = 300;
            snakeAngle++;
            List<PointF> points = new List<PointF>();
            for (int i = 0; i <= 360; i++)
            {
                var ang = i * Math.PI / 180.0;
                var x = ClientSize.Width / 2 + a * Math.Cos(ang) / (1 + Math.Pow(Math.Sin(ang), 2));
                var y = ClientSize.Height / 2 + a * Math.Cos(ang) * Math.Sin(ang) / (1 + Math.Pow(Math.Sin(ang), 2));
                points.Add(new PointF((float)x, (float)y));
            }
            e.Graphics.DrawPolygon(Pens.White, points.ToArray());
            for (int i = 0; i < snakeQty; i++)
            {
                var ang = (snakeAngle + i * 8) * Math.PI / 180.0;
                var x = (float)(ClientSize.Width / 2 + a * Math.Cos(ang) / (1 + Math.Pow(Math.Sin(ang), 2)));
                var y = (float)(ClientSize.Height / 2 + a * Math.Cos(ang) * Math.Sin(ang) / (1 + Math.Pow(Math.Sin(ang), 2)));
                e.Graphics.FillEllipse(Brushes.Green, x - snakeRadius, y - snakeRadius, snakeRadius * 2, snakeRadius * 2);
                e.Graphics.DrawEllipse(Pens.Silver, x - snakeRadius, y - snakeRadius, snakeRadius * 2, snakeRadius * 2);
            }

        }
    }
}
