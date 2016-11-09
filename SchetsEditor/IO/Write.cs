using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SchetsEditor.IO
{
    class Write
    {
        /// <summary>
        /// Maakt een XML met alle informatie over de tekening
        /// </summary>
        /// <param name="path">De plek om de XML op te slaan</param>
        /// <param name="items">De elementen die opgeslagen moeten worden</param>
        public void WriteXML(string path, List<DrawnItem> items)
        {
            XElement xml = new XElement("Items");
            foreach (DrawnItem obj in items)
            {
                //Tekst wordt anders opgeslagen dan andere objecten om ruimte te besparen
                if (obj.elements[0] is Objects.Text)
                {
                    xml.Add(new XElement("TextObject",
                                                       new XElement("Color", obj.color.ToArgb()),
                                                       new XElement("PointA", obj.elements[0].pointA),
                                                       new XElement("PointB", obj.elements[0].pointB),
                                                       from el in obj.elements
                                                       select new XElement("Text", ((Objects.Text)el).text)
                                                            ));
                }
                //Geïmporteerde afbeeldingen worden als verwijzing opgeslagen
                else if (obj.elements[0] is DrawingObjects.Image)
                {
                    xml.Add(new XElement("Image",
                                                       new XElement("PointA", obj.elements[0].pointA),
                                                       new XElement("PointB", obj.elements[0].pointB),
                                                       new XElement("Path", ((DrawingObjects.Image)obj.elements[0]).path)
                                                            ));
                }
                else
                {
                    xml.Add(new XElement("Object",
                                                new XElement("Color", obj.color.ToArgb()),
                                                from el in obj.elements
                                                select new XElement("Elements",
                                                    new XElement("Type", el.GetType().ToString()),
                                                    new XElement("PointA", el.pointA),
                                                    new XElement("PointB", el.pointB),
                                                    new XElement("Thickness", el.LineThickness)
                                                    )));
                }
            }
            xml.Save(path);
        }
    }
}
