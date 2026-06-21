namespace Gradient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Paint += Form1_Paint;
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            Init();
        }

        public void Init()
        {
            DrawHorizontalGradient(Color.DeepSkyBlue);
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(bmp, 0, 0);
        }

        Bitmap bmp;
        public void DrawHorizontalGradient(Color color)
        {
            for (int i = 0; i < ClientSize.Width; i++)
            {
                double koef = (i) / (double)ClientSize.Width;

                Color _color = Color.FromArgb((int)(koef * color.R), (int)(koef * color.G), (int)(koef * color.B));
                for (int j = 0; j < ClientSize.Height; j++)
                {
                    bmp.SetPixel(i, j, _color);
                }
            }
        }

        public void Fill(Color color)
        {
            for (int i = 0; i < ClientSize.Width; i++)
            {
                for (int j = 0; j < ClientSize.Height; j++)
                {
                    bmp.SetPixel(i, j, color);
                }
            }
        }

        public void DrawVerticalGradient(Color color1)
        {
            for (int i = 0; i < ClientSize.Height; i++)
            {
                double koef = (i) / (double)ClientSize.Height;
                Color color = Color.FromArgb((int)(koef * color1.R), (int)(koef * color1.G), (int)(koef * color1.B));
                for (int j = 0; j < ClientSize.Width; j++)
                {
                    bmp.SetPixel(j, i, color);
                }
            }
        }

        public void DrawDiagGradient()
        {
            for (int i = 0; i < ClientSize.Height; i++)
            {
                for (int j = 0; j < ClientSize.Width; j++)
                {
                    double koef = (i + j) / (double)(ClientSize.Height + ClientSize.Width);
                    Color color = Color.FromArgb(0, (int)(koef * 255), 0);
                    bmp.SetPixel(j, i, color);
                }
            }
        }


        public void DrawRadialGradient(Color color)
        {
            for (int i = 0; i < ClientSize.Height; i++)
            {
                for (int j = 0; j < ClientSize.Width; j++)
                {
                    var xc = i - ClientRectangle.Height / 2;
                    var yc = j - ClientRectangle.Width / 2;

                    var dist = Math.Sqrt(xc * xc + yc * yc);

                    var farPoint = Math.Sqrt(Math.Pow(ClientRectangle.Width / 2, 2) + Math.Pow(ClientRectangle.Height / 2, 2));
                    var val = dist / farPoint;

                    val = 1.0 - val;
                    
                    double r = color.R * val;
                    double g = color.G * val;
                    double b = color.B * val;

                    Color _color = Color.FromArgb((int)r, (int)g, (int)b);

                    bmp.SetPixel(j, i, _color);
                }
            }
        }


    }
}
