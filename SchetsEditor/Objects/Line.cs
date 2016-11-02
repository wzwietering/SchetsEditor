using System;
using System.Collections.Generic;
using System.Drawing;

namespace SchetsEditor
{
    public class Line : DrawnElement
    {
        public override string ToString() { return "lijn"; }

        public override void Draw(Graphics g, Brush brush)
        {
            g.DrawLine(helper.MaakPen(brush, this.LineThickness), this.pointA, this.pointB);
        }

        public override bool WasClicked(Point p)
        {
            double dAB = distance(pointA, pointB);
            double dApB = distance(pointA, p) + distance(pointB, p);

            if (dAB - 2 <= dApB && dAB + 2 >= dApB)
                return true; // p is on the line.
            return false;    // p is not on the line.
        }

        private double distance (Point a, Point b)
        {
            int dX = a.X - b.X;
            int dY = a.Y - b.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }
    }
}
