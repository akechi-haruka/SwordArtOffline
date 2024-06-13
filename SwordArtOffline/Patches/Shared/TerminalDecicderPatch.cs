using HarmonyLib;
using System;

namespace SwordArtOffline.Patches.Shared {

    extern alias AssemblyNotice;

    public class TerminalDecicderPatch {

        [HarmonyPostfix, HarmonyPatch(typeof(UnityEngine.SystemInfo), "systemMemorySize", MethodType.Getter)]
        static void systemMemorySize(ref int __result) {
            __result = Plugin.WantsBootSatellite ? 8888 : 4000;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UnityEngine.SystemInfo), "processorType", MethodType.Getter)]
        static void processorType(ref string __result) {
            __result = "Intel";
        }

    }
}
