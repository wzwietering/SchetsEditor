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
                sw.schetscontrol.penkleur = Color.FromArgb(int.Parse(o.Element("Color").Value));

                foreach(XElement el in o.Descendants("Elements"))
                {
                    sw.schetscontrol.lijnDikte = int.Parse(el.Element("Thickness").Value);
                    switch (el.Element("Type").Value)
                    {
                        case "SchetsEditor.FullRectangle":
                            current = (ISchetsTool)(new TweepuntTool<FullRectangle>());
                            current.MuisVast(sw.schetscontrol, XElementToPoint(el, "PointA"));
                            current.MuisLos(sw.schetscontrol, XElementToPoint(el, "PointB"));
                            break;
                        case "SchetsEditor.LineRectangle":
                            current = (ISchetsTool)(new TweepuntTool<LineRectangle>());
                            current.MuisVast(sw.schetscontrol, XElementToPoint(el, "PointA"));
                            current.MuisLos(sw.schetscontrol, XElementToPoint(el, "PointB"));
                            break;
                        case "SchetsEditor.Line":
                            current = (ISchetsTool)(new TweepuntTool<Line>());
                            current.MuisVast(sw.schetscontrol, XElementToPoint(el, "PointA"));
                            current.MuisLos(sw.schetscontrol, XElementToPoint(el, "PointB"));
                            break;
                        case "SchetsEditor.LineCircle":
                            current = (ISchetsTool)(new TweepuntTool<LineCircle>());
                            current.MuisVast(sw.schetscontrol, XElementToPoint(el, "PointA"));
                            current.MuisLos(sw.schetscontrol, XElementToPoint(el, "PointB"));
                            break;
                        case "SchetsEditor.FullCircle":
                            current = (ISchetsTool)(new TweepuntTool<FullCircle>());
                            current.MuisVast(sw.schetscontrol, XElementToPoint(el, "PointA"));
                            current.MuisLos(sw.schetscontrol, XElementToPoint(el, "PointB"));
                            break;
                    }
                }
            }

            foreach(XElement to in xml.Descendants("TextObject"))
            {
                ISchetsTool text = new TekstTool();
                sw.schetscontrol.penkleur = Color.FromArgb(int.Parse(to.Element("Color").Value));

                foreach(XElement te in xml.Descendants("TextElements"))
                {
                    text.MuisVast(sw.schetscontrol, XElementToPoint(te, "PointA"));
                    text.MuisLos(sw.schetscontrol, XElementToPoint(te, "PointB"));
                    text.Letter(sw.schetscontrol, XElementToChar(te));
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

        /// <summary>
        /// This method reads the char from the XElement 'Text'
        /// </summary>
        /// <param name="xe">The XElement to read</param>
        /// <returns>The char which was read by the method</returns>
        public char XElementToChar(XElement xe)
        {
            return xe.Element("Text").Value.ToCharArray()[0];
        }
    }
}
