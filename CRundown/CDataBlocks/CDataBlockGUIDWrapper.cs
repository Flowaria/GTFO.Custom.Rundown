using Newtonsoft.Json;

namespace GTFO.Custom.Rundown.CRundown.CDataBlocks
{
    public class CDataBlockGUIDWrapper
    {
        [JsonProperty("persistentGUID")]
        public string GUID { get; set; }
    }
}