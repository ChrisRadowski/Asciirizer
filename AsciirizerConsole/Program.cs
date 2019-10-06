using System.IO;
using AsciirizerLib;

namespace AsciirizerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceImagePath = args[0];
            var targetPath = Path.Combine(args[1], $"{Path.GetFileNameWithoutExtension(sourceImagePath)}_asciirized{Path.GetExtension(sourceImagePath)}");

            var asciirizer = new Asciirizer();

            using (var sourceStream = new FileStream(sourceImagePath, FileMode.Open, FileAccess.Read))
            {
                using (var resultStream = asciirizer.Asciirize(sourceStream))
                using (var targetStream = File.Create(targetPath))
                {
                    resultStream.CopyTo(targetStream);
                }
            }
        }
    }
}
