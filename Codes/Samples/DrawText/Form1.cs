using System.Collections.Immutable;

namespace DrawText
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            StartPosition = FormStartPosition.CenterScreen;
            Width = 500;
            Height = 300;
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);

            int x = ClientSize.Width / 2;
            int y = ClientSize.Height / 2;
            Font font = new Font("Courier New", 22);
            string text = "Hello, world!";
            SizeF textSize = gr.MeasureString(text, font);
            gr.DrawString(text, font, Brushes.LimeGreen, x - textSize.Width / 2, y - textSize.Height / 2);
        }
    }
}
