namespace Ngon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            ClientSize = new Size(500, 500);
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);
            PointF? prevPoint = null;
            double radius = 160;

            var centerX = ClientRectangle.Width / 2;
            var centerY = ClientRectangle.Height / 2;
            int n = 6;
            var step = 360 / n;
            const int cornerSize = 3;
            for (int i = 0; i <= 360; i += step)
            {
                var radians = i * Math.PI / 180.0;
                var x = centerX + radius * Math.Cos(radians);
                var y = centerY + radius * Math.Sin(radians);
                PointF point = new PointF((float)x, (float)y);

                gr.DrawRectangle(Pens.Yellow, (float)x - cornerSize, (float)y - cornerSize, 2 * cornerSize, 2 * cornerSize);
                if (prevPoint != null)
                    gr.DrawLine(Pens.White, prevPoint.Value, point);

                prevPoint = point;
            }
        }
    }
}
