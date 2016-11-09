using SchetsEditor.Tools;
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
        /// Leest een XML en maakt er een tekening van
        /// </summary>
        /// <param name="path">De opgeslagen locatie van de XML</param>
        /// <param name="sw">Het window waar de tekening in komt</param>
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

            //Tekst wordt anders opgeslagen dan de andere tools, dus het wordt ook anders gelezen
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

            //Geïmporteerde afbeeldingen worden ook anders opgeslagen dan andere tools
            ImageTool imagetool;
            foreach(XElement i in xml.Descendants("Image"))
            {
                imagetool = new ImageTool();
                imagetool.MuisVast(sw.schetscontrol, XElementToPoint(i, "PointA"));
                imagetool.MuisLos(sw.schetscontrol, XElementToPoint(i, "PointB"));
                imagetool.DrawImage(sw.schetscontrol, i.Element("Path").Value);
            }
        }

        /// <summary>
        /// Converteert een XElement naar een punt
        /// </summary>
        /// <param name="xe">Het XElement</param>
        /// <param name="filter">Het filter wat toegepast wordt om het punt te vinden</param>
        /// <returns>Een nieuw punt die ooit een XElement was</returns>
        public Point XElementToPoint(XElement xe, string filter)
        {
            string[] xy = xe.Element(filter).Value.Split(',', '=', '}');
            Point p = new Point(int.Parse(xy[1]), int.Parse(xy[3]));
            return p;
        }
    }
}
