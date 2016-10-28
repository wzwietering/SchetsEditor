﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace SchetsEditor
{
    public interface ISchetsTool
    {
        void MuisVast(SchetsControl s, Point p);
        void MuisDrag(SchetsControl s, Point p);
        void MuisLos(SchetsControl s, Point p);
        void Letter(SchetsControl s, char c);
    }

    public abstract class StartpuntTool : ISchetsTool
    {
        protected Point startpunt;
        protected Brush kwast;
        protected int brushSize;

        public virtual void MuisVast(SchetsControl s, Point p)
        {
            startpunt = p;
        }
        public virtual void MuisLos(SchetsControl s, Point p)
        {
            kwast = new SolidBrush(s.PenKleur);
        }
        public abstract void MuisDrag(SchetsControl s, Point p);
        public abstract void Letter(SchetsControl s, char c);

        public virtual Element CreateElement(SchetsControl s, Point p)
        {
            return new SchetsEditor.Element()
            {
                color = s.PenKleur,
                pointA = startpunt,
                pointB = p,
                toolType = this.GetType()
            };
        }
    }

    public class TekstTool : StartpuntTool
    {
        public override string ToString() { return "tekst"; }

        public override void MuisDrag(SchetsControl s, Point p) { }

        public override void Letter(SchetsControl s, char c)
        {
            if (c >= 32)
            {
                Graphics gr = s.MaakBitmapGraphics();
                Font font = new Font("Tahoma", 40);
                string tekst = c.ToString();
                SizeF sz =
                gr.MeasureString(tekst, font, this.startpunt, StringFormat.GenericTypographic);
                gr.DrawString(tekst, font, kwast,
                                              this.startpunt, StringFormat.GenericTypographic);

                Element element = base.CreateElement(s, new Point(startpunt.X + (int)sz.Width, startpunt.Y + (int)sz.Height));
                element.Text = c;
                s.Schets.AddElement(element);

                // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
                startpunt.X += (int)sz.Width;
                s.Invalidate();                
            }
        }
    }

    public abstract class TweepuntTool : StartpuntTool
    {
        public static Rectangle Punten2Rechthoek(Point p1, Point p2)
        {
            return new Rectangle(new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y))
                                , new Size(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y))
                                );
        }
        public static Pen MaakPen(Brush b, int dikte)
        {
            Pen pen = new Pen(b, dikte);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            return pen;
        }
        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);
            kwast = Brushes.Gray;
        }
        public override void MuisDrag(SchetsControl s, Point p)
        {
            s.Refresh();
            this.Bezig(s.CreateGraphics(), this.startpunt, p, s.lijnDikte);
        }
        public override void MuisLos(SchetsControl s, Point p)
        {   base.MuisLos(s, p);
            this.Compleet(s.MaakBitmapGraphics(), this.startpunt, p, s);
            s.Schets.AddElement(this.CreateElement(s, p));
            s.Invalidate();
        }
        public override void Letter(SchetsControl s, char c)
        {
        }
        public abstract void Bezig(Graphics g, Point p1, Point p2, int d);

        public virtual void Compleet(Graphics g, Point p1, Point p2, SchetsControl s)
        {
            this.Bezig(g, p1, p2, s.lijnDikte);
        }
    }

    public class RechthoekTool : TweepuntTool
    {
        public override string ToString() { return "kader"; }

        public override void Bezig(Graphics g, Point p1, Point p2, int d)
        {
            g.DrawRectangle(MaakPen(kwast, d), TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }

    public class VolRechthoekTool : RechthoekTool
    {
        public override string ToString() { return "vlak"; }

        public override void Compleet(Graphics g, Point p1, Point p2, SchetsControl s)
        {
            g.FillRectangle(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }

    public class CirkelTool : TweepuntTool
    {
        public override string ToString() { return "cirkel"; }

        public override void Bezig(Graphics g, Point p1, Point p2, int d)
        {
            g.DrawEllipse(MaakPen(kwast, d), TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }

    public class VolCirkelTool : CirkelTool
    {
        public override string ToString() { return "bal"; }

        public override void Compleet(Graphics g, Point p1, Point p2, SchetsControl s)
        {
            g.FillEllipse(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }

    public class LijnTool : TweepuntTool
    {
        public override string ToString() { return "lijn"; }

        public override void Bezig(Graphics g, Point p1, Point p2, int d)
        {
            g.DrawLine(MaakPen(this.kwast, d), p1, p2);
        }
    }

    public class PenTool : LijnTool
    {
        public override string ToString() { return "pen"; }

        public override void MuisDrag(SchetsControl s, Point p)
        {
            this.MuisLos(s, p);
            this.MuisVast(s, p);
        }
    }

    public class GumTool : StartpuntTool
    {
        public override string ToString() { return "gum"; }

        public override void MuisDrag(SchetsControl s, Point p) { }
        public override void Letter(SchetsControl s, char c) { }

        public override void MuisLos(SchetsControl s, Point p)
        {
            var clickedElements = s.Schets.GetElements().Where(e => e.pointA.X <= p.X 
                                            && e.pointA.Y <= p.Y
                                            && e.pointB.X >= p.X 
                                            && e.pointB.Y >= p.Y);
            if (clickedElements != null && clickedElements.Count() > 0)
            {
                var clickedElement = clickedElements.Last();
                s.Schets.RemoveElement(clickedElement);
                s.RebuildBitmap(this, new EventArgs());
            }
        }
    }
}
