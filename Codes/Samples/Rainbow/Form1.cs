namespace Rainbow
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var h = ClientSize.Height / 7;
            e.Graphics.FillRectangle(Brushes.Red, 0, 0, ClientSize.Width, h);
            e.Graphics.FillRectangle(Brushes.Orange, 0, h, ClientSize.Width, h);
            e.Graphics.FillRectangle(Brushes.Yellow, 0, 2 * h, ClientSize.Width, h);
            e.Graphics.FillRectangle(Brushes.Green, 0, 3 * h, ClientSize.Width, h);
            e.Graphics.FillRectangle(Brushes.LightBlue, 0, 4 * h, ClientSize.Width, h);
            e.Graphics.FillRectangle(Brushes.Blue, 0, 5 * h, ClientSize.Width, h);
            e.Graphics.FillRectangle(Brushes.Violet, 0, 6 * h, ClientSize.Width, h);
        }
    }
}
