using SchetsEditor.DrawingObjects;
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
        public Color penkleur = Color.Black;

        public Color PenKleur
        {
            get { return penkleur; }
        }
        public Schets Schets
        {
            get { return schets; }
        }

        public int lijnDikte = 3;

        /// <summary>
        /// Maakt een schetscontrol. Een schetscontrol voert alle acties die met de tekening te maken hebben uit.
        /// </summary>
        public SchetsControl()
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }
        /// <summary>
        /// Eventhandler voor tekenen
        /// </summary>
        private void teken(object o, PaintEventArgs pea)
        {
            schets.Teken(pea.Graphics);
        }
        /// <summary>
        /// Eventhandler die de afmetingen van een afbeelding aanpast
        /// </summary>
        private void veranderAfmeting(object o, EventArgs ea)
        {
            schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }
        /// <summary>
        /// Maakt de graphics van een bitmap
        /// </summary>
        /// <returns>Het graphics object</returns>
        public Graphics MaakBitmapGraphics()
        {
            Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }
        /// <summary>
        /// Maakt de afbeelding leeg
        /// </summary>
        public void Schoon(object o, EventArgs ea)
        {
            schets.Schoon();
            this.Invalidate();
        }
        /// <summary>
        /// Draait de afbeelding
        /// </summary>
        public void Roteer(object o, EventArgs ea)
        {
            schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
            schets.Roteer();
            this.Invalidate();
        }

        /// <summary>
        /// Voert een ongedane actie opnieuw uit
        /// </summary>
        internal void Redo()
        {
            if (schets.undoStack.Count > 0)
            {
                DrawnItem item = schets.undoStack.Pop();
                schets.drawnItems.Add(item);
                this.RebuildBitmap(this, new EventArgs());
            }
        }

        /// <summary>
        /// Maakt een actie ongedaan
        /// </summary>
        internal void Undo()
        {
            if (schets.drawnItems.Count > 0)
            {
                schets.undoStack.Push(schets.drawnItems[schets.drawnItems.Count - 1]);
                schets.drawnItems.Remove(schets.drawnItems[schets.drawnItems.Count - 1]);
                this.RebuildBitmap(this, new EventArgs());
            }
        }

        public void VeranderKleur(object obj, EventArgs ea)
        {
            ColorDialog kleurDialoog = new ColorDialog();
            DialogResult resultaat = kleurDialoog.ShowDialog();
            if (resultaat == DialogResult.OK)
            {
                penkleur = kleurDialoog.Color;
            }
        }

        /// <summary>
        /// Deze methode verwijdert alles op de bitmap en tekent hem dan opnieuw
        /// </summary>
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
