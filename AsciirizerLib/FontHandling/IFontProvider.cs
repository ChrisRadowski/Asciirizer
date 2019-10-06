using System.Drawing;

namespace AsciirizerLib.FontHandling
{
    public interface IFontProvider
    {
        IFontInfo GetFontInfo(string fontFamily, float emSize);
        IFontInfo GetFontInfo(Font font);
    }
}