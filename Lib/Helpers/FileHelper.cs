using System.IO;
using Lib.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lib.Helpers
{
    public class FileHelper : IFileHelper
    {
        public void WriteToFile<T>(string filepath, T content)
        {
            var json = Serialize(content, Formatting.Indented);
            File.WriteAllText(filepath, json);
        }

        private static string Serialize<T>(T content, Formatting formatting)
        {
            const string name = "attributes";
            var json = JsonConvert.SerializeObject(content);
            var obj = JArray.Parse(json);
            var tokens = obj.FindTokens(name);
            tokens.ForEach(x => x?.Parent?.Remove());
            return JsonConvert.SerializeObject(obj, formatting);
        }
    }
}