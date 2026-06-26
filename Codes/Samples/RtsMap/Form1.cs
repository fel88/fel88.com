using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RtsMap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            var tileset = Bitmap.FromFile("tileset.png") as Bitmap;

            LoadTileSet(tileset, TileSize);

            ClientSize = new Size(TileSize * InitMapCols, TileSize * InitMapRows);
            Paint += Form1_Paint;
            GenerateMap();
        }

        const int InitMapRows = 20;
        const int InitMapCols = 20;

        int[,] map = new int[InitMapCols, InitMapRows];

        Random rand = new Random();
        const int TileSize = 32;
        List<Bitmap> tiles = new List<Bitmap>();
        int[][] rights;
        int[][] downs;
        bool LegengVisible = true;
        bool AnimateSearchEnabled = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.A)
            {
                AnimateSearchEnabled = !AnimateSearchEnabled;
                Invalidate();
            }

            else if (keyData == Keys.L)
                LoadMap();

            else if (keyData == Keys.V)
            {
                LegengVisible = !LegengVisible;
                Invalidate();
            }
            else if (keyData == Keys.R)
                RegenerateMap();

            else
            if (keyData == Keys.B)
                SaveMapToBitmap();
            else
            if (keyData == Keys.S)
                SaveMapToXml();

            else
            if (keyData == Keys.O)
                LoadRules();
            else
            if (keyData == Keys.G)
            {
                GridVisible = !GridVisible;
                Invalidate();
            }

            else
            if (keyData == Keys.C)
            {
                var d = AutoDialog.DialogHelpers.StartDialog();
                d.AddInt("w", "Width", map.GetLength(0), 2, 96);
                d.AddInt("h", "Height", map.GetLength(1), 2, 96);
                if (d.ShowDialog())
                {
                    map = new int[d.GetInt("w"), d.GetInt("h")];
                    ClientSize = new Size(TileSize * map.GetLength(0), TileSize * map.GetLength(1));

                    RegenerateMap();
                }

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadMap()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xml files (*.xml)|*.xml";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var doc = XDocument.Load(ofd.FileName);
            var mapEl = doc.Root.Element("map");
            var rows = int.Parse(mapEl.Attribute("rows").Value);
            var cols = int.Parse(mapEl.Attribute("cols").Value);
            map = new int[cols, rows];
            foreach (var item in mapEl.Elements("cell"))
            {
                var x = int.Parse(item.Attribute("x").Value);
                var y = int.Parse(item.Attribute("y").Value);
                var tileIdx = int.Parse(item.Attribute("tileIdx").Value);
                map[x, y] = tileIdx;
            }
            ClientSize = new Size(TileSize * map.GetLength(0), TileSize * map.GetLength(1));
            Invalidate();
        }

        bool GridVisible = false;

        private void RegenerateMap()
        {
            if (rights != null)
                GenerateMapWithRules();
            else
                GenerateMap();
            Invalidate();
        }

        private void LoadRules()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xml files (*.xml)|*.xml";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var doc = XDocument.Load(ofd.FileName);
            ExtractRules(doc);
            GenerateMapWithRules();
            Invalidate();

        }

        private void SaveMapToXml()
        {
            XElement root = new XElement("root");
            XElement mapNode = new XElement("map");
            root.Add(mapNode);
            mapNode.Add(new XAttribute("rows", map.GetLength(1)));
            mapNode.Add(new XAttribute("cols", map.GetLength(0)));
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    XElement el = new XElement("cell");
                    el.Add(new XAttribute("x", i));
                    el.Add(new XAttribute("y", j));
                    el.Add(new XAttribute("tileIdx", map[i, j]));
                    mapNode.Add(el);
                }
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xml files (*.xml)|*.xml";
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            root.Save(sfd.FileName);
        }

        private void SaveMapToBitmap()
        {
            using Bitmap output = new Bitmap(map.GetLength(0) * TileSize, map.GetLength(1) * TileSize);
            using Graphics graphics = Graphics.FromImage(output);
            DrawMap(graphics);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "png images (*.png)|*.png|jpeg images (*.jpg)|*.jpg";
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            output.Save(sfd.FileName);
        }

        private List<int> Shuffle(List<int> list)
        {
            return list.OrderBy(z => rand.Next(list.Count)).ToList();
        }

        private void GenerateMapWithRules()
        {
            var shuffle = tiles.OrderBy(z => rand.Next(1000)).ToArray();
            bool found = false;


            for (int k = 0; k < tiles.Count; k++)
            {
                Fill(map, 0);
                map[0, 0] = tiles.IndexOf(shuffle[k]);
                Stack<(int, int, int[,])> stack = new Stack<(int, int, int[,])>();
                stack.Push((0, 1, map));
                int iteration = 0;
                const int IterationsLimit = 100_000;
                while (stack.Any())
                {
                    iteration++;
                    if (iteration > IterationsLimit)
                        break;
                    var s = stack.Pop();
                    var i = s.Item1;
                    var j = s.Item2;
                    map = s.Item3;

                    if (AnimateSearchEnabled)
                        Invoke(() => { Application.DoEvents(); Invalidate(); });

                    List<int> allowed = new List<int>();
                    if (i > 0)
                    {
                        var left = map[i - 1, j];
                        if (left >= 0)
                            allowed.AddRange(rights[left]);
                    }
                    if (j > 0)
                    {
                        var up = map[i, j - 1];
                        if (up >= 0)
                        {
                            if (allowed.Count == 0)
                                allowed.AddRange(downs[up]);
                            else
                                allowed = allowed.Intersect(downs[up]).ToList();
                        }
                    }


                    if (i == map.GetLength(0) - 1 && j == map.GetLength(1) - 1 && allowed.Any())//finish
                    {
                        map[i, j] = allowed[0];
                        //solution was found
                        found = true;
                        break;
                    }
                    allowed = Shuffle(allowed);
                    foreach (var item in allowed)
                    {

                        map[i, j] = item;

                        if (j == map.GetLength(1) - 1)
                            stack.Push((i + 1, 0, (int[,])map.Clone()));
                        else
                            stack.Push((i, j + 1, (int[,])map.Clone()));

                    }
                    found = false;
                    continue;
                }

                if (found)
                    break;
            }
            Invalidate();
        }

        private Point GetFirstEmptyCell(int[,] map)
        {
            Point minCell = new Point(map.GetLength(0) - 1, map.GetLength(1) - 1);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != 0)
                        continue;
                    if (i > 0 && map[i - 1, j] == 0)
                        continue;

                    if (j > 0 && map[i, j - 1] == 0)
                        continue;

                    var score = minCell.X + minCell.Y;
                    if ((i + j) < score)
                    {
                        minCell = new Point(i, j);
                    }
                }
            }
            return minCell;
        }

        private void Fill(int[,] map, int v)
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    map[i, j] = v;

        }

        private void ExtractRules(XDocument doc)
        {
            rights = new int[tiles.Count][];
            downs = new int[tiles.Count][];
            for (int i = 0; i < rights.Length; i++)
            {
                rights[i] = [];
                downs[i] = [];
            }
            foreach (var item in doc.Root.Elements("rule"))
            {
                var tileIdx = int.Parse(item.Attribute("tileIdx").Value);
                var rightsParsed = item.Element("right").Value.Split([';'], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                var downsParsed = item.Element("down").Value.Split([';'], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                rights[tileIdx] = rightsParsed;
                downs[tileIdx] = downsParsed;
            }
        }

        private void GenerateMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = rand.Next(16, tiles.Count - 8);
                }
            }
        }

        private void LoadTileSet(Bitmap? tileset, int tileSize)
        {
            var rows = tileset.Height / tileSize;
            var cols = tileset.Width / tileSize;


            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < cols; i++)
                {
                    Bitmap b = new Bitmap(TileSize, tileSize);
                    using var gr2 = Graphics.FromImage(b);
                    gr2.DrawImage(tileset, new Rectangle(0, 0, TileSize, TileSize), new Rectangle(i * tileSize + i, j + j * tileSize, tileSize, tileSize), GraphicsUnit.Pixel);
                    tiles.Add(b);                    
                }
            }

            using var gr = this.CreateGraphics();
            foreach (var item in tiles)
            {
                item.SetResolution(gr.DpiX, gr.DpiY);
            }
        }


        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            DrawMap(gr);
            if (GridVisible)
                DrawGrid(gr);

            if (!LegengVisible)
                return;

            var font = new Font("Consolas", 10);
            var brush = Brushes.Yellow;
            gr.FillRectangle(new SolidBrush(Color.FromArgb(164, Color.Silver)), 0, 0, 200, 110);
            gr.DrawString("R - re-generate map", font, brush, 5, 5);
            gr.DrawString("O - load rules.xml file", font, brush, 5, 15);
            gr.DrawString("S - save map to xml", font, brush, 5, 25);
            gr.DrawString("B - save map to bmp", font, brush, 5, 35);
            gr.DrawString("C - change map size", font, brush, 5, 45);
            gr.DrawString("G - grid visible", font, brush, 5, 55);
            gr.DrawString("L - load map", font, brush, 5, 65);
            gr.DrawString("V - legend visible", font, brush, 5, 75);
            gr.DrawString($"A - animate search ({(AnimateSearchEnabled ? "on" : "off")})", font, brush, 5, 85);
        }

        private void DrawGrid(Graphics gr)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    gr.DrawRectangle(Pens.Gray, i * TileSize, j * TileSize, TileSize, TileSize);
                }
            }
        }

        void DrawMap(Graphics gr)
        {
            var temp = gr.CompositingMode;
            gr.CompositingMode = CompositingMode.SourceCopy;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    gr.DrawImageUnscaled(tiles[map[i, j]], i * TileSize, j * TileSize);
                }
            }
            gr.CompositingMode = temp;
        }

    }
}
