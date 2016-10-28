using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SchetsEditor
{
    public class Schets
    {
        private List<DrawnItem> objects = new List<DrawnItem>();
        private Bitmap bitmap;
        
        public Schets()
        {
            bitmap = new Bitmap(1, 1);
        }
        public Graphics BitmapGraphics
        {
            get { return Graphics.FromImage(bitmap); }
        }
        public void VeranderAfmeting(Size sz)
        {
            if (sz.Width > bitmap.Size.Width || sz.Height > bitmap.Size.Height)
            {
                Bitmap nieuw = new Bitmap( Math.Max(sz.Width,  bitmap.Size.Width)
                                         , Math.Max(sz.Height, bitmap.Size.Height)
                                         );
                Graphics gr = Graphics.FromImage(nieuw);
                gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
                gr.DrawImage(bitmap, 0, 0);
                bitmap = nieuw;
            }
        }
        public void Teken(Graphics gr)
        {
            gr.DrawImage(bitmap, 0, 0);
        }
        public void Schoon()
        {
            Graphics gr = BitmapGraphics;
            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        public void Opslaan()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Image";
            sfd.Filter = "PNG|*.png|JPEG|*.jpg|Bitmap Image|*.bmp|GIF|*.gif|";
            sfd.FileName = "New image";
            sfd.ShowDialog();

            try
            {
                System.IO.FileStream fs = (System.IO.FileStream)sfd.OpenFile();
                bitmap.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
            }
            //When there is no image, this error is thrown
            catch (NullReferenceException nre)
            {
                MessageBox.Show("No image available\n" + nre);
            }
            //This error is thrown when the user cancels the save
            catch (IndexOutOfRangeException i) { }
        }

        internal void AddElement(DrawnItem objects)
        {
            if (objects.toolType != typeof(GumTool))
            {
                this.objects.Add(objects);
            }
        }

        internal void ResetAllObjects()
        {
            this.objects.Clear();
        }

        internal List<DrawnItem> GetObjects()
        {
            return this.objects;
        }

        internal void RemoveElement(DrawnItem clickedObject)
        {
            this.objects.Remove(clickedObject);
        }
    }
}
