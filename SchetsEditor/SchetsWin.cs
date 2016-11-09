using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using SchetsEditor.IO;

namespace SchetsEditor
{
    public class SchetsWin : Form
    {
        MenuStrip menuStrip;
        public SchetsControl schetscontrol;
        ISchetsTool huidigeTool;
        Panel paneel;
        bool vast;
        ResourceManager resourcemanager
            = new ResourceManager("SchetsEditor.Properties.Resources"
                                 , Assembly.GetExecutingAssembly()
                                 );
        bool unsavedChanges = false;

        /// <summary>
        /// Geeft een schetscontrol nieuwe afmetingen
        /// </summary>
        private void veranderAfmeting(object o, EventArgs ea)
        {
            schetscontrol.Size = new Size(this.ClientSize.Width - 70
                                          , this.ClientSize.Height - 50);
            paneel.Location = new Point(64, this.ClientSize.Height - 30);
            schetscontrol.Schets.VeranderAfmeting(schetscontrol.Size);
            schetscontrol.RebuildBitmap(this, new EventArgs());
        }

        /// <summary>
        /// Voert de selectie van een nieuwe tool via het menu uit
        /// </summary>
        private void klikToolMenu(object obj, EventArgs ea)
        {
            this.huidigeTool.Finalize(schetscontrol);
            this.huidigeTool = (ISchetsTool)((ToolStripMenuItem)obj).Tag;
        }

        /// <summary>
        /// Voert de selectie van een nieuwe tool via een knop uit
        /// </summary>
        private void klikToolButton(object obj, EventArgs ea)
        {
            this.huidigeTool.Finalize(schetscontrol);
            this.huidigeTool = (ISchetsTool)((RadioButton)obj).Tag;
        }

        /// <summary>
        /// Voorkomt dat de form gesloten wordt als er wijzigingen zijn.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (unsavedChanges)
            {
                if (MessageBox.Show("Er zijn onopgeslagen veranderingen, weet u zeker dat u de tekening wilt sluiten?", "Bevestiging", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Sluit de form af
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ea"></param>
        private void afsluiten(object obj, EventArgs ea)
        {
            OnFormClosing(new FormClosingEventArgs(CloseReason.UserClosing, false));
        }

        /// <summary>
        /// Exporteerd de afbeelding
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ea"></param>
        private void Export(object obj, EventArgs ea)
        {
            schetscontrol.Schets.Export();
        }

        /// <summary>
        /// Eventhandler voor een undo via het menu of knop
        /// </summary>
        private void Undo(object obj, EventArgs ea)
        {
            unsavedChanges = true;
            schetscontrol.Undo();
        }

        /// <summary>
        /// Eventhandler voor een redo via het menu of knop
        /// </summary>
        private void Redo(object obj, EventArgs ea)
        {
            unsavedChanges = true;
            schetscontrol.Redo();
        }

        /// <summary>
        /// Laat een savefiledialog zien en slaat de afbeelding op.
        /// </summary>
        private void Opslaan(object obj, EventArgs ea)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save drawing";
            sfd.Filter = "XML|*.xml";
            sfd.FileName = "New drawing";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                huidigeTool.Finalize(schetscontrol);
                unsavedChanges = false;
                Write write = new Write();
                write.WriteXML(sfd.FileName, schetscontrol.Schets.drawnItems);
            }
        }

        /// <summary>
        /// Maakt een nieuw venster met een tekening
        /// </summary>
        public SchetsWin()
        {
            ISchetsTool[] deTools = { new TwoDimensionalTool<Line>()
                                    , new PencilTool()
                                    , new TwoDimensionalTool<FullRectangle>()
                                    , new TwoDimensionalTool<LineRectangle>()
                                    , new TwoDimensionalTool<FullCircle>()
                                    , new TwoDimensionalTool<LineCircle>()
                                    , new TekstTool()
                                    , new GumTool()
                                    , new SortTool() {directionUp = true }
                                    , new SortTool() {directionUp = false }
                                    };

            this.ClientSize = new Size(700, 600);
            this.WindowState = FormWindowState.Maximized;
            huidigeTool = deTools[0];

            schetscontrol = new SchetsControl();
            schetscontrol.Location = new Point(64, 10);
            schetscontrol.MouseDown += (object o, MouseEventArgs mea) =>
                                       {
                                           vast = true;
                                           unsavedChanges = true;
                                           huidigeTool.MuisVast(schetscontrol, mea.Location);
                                       };
            schetscontrol.MouseMove += (object o, MouseEventArgs mea) =>
                                       {
                                           if (vast)
                                               huidigeTool.MuisDrag(schetscontrol, mea.Location);
                                       };
            schetscontrol.MouseUp += (object o, MouseEventArgs mea) =>
                                     {
                                         if (vast)
                                         {
                                             huidigeTool.MuisLos(schetscontrol, mea.Location);
                                             unsavedChanges = true;
                                         }
                                         vast = false;
                                     };
            schetscontrol.KeyPress += (object o, KeyPressEventArgs kpea) =>
                                      {
                                          unsavedChanges = true;
                                          huidigeTool.Letter(schetscontrol, kpea.KeyChar);
                                      };
            this.Controls.Add(schetscontrol);

            menuStrip = new MenuStrip();
            menuStrip.Visible = false;
            this.Controls.Add(menuStrip);
            this.maakFileMenu();
            this.maakToolMenu(deTools);
            this.maakAktieMenu();
            this.maakToolButtons(deTools);
            this.maakAktieButtons();
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }

        /// <summary>
        /// Toevoeging van vele mogelijkheden in het menu
        /// </summary>
        private void maakFileMenu()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("File");
            menu.MergeAction = MergeAction.MatchOnly;
            ToolStripMenuItem tsm = new ToolStripMenuItem("Undo", null, this.Undo);
            tsm.ShortcutKeys = Keys.Control | Keys.Z;
            menu.DropDownItems.Add(tsm);
            tsm = new ToolStripMenuItem("Redo", null, this.Redo);
            tsm.ShortcutKeys = Keys.Control | Keys.Y;
            menu.DropDownItems.Add(tsm);
            tsm = new ToolStripMenuItem("Opslaan", null, this.Opslaan);
            tsm.ShortcutKeys = Keys.Control | Keys.S;
            menu.DropDownItems.Add(tsm);
            tsm = new ToolStripMenuItem("Exporteren", null, this.Export);
            tsm.ShortcutKeys = Keys.Control | Keys.E;
            menu.DropDownItems.Add(tsm);
            menu.DropDownItems.Add("Sluit tekening", null, this.afsluiten);
            menuStrip.Items.Add(menu);
        }

