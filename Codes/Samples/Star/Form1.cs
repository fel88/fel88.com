using static System.Windows.Forms.AxHost;

namespace Star
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
            DoubleBuffered = true;
            ClientSize = new Size(500, 500);
        }

        public void DrawStar(Graphics gr,
          float centerX,   // X центра звезды 
          float centerY,   // Y центра звезды 
          float outerRadius,  // внешний радус звезды
          float innerRadius,   // внутренний радиус звезды          
          Pen? borderPen = null,
          Brush? fillBrush = null,
          int n = 5
          )
        {
            List<PointF> points = new List<PointF>();

            var step = 360.0f / (n * 2); // шаг градусов
            bool even = false;  // флаг указывающий какой из радиусов использовать на текущем шагу
            for (int i = 0; i <= n * 2; i++)  // пробегаемся циклом по всем иднексам вершин звезды
            {
                float ang = i * step;   // получаем из индекса  угол путем домнажения на шаг step
                var radians = ang * Math.PI / 180.0 - Math.PI / 2;  // переводим в радианы и поворачиваем на -90°

                var radius = (i % 2 == 0) ? outerRadius : innerRadius;  // выбираем радиус исходя из текущего состояния флага even

                // расчитываем координаты точки (x, y)
                var x = centerX + radius * Math.Cos(radians);
                var y = centerY + radius * Math.Sin(radians);

                PointF point = new PointF((float)x, (float)y);
                points.Add(point);
            }

            if (fillBrush != null)
                gr.FillPolygon(fillBrush, points.ToArray());
            if (borderPen != null)
                gr.DrawPolygon(borderPen, points.ToArray());
        }



        public void StarMode()
        {
            DrawFunction = Star;
        }

        public void RowMode()
        {
            DrawFunction = Row;
        }

        public void GridMode()
        {
            DrawFunction = Grid;
        }

        public Action<Graphics> DrawFunction;
        public void Row(Graphics gr)
        {
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);
            int qty = 5;
            float step = 180;
            float startX = 100;
            float y = 100;
            float radius1 = 80;
            float radius2 = 30;
            int n = 5;

            for (int i = 0; i < qty; i++)
            {
                DrawStar(gr, startX + i * step, y, radius1, radius2, Pens.White, Brushes.Orange);
            }
        }

        public void Grid(Graphics gr)
        {
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);
            int rows = 3;
            int cols = 4;

            float y = 100;
            float radius1 = 60;
            float radius2 = 20;
            float gap = 10;
            float startX = radius1 + gap;
            float startY = radius1 + gap;

            float stepX = radius1 * 2 + gap;
            float stepY = radius1 * 2 + gap;
            ClientSize = new Size((int)((radius1 * 2 + gap) * cols), (int)((radius1 * 2 + gap) * rows));

            for (int j = 0; j < rows; j++)
                for (int i = 0; i < cols; i++)
                {
                    var starX = startX + i * stepX;
                    var starY = startY + j * stepY;
                    DrawStar(gr, starX, starY, radius1, radius2, Pens.White, Brushes.LimeGreen);
                }
        }

        public void Star(Graphics gr)
        {
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.Clear(Color.Black);

            PointF? prevPoint = null;
            float radius1 = 160;
            float radius2 = 60;
            var centerX = ClientRectangle.Width / 2;
            var centerY = ClientRectangle.Height / 2;
            int n = 5;

            DrawStar(gr, centerX, centerY, radius1, radius2, Pens.White, null, n);

        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            DrawFunction?.Invoke(gr);
        }
    }
}
