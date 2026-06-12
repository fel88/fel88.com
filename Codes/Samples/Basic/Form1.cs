namespace Basic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            ClientSize = new Size(400, 400);
            
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.Black);
            e.Graphics.DrawRectangle(Pens.White, 100, 100, 200, 200);
            e.Graphics.DrawEllipse(Pens.White, 100, 100, 200, 200);
        }
    }
}
