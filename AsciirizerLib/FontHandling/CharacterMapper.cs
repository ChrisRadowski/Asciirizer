using System.Drawing;

namespace AsciirizerLib.FontHandling
{
    public class CharacterMapper
    {
        public string FindBestVisualMatch(FontInfo fontInfo, Bitmap referenceImage)
        {
            // TODO: improve grayscaling color matrix this currently results in 'N' being the best match for all comparisons
            // referenceImage = BitmapTools.ConvertToGrayscale(referenceImage);

            float bestMatchDistance = 0;
            Bitmap bestMatch = null; // TODO: specify a fallback character in the FontInfo class that is used instead of the first literal one

            foreach (var characterBitmap in fontInfo.CharacterMap.Keys)
            {
                var distance = BitmapTools.CalculateDifference(characterBitmap, referenceImage);
                if (bestMatch == null || distance < bestMatchDistance)
                {
                    bestMatchDistance = distance;
                    bestMatch = characterBitmap;
                }
            }

            return fontInfo.CharacterMap[bestMatch];
        }
    }
}
