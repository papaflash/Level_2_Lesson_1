using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//Добавил свой класс корабль

namespace MyGameAsteroids
{
    class SpaceShip
    {
        Point _pos;
        Point _dir;
        Size _size;
        Image _image = Image.FromFile(@"D:\C#\MyGameAsteroids\Properties\cb.png");
        public SpaceShip(Point pos, Point dir, Size size)
        {
            _pos = pos;
            _dir = dir;
            _size = size;
        }
        public void Draw()
        {
            Game.Buffer.Graphics.DrawImage(_image, _pos.X, _pos.Y, _size.Width, _size.Height);
        }
        public void Update()
        {
            _pos.Y += -_dir.Y;
            if (_pos.Y >= 400) _dir.Y = -_dir.Y;
            if (_pos.Y <= 300) _dir.Y = -_dir.Y;
        }
    }
}
