using System.Drawing;

namespace SchetsEditor
{
    // The pencil tool is like a line tool, except it draws multiple lined while the user drags his mouse around.
    // Thus, the drawnItem of the pencil tool can contain many drawnElements, one for each line segment.
     public class PencilTool : TwoDimensionalTool<Line>
    {
        public override string ToString()
        {
            return "pen";
        }

        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);
        }

        // user drags the mouse. Calling Muislos() and MuisVast() will continuously create new line segments.
        public override void MuisDrag(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
            base.MuisVast(s, p);
        }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
            this.drawnItem.elements.Add(element);
            base.Finalize(s);
        }

        // Empty finalize. We need this because the base.Muislos() will call finalize but we only want to finalize on the this.Muislos() event
        public override void Finalize(SchetsControl s)
        {
            // Do nothing 
        }
    }
}
