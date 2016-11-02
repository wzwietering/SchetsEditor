﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SchetsEditor
{
    public class Schets
    {
        private List<DrawnItem> objects = new List<DrawnItem>();
        private Bitmap bitmap;

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
            Bitmap nieuw = new Bitmap(sz.Width, sz.Height);
            Graphics gr = Graphics.FromImage(nieuw);
            gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
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
            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        /// <summary>
        /// Export shows a savefiledialog to save the image as an image file
        /// </summary>
        public void Export()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Image";
            sfd.Filter = "PNG|*.png|JPEG|*.jpg|Bitmap Image|*.bmp|GIF|*.gif";
            sfd.FileName = "New image";
            ImageFormat f = ImageFormat.Png;

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                String extension = System.IO.Path.GetExtension(sfd.FileName);
                switch (extension)
                {
                    case ".png":
                        f = ImageFormat.Png;
                        break;
                    case ".jpg":
                        f = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        f = ImageFormat.Bmp;
                        break;
                    case ".gif":
                        f = ImageFormat.Gif;
                        break;
                }
                System.IO.FileStream fs = (System.IO.FileStream)sfd.OpenFile();
                bitmap.Save(fs, f);
            }
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
