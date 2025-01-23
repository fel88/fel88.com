using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Chessboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            Timer timer = new Timer();
            timer.Start();
            timer.Tick += Timer_Tick;
            timer.Interval = 20;
            DoubleBuffered = true;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }        

        float ButtonLeft = 140;
        float ButtonTop = 80;
        float ButtonWidth = 120;
        float ButtonHeight = 120;

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            var gr = e.Graphics;
            var point = PointToClient(Cursor.Position);

            bool isInsideRectangle = point.X >= ButtonLeft && point.X <= (ButtonLeft + ButtonWidth) && point.Y >= ButtonTop && point.Y <= (ButtonTop + ButtonHeight);
            gr.FillRectangle(isInsideRectangle ? Brushes.Blue : Brushes.White, ButtonLeft, ButtonTop, ButtonWidth, ButtonHeight);
            gr.DrawRectangle(Pens.Black, ButtonLeft, ButtonTop, ButtonWidth, ButtonHeight);
        }
    }
}
