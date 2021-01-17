using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

namespace screencapture
{


    public class RenderImage
    {

        public static void SetBoundaryRect(Bitmap bmp, int x1, int y1, int x2, int y2 )
        {
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                
                Pen p = new Pen(new SolidBrush(Color.Red), 4);
                graph.DrawRectangle(p, x1, y1, x2-x1, y2-y1);
                graph.Flush();
            }

        }

        public static void SetMarkerAtPosition(Bitmap bmp, int x, int y)
        {
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                graph.DrawRectangle(Pens.Red, x,y,10,10);
                graph.Flush();
            }

        }
        public static Bitmap BitmapFromBmpByteArray(byte[] data)
        {
            MemoryStream m = new MemoryStream(data);

            Bitmap r = (Bitmap)Bitmap.FromStream(m);
            return r;
        }

        public static Bitmap GetWhiteBitmap(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(Brushes.White, ImageSize);
            }
            return bmp;
        }

        public static Bitmap TrimBitmap(Bitmap master, Rectangle r)
        {
            Bitmap bmp = new Bitmap(r.Width, r.Height);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                graph.DrawImage(master, (float)0, (float)0, r, GraphicsUnit.Pixel);
                graph.Flush();
            }
            return bmp;


        }

        public static Image HorizontalConcatenate(List<Image> images, double desiredWidth)
        {
            //Images have different ratios
            //First determine width each image would have if scaled to the same height
            double desiredHeight = 200;
            double totalScaledWidth = 0;
            foreach (var i in images)
            {
                //Assume each image has fixed heigth of 200
                double scaledWidth = (desiredHeight / (double)i.Height) * (double)i.Width;
                totalScaledWidth += scaledWidth;
            }

            double xScale = desiredWidth / totalScaledWidth;
            double finalHeight = desiredHeight * xScale;

            //Now we know that if we scale each image to finalHeight, the sum of all width will equate desiredWidth

            Bitmap bmp = new Bitmap((int)desiredWidth, (int)finalHeight);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                double curX = 0;
                foreach (var i in images)
                {
                    double newWidth = finalHeight / (double)i.Height * (double)i.Width;
                    graph.DrawImage(i, (int)curX, 0, (int)newWidth, (int)finalHeight);
                    curX += newWidth;
                }

                graph.Flush();
            }

            return bmp;

        }
        public static void RenderBitmapFromTextSnippets(Bitmap bmp, List<ScreenText> snippets)
        {

            // Create a rectangle for the entire bitmap

            // Create graphic object that will draw onto the bitmap
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            // Create string formatting options (used for alignment)
            StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            };

            foreach (var snip in snippets)
            {
                RectangleF rectf = new RectangleF(snip.Position.Left,
                                                    snip.Position.Top,
                                                    snip.Position.Right - snip.Position.Left,
                                                    snip.Position.Bottom - snip.Position.Top);

                Rectangle rect = new Rectangle(snip.Position.Left,
                snip.Position.Top,
                snip.Position.Right - snip.Position.Left,
                snip.Position.Bottom - snip.Position.Top);



                // Draw the text onto the image
                g.DrawString(snip.Content, new Font("Tahoma", 6), Brushes.Black, rectf, format);


                g.DrawRectangle(Pens.Red, rect);
            }
            // Flush all graphics changes to the bitmap
            g.Flush();

        }
    }
}