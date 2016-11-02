using SchetsEditor.Objects;
using System;
using System.Drawing;
using System.Linq;

namespace SchetsEditor
{
    public interface ISchetsTool
    {
        void MuisVast(SchetsControl s, Point p);
        void MuisDrag(SchetsControl s, Point p);
        void MuisLos(SchetsControl s, Point p);
        void Letter(SchetsControl s, char c);
        void Reset(SchetsControl s);
    }


    public abstract class StartpuntTool : ISchetsTool
    {
        protected Point startpunt;
        protected Brush kwast;
        protected int brushSize;

        protected DrawnItem drawnItem;

        public virtual void MuisVast(SchetsControl s, Point p)
        {
            drawnItem = new DrawnItem() { color = s.PenKleur, toolType = this.GetType() };
            this.startpunt = p;
        }
        public virtual void MuisLos(SchetsControl s, Point p)
        {
            kwast = new SolidBrush(s.PenKleur);
        }

        public abstract void MuisDrag(SchetsControl s, Point p);
        public abstract void Letter(SchetsControl s, char c);

        public virtual void Reset(SchetsControl s)
        {

        }
    }

    public class TekstTool : StartpuntTool
    {
        public Text element;

        public override void MuisVast(SchetsControl s, Point p)
        {
            Reset(s);
            base.MuisVast(s, p);
        }

        public override void Reset(SchetsControl s)
        {
            if ( this.drawnItem != null && this.drawnItem.elements.Count > 0)
            {
                s.Schets.AddElement(this.drawnItem);
            }
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

    public class TweepuntTool<T> : StartpuntTool where T : DrawnElement
    {
        public T element;

        public override string ToString() {
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
            s.Schets.AddElement(this.drawnItem);
        }
        public override void Letter(SchetsControl s, char c)
        {
        }
    }


    //public class LijnTool : TweepuntTool
    //{
    //    public override string ToString() { return "lijn"; }

    //    public override void Bezig(Graphics g, Point p1, Point p2, int d)
    //    {
    //        g.DrawLine(MaakPen(this.kwast, d), p1, p2);
    //    }

    //    public override void MuisLos(SchetsControl s, Point p)
    //    {
    //        base.MuisLos(s, p);
    //        s.Schets.AddElement(this.drawnItem);
    //    }
    //}

    //public class PenTool : LijnTool
    //{
    //    public override string ToString() { return "pen"; }

    //    public override void MuisDrag(SchetsControl s, Point p)
    //    {
    //        base.MuisLos(s, p);
    //        this.MuisVast(s, p);
    //    }

    //    public override void MuisLos(SchetsControl s, Point p)
    //    {
    //        base.MuisLos(s, p);
    //        s.Schets.AddElement(this.drawnItem);
    //    }
    //}

    public class GumTool : StartpuntTool
    {
        public override string ToString() { return "gum"; }

        public override void MuisDrag(SchetsControl s, Point p) { }
        public override void Letter(SchetsControl s, char c) { }

        public override void MuisLos(SchetsControl s, Point p)
        {
            var clickedObjects = s.Schets.GetDrawnItems().Where(o => o.elements.Any(e => e.WasClicked(p)));
            if (clickedObjects != null && clickedObjects.Count() > 0)
            {
                var clickedObject = clickedObjects.Last();
                s.Schets.RemoveElement(clickedObject);
                s.RebuildBitmap(this, new EventArgs());
            }
        }
    }
}
