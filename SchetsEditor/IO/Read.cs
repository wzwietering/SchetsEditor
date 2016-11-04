using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SchetsEditor.IO
{
    class Read
    {
        public void ReadXML(string path, SchetsWin sw)
        {
            XDocument xml = XDocument.Load(path);
            ISchetsTool[] ist = sw.GetTools();
            foreach (var o in xml.Descendants("Object"))
            {
                if (o.Element("Elements").Element("Type").Value == "SchetsEditor.FullRectangle")
                {

                }
                else if (o.Element("Elements").Element("Type").Value == "SchetsEditor.LineRectangle")
                {

                }
                else if (o.Element("Elements").Element("Type").Value == "SchetsEditor.Line")
                {

                }
                else if (o.Element("Elements").Element("Type").Value == "SchetsEditor.LineCircle")
                {

                }
                else if (o.Element("Elements").Element("Type").Value == "SchetsEditor.FullCircle")
                {

                }
            }
        }
    }
}
