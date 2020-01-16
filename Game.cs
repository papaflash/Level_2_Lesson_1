using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
namespace MyGameAsteroids
{
    static class Game
    {
        static SpaceShip sp;
        static List<Pen> lsPens = new List<Pen>() {Pens.White, Pens.Red, Pens.Aquamarine, Pens.Blue, Pens.Coral };
        static Random r = new Random();
        private static BaseObject[] _objs;
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static int Width { set; get; }
        public static int Height { set; get; }
        static Game() { }

        public static void Init(Form form)
        {
            
            Graphics g;
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
            Timer timer = new Timer { Interval = 50};
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            
            Buffer.Graphics.FillEllipse(Brushes.Aquamarine, new Rectangle(50, 50, 200, 200));
           

            //Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            sp.Draw();
            Buffer.Render();
        }
        public static void Load()
        {
            
            _objs = new BaseObject[80];
            for (int i = 0; i < _objs.Length / 8; i++)
            {
                _objs[i] = new BaseObject(new Point(r.Next(100, 600), (i + 1) * 2), new Point(-i, i), new Size(20, 20), lsPens[r.Next(0,5)]);
            }
            for(int i = _objs.Length / 8; i < _objs.Length; i++)
            {
                _objs[i] = new Star(new Point(r.Next(1, 1280), r.Next(1, 740)), new Point(-i, 0), new Size(2, 2), lsPens[r.Next(0, 4)]);
            }
            sp = new SpaceShip(new Point(200, 350), new Point(0, 2), new Size(160, 128));
        }
        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
            sp.Update();
        }
    }
}
