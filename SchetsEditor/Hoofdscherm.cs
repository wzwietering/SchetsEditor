using SchetsEditor.IO;
using SchetsEditor.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SchetsEditor
{
    public class Hoofdscherm : Form
    {
        MenuStrip menuStrip;

        /// <summary>
        /// Maakt de form die de gebruiker ziet bij het opstarten
        /// </summary>
        public Hoofdscherm()
        {
            this.ClientSize = new Size(800, 600);
            menuStrip = new MenuStrip();
            this.Controls.Add(menuStrip);
            this.maakFileMenu();
            this.maakHelpMenu();
            this.Text = "Schets editor";
            this.IsMdiContainer = true;
            this.MainMenuStrip = menuStrip;
        }
        /// <summary>
        /// Voegt opties toe aan het menu
        /// </summary>
        private void maakFileMenu()
        {
            ToolStripDropDownItem menu;
            menu = new ToolStripMenuItem("File");
            ToolStripMenuItem n = new ToolStripMenuItem("Nieuw", null, this.nieuw);
            n.ShortcutKeys = Keys.Control | Keys.N;
            menu.DropDownItems.Add(n);
            n = new ToolStripMenuItem("Open", null, this.Open);
            n.ShortcutKeys = Keys.Control | Keys.O;
            menu.DropDownItems.Add(n);
            n = new ToolStripMenuItem("Importeren", null, this.Import);
            n.ShortcutKeys = Keys.Control | Keys.I;
            menu.DropDownItems.Add(n);
            menu.DropDownItems.Add("Sluiten", null, this.afsluiten);
            menuStrip.Items.Add(menu);
        }
        /// <summary>
        /// Maakt een menu informatie
        /// </summary>
        private void maakHelpMenu()
        {
            ToolStripDropDownItem menu;
            menu = new ToolStripMenuItem("Help");
            menu.DropDownItems.Add("Over \"Schets\"", null, this.about);
            menuStrip.Items.Add(menu);
        }
        /// <summary>
        /// Laat een dialoog zien met informatie over de auteurs
        /// </summary>
        private void about(object o, EventArgs ea)
        {
            MessageBox.Show("Schets versie 1.0\n(c) UU Informatica 2010\n\nUitgebreid en aangepast door M. Galama en W. Zwietering"
                           , "Over \"Schets\""
                           , MessageBoxButtons.OK
                           , MessageBoxIcon.Information
                           );
        }

        /// <summary>
        /// Maakt een nieuwe tekening in een apart venster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nieuw(object sender, EventArgs e)
        {
            SchetsWin s = new SchetsWin();
            s.MdiParent = this;
            s.Show();
        }

        /// <summary>
        /// Sluit het venster af
        /// </summary>
        private void afsluiten(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Opent een XML file
        /// </summary>
        private void Open(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Kies een bestand om te openen";
            ofd.Filter = "XML|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SchetsWin s = new SchetsWin();
                s.MdiParent = this;

                Read read = new Read();
                read.ReadXML(ofd.FileName, s);

                s.Show();
            }
        }

        /// <summary>
        /// Opent een afbeelding
        /// </summary>
        private void Import(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Kies een bestand om te openen";
            ofd.Filter = "PNG|*.png|JPEG|*.jpg|Bitmap Image|*.bmp|GIF|*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SchetsWin s = new SchetsWin();
                s.MdiParent = this;
                ImageTool i = new ImageTool();
                i.DrawImage(s.schetscontrol, ofd.FileName);

                s.Show();
            }
        }
    }
}
