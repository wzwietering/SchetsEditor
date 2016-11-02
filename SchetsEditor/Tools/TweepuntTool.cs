using SchetsEditor.Objects;
using System;
using System.Drawing;
using System.Linq;

namespace SchetsEditor
{
    public class TweepuntTool<T> : StartpuntTool where T : DrawnElement
    {
        public T element;

        public override string ToString()
        {
            return Activator.CreateInstance<T>().ToString();
        }

        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);
            kwast = Brushes.Gray;

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
