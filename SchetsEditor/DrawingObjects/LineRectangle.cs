using System;
using System.Collections.Generic;
using System.Drawing;

namespace SchetsEditor
{
    public class LineRectangle : DrawnElement
    {
        public override string ToString() { return "kader"; }

        public override void Draw(Graphics g, Brush brush)
        {
            g.DrawRectangle(Helper.MaakPen(brush, LineThickness), Helper.Punten2Rechthoek(this.pointA, this.pointB));
        }

        public override bool WasClicked(Point p)
        {
            var clicked = (this.pointA.X <= p.X && this.pointA.Y <= p.Y
                            && this.pointB.X >= p.X && this.pointB.Y >= p.Y);
            if (clicked)
            {
                clicked =!(this.pointA.X + LineThickness + 2 <= p.X 
                    && this.pointA.Y + LineThickness + 2 <= p.Y
                    && this.pointB.X - LineThickness + 2 >= p.X 
                    && this.pointB.Y - LineThickness + 2 >= p.Y);
            }
            return clicked;
        }
    }
}
