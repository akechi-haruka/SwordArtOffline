using HarmonyLib;
using LINK;
using System;
using System.Xml;

namespace SwordArtOffline.Patches.Testmode
{

    extern alias AssemblyNotice;

    public class KeychipPatchesTestmode
    {

        [HarmonyPostfix, HarmonyPatch(typeof(ErrorCheckManager), MethodType.Constructor)]
        static void Ctor(ErrorCheckManager __instance)
        {
            string text = Plugin.KeychipId;
            Plugin.Log.LogDebug("Keychip ID = " + text);
            __instance.dongleAll = text.Substring(0, 6) + "-" + text.Substring(6, 6);
            __instance.allnetID = "ABLN-" + text.Substring(5, 7);
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ErrorCheckManager), "checkDongleError")]
        static bool checkDongleError(ErrorCheckManager __instance)
        {
            Plugin.Log.LogDebug("checkdongle");
            return false;
        }

    }
}
