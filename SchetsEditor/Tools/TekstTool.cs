using SchetsEditor.Objects;
using System.Drawing;

namespace SchetsEditor
{
    // This tool makes bits of text. The text is saved letter by letter in a drawnElement, and these elements are grouped together in
    // a single drawnItem (so they can be erased all together).
    //
    // What you can't see in this class definition is that when the user selects a new tool, Finalize() will also be called
    // so the typed text is added as a new drawnElement.
    public class TekstTool : OneDimensionalTool
    {
        public Text element;
        public Point startpunt;

        public override void MuisVast(SchetsControl s, Point p)
        {
            startpunt = p;

            // Call to finalize in case the user has typed text before this click. If this is the case, the previously typed text should be
            // saved as one drawnItem, and we now need a new drawnItem.
            base.Finalize(s);
            base.MuisVast(s, p);
        }

        public override string ToString() { return "tekst"; }

        public override void MuisDrag(SchetsControl s, Point p) { }

        // User typed something. Add it as a new element to the DrawnItem!
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
