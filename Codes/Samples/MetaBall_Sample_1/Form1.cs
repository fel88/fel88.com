using System;
using System.Drawing.Design;

namespace MetaBall_Sample_1
{
    public partial class Form1 : Form
    {
        public struct MetaBall
        {
            public double X;
            public double Y;
            public double R;
        }

        // Define the metaballs: [x, y, radius]
        MetaBall[] metaballs = new MetaBall[] {
            new MetaBall (){X=150, Y=150, R=40},
            new MetaBall (){X=250,Y=200, R=60}
        };

        const double threshold = 1.0;
        PictureBox pb = new PictureBox();
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            pb.Dock = DockStyle.Fill;
            Controls.Add(pb);
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            draw();
        }

        public void draw()
        {
            Bitmap bmp = new Bitmap((int)ClientRectangle.Width, (int)ClientRectangle.Height);
            for (int  x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    double sum = 0;

                    for (int i = 0; i < metaballs.Length; i++)
                    {
                        var mb = metaballs[i];
                        // Distance from pixel to the metaball center
                        var dx = x - mb.X;
                        var dy = y - mb.Y;
                        var d = Math.Sqrt(dx * dx + dy * dy);

                        // Prevent division by zero
                        if (d < 1) 
                            d = 1;

                        // Logarithmic influence function
                        // Using log allows for a smooth, organic fall-off
                        sum += -Math.Log(d / mb.R);
                    }

                    if (sum > threshold)
                    {
                        bmp.SetPixel(x, y, Color.FromArgb(0, 128, 255));
                    }
                        
                    //// Index in the image data array (RGBA)
                    //var index = (x + y * Width) * 4;

                    //// If the summed influence passes the threshold, color the pixel
                    //if (sum > threshold)
                    //{
                    //    data[index] = 0;     // R
                    //    data[index + 1] = 128; // G
                    //    data[index + 2] = 255; // B
                    //    data[index + 3] = 255; // A
                    //}
                    //else
                    //{
                    //    data[index + 3] = 0;   // Transparent outside the shape
                    //}
                }
            }
            pb.Image = bmp;            
        }
    }
}
