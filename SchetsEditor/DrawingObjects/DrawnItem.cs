using System;
using System.Collections.Generic;
using System.Drawing;

namespace SchetsEditor
{
    public class DrawnItem
    {
        public Color color;

        public List<DrawnElement> elements = new List<DrawnElement>();

        internal void Draw(SchetsControl s)
        {
            foreach (var element in elements)
            {
                Brush brush = new SolidBrush(color);
                element.Draw(s.MaakBitmapGraphics(), brush);
            }
        }
    }
}
