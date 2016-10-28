using System;
using System.Collections.Generic;
using System.Drawing;

namespace SchetsEditor
{
    public class DrawnItem
    {
        public Type toolType;

        public Color color;

        public List<Element> elements = new List<Element>();
    }

    public class Element
    {
        public Point pointA;

        public Point pointB;

        public char Text;
    }
}
