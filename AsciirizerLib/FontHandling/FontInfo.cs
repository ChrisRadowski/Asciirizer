using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace AsciirizerLib.FontHandling
{
    public class FontInfo
    {
        public Font Font { get; }

        public int Width { get; }
        public int Height { get; }

        public ReadOnlyDictionary<Bitmap, string> CharacterMap { get; }

        public FontInfo(Font font, Size measure, IDictionary<Bitmap, string> characterMap)
        {
            Font = font;
            Width = measure.Width;
            Height = measure.Height;
            CharacterMap = new ReadOnlyDictionary<Bitmap, string>(characterMap);
        }
    }
}