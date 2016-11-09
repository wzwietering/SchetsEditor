using System;
using System.Drawing;
using System.Linq;

namespace SchetsEditor
{
    // With this tool you can select an element by clicking on it.
    public class SelectorTool : OneDimensionalTool
    {
        public override string ToString() { return "gum"; }

        // The item that was clicked
        internal DrawnItem selectedItem;

        public override void MuisLos(SchetsControl s, Point p)
        {
            // Reset the selected item (because the user made a new click)
            this.selectedItem = null;

            // Get the object that was clicked
            var clickedObjects = s.Schets.drawnItems.Where(o => o.elements.Any(e => e.WasClicked(p)));
            if (clickedObjects != null && clickedObjects.Count() > 0)
            {
                selectedItem = clickedObjects.Last();
            }
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
        }
    }
}
