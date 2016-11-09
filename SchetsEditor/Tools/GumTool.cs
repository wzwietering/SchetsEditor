using System;
using System.Drawing;
using System.Linq;

namespace SchetsEditor
{
    // With this tool you can erase an element from the list of drawnItems.
    public class GumTool : OneDimensionalTool
    {
        public override string ToString() { return "gum"; }

        public override void MuisLos(SchetsControl s, Point p)
        {
            // Get the object that was clicked
            var clickedObjects = s.Schets.GetDrawnItems().Where(o => o.elements.Any(e => e.WasClicked(p)));
            if (clickedObjects != null && clickedObjects.Count() > 0)
            {
                var clickedObject = clickedObjects.Last();

                // remove it and rebuild the bitmap.
                s.Schets.RemoveElement(clickedObject);
                s.RebuildBitmap(this, new EventArgs());
            }
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
        }
    }
}
