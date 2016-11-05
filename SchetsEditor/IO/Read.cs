using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SchetsEditor.IO
{
    class Read
    {
        /// <summary>
        /// Reads a saved XML and converts it to a drawing
        /// </summary>
        /// <param name="path">The location of the XML on the disk</param>
        /// <param name="sw">The schetswin to draw on</param>
        public void ReadXML(string path, SchetsWin sw)
        {
            XDocument xml = XDocument.Load(path);
            ISchetsTool current;
            foreach (XElement o in xml.Descendants("Object"))
            {
                switch (o.Element("Elements").Element("Type").Value)
                {
                    case "SchetsEditor.FullRectangle":
                        current = (ISchetsTool)(new TweepuntTool<FullRectangle>());
                        break;
                    case "SchetsEditor.LineRectangle":
                        current = (ISchetsTool)(new TweepuntTool<LineRectangle>());
                        break;
                    case "SchetsEditor.Line":
                        current = (ISchetsTool)(new TweepuntTool<Line>());
                        break;
                    case "SchetsEditor.LineCircle":
                        current = (ISchetsTool)(new TweepuntTool<LineCircle>());
                        break;
                    case "SchetsEditor.FullCircle":
                        current = (ISchetsTool)(new TweepuntTool<FullCircle>());
                        break;
                    default:
                        current = current = (ISchetsTool)(new TweepuntTool<Line>());
                        break;
                }
                sw.schetscontrol.penkleur = Color.FromArgb(int.Parse(o.Element("Color").Value));
                sw.schetscontrol.lijnDikte = int.Parse(o.Element("Elements").Element("Thickness").Value);
                if (o.Descendants("Elements").Count() > 1) current = (ISchetsTool)new Pencil();

                current.MuisVast(sw.schetscontrol, XElementToPoint(o.Element("Elements"), "PointA"));
                foreach (XElement el in o.Descendants("Elements"))
                {
                    current.MuisDrag(sw.schetscontrol, XElementToPoint(el, "PointB"));
                }
                current.MuisLos(sw.schetscontrol, XElementToPoint(o.Descendants("Elements").Last(), "PointB"));
            }

            ISchetsTool text;
            foreach (XElement to in xml.Descendants("TextObject"))
            {
                text = new TekstTool();
                sw.schetscontrol.penkleur = Color.FromArgb(int.Parse(to.Element("Color").Value));
                text.MuisVast(sw.schetscontrol, XElementToPoint(to, "PointA"));
                text.MuisLos(sw.schetscontrol, XElementToPoint(to, "PointB"));

                foreach (var x in to.Elements("Text"))
                {
                    text.Letter(sw.schetscontrol, x.Value.ToCharArray()[0]);
                }

                text.Finalize(sw.schetscontrol);
            }
        }

        /// <summary>
        /// This method gets the saved coordinates from an XElement and makes a point from them
        /// </summary>
        /// <param name="xe">The XElement to read</param>
        /// <param name="variable">The name of the variable to make a point from</param>
        /// <returns>A new point made from the coordinates in the XElement</returns>
        public Point XElementToPoint(XElement xe, string variable)
        {
            string[] xy = xe.Element(variable).Value.Split(',', '=', '}');
            Point p = new Point(int.Parse(xy[1]), int.Parse(xy[3]));
            return p;
        }
    }
}
