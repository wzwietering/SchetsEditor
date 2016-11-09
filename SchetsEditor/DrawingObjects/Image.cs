using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SchetsEditor.DrawingObjects
{
    class Image : DrawnElement
    {
        public Bitmap bitmap;
        public string path;

        public override void Draw(Graphics g, Brush b)
        {
            g.DrawImage(bitmap, this.pointA);
        }

        public override bool WasClicked(Point p)
        {
            return (this.pointA.X <= p.X && this.pointA.Y <= p.Y
                && this.pointB.X >= p.X && this.pointB.Y >= p.Y);
        }
    }
}
