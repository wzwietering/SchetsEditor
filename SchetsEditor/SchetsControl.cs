using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchetsEditor
{
    public class SchetsControl : UserControl
    {
        private Schets schets;
        private Color penkleur = Color.Black;

        public Color PenKleur
        {
            get { return penkleur; }
        }
        public Schets Schets
        {
            get { return schets; }
        }

        public int lijnDikte = 3;

        public SchetsControl()
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        private void teken(object o, PaintEventArgs pea)
        {
            schets.Teken(pea.Graphics);
        }
        private void veranderAfmeting(object o, EventArgs ea)
        {
            schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }
        public Graphics MaakBitmapGraphics()
        {
            Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }
        public void Schoon(object o, EventArgs ea)
        {
            schets.Schoon();
            this.Invalidate();
        }
        public void Roteer(object o, EventArgs ea)
        {
            schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
            schets.Roteer();
            this.Invalidate();
        }
        public void VeranderKleur(object obj, EventArgs ea)
        {
            string kleurNaam = ((ComboBox)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }
        public void VeranderKleurViaMenu(object obj, EventArgs ea)
        {
            ColorDialog kleurDialoog = new ColorDialog();
            DialogResult resultaat = kleurDialoog.ShowDialog();
            if(resultaat == DialogResult.OK)
            {
                penkleur = kleurDialoog.Color;
            }
        }

        public void RebuildBitmap(object sender, EventArgs e)
        {
            this.Schets.Schoon();
            this.Invalidate();

            List<DrawnItem> items = schets.GetDrawnItems();

            var color = this.penkleur;
            foreach (DrawnItem item in items)
            {
                if (item != null)
                {
                    item.Draw(this);
                }
            }
        }

        public void VeranderBrush(object obj, EventArgs ea)
        {
            lijnDikte = ((TrackBar)obj).Value;
        }
    }
}
