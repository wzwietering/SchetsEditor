using System;
using System.Collections.Generic;
using System.Drawing;

namespace SchetsEditor
{
    public class FullCircle : LineCircle
    {
        public override string ToString() { return "bal"; }

        public override bool WasClicked(Point p)
        {
            return helper.EllipseClicked(pointA, pointB, p, 0);
        }

        public override void Draw(Graphics g, Brush brush)
        {
            base.Draw(g, brush);
            g.FillEllipse(brush, helper.Punten2Rechthoek(this.pointA, this.pointB));
        }
    }
}
