using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SchetsEditor
{
    public static class helper
    {
        public static Pen MaakPen(Brush b, int dikte)
        {
            Pen pen = new Pen(b, dikte);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            return pen;
        }

        public static Rectangle Punten2Rechthoek(Point p1, Point p2)
        {
            return new Rectangle(new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y))
                                , new Size(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y))
                                );
        }

        public static double distance(Point a, Point b)
        {
            int dX = a.X - b.X;
            int dY = a.Y - b.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }

        public static bool EllipseClicked(Point pointA, Point pointB, Point p, int lineThickness)
        {
            Point center = new Point(pointA.X + (pointB.X - pointA.X) / 2, pointA.Y + (pointB.Y - pointA.Y) / 2);
            double h = center.X;
            double k = center.Y;
            double rx = Math.Abs(pointA.X - pointB.X) / 2 + lineThickness / 2;
            double ry = Math.Abs(pointA.Y - pointB.Y) / 2 + lineThickness / 2;

            return (((p.X - h) * (p.X - h)) / (rx * rx) + ((p.Y - k) * (p.Y - k)) / (ry * ry) <= 1);
        }
    }
}
