using System.Drawing;

namespace SchetsEditor
{
    public class LineCircle : DrawnElement
    {
        public override string ToString() { return "cirkel"; }

        public override void Draw(Graphics g, Brush brush)
        {
            g.DrawEllipse(helper.MaakPen(brush, LineThickness), helper.Punten2Rechthoek(this.pointA, this.pointB));
        }

        public override bool WasClicked(Point p)
        {
            return (this.pointA.X <= p.X && this.pointA.Y <= p.Y
                && this.pointB.X >= p.X && this.pointB.Y >= p.Y);
        }
    }
}
