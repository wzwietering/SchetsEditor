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
            double dAB = helper.distance(pointA, pointB);
            double dApB = helper.distance(pointA, p) + helper.distance(pointB, p);

            if (dAB - 0.5 * LineThickness <= dApB && dAB + 0.5 * LineThickness >= dApB)
                return true; // p is on the line.
            return false;    // p is not on the line.
        }
    }
}
