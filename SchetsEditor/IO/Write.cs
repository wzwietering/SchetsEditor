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
        /// Creates a XML with all the information about the drawing
        /// </summary>
        /// <param name="path">The place to save the XML</param>
        /// <param name="objects">The objects to save</param>
        public void WriteXML(string path, List<DrawnItem> objects)
        {
            XElement xml = new XElement("Objects");
            foreach(DrawnItem obj in objects)
            {
                foreach (DrawnElement el in obj.elements)
                {
                    if(el is Objects.Text)
                    {
                        xml.Add(new XElement("TextObject", new XElement("Color", obj.color.ToArgb()), new XElement("Elements",
                            new XElement("Type", el.GetType().ToString()),
                            new XElement("Text", ((Objects.Text)el).text),
                            new XElement("Font", ((Objects.Text)el).font),
                            new XElement("PointA", el.pointA),
                            new XElement("PointB", el.pointB)
                            )));
                    }
                    else
                    {
                        xml.Add(new XElement("Object", new XElement("Color", obj.color.ToArgb()), new XElement("Elements",
                            new XElement("Type", el.GetType().ToString()),
                            new XElement("PointA", el.pointA),
                            new XElement("PointB", el.pointB),
                            new XElement("Thickness", el.LineThickness)
                            )));
                    }
                }
            }
            xml.Save(path);
        }
    }
}
