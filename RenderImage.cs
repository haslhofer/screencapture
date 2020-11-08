using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace screencapture
{


    public class RenderImage
    {
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