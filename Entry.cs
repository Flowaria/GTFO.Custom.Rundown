using CellMenu;
using GameData;
using Globals;
using GTFO.Custom.Rundown;
using GTFO.Custom.Rundown.CRundown;
using GTFO.Custom.Rundown.Patches;
using Harmony;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[assembly: MelonInfo(typeof(Entry), "Custom Rundown Core", "1.0", "Flowaria")]
[assembly: MelonGame("10 Chambers Collective", "GTFO")]
namespace GTFO.Custom.Rundown
{
    public class Entry : MelonMod
    {
        public override void OnApplicationStart()
        {
            var harmony = HarmonyInstance.Create("Custom Rundown");

            var type = typeof(ChecksumGenerator_32);
            var method = type.GetMethod("Insert", new Type[] { typeof(string), typeof(string) });
            var harmonyMethod = new HarmonyMethod(typeof(ChecksumGenerator_Insert).GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public));
            harmony.Patch(method, harmonyMethod);

            type = typeof(CM_StartupScreen);
            method = type.GetMethod("Setup");
            harmonyMethod = new HarmonyMethod(typeof(CM_StartupScreen_Setup).GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public));
            harmony.Patch(method, null, harmonyMethod);

            
        }

        
        public override void OnUpdate()
        {
            if(!DataBlocksDissolver.IsCustomDataLoaded && GameDataInit.IsLoaded)
            {
                var basePath = Path.Combine(Imports.GetGameDirectory(), "Mods");
                var customRdwsPath = $"{basePath}\\Rundowns";

                if (!Directory.Exists(customRdwsPath))
                {
                    Directory.CreateDirectory(customRdwsPath);
                }

                DataBlocksDissolver.IsCustomDataLoaded = true;

                var rundowns = DataBlocksDissolver.DissolveAllRundowns(customRdwsPath);
                foreach (var rundown in rundowns)
                {
                    RundownPicker.AddItem(rundown.Name, rundown.Checksum, rundown.ID);
                }
            }

            if(!RundownPicker.IsMainScreen)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RundownPicker.Previous();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RundownPicker.Next();
            }
        }
    }
}