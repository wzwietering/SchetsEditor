using System;
using System.Drawing;

namespace SchetsEditor
{
    public class LineCircle : DrawnElement
    {
        public override string ToString() { return "cirkel"; }

        public override void Draw(Graphics g, Brush brush)
        {
            g.DrawEllipse(Helper.MaakPen(brush, LineThickness), Helper.Punten2Rechthoek(this.pointA, this.pointB));
        }

        public override bool WasClicked(Point p)
        {
            return Helper.EllipseClicked(pointA, pointB, p, LineThickness + 2) && !Helper.EllipseClicked(pointA, pointB, p, -(LineThickness + 2)); 
        }
    }
}
