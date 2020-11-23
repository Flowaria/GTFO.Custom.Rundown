using GameData;
using GTFO.Custom.Rundown.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace GTFO.Custom.Rundown.CRundown.CDataBlocks
{
    public class CDataBlock<T> where T : GameDataBlockBase<T>
    {
        public CDataBlockGUIDMapper Mapper { get; private set; } = new CDataBlockGUIDMapper();

        private readonly JsonSerializerSettings _DeserializeSetting = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>
            {
                new ColorConverter(),
                new StringEnumConverter(),
                new Il2CppListReadOnlyConverter()
            }
        };

        //DON'T USE DEFAULT CTOR
        private CDataBlock() { }

        /// <summary>
        /// ctor for CustomDataBlock
        /// </summary>
        /// <param name="guidContexts">Tuple array for GUID context</param>
        public CDataBlock((string[] allowedPath, CDataBlockGUIDMapper contextMapper)[] guidContexts = null)
        {
            if (guidContexts == null)
            {
                return;
            }

            //_DeserializeSetting.Converters.Add(new StringUintDelegateConverter());
        }

        /// <summary>
        /// Load DataBlock from JSON text
        /// </summary>
        /// <param name="content">JSON text content</param>
        /// <returns></returns>
        public T LoadBlock(string content)
        {
            var block = JsonConvert.DeserializeObject<T>(content, _DeserializeSetting);
            var guidObject = JsonConvert.DeserializeObject<CDataBlockGUIDWrapper>(content);
            var guid = guidObject.GUID;

            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }

            if (Mapper.TryAdd(guid, out uint id))
            {
                block.persistentID = id;
                GameDataBlockBase<T>.AddBlock(block, -1);
                return block;
            }
            else
            {
                return null;
            }
        }
    }
}