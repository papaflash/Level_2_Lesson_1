using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGameAsteroids
{
    class BaseObject
    {
        
        Image image = Image.FromFile(@"D:\C#\MyGameAsteroids\Properties\asteroid.png");
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        protected Pen MyPen;

        public BaseObject(Point pos, Point dir, Size size, Pen myPen)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
            MyPen = myPen;
        }
        public virtual void Draw()
        {
            //Заменить кружочки картинкой используя DrawImage
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
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
