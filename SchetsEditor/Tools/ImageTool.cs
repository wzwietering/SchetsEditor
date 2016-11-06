using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SchetsEditor.Tools
{
    class ImageTool : StartpuntTool
    {
        public DrawingObjects.Image image;
        private Point point;

        public override void MuisVast(SchetsControl s, Point p)
        {
            point = p;
            base.MuisVast(s, p);
        }
        public override void MuisDrag(SchetsControl s, Point p) { }

        public void DrawImage(SchetsControl s, string location)
        {
            image = new DrawingObjects.Image();
            image.pointA = point;
            image.image = new Bitmap(location);
            image.path = location;
            Graphics g = s.MaakBitmapGraphics();
            image.Draw(g, kwast);

            this.drawnItem.elements.Add(image);
            this.Finalize(s);
            s.Invalidate();
        }
    }
}
