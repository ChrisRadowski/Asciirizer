using System.Drawing;

namespace AsciirizerLib.FontHandling
{
    internal partial class FontProvider : IFontProvider
    {
        public IFontInfo GetFontInfo(string fontFamily, float emSize)
        {
            var font = new Font(fontFamily, emSize);
            return GetFontInfo(font);
        }

        public IFontInfo GetFontInfo(Font font) => new FontInfo(font);
    }
}
