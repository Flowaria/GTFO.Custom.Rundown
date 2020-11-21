using CellMenu;
using GameData;
using GTFO.Custom.Rundown;
using GTFO.Custom.Rundown.CRundown;
using GTFO.Custom.Rundown.Patches;
using GTFO.Custom.Rundown.Utils;
using Harmony;
using MelonLoader;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

[assembly: MelonInfo(typeof(Entry), "Custom Rundown Core", "1.0", "Flowaria")]
[assembly: MelonGame("10 Chambers Collective", "GTFO")]

namespace GTFO.Custom.Rundown
{
    public class Entry : MelonMod
    {
        public static bool CustomDataLoadCalled = false;

        public override void OnApplicationStart()
        {
            //Harmony Patch is not needed, it will be automatically added by MelonLoader
        }

        public override void OnUpdate()
        {
            Update_CheckCustomDataIsLoaded();
            Update_MainScreenRundownSelector();
        }

        private void Update_CheckCustomDataIsLoaded()
        {
            if(CustomDataLoadCalled)
            {
                return;
            }

            if (!DataBlocksDissolver.IsCustomDataLoaded && GameDataInit.IsLoaded)
            {
                var basePath = Path.Combine(Imports.GetGameDirectory(), "Mods");
                var customRdwsPath = $"{basePath}\\Rundowns";

                if (!Directory.Exists(customRdwsPath))
                {
                    Directory.CreateDirectory(customRdwsPath);
                }

                CustomDataLoadCalled = true;

                var rundowns = DataBlocksDissolver.DissolveAllRundowns(customRdwsPath);
                foreach (var rundown in rundowns)
                {
                    RundownPicker.AddItem(rundown.Name, rundown.Checksum, rundown.ID);
                }
            }
        }

        private void Update_MainScreenRundownSelector()
        {
            if (!RundownPicker.IsMainScreen)
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