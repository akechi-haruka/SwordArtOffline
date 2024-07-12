using HarmonyLib;
using LINK;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SwordArtOffline.Patches.Game {

    public class KeychipPatchesGame {

        [HarmonyPrefix, HarmonyPatch(typeof(nbamUsbFinder), "GetSerialNumber")]
        public static bool GetSerialNumber(ref NbamUsbFindError __result, ulong usercode, [Out] IntPtr serialnumber) {
            Plugin.Log.LogDebug("ptr="+serialnumber);
            string id = Plugin.GetKeychip();
            byte[] kcbytes = Encoding.ASCII.GetBytes(id + char.MinValue);
            Marshal.Copy(kcbytes, 0, serialnumber, kcbytes.Length);
            __result = NbamUsbFindError.NBAM_USBFIND_SUCCESS;
            return false;
        }
    }
}
