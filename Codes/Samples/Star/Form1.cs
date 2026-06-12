namespace Star
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            DoubleBuffered = true;
            ClientSize = new Size(500, 500);
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);
            PointF? prevPoint = null;
            double radius1 = 160;
            double radius2 = 60;
            var centerX = ClientRectangle.Width / 2;
            var centerY = ClientRectangle.Height / 2;
            int n = 5;
            var step = 360.0f / (n * 2);
            bool even = false;
            for (int i = 0; i <= n * 2; i++)
            {
                float ang = i * step;
                var radians = ang * Math.PI / 180.0 - Math.PI / 2;
                even = !even;
                var radius = even ? radius1 : radius2;
                var x = centerX + radius * Math.Cos(radians);
                var y = centerY + radius * Math.Sin(radians);
                PointF point = new PointF((float)x, (float)y);

                if (prevPoint != null)
                    gr.DrawLine(Pens.White, prevPoint.Value, point);

                prevPoint = point;
            }
        }
    }
}
