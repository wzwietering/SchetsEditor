using SchetsEditor.DrawingObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SchetsEditor
{
    public class Schets
    {
        internal List<DrawnItem> drawnItems = new List<DrawnItem>();
        internal Stack<DrawnItem> undoStack = new Stack<DrawnItem>();

        public Bitmap bitmap;
        public SolidBrush background = (SolidBrush)Brushes.White;

        public Schets()
        {
            bitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        }
        public Graphics BitmapGraphics
        {
            get { return Graphics.FromImage(bitmap); }
        }
        public void VeranderAfmeting(Size sz)
        {
            Bitmap nieuw = new Bitmap(sz.Width, sz.Height, PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(nieuw);
            gr.FillRectangle(background, 0, 0, sz.Width, sz.Height);
            gr.DrawImage(bitmap, 0, 0);
            bitmap = nieuw;
        }
        public void Teken(Graphics gr)
        {
            gr.DrawImage(bitmap, 0, 0);
        }
        public void Schoon()
        {
            Graphics gr = BitmapGraphics;
            gr.FillRectangle(background, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        /// <summary>
        /// Export laat een savefiledialog zien en exporteert de afbeelding als de gebruiker dat wil
        /// </summary>
        public void Export()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Afbeelding opslaan";
            sfd.Filter = "PNG|*.png|JPEG|*.jpg|Bitmap Image|*.bmp|GIF|*.gif";
            sfd.FileName = "Nieuwe tekening";

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                ImageFormat f;
                String extension = System.IO.Path.GetExtension(sfd.FileName);
                switch (extension)
                {
                    case ".jpg":
                        f = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        f = ImageFormat.Bmp;
                        break;
                    case ".gif":
                        f = ImageFormat.Gif;
                        break;
                    default:
                        f = ImageFormat.Png;
                        break;
                }
                System.IO.FileStream fs = (System.IO.FileStream)sfd.OpenFile();
                bitmap.Save(fs, f);
                fs.Close();
            }
        }
    }
}
