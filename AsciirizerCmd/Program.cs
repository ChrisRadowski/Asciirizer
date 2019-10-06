using AsciirizerLib;
using System;
using System.IO;

namespace AsciirizerCmd
{
    class Program
    {
        /// <summary>
        /// Command-line runner for Asccirizer library
        /// </summary>
        /// <param name="args">
        /// [0] Path to source image
        /// [1] Path to output directory
        /// </param>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("No arguments provided");
                Console.Out.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} sourceImagePath [targetDirectoryPath]");
                Environment.Exit(1);
            }

            var sourceImagePath = GetSourceImagePath();
            if (!File.Exists(sourceImagePath))
            {
                Console.Error.WriteLine("Source image does not exist");
                Environment.Exit(1);
            }

            var targetImageDirectory = TryGetTargetImageDirectory(out var directory) ? directory : Path.GetDirectoryName(sourceImagePath);
            var targetPath = Path.Combine(targetImageDirectory, GetSuffixedFileName(sourceImagePath));
            if (!Directory.Exists(targetImageDirectory))
            {
                Directory.CreateDirectory(targetImageDirectory);
            }

            var asciirizer = new Asciirizer();
            using (var sourceStream = new FileStream(sourceImagePath, FileMode.Open, FileAccess.Read))
            {
                using (var resultStream = asciirizer.Asciirize(sourceStream))
                using (var targetStream = File.Create(targetPath))
                {
                    resultStream.CopyTo(targetStream);
                }
            }

            string GetSourceImagePath() => args[0];

            bool TryGetTargetImageDirectory(out string directory)
            {
                if (args.Length > 1)
                {
                    directory = args[1];
                    return true;
                }

                directory = null;
                return false;
            }
        }

        private static string GetSuffixedFileName(string originalFileName)
        {
            return $"{Path.GetFileNameWithoutExtension(originalFileName)}_asciirized{Path.GetExtension(originalFileName)}";
        }
    }
}
