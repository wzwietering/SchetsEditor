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
        private Point pointb;

        //De tool is ontworpen om een afbeelding op een door de gebruiker geselecteerde plek neer te zetten, dit is niet geïmplementeerd in de UI
        public override void MuisVast(SchetsControl s, Point p)
        {
            point = p;
            base.MuisVast(s, p);
        }

        public override void MuisLos(SchetsControl s, Point p)
        {
            pointb = p;
            base.MuisLos(s, p);
        }
        public override void MuisDrag(SchetsControl s, Point p) { }

        /// <summary>
        /// Tekent een afbeelding op het scherm
        /// </summary>
        /// <param name="s">De gewenste control om op te tekenen</param>
        /// <param name="location">De plaats van de afbeelding op de disk</param>
        public void DrawImage(SchetsControl s, string location)
        {
            image = new DrawingObjects.Image();
            image.bitmap = new Bitmap(location);
            image.pointA = point;
            image.pointB = new Point(point.X + image.bitmap.Width, point.Y + image.bitmap.Height);
            image.path = location;
            Graphics g = s.MaakBitmapGraphics();
            image.Draw(g, kwast);

            this.drawnItem.elements.Add(image);
            this.Finalize(s);
            s.Invalidate();
        }
    }
}
