using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MyGameAsteroids.Properties;

namespace MyGameAsteroids
{
    class BaseObject
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        protected Pen MyPen;
        readonly Image _image;

        public BaseObject(Point pos, Point dir, Size size, Pen myPen)
        {
            _image = Resources.asteroid;
            Pos = pos;
            Dir = dir;
            Size = size;
            MyPen = myPen;
        }
        public virtual void Draw()
        {
            //Заменить кружочки картинкой используя DrawImage
            Game.Buffer.Graphics.DrawImage(_image, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public virtual void Update()
        {
            Pos.X += Dir.X;
            Pos.Y += Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;  
        }
    }
}
