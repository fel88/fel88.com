namespace Stars
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
            e.Graphics.Clear(Color.Black);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
