using System.Drawing;

namespace AsciirizerLib.FontHandling
{
    public interface IFontInfo
    {
        Font Font { get; }
        Size Measure { get; }
    }
}