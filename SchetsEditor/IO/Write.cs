using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SchetsEditor.IO
{
    class Write
    {
        /// <summary>
        /// Maakt een csv file met alle data van de tekening
        /// </summary>
        /// <param name="path">De locatie om het bestand op te slaan</param>
        /// <param name="objects">De objecten die opgeslagen moeten worden</param>
        public void WriteCSV(string path, List<DrawnItem> objects)
        {
            StreamWriter fs = new StreamWriter(path);
            foreach(DrawnItem d in objects)
            {
                string line = d.color.ToArgb().ToString();

                foreach(DrawnElement de in d.elements)
                {
                    line = line + "," + de.GetType().ToString() + "," + de.pointA + "," + de.pointB + "," + de.LineThickness;
                }
                fs.WriteLine(line);
            }
            fs.Close();
        }
    }
}
