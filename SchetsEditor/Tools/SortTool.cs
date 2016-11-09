using System;
using System.Drawing;

namespace SchetsEditor
{
    // Tool to move drawn items up and down in the list of drawn items, to move them on top of or underneath other items.
    public class SortTool : SelectorTool
    {
        // Determines whether this tool moves drawn items up (if true) or down (if false)
        public bool directionUp;

        public override string ToString() { return directionUp ? "op" : "neer" ; }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);

            if (this.selectedItem != null)
            {
                int currentIndex = s.Schets.drawnItems.IndexOf(this.selectedItem);

                int newIndex = directionUp 
                    // Only move up if it is not already the element with the highest index
                    ? currentIndex == s.Schets.drawnItems.Count - 1 ? currentIndex : currentIndex + 1 
                    // Only move down if index is not 0
                    : currentIndex == 0 ? 0 : currentIndex - 1;

                // Remove and insert in new position.
                s.Schets.drawnItems.Remove(this.selectedItem);
                s.Schets.drawnItems.Insert(newIndex, this.selectedItem);

                s.RebuildBitmap(this, new EventArgs());
            }
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
        }
    }
}
