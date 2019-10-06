using AsciirizerLib.FontHandling;
using System.Drawing;
using System.IO;

namespace AsciirizerLib
{
    public class Asciirizer
    {
        private const string DefaultFontFamily = "Courier";
        private const float DefaultFontSize = 8;

        private readonly FontProvider _fontProvider;
        private readonly CharacterMapper _characterMapper;
        public Asciirizer()
        {
            _fontProvider = new FontProvider();
            _characterMapper = new CharacterMapper();
        }

        public Stream Asciirize(Stream sourceStream) => Asciirize(sourceStream, _fontProvider.GetFontInfo(DefaultFontFamily, DefaultFontSize));

        public Stream Asciirize(Stream sourceStream, string fontFamily, float emSize) => Asciirize(sourceStream, _fontProvider.GetFontInfo(fontFamily, emSize));

        public Stream Asciirize(Stream sourceStream, Font font) => Asciirize(sourceStream, _fontProvider.GetFontInfo(font));

        private Stream Asciirize(Stream sourceStream, FontInfo fontInfo)
        {
            using (var sourceImage = new Bitmap(sourceStream))
            using (var targetImage = new Bitmap(sourceImage.Width, sourceImage.Height))
            using (var targetGraphics = Graphics.FromImage(targetImage))
            {
                var graphicsUnit = GraphicsUnit.Pixel;
                var sourceBounds = sourceImage.GetBounds(ref graphicsUnit);

                targetGraphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(Point.Empty, targetImage.Size));
                for (var widthPos = 0; widthPos < sourceImage.Width; widthPos += fontInfo.Width)
                {
                    for (var heightPos = 0; heightPos < sourceImage.Height; heightPos += fontInfo.Height)
                    {
                        var rect = new Rectangle(widthPos, heightPos, fontInfo.Width, fontInfo.Height);
                        if (!sourceBounds.Contains(rect))
                        {
                            break;
                        }

                        using (var snippet = sourceImage.Clone(rect, sourceImage.PixelFormat))
                        {

                            var matchingCharacter = _characterMapper.FindBestVisualMatch(fontInfo, snippet);
                            var centerColor = snippet.GetPixel(fontInfo.Width / 2, fontInfo.Height / 2);
                            targetGraphics.DrawString(matchingCharacter, fontInfo.Font, new SolidBrush(centerColor), widthPos, heightPos);
                        }
                    }
                }

                var resultStream = new MemoryStream();
                targetGraphics.Flush();
                targetImage.Save(resultStream, sourceImage.RawFormat);

                resultStream.Seek(0, SeekOrigin.Begin);
                return resultStream;
            }
        }
    }
}
