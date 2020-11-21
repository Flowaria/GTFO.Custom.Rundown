using Harmony;
using MelonLoader;
using System;

namespace GTFO.Custom.Rundown.Patches
{
    [HarmonyPatch(typeof(ChecksumGenerator_32), "Insert", new Type[] { typeof(string), typeof(string) })]
    public static class ChecksumGenerator_Insert
    {
        public static void Prefix(string name, ref string value)
        {
            if (RundownPicker.IsDefault) //We don't need to touch it when it's official rundown
            {
                return;
            }

            value = $"{value} [CustomRundown]-{RundownPicker.SelectedChecksum}";
            MelonLogger.Log($"Found LevelName checksum generation! new name: {value}");
        }
    }
}