using Newtonsoft.Json;

namespace Lib.Services
{
    public class SalesForceObject
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
    }
}