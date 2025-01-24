using Timer = System.Windows.Forms.Timer;

namespace Chessboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            Timer timer = new Timer();
            timer.Start();
            timer.Tick += Timer_Tick;
            timer.Interval = 10;
            DoubleBuffered = true;
            Paint += Form1_Paint;
        }

        bool IsMovingLeft = false;
        bool IsMovingUp = false;
        float BallX = 100;
        float BallY = 100;
        float BallRadius = 30;
        float Speed = 2;

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {            
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            e.Graphics.Clear(Color.White);

            var gr = e.Graphics;            
            
            BallX += IsMovingLeft ? -Speed : Speed;
            BallY += IsMovingUp ? -Speed : Speed;

            if (BallX >= (ClientRectangle.Width - BallRadius) || BallX <= BallRadius)
                IsMovingLeft = !IsMovingLeft;

            if (BallY >= (ClientRectangle.Height - BallRadius) || BallY <= BallRadius)
                IsMovingUp = !IsMovingUp;

            gr.FillEllipse(Brushes.Blue, BallX - BallRadius, BallY - BallRadius, BallRadius * 2, BallRadius * 2);
            gr.DrawEllipse(Pens.Black, BallX - BallRadius, BallY - BallRadius, BallRadius * 2, BallRadius * 2);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }

     
    }
}
