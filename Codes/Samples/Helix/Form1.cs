namespace Helix
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Paint += Form1_Paint;            
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.White);
            const int turnsQty = 5;
            const int degreeIncrement = 3;
            var centerX = ClientRectangle.Width / 2;
            var centerY = ClientRectangle.Height / 2;
            double radiusStep = 50; // radius inrement for each 360° turn
            PointF? prevPoint = null;
            for (int ang = 0; ang < turnsQty * 360; ang += degreeIncrement)
            {
                var angRadians = ang * Math.PI / 180.0;
                var radius = (ang / 360.0) * radiusStep;
                var x = centerX + radius * Math.Cos(angRadians);
                var y = centerY + radius * Math.Sin(angRadians);
                PointF point1 = new PointF((float)x, (float)y);
                if (prevPoint != null)
                    gr.DrawLine(Pens.Black, prevPoint.Value, point1);
                
                prevPoint = point1;
            }
        }
    }
}
