using System.Drawing;

namespace SchetsEditor
{
    // On dimensional tool has a starting point and that's it.
    public abstract class OneDimensionalTool : ISchetsTool
    {
        protected Brush kwast;

        protected DrawnItem drawnItem = new DrawnItem();

        public virtual void MuisVast(SchetsControl s, Point p)
        {
            drawnItem.color = s.PenKleur;
        }
        public virtual void MuisLos(SchetsControl s, Point p)
        {
            kwast = new SolidBrush(s.PenKleur);
        }

        public virtual void Finalize(SchetsControl s)
        {
            if (this.drawnItem != null && this.drawnItem.elements != null && this.drawnItem.elements.Count > 0)
            {
                s.Schets.AddElement(this.drawnItem);
            }
            drawnItem = new DrawnItem();
        }

        public abstract void MuisDrag(SchetsControl s, Point p);
        public virtual void Letter(SchetsControl s, char c) { }
    }
}
