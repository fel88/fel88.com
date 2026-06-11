namespace Gradient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DrawHorizontalGradient();
        }

        public void DrawHorizontalGradient()
        {
            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            for (int i = 0; i < ClientSize.Width; i++)
            {
                double koef = (i) / (double)ClientSize.Width;
                Color color = Color.FromArgb((int)(koef * 255), 0, 0);
                for (int j = 0; j < ClientSize.Height; j++)
                {
                    bmp.SetPixel(i, j, color);
                }
            }
            pictureBox1.Image = bmp;
        }

        public void DrawVerticalGradient()
        {
            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            for (int i = 0; i < ClientSize.Height; i++)
            {
                double koef = (i) / (double)ClientSize.Height;
                Color color = Color.FromArgb(0, (int)(koef * 255), 0);
                for (int j = 0; j < ClientSize.Width; j++)
                {
                    bmp.SetPixel(j, i, color);
                }
            }
            pictureBox1.Image = bmp;
        }

        public void DrawDiagGradient()
        {
            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            for (int i = 0; i < ClientSize.Height; i++)
            {
                for (int j = 0; j < ClientSize.Width; j++)
                {
                    double koef = (i + j) / (double)(ClientSize.Height + ClientSize.Width);
                    Color color = Color.FromArgb(0, (int)(koef * 255), 0);
                    bmp.SetPixel(j, i, color);
                }
            }
            pictureBox1.Image = bmp;
        }
    }
}
