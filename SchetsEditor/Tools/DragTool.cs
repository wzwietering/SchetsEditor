using System;
using System.Drawing;
using System.Linq;

namespace SchetsEditor
{
    // Tool to move drawn items up and down in the list of drawn items, to move them on top of or underneath other items.
    public class DragTool : SelectorTool
    {
        public override string ToString() { return "sleep" ; }

        public override void MuisLos(SchetsControl s, Point p)
        {
           
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
        }
    }
}
