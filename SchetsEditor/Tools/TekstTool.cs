using SchetsEditor.Objects;
using System.Drawing;

namespace SchetsEditor
{
    public class TekstTool : StartpuntTool
    {
        public Text element;
        public Point startpunt;

        public override void MuisVast(SchetsControl s, Point p)
        {
            startpunt = p;
            base.Finalize(s);
            base.MuisVast(s, p);
        }

        public override string ToString() { return "tekst"; }

        public override void MuisDrag(SchetsControl s, Point p) { }

        public override void Letter(SchetsControl s, char c)
        {
            if (c >= 32)
            {
                string tekst = c.ToString();
                Graphics g = s.MaakBitmapGraphics();
                Font font = new Font("Tahoma", 40);
                SizeF sz = g.MeasureString(tekst, font, startpunt, StringFormat.GenericTypographic);

                element = new Text()
                {
                    pointA = startpunt,
                    pointB = new Point(startpunt.X + (int)sz.Width, startpunt.Y + (int)sz.Height),
                    text = tekst,
                    font = font
                };

                element.Draw(g, kwast);
                this.drawnItem.elements.Add(element);

                startpunt.X += (int)sz.Width;
                s.Invalidate();
            }
        }
    }
}
