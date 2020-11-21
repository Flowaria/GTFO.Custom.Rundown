using GameData;
using GTFO.Custom.Rundown.Converters;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GTFO.Custom.Rundown.CRundown
{
    using Il2CppCollections = Il2CppSystem.Collections.Generic;

    public struct IDChangePair
    {
        public uint Previous;
        public uint New;
    }

    public struct RundownChangePair
    {
        public IDChangePair[] FogSettings;
        public IDChangePair[] LightSettings;
        public IDChangePair[] Objectives;
        public IDChangePair[] Layouts;
    }

    public static partial class DataBlocksDissolver
    {
        private static uint _LightOffset = 200000u;
        private static uint _PuzzleOffset = 200000u;
        private static uint _LayoutOffset = 200000u;

        private static uint _FogOffset = 200000u;
        private static uint _WardenOffset = 200000u;
        private static uint _RundownOffset = 200000u;

        public static JsonSerializerSettings GameDataSettingBase
        {
            get
            {
                return new JsonSerializerSettings()
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
            }
        }

        private static IDChangePair DissolveBase<T>(string content, uint offset) where T : GameDataBlockBase<T>
        {
            try
            {
                var setting = GameDataSettingBase;
                
                var block = JsonConvert.DeserializeObject<T>(content, setting);

                if (GameDataBlockBase<T>.HasBlock(block.persistentID))
                {
                    throw new InvalidDataException($"Data already exist for ID: {block.persistentID} ({typeof(T).Name})");
                }

                var change = new IDChangePair()
                {
                    Previous = block.persistentID,
                    New = offset
                };

                block.persistentID = change.New;

                GameDataBlockBase<T>.AddBlock((T)block, -1);
                return change;
            }
            catch(Exception e)
            {
                MelonLogger.Log(e.ToString());
                return default;
            }
        }

        private static IDChangePair DissolveChainedPuzzle(string content)
        {
            return DissolveBase<ChainedPuzzleDataBlock>(content, _PuzzleOffset++);
        }

        private static IDChangePair DissolveLightSetting(string content)
        {
            return DissolveBase<LightSettingsDataBlock>(content, _LightOffset++);
        }

        private static IDChangePair DissolveWardenObjective(string content)
        {
            return DissolveBase<WardenObjectiveDataBlock>(content, _WardenOffset++);
        }

        private static IDChangePair DissolveFogSetting(string content)
        {
            return DissolveBase<FogSettingsDataBlock>(content, _FogOffset++);
        }

        private static IDChangePair DissolveLayout(string content, IDChangePair[] lightChange, IDChangePair[] puzzleChange)
        {
            var change = DissolveBase<LevelLayoutDataBlock>(content, _LayoutOffset++);
            var block = LevelLayoutDataBlock.GetBlock(change.New);

            foreach(var zone in block.Zones)
            {
                ApplyChangedPair(puzzleChange, zone.ChainedPuzzleToEnter, (uint newID) =>
                {
                    zone.ChainedPuzzleToEnter = newID;
                });

                ApplyChangedPair(lightChange, zone.LightSettings, (uint newID) =>
                {
                    zone.LightSettings = newID;
                });
            }

            return change;
        }

        private static IDChangePair DissolveRundown(string content, RundownChangePair pair)
        {
            var change = DissolveBase<RundownDataBlock>(content, _RundownOffset++);
            var block = RundownDataBlock.GetBlock(change.New);

            MelonLogger.Log("Null?");
            if(block == null)
            {
                MelonLogger.Log($"Null {change.New}");
            }

            foreach(var map in block.TierA)
            {
                applyChanges(map);
            }

            foreach (var map in block.TierB)
            {
                applyChanges(map);
            }

            foreach (var map in block.TierC)
            {
                applyChanges(map);
            }

            foreach (var map in block.TierD)
            {
                applyChanges(map);
            }

            foreach (var map in block.TierE)
            {
                applyChanges(map);
            }

            return change;

            void applyChanges(ExpeditionInTierData map)
            {
                ApplyChangedPair(pair.FogSettings, map.Expedition.FogSettings, (uint newID) =>
                {
                    map.Expedition.FogSettings = newID;
                });

                ApplyChangedPair(pair.LightSettings, map.Expedition.LightSettings, (uint newID) =>
                {
                    map.Expedition.LightSettings = newID;
                });

                ApplyChangedPair(pair.Layouts, map.LevelLayoutData, (uint newID) =>
                {
                    map.LevelLayoutData = newID;
                });

                ApplyChangedPair(pair.Layouts, map.SecondaryLayout, (uint newID) =>
                {
                    map.SecondaryLayout = newID;
                });

                ApplyChangedPair(pair.Layouts, map.ThirdLayout, (uint newID) =>
                {
                    map.ThirdLayout = newID;
                });

                ApplyChangedPair(pair.Objectives, map.MainLayerData.ObjectiveData.DataBlockId, (uint newID) =>
                {
                    map.MainLayerData.ObjectiveData.DataBlockId = newID;
                });

                ApplyChangedPair(pair.Objectives, map.SecondaryLayerData.ObjectiveData.DataBlockId, (uint newID) =>
                {
                    map.SecondaryLayerData.ObjectiveData.DataBlockId = newID;
                });

                ApplyChangedPair(pair.Objectives, map.ThirdLayerData.ObjectiveData.DataBlockId, (uint newID) =>
                {
                    map.ThirdLayerData.ObjectiveData.DataBlockId = newID;
                });
            }
        }

        private static void ApplyChangedPair(IDChangePair[] pairs, uint currentID, Action<uint> onChangeDetected)
        {
            if(pairs == null)
            {
                return;
            }

            var change = pairs.FirstOrDefault(x => x.Previous == currentID);
            if (!change.Equals(default(IDChangePair)))
            {
                onChangeDetected?.Invoke(change.New);
            }
        }
    }
}
