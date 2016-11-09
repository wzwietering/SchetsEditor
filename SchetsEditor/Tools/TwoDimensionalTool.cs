using SchetsEditor.Objects;
using System;
using System.Drawing;
using System.Linq;

namespace SchetsEditor
{
    // Twodimensional tool has a start and end point. It can be of various types like circle, rectangle etc.
    // It can make one element that will be added to a drawnItem, which will be added to the sketch (on Finalize())
    // This is unlike (for example) a text tool, where multiple elements are added to a drawnItem (one for each typed letter).
    public class TwoDimensionalTool<T> : OneDimensionalTool where T : DrawnElement
    {
        public T element;

        public override string ToString()
        {
            return Activator.CreateInstance<T>().ToString();
        }

        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);

            element = Activator.CreateInstance<T>();
            element.pointA = p;
            element.LineThickness = s.lijnDikte;
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
            s.Refresh();
            element.pointB = p;
            element.Draw(s.CreateGraphics(), kwast);
        }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
            element.pointB = p;
            element.Draw(s.MaakBitmapGraphics(), kwast);
            s.Invalidate();

            this.drawnItem.elements.Add(element);
            Finalize(s);
        }
    }
}
