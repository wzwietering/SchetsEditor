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
        /// <param name="items">The items to save</param>
        public void WriteXML(string path, List<DrawnItem> items)
        {
            XElement xml = new XElement("Items");
            foreach (DrawnItem obj in items)
            {
                if (obj.elements[0] is Objects.Text)
                {
                    xml.Add(new XElement("TextObject",
                                                       new XElement("Color", obj.color.ToArgb()),
                                                       from el in obj.elements
                                                       select new XElement("TextElement", 
                                                       new XElement("Elements",
                                                            new XElement("Type", el.GetType().ToString()),
                                                            new XElement("Text", ((Objects.Text)el).text),
                                                            new XElement("Font", ((Objects.Text)el).font),
                                                            new XElement("PointA", el.pointA),
                                                            new XElement("PointB", el.pointB)
                                                            ))));
                }
                else
                {
                    xml.Add(new XElement("Object",
                                                new XElement("Color", obj.color.ToArgb()),
                                                from el in obj.elements
                                                select new XElement("Elements",
                                                new XElement("Element",
                                                    new XElement("Type", el.GetType().ToString()),
                                                    new XElement("PointA", el.pointA),
                                                    new XElement("PointB", el.pointB),
                                                    new XElement("Thickness", el.LineThickness)
                                                    ))));
                }
            }
            xml.Save(path);
        }
    }
}
