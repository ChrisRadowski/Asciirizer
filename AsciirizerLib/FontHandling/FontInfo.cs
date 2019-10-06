using System.Drawing;

namespace AsciirizerLib.FontHandling
{
    partial class FontProvider
    {
        private class FontInfo : IFontInfo
        {
            private const char ReferenceChar = 'X';

            public Font Font { get; }
            public Size Measure { get; }

            public FontInfo(Font font)
            {
                Font = font;
                Measure = CalculateMeasure(font).ToSize();
            }

            private SizeF CalculateMeasure(Font font)
            {
                using (var image = new Bitmap(1, 1))
                using (var graphics = Graphics.FromImage(image))
                {
                    return graphics.MeasureString(ReferenceChar.ToString(), font);
                }
            }
        }
    }
}