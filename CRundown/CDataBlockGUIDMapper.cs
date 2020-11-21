using System.Collections.Generic;

namespace GTFO.Custom.Rundown.CRundown
{
    public class CDataBlockGUIDMapper
    {
        private uint _Offset = 50000u;
        private readonly Dictionary<string, uint> _Map = new Dictionary<string, uint>();

        public bool TryAdd(string guid, out uint id)
        {
            if (_Map.ContainsKey(guid))
            {
                _Map.Add(guid, _Offset++);
                id = _Offset - 1;
                return true;
            }

            id = 0;
            return false;
        }

        public bool TryGet(string guid, out uint id)
        {
            if (_Map.ContainsKey(guid))
            {
                id = _Map[guid];
                return true;
            }

            id = 0;
            return false;
        }
    }
}