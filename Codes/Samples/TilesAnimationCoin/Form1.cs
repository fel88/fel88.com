namespace TilesAnimationCoin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(300, 300);
            MaximizeBox = false;
            MinimizeBox = false;

            InitTileSet();            

            Paint += Form1_Paint;
        }

        public void InitTileSet()
        {
            var tileset = Bitmap.FromFile("Coins.png") as Bitmap;

            for (int i = 0; i < 8; i++)
            {
                frames.Add(tileset.Clone(new Rectangle(i * 16, 0, 16, 16), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
        }

        List<Bitmap> frames = new List<Bitmap>();
        int frameIdx = 0;        
        int speedMs = 120;

        DateTime last = DateTime.Now;
        double animMsAccum = 0;
        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var now = DateTime.Now;
            var diff = now.Subtract(last);
            last = now;
            animMsAccum += diff.TotalMilliseconds;
            int scale = 3;
            var frame = frames[frameIdx];

            e.Graphics.Clear(Color.Black);
            e.Graphics.DrawImage(frame, new RectangleF(ClientRectangle.Width / 2 - scale * frame.Width / 2, ClientRectangle.Height / 2 - scale * frame.Height / 2, frame.Width * scale, frame.Height * scale),
                new RectangleF(0, 0, frame.Width, frame.Height), GraphicsUnit.Pixel);
            
            if (animMsAccum > speedMs)
            {
                frameIdx++;
                animMsAccum %= speedMs;
            }
            frameIdx %= frames.Count;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
