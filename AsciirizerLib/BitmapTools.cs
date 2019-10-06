using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AsciirizerLib
{
    public static class BitmapTools
    {
        public static Bitmap ConvertToGrayscale(Bitmap original)
        {
            //create the grayscale ColorMatrix
            var colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
               }
            );

            var grayscaleBitmap = new Bitmap(original.Width, original.Height);
            using (var graphics = Graphics.FromImage(grayscaleBitmap))
            {
                var attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                var drawingArea = new Rectangle(0, 0, original.Width, original.Height);
                graphics.DrawImage(original, drawingArea, 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }

            return grayscaleBitmap;
        }

        public static float CalculateDifference(Bitmap a, Bitmap b)
        {
            if (a.Size != b.Size)
            {
                return -1;
            }

            var bounds = new Rectangle(0, 0, a.Width, a.Height);
            var dataA = a.LockBits(bounds, ImageLockMode.ReadOnly, a.PixelFormat);
            var dataB = b.LockBits(bounds, ImageLockMode.ReadOnly, b.PixelFormat);

            var diff = 0.0f;
            var byteCount = bounds.Width * bounds.Height * Image.GetPixelFormatSize(a.PixelFormat) / 8;

            unsafe
            {
                var scanPointerA = (byte*)dataA.Scan0.ToPointer();
                var scanPointerB = (byte*)dataB.Scan0.ToPointer();

                for (var idx = 0; idx < byteCount; idx++)
                {
                    diff += (float)Math.Abs(*scanPointerA - *scanPointerB) / 255;
                    scanPointerA++;
                    scanPointerB++;
                }
            }

            a.UnlockBits(dataA);
            b.UnlockBits(dataB);

            return diff / byteCount;
        }
    }
}
