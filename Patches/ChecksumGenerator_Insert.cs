using Harmony;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFO.Custom.Rundown.Patches
{
    [HarmonyPatch(typeof(ChecksumGenerator_32), "Insert", new Type[] { typeof(string), typeof(string) })]
    public static class ChecksumGenerator_Insert
    {
        public static void Prefix(string name, ref string value)
        {
            if(RundownPicker.IsDefault)
            {
                return;
            }

            if(name.Equals("PublicName") && !value.Contains("[CustomRundown]"))
            {
                value = $"{value} [CustomRundown]-{RundownPicker.SelectedChecksum}";
                MelonLogger.Log($"Found LevelName checksum generation! new name: {value}");
            }
        }
    }
}
