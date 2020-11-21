using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFO.Custom.Rundown.CRundown
{
    public class CDataBlockGUIDObject
    {
        [JsonProperty("_guid")]
        public string GUID { get; set; }
    }
}
