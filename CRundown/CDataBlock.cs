using GameData;
using GTFO.Custom.Rundown.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace GTFO.Custom.Rundown.CRundown
{
    public class CDataBlock<T> where T : GameDataBlockBase<T>
    {
        private readonly CDataBlockGUIDMapper _Mapper = new CDataBlockGUIDMapper();

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

        private CDataBlock() {}

        public CDataBlock(string[] supportedPathForWrite, string[] supportedPathForRead)
        {
            //_DeserializeSetting.Converters.Add(new StringUintDelegateConverter());
        }

        public T ReadBlock(string content)
        {
            var block = JsonConvert.DeserializeObject<T>(content, _DeserializeSetting);
            var guidObject = JsonConvert.DeserializeObject<CDataBlockGUIDObject>(content);
            var guid = guidObject.GUID;

            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }

            if (_Mapper.TryAdd(guid, out uint id))
            {
                block.persistentID = id;
                return block;
            }
            else
            {
                return null;
            }
        }

        public void AddBlock()
        {
        }
    }
}