using System.IO;
using System.Linq;

namespace SchetsEditor.IO
{
    class Read
    {
        public void ReadXML(string path)
        {

        }

        /// <summary>
        /// Reads a CSV file
        /// </summary>
        /// <param name="path">The location of the file to read</param>
        public void ReadCSV(string path)
        {
            StreamReader sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                //Commas can be escaped in the file using '\'.  It is replaced by a '#' to correct this later on
                char[] c = line.ToCharArray();
                for(int a = 0; a < c.Length - 1; a++)
                {
                    if (c[a] == '\\') c[a + 1] = '#';
                }
                line = new string(c);

                string[] variables = line.Split(',');
                for(int i = 0; i < variables.Length; i++)
                {
                    c = variables[i].ToCharArray();

                    //Remove escape characters
                    char backslash = '\\';
                    c = c.Where(j => j != backslash).ToArray();

                    //Replace escaped commas with a comma
                    for (int b = 0; b < c.Length; b++)
                    {
                        if(c[b] == '#')
                        {
                            c[b] = ',';
                        }
                    }
                    variables[i] = new string(c);
                    System.Diagnostics.Debug.WriteLine(variables[i]);
                }
            }
            sr.Close();
        }
    }
}
