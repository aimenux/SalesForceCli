using System.IO;

namespace Lib.Services
{
    public class SalesForceParameters
    {
        public int MaxItems { get; set; }

        public string Pattern { get; set; }

        public string FileName { get; set; }

        public string ObjectName { get; set; }

        public string OutputFileName => GenerateOutputFileName(FileName);

        private static string GenerateOutputFileName(string inputFile)
        {
            var directory = Path.GetDirectoryName(inputFile);
            var filename = Path.GetFileNameWithoutExtension(inputFile);
            return $"{directory}/{filename}-results.txt";
        }
    }
}