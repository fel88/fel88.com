namespace Chessboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;            
        }

        float Radius = 50;
        float Step = 10;
        float Qty = 15;

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var gr = e.Graphics;            

            var centerX = ClientSize.Width / 2;
            var centerY = ClientSize.Height / 2;

            var radius = Radius;

            for (int i = 0; i < Qty; i++)
            {
                gr.DrawEllipse(Pens.Black, centerX - radius, centerY - radius, radius * 2, radius * 2);
                radius += Step;
            }
        }
    }
}
