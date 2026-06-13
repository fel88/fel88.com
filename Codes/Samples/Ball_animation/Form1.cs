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
            timer.Interval = 2;

            DoubleBuffered = true;
            Paint += Form1_Paint;
            BallY = ClientSize.Height / 2;
        }

        bool IsMovingLeft = false;
        bool IsMovingUp = false;
        float BallX = 100;
        float BallY = 100;
        float BallRadius = 20;
        float Speed = 2;

        public void MoveLeftAndTeleport(Graphics gr)
        {
            gr.Clear(Color.Black);

            BallX -= Speed;

            if (BallX < 0)
                BallX = Width;          
            
            gr.FillEllipse(Brushes.Blue, BallX - BallRadius, BallY - BallRadius, BallRadius * 2, BallRadius * 2);
            gr.DrawEllipse(Pens.White, BallX - BallRadius, BallY - BallRadius, BallRadius * 2, BallRadius * 2);
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            Move2DirAndBounce(e.Graphics);
            //BounceOnlyHorizontal(e.Graphics);
            //MoveLeftAndTeleport(e.Graphics);
        }

        public void BounceOnlyHorizontal(Graphics gr)
        {
            gr.Clear(Color.Black);

            BallX += IsMovingLeft ? -Speed : Speed;            

            if (BallX >= (ClientRectangle.Width) || BallX <= 0)
                IsMovingLeft = !IsMovingLeft;
            
            gr.FillEllipse(Brushes.Blue, BallX - BallRadius, BallY - BallRadius, BallRadius * 2, BallRadius * 2);
            gr.DrawEllipse(Pens.White, BallX - BallRadius, BallY - BallRadius, BallRadius * 2, BallRadius * 2);
        }

        public void Move2DirAndBounce(Graphics gr)
        {
            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            gr.Clear(Color.Black);            

            BallX += IsMovingLeft ? -Speed : Speed;
            BallY += IsMovingUp ? -Speed : Speed;

            if (BallX >= (ClientRectangle.Width - BallRadius) || BallX <= BallRadius)
                IsMovingLeft = !IsMovingLeft;

            if (BallY >= (ClientRectangle.Height - BallRadius) || BallY <= BallRadius)
                IsMovingUp = !IsMovingUp;

            gr.FillEllipse(Brushes.Blue, BallX - BallRadius, BallY - BallRadius, BallRadius * 2, BallRadius * 2);
            gr.DrawEllipse(Pens.White, BallX - BallRadius, BallY - BallRadius, BallRadius * 2, BallRadius * 2);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }


    }
}
