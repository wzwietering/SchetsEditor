using System;
using System.Drawing;
using System.Linq;

namespace SchetsEditor
{
    // Tool to move drawn items up and down in the list of drawn items, to move them on top of or underneath other items.
    public class SortTool : StartpuntTool
    {
        // Determines whether this tool moves drawn items up (if true) or down (if false)
        public bool directionUp;

        public override string ToString() { return directionUp ? "op" : "neer" ; }

        public override void MuisLos(SchetsControl s, Point p)
        {
            var clickedObjects = s.Schets.GetDrawnItems().Where(o => o.elements.Any(e => e.WasClicked(p)));
            if (clickedObjects != null && clickedObjects.Count() > 0)
            {
                var clickedObject = clickedObjects.Last();
                int currentIndex = s.Schets.drawnItems.IndexOf(clickedObject);

                int newIndex = directionUp 
                    // Only move up if it is not already the element with the highest index
                    ? currentIndex == s.Schets.drawnItems.Count - 1 ? currentIndex : currentIndex + 1 
                    // Only move down if index is not 0
                    : currentIndex == 0 ? 0 : currentIndex - 1;

                // Remove and insert in new position.
                s.Schets.RemoveElement(clickedObject);
                s.Schets.drawnItems.Insert(newIndex, clickedObject);

                s.RebuildBitmap(this, new EventArgs());
            }
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
        }
    }
}
