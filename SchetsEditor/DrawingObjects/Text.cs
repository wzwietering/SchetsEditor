using System.Drawing;

namespace SchetsEditor.Objects
{
    public class Text : DrawnElement
    {
        public string text;

        public Font font;

        public override void Draw(Graphics g, Brush brush)
        {
            g.DrawString(text, font, brush, this.pointA, StringFormat.GenericTypographic);
        }

        public override bool WasClicked(Point p)
        {
            return (this.pointA.X <= p.X && this.pointA.Y <= p.Y
                && this.pointB.X >= p.X && this.pointB.Y >= p.Y);
        }
    }
}
