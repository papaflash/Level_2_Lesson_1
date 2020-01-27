using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MyGameAsteroids
{
    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }
    static class Game
    {
        static string _applicationRoot = $"{AppDomain.CurrentDomain.BaseDirectory}log.txt"; //получаем папку в кот. лежит приложение, рядом будет записан лог
        static int _score = 0;  //очки за сбитые астероиды
        static ListBox _log;
        static readonly Size _maxSize = new Size(1280, 720);
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        private static SpaceShip _spaceShip;
        private static MedicineChest[] _chests;
        static List<Pen> lsPens = new List<Pen>() {Pens.White, Pens.Red, Pens.Aquamarine, Pens.Blue, Pens.Coral };
        static Random r = new Random();
        private static BaseObject[] _objs;
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        private static int _width;
        private static Timer _timer = new Timer() { Interval = 50};
        /// <summary>
        ///Уровень_2.Урок_2.Задание_4: реализация проверки на допустимые размеры экрана, выбросить исключение ArgumentOutOfRangeException().
        /// </summary>
        public static int Width 
        {
            set
            {
                if (value < _maxSize.Width && value > 0) _width = value;
                else throw new ArgumentOutOfRangeException("Width");
            }
            get { return _width; }
        }
        private static int _height;
        public static int Height
        {
            set
            {
                if (value < _maxSize.Height && value > 0) _height = value;
                else throw new ArgumentOutOfRangeException("Height");
            }
            get { return _height; }
        }
        static Game() { }

        public static void Init(Form form, ListBox logBox)
        {
            _log = logBox;
            _log.ScrollAlwaysVisible = true;
            _log.Enabled = false;
            Graphics g;
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height - 80;
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            form.KeyDown += Form_KeyDown;
            Load();
            _timer.Start();
            _timer.Tick += Timer_Tick;
            SpaceShip.Message += PrintLog;
            Bullet.MessageJournal += PrintLog;
            SpaceShip.MessageDie += Finish;
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullet = new Bullet(new
                 Point(_spaceShip.Rect.X + 10, _spaceShip.Rect.Y + 4), new Point(4, 0), new Size(4, 1), Pens.Orange);
            if (e.KeyCode == Keys.Up) _spaceShip.Up();
            if (e.KeyCode == Keys.Down) _spaceShip.Down();
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
            foreach (BaseObject aster in _asteroids)
                aster?.Draw();
            //Рисуем аптечки
            foreach (MedicineChest chest in _chests)
                chest?.Draw();

            _bullet?.Draw();
            _spaceShip?.Draw();
            if (_spaceShip != null)
                Buffer.Graphics.DrawString("Energy" + _spaceShip.Eneregy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            //Рисуем очки за сбитые астероиды
            Buffer.Graphics.DrawString("Очки" + _score, SystemFonts.DefaultFont, Brushes.Gold, 100, 0);
            Buffer.Render();
        }
        public static void Load()
        {
            _objs = new BaseObject[80];
            _asteroids = new Asteroid[5];
            _chests = new MedicineChest[3];
            //оборочиваем в try catch для обработки своего исключения
            try
            {
                for (int i = 0; i < _asteroids.Length; i++)
                {
                    _asteroids[i] = new Asteroid(new Point(r.Next(200, 600), 2), new Point(5 + i, 5 + i), new Size(20, 20), lsPens[r.Next(0, 5)]);
                }
            }catch(MyExceptions.GameObjectException err)
            {
                Debug.WriteLine(err.Message);
            }
            for (int i = 0; i < _objs.Length; i++)
            {
                _objs[i] = new Star(new Point(r.Next(1, 1280), r.Next(1, 740)), new Point(-i - 1, 0), new Size(2, 2), lsPens[r.Next(0, 4)]);
            }
            // Заполеяем массив аптечками
            for(int i = 0; i < _chests.Length; i++)
            {
                _chests[i] = new MedicineChest(new Point(r.Next(200, 1000), 300), new Point(5 + i, 5 + i), new Size(20, 20), lsPens[r.Next(0, 5)]);
            }
            _spaceShip = new SpaceShip(new Point(200, 350), new Point(0, 10), new Size(80, 60), null);
            
        }
        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
            _bullet?.Update();
            for (int i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                if (_bullet != null && _bullet.Collision(_asteroids[i])) {
                    _score += 100; // Уровень_2.Урок_3.Задание 4: очки за сбитые астероиды
                    System.Media.SystemSounds.Hand.Play();
                    _asteroids[i] = null;
                    _bullet = null;
                    continue;
                    //aster.Resurrection(Width); //метод восстановления уничтоженного астероида
                    //_bullet.Resurrection(_spaceShip);  //метод нового выстрела после попадения в астероид
                }
                if (!_spaceShip.Collision(_asteroids[i])) continue;
                _spaceShip?.EnergyLow(20);
                System.Media.SystemSounds.Asterisk.Play();
                if (_spaceShip.Eneregy <= 0) _spaceShip?.Die();
            }
            //Аптечки
            foreach (MedicineChest chest in _chests)
            {
                chest.Update();
                if (_spaceShip.Collision(chest))
                   _spaceShip?.EnergyUp(10);
                System.Media.SystemSounds.Exclamation.Play();
            }
                

            _spaceShip.Update(); 
        }
        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 500, 100);
            Buffer.Render();

            #region Задание2 добавить ведение журнала в файл
            using (StreamWriter sw = new StreamWriter(_applicationRoot, false, System.Text.Encoding.UTF8))
            {
               for(int i = 0; i < _log.Items.Count - 1; i++)
                {
                    sw.WriteLine(_log.Items[i]);
                }
            }
            #endregion
        }
        #region Задание2  Метод ведения журнала для передачи делегату
        public static void PrintLog(string s)
        {
            _log.Items.Add($"{DateTime.Now.ToString()} {s}");
            _log.SelectedIndex = _log.Items.Count - 1;
            _log.SelectedIndex = -1; 
        }
        #endregion
    }
}
