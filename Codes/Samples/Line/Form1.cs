namespace Line
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            SizeChanged += Form1_SizeChanged;
        }

        private void Form1_SizeChanged(object? sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            DrawCross(e.Graphics);
            //DrawLines(e.Graphics);
        }
        public void DrawCross(Graphics gr)
        {
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);
            gr.DrawLine(Pens.Green, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
            gr.DrawLine(Pens.Green, 0, ClientRectangle.Height, ClientRectangle.Width, 0);
        }

        public void DrawLines(Graphics gr)
        {

            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);

            gr.DrawString("Thickness 1", SystemFonts.MessageBoxFont, Brushes.White, 20, 5);
            gr.DrawLine(new Pen(Color.Red, 1), 20, 25, ClientRectangle.Width - 20, 25);

            gr.DrawString("Thickness 10", SystemFonts.MessageBoxFont, Brushes.White, 20, 35);
            gr.DrawLine(new Pen(Color.Green, 10), 20, 55, ClientRectangle.Width - 20, 55);

            gr.DrawString("Thickness 25", SystemFonts.MessageBoxFont, Brushes.White, 20, 70);
            gr.DrawLine(new Pen(Color.Yellow, 25), 20, 100, ClientRectangle.Width - 20, 100);

            gr.DrawString("Thickness 50", SystemFonts.MessageBoxFont, Brushes.White, 20, 130);
            gr.DrawLine(new Pen(Color.MediumVioletRed, 50), 20, 180, ClientRectangle.Width - 20, 180);

        }
    }
}
