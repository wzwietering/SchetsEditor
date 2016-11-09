using System;
using System.Drawing;
using System.Linq;

namespace SchetsEditor
{
    // With this tool you can erase an element from the list of drawnItems.
    public class GumTool : SelectorTool
    {
        public override string ToString() { return "gum"; }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
         
            if (this.selectedItem != null)
            {
                // remove it and rebuild the bitmap.
                s.Schets.RemoveElement(selectedItem);
                s.RebuildBitmap(this, new EventArgs());
            }
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
        }
    }
}
