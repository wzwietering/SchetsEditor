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
        SchetsControl schetscontrol;
        ISchetsTool huidigeTool;
        Panel paneel;
        bool vast;
        ResourceManager resourcemanager
            = new ResourceManager("SchetsEditor.Properties.Resources"
                                 , Assembly.GetExecutingAssembly()
                                 );

        private void veranderAfmeting(object o, EventArgs ea)
        {
            schetscontrol.Size = new Size(this.ClientSize.Width - 70
                                          , this.ClientSize.Height - 50);
            paneel.Location = new Point(64, this.ClientSize.Height - 30);
            schetscontrol.Schets.VeranderAfmeting(schetscontrol.Size);
            schetscontrol.RebuildBitmap(this, new EventArgs());
        }

        private void klikToolMenu(object obj, EventArgs ea)
        {
            this.huidigeTool.Finalize(schetscontrol);
            this.huidigeTool = (ISchetsTool)((ToolStripMenuItem)obj).Tag;
        }

        private void klikToolButton(object obj, EventArgs ea)
        {
            this.huidigeTool.Finalize(schetscontrol);
            this.huidigeTool = (ISchetsTool)((RadioButton)obj).Tag;
        }

        private void afsluiten(object obj, EventArgs ea)
        {
            this.Close();
        }

        private void Export(object obj, EventArgs ea)
        {
            schetscontrol.Schets.Export();
        }

        private void Undo(object obj, EventArgs ea)
        {
            schetscontrol.Undo();
            
        }

        private void Redo(object obj, EventArgs ea)
        {
            schetscontrol.Redo();
        }

        private void Opslaan(object obj, EventArgs ea)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save drawing";
            sfd.Filter = "CSV|*.csv";
            sfd.FileName = "New drawing";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Write write = new Write();
                write.WriteCSV(sfd.FileName, schetscontrol.Schets.drawnItems);
            }
        }

        public SchetsWin()
        {
            ISchetsTool[] deTools = {// new PenTool()
                                     //  , new LijnTool()
                                     //, 
                                      new TweepuntTool<Line>()
                                    , new Pencil()
                                    , new TweepuntTool<FullRectangle>()
                                    , new TweepuntTool<LineRectangle>()
                                    , new TweepuntTool<FullCircle>()
                                    , new TweepuntTool<LineCircle>()
                                    , new TekstTool()
                                    , new GumTool()
                                    };

            this.ClientSize = new Size(700, 500);
            huidigeTool = deTools[0];

            schetscontrol = new SchetsControl();
            schetscontrol.Location = new Point(64, 10);
            schetscontrol.MouseDown += (object o, MouseEventArgs mea) =>
                                       {
                                           vast = true;
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
                                             huidigeTool.MuisLos(schetscontrol, mea.Location);
                                         vast = false;
                                     };
            schetscontrol.KeyPress += (object o, KeyPressEventArgs kpea) =>
                                      {
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

        private void maakAktieMenu()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Aktie");
            menu.DropDownItems.Add("Clear", null, schetscontrol.Schoon);
            menu.DropDownItems.Add("Roteer", null, schetscontrol.Roteer);
            menu.DropDownItems.Add("Kleur", null, schetscontrol.VeranderKleurViaMenu);
            menuStrip.Items.Add(menu);
        }

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

        private void maakAktieButtons()
        {
            paneel = new Panel();
            paneel.Size = new Size(600, 30);
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
            b.Click += schetscontrol.VeranderKleurViaMenu;
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
        }
    }
}
