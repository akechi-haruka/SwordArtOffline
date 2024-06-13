using HarmonyLib;
using LINK;
using System;

namespace SwordArtOffline.Patches.Game {

    extern alias AssemblyNotice;

    public class KeychipPatchesGame {
        /*private static byte[] dongleid;
        [HarmonyPrefix, HarmonyPatch(typeof(nbamUsbFinder), "GetSerialNumber")]
        static unsafe bool GetSerialNumber(ref NbamUsbFindError __result, ulong usercode, ref IntPtr serialnumber) {
            if (dongleid == null) {
                dongleid = Encoding.ASCII.GetBytes(Plugin.KeychipId);
                Plugin.Log.LogDebug("Keychip ID = " + Plugin.KeychipId);
            }
            Marshal.Copy(dongleid, 0, serialnumber, dongleid.Length);
            __result = NbamUsbFindError.NBAM_USBFIND_SUCCESS;
            serialnumber = serialnumber;
            return false;
        }*/
        [HarmonyPostfix, HarmonyPatch(typeof(ErrorCheckManager), MethodType.Constructor)]
        static void Ctor(ErrorCheckManager __instance) {
            string text = Plugin.KeychipId;
            Plugin.Log.LogDebug("Keychip ID = " + text);
            __instance.dongleAll = text.Substring(0, 6) + "-" + text.Substring(6, 6);
            __instance.allnetID = "ABLN-" + text.Substring(5, 7);
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ErrorCheckManager), "checkDongleError")]
        static bool checkDongleError(ErrorCheckManager __instance) {
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(nbamUsbFinder), "GetSerialNumber_wrapped")]
        static bool GetSerialNumber_wrapped(ref NbamUsbFindError __result, out string serial_str) {
            __result = NbamUsbFindError.NBAM_USBFIND_SUCCESS;
            serial_str = Plugin.KeychipId;
            return false;
        }
    }
}
