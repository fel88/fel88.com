namespace Chessboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            ClientSize = new Size(CellSize * 8, CellSize * 8);
        }

        int CellSize = 50;

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            bool isBlack = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    isBlack = !isBlack;
                    e.Graphics.FillRectangle(
                        isBlack ? Brushes.Black : Brushes.White, // текущий цвет клетки
                        i * CellSize,  // начальная позиция по X
                    j * CellSize, // начальная позиция по Y
                        CellSize, CellSize
                    ); // ширина клетки по X и Y 
                }
                isBlack = !isBlack;
            }
        }
    }
}
