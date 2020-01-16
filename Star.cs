﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace MyGameAsteroids
{
    class Star : BaseObject
    {
        public Star(Point pos, Point dir, Size size, Pen pen) : base(pos, dir, size, pen)
        {

        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(MyPen, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
            Game.Buffer.Graphics.DrawLine(MyPen, Pos.X + Size.Width, Pos.Y, Pos.X, Pos.Y + Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width;
        }
    }
}
