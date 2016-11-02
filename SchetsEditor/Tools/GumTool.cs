using SchetsEditor.Objects;
using System;
using System.Drawing;
using System.Linq;

namespace SchetsEditor
{
    public class GumTool : StartpuntTool
    {
        public override string ToString() { return "gum"; }

        public override void MuisLos(SchetsControl s, Point p)
        {
            var clickedObjects = s.Schets.GetDrawnItems().Where(o => o.elements.Any(e => e.WasClicked(p)));
            if (clickedObjects != null && clickedObjects.Count() > 0)
            {
                var clickedObject = clickedObjects.Last();
                s.Schets.RemoveElement(clickedObject);
                s.RebuildBitmap(this, new EventArgs());
            }
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
        }
    }
}
