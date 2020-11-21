using CellMenu;
using Globals;
using Harmony;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFO.Custom.Rundown.Patches
{
    [HarmonyPatch(typeof(CM_StartupScreen), "Setup")]
    public static class CM_StartupScreen_Setup
    {
        public static void Postfix(CM_StartupScreen __instance)
        {
            RundownPicker.Button = __instance.m_btnStart;
            RundownPicker.IsMainScreen = true;

            __instance.m_btnStart.add_OnBtnPressCallback(new Action<int>(id => 
            {
                RundownPicker.IsMainScreen = false;

                if(!RundownPicker.IsDefault)
                {
                    MelonLogger.Log($"Value before: {Global.RundownIdToLoad}");

                    Global.RundownIdToLoad = RundownPicker.SelectedID;

                    MelonLogger.Log($"Value after: {Global.RundownIdToLoad}");
                }
            }));
        }
    }
}
