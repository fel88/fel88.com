using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Stars_Horizontal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            InitStars();
            SizeChanged += Form1_SizeChanged;
            timer.Tick += (sender, e) =>
            {                
                    Invalidate();
            };
            timer.Interval = 1;
            timer.Start();
            DoubleBuffered = true;
        }

        private void Form1_SizeChanged(object? sender, EventArgs e)
        {
            InitStars();
        }

        Timer timer = new Timer();

        Random rand = new Random();
        private void InitStars()
        {
            var width = ClientSize.Width;
            var height = ClientSize.Height;
            for (int i = 0; i < N; i++)
            {
                starsX[i] = rand.Next(width);
                starsY[i] = rand.Next(height);
                starsSpeed[i] = rand.Next(10, 100) / 10f;
                starsSize[i] = rand.Next(1, 4);
            }
        }

        const int N = 400;

        float[] starsX = new float[N];
        float[] starsY = new float[N];
        float[] starsSize = new float[N];
        float[] starsSpeed = new float[N];

        
        private void Form1_Paint(object? sender, PaintEventArgs e)
        {            
            var gr = e.Graphics;
            gr.Clear(Color.Black);
            for (int i = 0; i < N; i++)
            {
                gr.FillEllipse(Brushes.Silver, starsX[i], starsY[i], starsSize[i], starsSize[i]);
                starsX[i] -= starsSpeed[i];
                if (starsX[i] < 0) starsX[i] = Width;
            }
        }
    }
}