        /// <summary>
        /// Maakt een menu met alle tools er in
        /// </summary>
        /// <param name="tools"></param>
        private void maakToolMenu(ICollection<ISchetsTool> tools)
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Tool");
            foreach (ISchetsTool tool in tools)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Tag = tool;
                item.Text = tool.ToString();
                item.Image = (Image)resourcemanager.GetObject(tool.ToString());
                item.Click += this.klikToolMenu;
                menu.DropDownItems.Add(item);
            }
            menuStrip.Items.Add(menu);
        }

        /// <summary>
        /// Maakt een menu met alle acties er in
        /// </summary>
        private void maakAktieMenu()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Aktie");
            menu.DropDownItems.Add("Clear", null, schetscontrol.Schoon);
            menu.DropDownItems.Add("Roteer", null, schetscontrol.Roteer);
            menu.DropDownItems.Add("Kleur", null, schetscontrol.VeranderKleur);
            menu.DropDownItems.Add("Herteken", null, schetscontrol.RebuildBitmap);
            menuStrip.Items.Add(menu);
        }

        /// <summary>
        /// Maakt knoppen voor alle tools
        /// </summary>
        /// <param name="tools">De tools die een knop nodig hebben</param>
        private void maakToolButtons(ICollection<ISchetsTool> tools)
        {
            int t = 0;
            foreach (ISchetsTool tool in tools)
            {
                RadioButton b = new RadioButton();
                b.Appearance = Appearance.Button;
                b.Size = new Size(45, 62);
                b.Location = new Point(10, 10 + t * 62);
                b.Tag = tool;
                b.Text = tool.ToString();
                b.Image = (Image)resourcemanager.GetObject(tool.ToString());
                b.TextAlign = ContentAlignment.TopCenter;
                b.ImageAlign = ContentAlignment.BottomCenter;
                b.Click += this.klikToolButton;
                this.Controls.Add(b);
                if (t == 0) b.Select();
                t++;
            }
        }

        /// <summary>
        /// Maakt de knoppen onder aan het venster
        /// </summary>
        private void maakAktieButtons()
        {
            paneel = new Panel();
            paneel.Size = new Size(640, 30);
            this.Controls.Add(paneel);

            Button b = new Button();
            b.Text = "Clear";
            b.Location = new Point(0, 0);
            b.Click += schetscontrol.Schoon;
            paneel.Controls.Add(b);

            b = new Button();
            b.Text = "Rotate";
            b.Location = new Point(80, 0);
            b.Click += schetscontrol.Roteer;
            paneel.Controls.Add(b);

            b = new Button();
            b.Text = "Kleur";
            b.Location = new Point(160, 0);
            b.Click += schetscontrol.VeranderKleur;
            paneel.Controls.Add(b);

            Label l = new Label();
            l.Text = "Lijn dikte:";
            l.Location = new Point(240, 5);
            l.Size = new Size(60, 20);
            paneel.Controls.Add(l);

            TrackBar t = new TrackBar();
            t.Minimum = 1;
            t.Maximum = 30;
            t.TickFrequency = 3;
            t.LargeChange = 3;
            t.SmallChange = 1;
            t.Location = new Point(300, 0);
            t.Size = new Size(80, 20);
            t.TickStyle = TickStyle.BottomRight;
            t.ValueChanged += schetscontrol.VeranderBrush;
            paneel.Controls.Add(t);

            b = new Button();
            b.Text = "Herteken";
            b.Location = new Point(380, 0);
            b.Click += schetscontrol.RebuildBitmap;
            paneel.Controls.Add(b);

            b = new Button();
            b.Text = "Undo";
            b.Location = new Point(460, 0);
            b.Click += this.Undo;
            paneel.Controls.Add(b);

            b = new Button();
            b.Text = "Redo";
            b.Location = new Point(540, 0);
            b.Click += this.Redo;
            paneel.Controls.Add(b);
        }
    }
}
