using System.IO;
using Newtonsoft.Json;

namespace Lib.Helpers
{
    public class FileHelper : IFileHelper
    {
        public void WriteToFile<T>(string filepath, T content)
        {
            var json = JsonConvert.SerializeObject(content, Formatting.Indented);
            File.WriteAllText(filepath, json);
        }
    }
}