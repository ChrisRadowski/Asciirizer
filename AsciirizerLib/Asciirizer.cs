using AsciirizerLib.FontHandling;
using System.Drawing;
using System.IO;

namespace AsciirizerLib
{
    public class Asciirizer
    {
        private const string DefaultFontFamily = "Calibri";
        private const float DefaultFontSize = 8;

        private readonly IFontProvider _fontProvider;

        public Asciirizer()
        {
            _fontProvider = new FontProvider();
        }

        public Stream Asciirize(Stream sourceStream) => Asciirize(sourceStream, _fontProvider.GetFontInfo(DefaultFontFamily, DefaultFontSize));

        public Stream Asciirize(Stream sourceStream, string fontFamily, float emSize) => Asciirize(sourceStream, _fontProvider.GetFontInfo(fontFamily, emSize));

        public Stream Asciirize(Stream sourceStream, Font font) => Asciirize(sourceStream, _fontProvider.GetFontInfo(font));

        private Stream Asciirize(Stream sourceStream, IFontInfo fontInfo)
        {
            var widthStep = fontInfo.Measure.Width;
            var heightStep = fontInfo.Measure.Height;

            using (var sourceImage = new Bitmap(sourceStream))
            using (var targetImage = new Bitmap(sourceImage.Width, sourceImage.Height))
            using (var targetGraphics = Graphics.FromImage(targetImage))
            {
                var graphicsUnit = GraphicsUnit.Pixel;
                var sourceBounds = sourceImage.GetBounds(ref graphicsUnit);

                targetGraphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(Point.Empty, targetImage.Size));
                for (var widthPos = 0; widthPos < sourceImage.Width; widthPos += widthStep)
                {
                    for (var heightPos = 0; heightPos < sourceImage.Height; heightPos += heightStep)
                    {
                        var rect = new Rectangle(widthPos, heightPos, widthStep, heightStep);
                        if (!sourceBounds.Contains(rect))
                        {
                            break;
                        }

                        using (var snippet = sourceImage.Clone(rect, sourceImage.PixelFormat))
                        {
                            // find character 
                            var centerColor = snippet.GetPixel(widthStep / 2, heightStep / 2);
                            targetGraphics.DrawString("X", fontInfo.Font, new SolidBrush(centerColor), widthPos, heightPos);
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
