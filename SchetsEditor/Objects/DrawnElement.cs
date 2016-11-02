using System;
using System.Collections.Generic;
using System.Drawing;

namespace SchetsEditor
{
    public abstract class DrawnElement
    {
        public Point pointA;

        public Point pointB;

        public int LineThickness;

        public abstract bool WasClicked(Point p);

        public abstract void Draw(Graphics g, Brush brush);
    }
}
