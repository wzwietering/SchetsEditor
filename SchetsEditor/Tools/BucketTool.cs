using System;
using System.Drawing;

namespace SchetsEditor.Tools
{
    public class BucketTool : SelectorTool
    {
        //Deze tool maakt een object een andere kleur
        public override string ToString() { return "emmer"; }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);

            if (this.selectedItem != null)
            {
                DrawnItem temp = selectedItem;
                s.Schets.drawnItems.Remove(selectedItem);
                temp.color = s.penkleur;
                temp.Draw(s);
                s.Schets.drawnItems.Add(temp);
                s.RebuildBitmap(this, new EventArgs());
            }
            else
            {
                s.Schets.background = new SolidBrush(s.penkleur);
                s.Schets.Schoon();
                s.RebuildBitmap(this, new EventArgs());
            }
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
        }
    }
}
