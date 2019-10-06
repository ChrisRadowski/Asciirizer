using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AsciirizerLib.FontHandling
{
    public class FontProvider
    {
        private const string ReferenceCharacter = "X";

        public FontInfo GetFontInfo(string fontFamily, float emSize)
        {
            var font = new Font(fontFamily, emSize);
            return GetFontInfo(font);
        }

        public FontInfo GetFontInfo(Font font)
        {
            var measure = CalculateMeasure(font);
            var characterMap = BuildCharacterMap(font, measure);
            return new FontInfo(font, measure, characterMap);
        }
        private Size CalculateMeasure(Font font)
        {
            // TODO: add support for non monospaced fonts
            using (var image = new Bitmap(1, 1))
            using (var graphics = Graphics.FromImage(image))
            {
                return graphics.MeasureString(ReferenceCharacter, font).ToSize();
            }
        }

        private Dictionary<Bitmap, string> BuildCharacterMap(Font font, Size measure)
        {
            var chars = Enumerable.Range(33, 93).Select(c => ((char)c).ToString()).ToList();
            var characterMap = new Dictionary<Bitmap, string>(chars.Count);

            using (var solidBlackBrush = new SolidBrush(Color.Black))
            {
                foreach (var c in chars)
                {
                    var characterBitmap = new Bitmap(measure.Width, measure.Height);
                    using (var graphics = Graphics.FromImage(characterBitmap))
                    {
                        graphics.DrawString(c, font, solidBlackBrush, Point.Empty);
                        graphics.Flush();

                        characterMap.Add(characterBitmap, c);
                    }
                }
            }

            return characterMap;
        }
    }
}
