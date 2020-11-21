using Newtonsoft.Json;

namespace GTFO.Custom.Rundown.CRundown
{
    public class CDataBlockGUIDObject
    {
        [JsonProperty("_guid")]
        public string GUID { get; set; }
    }
}