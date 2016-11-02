using System.Drawing;

namespace SchetsEditor
{
     public class Pencil : TweepuntTool<Line>
    {
        public override string ToString()
        {
            return "pen";
        }

        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);
        }

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
        public override void Finalize(SchetsControl s)
        {
            // Do nothing 
        }
    }
}
