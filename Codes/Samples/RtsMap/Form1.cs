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

            ClientSize = new Size(TileSize * MapCols, TileSize * MapRows);
            Paint += Form1_Paint;
            GenerateMap();
        }

        const int MapRows = 20;
        const int MapCols = 20;

        int[,] map = new int[MapCols, MapRows];

        Random rand = new Random();
        const int TileSize = 32;
        List<Bitmap> tiles = new List<Bitmap>();
        int[][] rights;
        int[][] downs;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.R)
            {
                if (rights != null)
                    GenerateMapWithRules();
                else
                    GenerateMap();
                Invalidate();
            }
            if (keyData == Keys.O)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "xml files (*.xml)|*.xml";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var doc = XDocument.Load(ofd.FileName);
                    ExtractRules(doc);
                    GenerateMapWithRules();
                    Invalidate();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
                map[0, 0] = tiles.IndexOf(shuffle[k]);
                Stack<(int, int, int[,])> stack = new Stack<(int, int, int[,])>();
                stack.Push((0, 1, map));
                int iteration = 0;
                const int IterationsLimit = 1_000_000;
                while (stack.Any())
                {
                    iteration++;
                    if (iteration > IterationsLimit)
                        break;
                    var s = stack.Pop();
                    var i = s.Item1;
                    var j = s.Item2;
                    map = s.Item3;

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
                    tiles.Add(tileset.Clone(new Rectangle(i * tileSize + i, j + j * tileSize, tileSize, tileSize), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
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
            DrawMap(e.Graphics);
        }

        void DrawMap(Graphics gr)
        {
            gr.CompositingMode = CompositingMode.SourceCopy;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    gr.DrawImageUnscaled(tiles[map[i, j]], i * TileSize, j * TileSize);
                }
            }
        }

    }
}
